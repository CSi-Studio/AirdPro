/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2.
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AirdPro.Asyncs;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Domains.Redis;
using AirdPro.Forms;
using AirdPro.Storage.Config;
using AirdPro.Utils;
using AirdSDK.Enums;
using HZH_Controls;
using Newtonsoft.Json;
using StackExchange.Redis;
using ClientInfo = AirdPro.Domains.ClientInfo;

namespace AirdPro.Redis
{
    public sealed class RedisManager
    {
        private static RedisManager _instance;
        private ConnectionMultiplexer _redis;
        private IDatabase _db;
        private readonly int _dbNum = 1;
        private static int _messageNum = 0;
        public const int HeartBeatInterval = 5000; //客户端心跳间隔,单位:秒
        public const int ConsumeInterval = 3000; //分布式任务消费间隔,单位:秒
        private static readonly object locker = new object();
        public static bool GlobalConsumeJobSwitch = false;

        private RedisManager()
        {
        }

        public static RedisManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisManager();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Connect(string host, int port, string user, string password)
        {
            ConfigurationOptions options = new ConfigurationOptions
            {
                EndPoints = { { host, port } },
                ConnectTimeout = 1000,
                ConnectRetry = 1,
                User = user.Equals("") ? null : user,
                Password = password.Equals("") ? null : password
            };

            try
            {
                _redis = ConnectionMultiplexer.Connect(options);
                _db = _redis.GetDatabase(_dbNum);
                _redis.GetSubscriber().Subscribe(RedisConst.SubscriberConsumeSwitch, ConsumeSwitchSubscriber);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public void ConsumeSwitchSubscriber(RedisChannel channel, RedisValue message)
        {
            ConsumeSwitchCommand command = JsonConvert.DeserializeObject<ConsumeSwitchCommand>(message.ToString());
            if (command.serverIps.Contains(ClientInfo.BuildUniqueID()))
            {
                Program.redisForm.Invoke((Action)(() =>
                {
                    Program.redisForm.switchConsumeJob.Checked = command.switcher;
                    Program.redisForm.LoadServers();
                }));
            }
        }
        public bool Check()
        {
            if (_redis != null && _redis.IsConnected)
            {
                return true;
            }

            return false;
        }

        //从Redis中读取相关的任务消息并转化为本地任务JobInfo
        public void Consume()
        {
            if (!Check()) return;
            if (!GlobalConsumeJobSwitch) return;
            bool needToExecute = false;
            string valueStr = null;
            RemoteConvertJob job = null;
            JobInfo jobInfo = null;

            try
            {
                RedisValue value = _db.SetPop(RedisConst.ConvertTask);
                if (!value.IsNullOrEmpty)
                {
                    // 如果获取到转换队列中相关的任务,那么将消息队列中的转换任务加入到执行队列中
                    valueStr = value.ToString();
                    job = JsonConvert.DeserializeObject<RemoteConvertJob>(valueStr);
                    ConversionConfig conversionConfig = new ConversionConfig
                    {
                        configName = "Redis",
                        suffix = job.suffix,
                        ignoreZeroIntensity = job.ignoreZeroIntensity,
                        creator = job.creator,
                    };

                    if (job.autoDecision != null)
                    {
                        conversionConfig.autoDecision = job.autoDecision.Value;
                    }

                    if (job.engine != null && job.engine == 1)
                    {
                        conversionConfig.engine = (int)AirdEngine.ColumnCompression;
                    }

                    if (job.mzPrecision != null)
                    {
                        conversionConfig.mzPrecision = job.mzPrecision.Value;
                    }

                    if (job.compressedIndex != null)
                    {
                        conversionConfig.compressedIndex = job.compressedIndex.Value;
                    }

                    if (job.indexFormat != null)
                    {
                        conversionConfig.indexFormat = job.indexFormat.Value;
                    }

                    if (job.mzIntComp != null)
                    {
                        conversionConfig.mzIntComp =
                            (SortedIntCompType)Enum.Parse(typeof(SortedIntCompType), job.mzIntComp);
                    }

                    if (job.mzByteComp != null)
                    {
                        conversionConfig.mzByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), job.mzByteComp);
                    }

                    if (job.intIntComp != null)
                    {
                        conversionConfig.intIntComp = (IntCompType)Enum.Parse(typeof(IntCompType), job.intIntComp);
                    }

                    if (job.intByteComp != null)
                    {
                        conversionConfig.intByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), job.intByteComp);
                    }

                    if (job.mobiIntComp != null)
                    {
                        conversionConfig.mobiIntComp = (IntCompType)Enum.Parse(typeof(IntCompType), job.mobiIntComp);
                    }

                    if (job.mobiByteComp != null)
                    {
                        conversionConfig.mobiByteComp =
                            (ByteCompType)Enum.Parse(typeof(ByteCompType), job.mobiByteComp);
                    }

                    jobInfo = new JobInfo(job.sourcePath, job.targetPath, job.type, conversionConfig);
                    jobInfo.fromRedis = true;
                    if (job.remoteId == null || job.remoteId.IsEmpty())
                    {
                        Guid uuid = Guid.NewGuid();
                        job.remoteId = uuid.ToString();
                    }
                    jobInfo.remoteId = job.remoteId;
                    needToExecute = true;
                }
            }
            catch (Exception)
            {
                //出现异常的情况下需要将消息会退给Redis,方便下一次重试
                if (valueStr != null)
                {
                    PublishJob(job);
                    RemoveConvertingJob(job.remoteId);
                }
            }

