using PDH_RestService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PDH_RestWindowsService
{
    partial class RestService : ServiceBase
    {

        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器
        private ServiceHost Host = null;
        public RestService()
        {
            InitializeComponent();
            ServiceName = "PDH_RestService";
        }

        protected override void OnStart(string[] args)
        {
            if (Host != null)
            {
                Host.Close();
            }
            else if (Host == null)
            {
                //Host = new WebServiceHost(typeof(CrmRestService), new Uri("http://petdreamhouse-a.cloudapp.net:8000"));
                Host = new WebServiceHost(typeof(CrmRestService), new Uri("http://146.255.33.119:8000"));
                ServiceEndpoint endpoint = Host.AddServiceEndpoint(typeof(ICrmRestService), new WebHttpBinding(), "");
                //endpoint.Behaviors.Add(new WebHttpBehavior());
                ServiceDebugBehavior sdb = Host.Description.Behaviors.Find<ServiceDebugBehavior>();
                sdb.HttpHelpPageEnabled = false;
                Host.Open();
                log.Info("Rest服务启动完成。。。");//写入一条新log

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
