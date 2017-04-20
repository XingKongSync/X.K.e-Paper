using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuaidi
{
    class KuaidiListItem
    {
        public string DisplayName;
        public string ComPyName;
        public string ComCnName;
        public string KdNumber;

        public override string ToString()
        {
            return string.Format("DisplayName:{0}, ComPyName:{1}, ComCnName:{2}, KdNumber:{3}", DisplayName, ComPyName, ComCnName, KdNumber);
        }
    }
}
