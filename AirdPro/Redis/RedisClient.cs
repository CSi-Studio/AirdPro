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
using AirdPro.Storage.Config;
using AirdPro.Utils;
using AirdSDK.Enums;
using AirdSDK.Utils;
using HZH_Controls;
using Newtonsoft.Json;
using StackExchange.Redis;
using ClientInfo = AirdPro.Domains.ClientInfo;

namespace AirdPro.Redis
{
    public sealed class RedisClient
    {
        private static RedisClient _instance;
        private ConnectionMultiplexer _redis;
        private IDatabase _db;
        private readonly int _dbNum = 1;
        private static int _messageNum = 0;
        public const int HeartBeatTime = 3; //客户端心跳时间,单位:秒
        private static readonly object locker = new object();

        private RedisClient()
        {
        }

        public static RedisClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisClient();
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
            }
            catch (Exception e)
            {
                // ignored
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
            bool needToExecute = false;
            string valueStr = null;
            RemoteConvertJob job = null;
            try
            {
                RedisValue value = _db.SetPop(RedisConst.Redis_Queue_Convert);
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
                        copyToLocal = job.copyToLocal
                    };

                    if (job.autoDesicion != null)
                    {
                        conversionConfig.autoDesicion = job.autoDesicion.Value;
                    }

                    if (job.scene != null && job.scene == "Search")
                    {
                        conversionConfig.scene = Scene.Search;
                    }

                    if (job.mzPrecision != null)
                    {
                        conversionConfig.mzPrecision = job.mzPrecision.Value;
                    }

                    if (job.centroid != null)
                    {
                        conversionConfig.centroid = job.centroid.Value;
                    }

                    if (job.compressedIndex != null)
                    {
                        conversionConfig.compressedIndex = job.compressedIndex.Value;
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

                    JobInfo jobInfo = new JobInfo(job.sourcePath, job.targetPath, job.type, conversionConfig);
                    jobInfo.fromRedis = true;
                    jobInfo.remoteId = job.remoteId;
                    ListViewItem item = jobInfo.BuildItem();
                    Program.conversionForm.lvFileList.Items.Add(item);
                    ConvertTaskManager.GetInstance().PushJob(jobInfo);
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
                Program.redisForm.consumeTimer.Stop();

                //开始本地转换任务前,需要将本任务的执行信息同步到Redis
                AddConvertingJob(job);
                Program.conversionForm.DoConvert();
            }
        }

        public void RegisterOrUpdate()
        {
            if (!Check()) return;
            _db.HashSet(RedisConst.Redis_Server_List, HttpUtil.GetServerName(),
                DateTime.Now.ToOADate());
            string clientInfo = ClientInfo.toJSON();
            _db.HashSet(RedisConst.Redis_Server_Info_List, HttpUtil.GetServerName(),
                clientInfo);
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

        public Dictionary<string, string> GetServerInfo(string ip)
        {
            if (!Check()) return null;
            string value = _db.HashGet(RedisConst.Redis_Server_Info_List, ip);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (value != null && !value.IsEmpty())
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, String>>(value);
            }

            return dict;
        }

        public void ClearServerCache()
        {
            if (!Check()) return;
            _db.KeyDelete(RedisConst.Redis_Server_List);
            _db.KeyDelete(RedisConst.Redis_Server_Info_List);
        }

        public void PublishJob(RemoteConvertJob job)
        {
            if (!Check()) return;
            Guid uuid = Guid.NewGuid();
            job.remoteId = uuid.ToString();
            string jobStr = JsonConvert.SerializeObject(job,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            _db.SetAdd(RedisConst.Redis_Queue_Convert, jobStr);
        }

        public void AddConvertingJob(RemoteConvertJob job)
        {
            if (!Check()) return;
            job.consumeIP = NetworkUtil.getHostIP();
            job.consumeTime = DateTime.Now.ToString();
            string jobStr = JsonConvert.SerializeObject(job);
            _db.HashSet(RedisConst.Redis_Queue_Converting, job.remoteId, jobStr);
        }

        public void RemoveConvertingJob(string jobId)
        {
            if (!Check()) return;
            try
            {
                bool result = _db.HashDelete(RedisConst.Redis_Queue_Converting, jobId);
                Console.WriteLine("删除" + result + ".JobId:" + jobId);
            }
            catch (Exception e)
            {
                Console.WriteLine("删除异常：" + e.Message);
            }
        }

        /**
        * 获取局域网内所有的AirdPro客户端信息
        */
        public List<string> GetServerList()
        {
            if (!Check()) return new List<string>();
            HashEntry[] entries = _db.HashGetAll(RedisConst.Redis_Server_List);
            List<string> servers = new List<string>();
            foreach (var entry in entries)
            {
                DateTime dateTime = DateTime.FromOADate(Double.Parse(entry.Value));
                if ((DateTime.Now - dateTime).TotalSeconds <= (HeartBeatTime + 1)) //客户端心跳时间为5秒
                {
                    servers.Add(entry.Name);
                }
            }

            return servers;
        }

        /**
         * 获取局域网内所有已经发布的任务列表
         */
        public List<RemoteConvertJob> GetTodoJobs()
        {
            List<RemoteConvertJob> jobStrList = new List<RemoteConvertJob>();
            if (!Check()) return jobStrList;
            RedisValue[] jobs = _db.SetMembers(RedisConst.Redis_Queue_Convert);
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
            var jobs = _db.HashGetAll(RedisConst.Redis_Queue_Converting);
            foreach (var entry in jobs)
            {
                string jobStr = entry.Value.ToString();
                RemoteConvertJob job = JsonConvert.DeserializeObject<RemoteConvertJob>(jobStr);
                jobStrList.Add(job);
            }

            return jobStrList;
        }
    }
}