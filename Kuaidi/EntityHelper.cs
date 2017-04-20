using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuaidi
{
    public class EntityHelper
    {
        public class ResponseEntity
        {
            public string message;
            public string status;
            public string state;
            public List<DataItemEntity> data;
        }

        public class DataItemEntity
        {
            public string time;
            public string ftime;
            public string context;
            public string location;
        }
    }
}
