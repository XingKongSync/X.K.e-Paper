using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpLinkNetMonitor
{
    public class PageManager
    {
        private List<EntityHelper.HostInfoEntity> localData;
        private Dictionary<string, EntityHelper.HostInfoEntity> ipAndPageMap;

        public List<EntityHelper.HostInfoEntity> Pages
        {
            get
            {
                if (localData == null)
                {
                    localData = new List<EntityHelper.HostInfoEntity>();
                }
                return localData;
            }

            private set
            {
                localData = value;
            }
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="dataSource"></param>
        public void SetDataSource(List<EntityHelper.HostInfoEntity> dataSource)
        {
            if (localData == null)
            {
                localData = new List<EntityHelper.HostInfoEntity>();
            }
            if (ipAndPageMap == null)
            {
                ipAndPageMap = new Dictionary<string, EntityHelper.HostInfoEntity>();
                foreach (var local in localData)
                {
                    ipAndPageMap.Add(local.ip, local);
                }
            }

            foreach (var source in dataSource)
            {
                if (ipAndPageMap.ContainsKey(source.ip))
                {
                    //如果已经存在该host，则更新值
                    var local = ipAndPageMap[source.ip];
                    local.mac = source.mac;
                    local.up_speed = source.up_speed;
                    local.down_speed = source.down_speed;
                    local.hostname = source.hostname;
                }
                else
                {
                    //如果不存在则添加该host
                    localData.Add(source);
                    ipAndPageMap.Add(source.ip, source);
                }
            }
        }

        public int GetPageCount()
        {
            int pageCount;
            pageCount = localData.Count / 4;
            if (localData.Count % 4 != 0)
            {
                pageCount++;
            }
            return pageCount;
        }

        public List<EntityHelper.HostInfoEntity> GetPageByIndex(int index)
        {
            int start = index * 4;
            int count = localData.Count - start;
            if (count > 4)
            {
                count = 4;
            }

            List<EntityHelper.HostInfoEntity> page = localData.GetRange(start, count);
            return page;
        }
    }
}
