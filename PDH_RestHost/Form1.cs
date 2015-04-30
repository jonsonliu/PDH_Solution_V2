using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDH_RestService;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace PDH_RestHost
{
    public partial class Form1 : Form
    {
        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器

        public Form1()
        {
            InitializeComponent();
        }
        private WebServiceHost Host = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (Host == null)
            {
                Host = new WebServiceHost(typeof(CrmRestService), new Uri("http://petdreamhouse-a.cloudapp.net:8000"));

                ServiceEndpoint endpoint = Host.AddServiceEndpoint(typeof(ICrmRestService), new WebHttpBinding(), "");
                //endpoint.Behaviors.Add(new WebHttpBehavior());
                ServiceDebugBehavior sdb = Host.Description.Behaviors.Find<ServiceDebugBehavior>();
                sdb.HttpHelpPageEnabled = false;
                Host.Open();
                log.Info("Rest服务启动完成。。。");//写入一条新log
                MessageBox.Show("服务启动完成。。。");
            }

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Host != null)
            {
                Host.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Host != null)
            {
                Host.Close();
                Host = null;
                log.Info("服务成功停止。。。");
                MessageBox.Show("服务成功停止。。。");
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)    //最小化到系统托盘
            {
                notifyIcon1.Visible = true;    //显示托盘图标
                this.Visible = false;
                this.Hide();    //隐藏窗口
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注意判断关闭事件Reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                notifyIcon1.Visible = true;
                this.Visible = false;
                this.Hide();
                return;
            }
        }
        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }
    }
}
