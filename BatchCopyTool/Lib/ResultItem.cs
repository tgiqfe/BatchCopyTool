using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchCopyTool
{
    internal class ResultItem
    {
        public string TargetMachine { get; set; }
        public bool IsPingResponse { get; set; }
        public string Status { get; set; }
        public string RemotePath { get; set; }
        public MachineAccount Account { get; set; }
    }
}
