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
using System.Threading;
using System.Windows.Forms;
using AirdPro.Asyncs;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Storage.Config;
using AirdSDK.Enums;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AirdPro.Redis
{
    class RedisClient
    {
        private static RedisClient _instance;
        private ConnectionMultiplexer _redis;
        private IDatabase _db;
        private readonly int _dbNum = 1;
        private static int _messageNum = 0;

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

        public bool Connect(string host, int port, string user, string password)
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
                return false;
            }

            return _redis.IsConnected;
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
        public bool Consume()
        {
            bool check = Check();
            if (check)
            {
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
                            conversionConfig.ignoreZeroIntensity = true;
                            conversionConfig.autoDesicion = false;
                            conversionConfig.threadAccelerate = true;
                            conversionConfig.configName = "RedisDefault";
                            conversionConfig.suffix = job.suffix;
                            conversionConfig.ignoreZeroIntensity = job.ignoreZeroIntensity;
                            conversionConfig.creator = job.creator;
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

                            JobInfo jobInfo = new JobInfo(job.sourcePath, job.targetPath, job.type, conversionConfig);
                            ListViewItem item = jobInfo.buildItem();
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

            return check;
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
    }
}