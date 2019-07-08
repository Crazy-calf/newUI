using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Satellite
    {
        public string 名字 = "未知卫星".PadRight(24,' ');
        public string line1 = "";
        public string line2 = "";
        public double 星下点轨迹经度宽度 = 0;

        public Satellite(string 名字, string line1, string line2, double 星下点轨迹经度宽度)
        {
            this.名字 = 名字;
            this.line1 = line1;
            this.line2 = line2;
            this.星下点轨迹经度宽度 = 星下点轨迹经度宽度;
        }
    }
}
