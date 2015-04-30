using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PDH_WcfWindowsService
{
    partial class WcfService : ServiceBase
    {
        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器
        private ServiceHost Host = null;
        public WcfService()
        {
            InitializeComponent();
            ServiceName = "PDH_WcfService";
        }

        protected override void OnStart(string[] args)
        {
            if (Host != null)
            {
                Host.Close();
            }
            else if (Host == null)
            {
                Host = new ServiceHost(typeof(PDH_WcfService.CrmOperationService));

                //绑定
                System.ServiceModel.Channels.Binding httpBinding = new BasicHttpBinding();
                //终结点
                //Host.AddServiceEndpoint(typeof(PDH_WcfService.ICrmOperationService), httpBinding, "http://petdreamhouse-a.cloudapp.net:8002/");
                Host.AddServiceEndpoint(typeof(PDH_WcfService.ICrmOperationService), httpBinding, "http://petdreamhouse-a.cloudapp.net:8002/");
                if (Host.Description.Behaviors.Find<System.ServiceModel.Description.ServiceMetadataBehavior>() == null)
                {
                    //行为
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;

                    //元数据地址
                    //behavior.HttpGetUrl = new Uri("http://petdreamhouse-a.cloudapp.net:8002/CrmOperationService");
                    behavior.HttpGetUrl = new Uri("http://petdreamhouse-a.cloudapp.net:8002/CrmOperationService");

                    Host.Description.Behaviors.Add(behavior);

                    //启动
                    Host.Open();
                }
                log.Info("服务启动完成。。。");//写入一条新log
            }


        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            if (Host != null)
            {
                Host.Close();
                Host = null;
                log.Info("服务停止完成。。。");//写入一条新log
            }
        }
    }
}
