using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RadarScreen
{
    public class Satellite
    {

        private string 名字1 = "未知卫星".PadRight(24, ' ');
        private string Line1 = "";
        private string Line2 = "";
        public double 星下点轨迹经度宽度 = 0;


        public Satellite()
        {
        }

        public Satellite(string 名字, string line1, string line2, double 星下点轨迹经度宽度)
        {
            this.名字1 = 名字;
            this.Line1 = line1;
            this.Line2 = line2;
            this.星下点轨迹经度宽度 = 星下点轨迹经度宽度;
        }


        public string 名字 { get => 名字1; set => 名字1 = value; }
        public string line1 { get => Line1; set => Line1 = value; }
        public string line2 { get => Line2; set => Line2 = value; }
    }
}