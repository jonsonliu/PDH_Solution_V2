using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupplierPortal.Util
{
    /// <summary>
    /// 所有对EXT树数据绑定相关的json对象都要继承的抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExtTreeData<T>
    {
        public string id { get; set; }

        public string text { get; set; }

        public string iconCls { get; set; }

        public bool leaf { get; set; }

        public virtual IList<T> children { get; set; }

        public virtual int level { get; set; }
    }
}