            //如果顺利从Redis获取分布式任务,则需暂停本地的任务获取,直至该任务转换完毕,每次仅从Redis获取一个转换任务
            if (needToExecute)
            {
                Console.WriteLine("Consume Job:" + job.remoteId);
                RedisForm.JobUnderConsuming = true;

                //开始本地转换任务前,需要将本任务的执行信息同步到Redis
                AddConvertingJob(job);
                ListViewItem item = jobInfo.BuildItem();
                ConvertTaskManager.GetInstance().PushJob(jobInfo);
                Program.conversionForm.lvFileList.Items.Add(item);
                Program.conversionForm.DoConvert();
            }
        }

        public void RegisterOrUpdate()
        {
            if (!Check()) return;
            ClientInfo info = new ClientInfo();
            info.init();
            string clientInfo = info.ToJson();
            _db.HashSet(RedisConst.ServerInfoList, info.ClientID, clientInfo);
        }

        public void Disconnect()
        {
            if (_redis != null)
            {
                _redis.Close();
                _redis = null;
                _db = null;
            }
        }

        public void ClearServerCache()
        {
            if (!Check()) return;
            _db.KeyDelete(RedisConst.ServerInfoList);
        }

        public void PublishJob(RemoteConvertJob job)
        {
            if (!Check()) return;
            Guid uuid = Guid.NewGuid();
            job.remoteId = uuid.ToString();
            string jobStr = JsonConvert.SerializeObject(job,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            _db.SetAdd(RedisConst.ConvertTask, jobStr);
        }

        /**
         * 加入一个远程转换任务
         */
        public void AddConvertingJob(RemoteConvertJob job)
        {
            if (!Check()) return;
            job.consumeIP = HttpUtil.GetIPV4ListAsString();
            job.consumeTime = DateTime.Now.ToString();
            string jobStr = JsonConvert.SerializeObject(job);
            _db.HashSet(RedisConst.ConvertingTask, job.remoteId, jobStr);
        }

        /**
         * 删除远程的转换任务
         */
        public void RemoveConvertingJob(string jobId)
        {
            if (!Check()) return;
            try
            {
                bool result = _db.HashDelete(RedisConst.ConvertingTask, jobId);
                Console.WriteLine("Delete" + result + ".JobId:" + jobId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Delete Error:" + e.Message);
            }
        }

        /**
        * 获取局域网内所有的AirdPro客户端信息
        */
        public Dictionary<string, ClientInfo> GetServerMap()
        {
            if (!Check()) return new Dictionary<string, ClientInfo>();
            HashEntry[] entries = _db.HashGetAll(RedisConst.ServerInfoList);
            Dictionary<string, ClientInfo> serverMap = new Dictionary<string, ClientInfo>();
            foreach (var entry in entries)
            {
                ClientInfo clientInfo = JsonConvert.DeserializeObject<ClientInfo>(entry.Value);
                //客户端心跳时间为5秒
                if ((DateTime.Now - DateTime.FromOADate(clientInfo.LastUpdateTime)).TotalSeconds <=
                    (HeartBeatInterval + 1))
                {
                    serverMap.Add(entry.Name, clientInfo);
                }
            }
            return serverMap;
        }

        /**
         * 按照IP获取该节点的所有具体信息
         */
        public Dictionary<string, object> GetServerInfo(string ip)
        {
            if (!Check()) return null;
            string value = _db.HashGet(RedisConst.ServerInfoList, ip);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (value != null && !value.IsEmpty())
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
            }

            return dict;
        }

        /**
         * 获取局域网内所有已经发布的任务列表
         */
        public List<RemoteConvertJob> GetTodoJobs()
        {
            List<RemoteConvertJob> jobStrList = new List<RemoteConvertJob>();
            if (!Check()) return jobStrList;
            RedisValue[] jobs = _db.SetMembers(RedisConst.ConvertTask);
            foreach (RedisValue jobValue in jobs)
            {
                string jobStr = jobValue.ToString();
                RemoteConvertJob job = JsonConvert.DeserializeObject<RemoteConvertJob>(jobStr);
                jobStrList.Add(job);
            }

            return jobStrList;
        }

        /**
         * 获取局域网内所有已经发布的任务列表
         */
        public List<RemoteConvertJob> GetConvertingJobs()
        {
            List<RemoteConvertJob> jobStrList = new List<RemoteConvertJob>();
            if (!Check()) return jobStrList;
            var jobs = _db.HashGetAll(RedisConst.ConvertingTask);
            foreach (var entry in jobs)
            {
                string jobStr = entry.Value.ToString();
                RemoteConvertJob job = JsonConvert.DeserializeObject<RemoteConvertJob>(jobStr);
                jobStrList.Add(job);
            }

            return jobStrList;
        }

        public void OpenConsume(List<string> serverIps)
        {
            ConsumeSwitchCommand command = new ConsumeSwitchCommand();
            command.serverIps = serverIps;
            command.switcher = true;
            string com = JsonConvert.SerializeObject(command);
            _db.Publish(RedisConst.SubscriberConsumeSwitch, com);
        } 
        
        public void CloseConsume(List<string> serverIps)
        {
            ConsumeSwitchCommand command = new ConsumeSwitchCommand();
            command.serverIps = serverIps;
            command.switcher = false;
            string com = JsonConvert.SerializeObject(command);
            _db.Publish(RedisConst.SubscriberConsumeSwitch, com);
        }
    }
}