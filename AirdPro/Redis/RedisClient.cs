using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirdPro.Asyncs;
using StackExchange.Redis;
using AirdPro.Constants;
using AirdPro.Domains.Convert;
using AirdPro.Domains.Job;
using AirdPro.Forms;
using Newtonsoft.Json;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Redis
{
    class RedisClient
    {
        private static RedisClient instance;
        private ConnectionMultiplexer redis;
        private string hostAndPort;

        private IDatabase db;

        private RedisClient()
        { 
           
        }

        public static RedisClient getInstance()
        {
            if(instance == null)
            {
                instance = new RedisClient();
            }
            return instance;
        }

        public Boolean connect(string hostAndPort)
        {
            if (!hostAndPort.Contains(":"))
            {
                hostAndPort = hostAndPort + ":" + 6379;
            }
           
            this.hostAndPort = hostAndPort;
            try
            {
                redis = ConnectionMultiplexer.Connect(hostAndPort);
                db = redis.GetDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
           
            if (redis.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean check()
        {
            if (redis != null && redis.IsConnected)
            {
                Program.mainForm.lblConnectStatus.Text="Connected";
                return true;
            }
            else
            {
                if(redis != null && redis.IsConnected)
                {
                    Program.mainForm.lblConnectStatus.Text = "Connected";
                    return true;
                }
                else
                {
                    Program.mainForm.lblConnectStatus.Text = "Not Connect";
                    return false;
                }
            }
        }

        //使用当前的配置进行重试
        public void retry()
        {
            if (this.hostAndPort != null && !this.hostAndPort.Equals(""))
            {
                try
                {
                    redis = ConnectionMultiplexer.Connect(hostAndPort);
                    db = redis.GetDatabase();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        public void consume()
        {
            if (check())
            {
                int i = 10;
                bool needToExe = false;
                while (i > 0)
                {
                    String valueStr = null;
                    try
                    { 
                        RedisValue value = db.SetPop(Const.Redis_Queue_Convert);
                        if (!value.IsNullOrEmpty)
                        {
                            // 如果获取到转换队列中相关的任务,那么将消息队列中的转换任务加入到执行队列中
                            valueStr = value.ToString();
                            RemoteConvertJob job = JsonConvert.DeserializeObject<RemoteConvertJob>(valueStr);
                            JobParams jobParams = new JobParams();
                            jobParams.ignoreZeroIntensity = true;
                            jobParams.log2 = false;
                            jobParams.threadAccelerate = true;
                            jobParams.suffix = "";
                            jobParams.creator = "LIMS Admin";
                            jobParams.mzPrecision = job.mzPrecision;

                            string[] items = new string[4];
                            ListViewItem item = new ListViewItem(items);
                            item.SubItems[0].Text = job.sourcePath;
                            item.SubItems[1].Text = job.type;
                            item.SubItems[2].Text = "Waiting";
                            item.SubItems[3].Text = job.targetPath;
                            JobInfo jobInfo = new JobInfo(job.sourcePath, job.targetPath,
                                job.type, jobParams, item);
                            if (!ConvertTaskManager.getInstance().jobTable.Contains(jobInfo.jobId))
                            {
                                Program.mainForm.lvFileList.Items.Add(item);
                                ConvertTaskManager.getInstance().pushJob(jobInfo);
                                needToExe = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //出现异常的情况下需要将消息会退给Redis,方便下一次重试
                        if (valueStr != null)
                        {
                            db.SetAdd(Const.Redis_Queue_Convert, valueStr);
                        }
                    }
                    i--;
                }
                //如果在Redis获取到了相关的转换任务
                if (needToExe)
                {
                    ConvertTaskManager.getInstance().run();
                }
               

            }
        }

        public void disconnect()
        {
            redis.Close();
            redis = null;
            db = null;
        }
    }
}
