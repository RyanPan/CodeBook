using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MyNewService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            SetMSMQ();
            return;

            if (Environment.UserInteractive)
            {
                MyNewService service = new MyNewService(args);
                service.TestMain();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MyNewService(args),
                    new DummyService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        static void SetMSMQ()
        {
            ResIDInfo resInfo = new ResIDInfo();
            resInfo.ResId = 10990;

            string strMSMQName = "HWUpSearch";
            string strMqRemoteIp = "192.168.10.101";
            MessageQueue meg = new MessageQueue();
            meg = new MessageQueue(string.Format(@"FormatName:Direct=TCP:{0}\Private$\{1}", strMqRemoteIp, strMSMQName));
            Message mymessage = new Message();
            mymessage.Body = resInfo;
            mymessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(ResIDInfo) });
            meg.Send(mymessage);
        }
    }

    [Serializable]
    public class ResIDInfo
    {
        public int ResId { get; set; }
        public int ReTryCount { get; set; }
    }
}
