using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SupplierPortal.Util;

namespace SupplierPortal.Views.Main
{
    public partial class MenuHandler : System.Web.UI.Page
    {
        public class MenuTree : ExtTreeData<MenuTree>
        {
            //请求的url值
            public string url { get; set; }
            //自定义节点样式
            public string cls { get; set; }
            //是否展开
            public bool expanded { get; set; }
        }
        //返回EXT树
        protected void Page_Load(object sender, EventArgs e)
        {
            var children_result = new List<MenuTree>();
            MenuTree mt1 = new MenuTree();
            mt1.id = "001001";
            mt1.text = "表一";
            mt1.leaf = true;
            mt1.url = "/Views/DataTable/ViewCrmProductData.aspx";
            mt1.expanded = true;
            mt1.level = 2;
            mt1.iconCls = "folder_user";
            children_result.Add(mt1);

            MenuTree mt2 = new MenuTree();
            mt2.id = "001002";
            mt2.text = "表二";
            mt2.leaf = true;
            mt2.url = "/Views/DataTable/Employee.aspx";
            mt2.expanded = true;
            mt2.level = 2;
            mt2.iconCls = "folder_user";
            children_result.Add(mt2);

            var result = new List<MenuTree>();
            MenuTree mt = new MenuTree();
            mt.id = "001";
            mt.text = "功能菜单";
            mt.leaf = false;
            mt.url = "";
            mt.expanded = true;
            mt.level = 1;
            mt1.iconCls = "user_gray";
            mt.children = children_result;

            result.Add(mt);
            string strJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            Response.Write(strJson);
            Response.End();

        }
    }
}