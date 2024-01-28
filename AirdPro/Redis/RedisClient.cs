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
using System.Threading;
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
    class RedisClient
    {
        private static RedisClient _instance;
        private ConnectionMultiplexer _redis;
        private IDatabase _db;
        private readonly int _dbNum = 1;
        private static int _messageNum = 0;
        public const int HeartBeatTime = 5; //客户端心跳时间,单位:秒
        private static string Increment()
        {
            return Interlocked.Increment(ref _messageNum) + "";
        }

        private RedisClient()
        {
        }

        public static RedisClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RedisClient();
            }

            return _instance;
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
            
            int i = 10;
            bool needToExecute = false;
            while (i > 0)
            {
                String valueStr = null;
                try
                {
                    RedisValue value = _db.SetPop(RedisConst.Redis_Queue_Convert);
                    if (!value.IsNullOrEmpty)
                    {
                        Program.redisForm.lblMessageNum.Text = Increment();
                        // 如果获取到转换队列中相关的任务,那么将消息队列中的转换任务加入到执行队列中
                        valueStr = value.ToString();
                        // 目前远程任务不支持Stack-ZDPD
                        RemoteConvertJob job = JsonConvert.DeserializeObject<RemoteConvertJob>(valueStr);
                        ConversionConfig conversionConfig = new ConversionConfig();
                        conversionConfig.configName = "Redis";
                        conversionConfig.suffix = job.suffix;
                        conversionConfig.ignoreZeroIntensity = job.ignoreZeroIntensity;
                        conversionConfig.creator = job.creator;

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
                            conversionConfig.mzIntComp = (SortedIntCompType)Enum.Parse(typeof(SortedIntCompType), job.mzIntComp);
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
                            conversionConfig.mobiByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), job.mobiByteComp);
                        }

                        JobInfo jobInfo = new JobInfo(job.sourcePath, job.targetPath, job.type, conversionConfig);
                        ListViewItem item = jobInfo.BuildItem();
                        if (!ConvertTaskManager.GetInstance().JobTable.Contains(jobInfo.jobId))
                        {
                            Program.conversionForm.lvFileList.Items.Add(item);
                            ConvertTaskManager.GetInstance().PushJob(jobInfo);
                            needToExecute = true;
                        }
                    }
                }
                catch (Exception)
                {
                    //出现异常的情况下需要将消息会退给Redis,方便下一次重试
                    if (valueStr != null)
                    {
                        _db.SetAdd(RedisConst.Redis_Queue_Convert, valueStr);
                    }
                }

                i--;
            }

            //如果在Redis获取到了相关的转换任务
            if (needToExecute)
            {
                Program.conversionForm.DoConvert();
            }
        }


        public void RegisterOrUpdate()
        {
            if (!Check()) return;
            _db.HashSet(RedisConst.Redis_Server_List, NetworkUtil.getHostIP(), DateTime.Now.ToOADate());
            string clientInfo = ClientInfo.toJSON();
            _db.HashSet(RedisConst.Redis_Server_Info_List, NetworkUtil.getHostIP(), clientInfo);
        }

        /**
         * 获取局域网内所有的AirdPro客户端
         */
        public List<string> GetServerList()
        {
            if (!Check()) return null;
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
        
    }
}