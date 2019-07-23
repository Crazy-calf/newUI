using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 轨道计算;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        #region 声明
        AutoSizeFormClass asc = new AutoSizeFormClass();//分辨率适配

        public bool isOpenWindow_天线 = false;
        public bool isOpenWindow_X光发射1 = false;

        //RadarScreen.Satellite satellite;
        #endregion

        public bool isDraw = false;

        #region 临时数据
        //static string name = "HAIYANG 2A";
        //static string line1 = "1 37781U 11043A   19084.96741172 -.00000044  00000-0  11863-6 0  9998";
        //static string line2 = "2 37781  99.3212  95.3692 0001741  85.5656 274.5707 13.78718376383104";
        //static double width = 0.0;
        //Satellite satellite = new Satellite(name, line1, line2, width);

        //static double x = 116.363;
        //static double y = 40.059;
        //static double z = 0.4;

        //DateTime 开始时间 = Convert.ToDateTime("2019/5/19 17:28:33");
        //DateTime 结束时间 = Convert.ToDateTime("2019/5/19 17:42:06");

        //int 时区 = 8;

        ////雷达半轴坐标上限为90
        //float x_max = 90;
        //float y_max = 90;
        #endregion


        #region 委托
        public delegate void ChangeStatus(Boolean isOpne);

        public static void win_Close(ref bool a)
        {
            a = false;
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!isOpenWindow_天线)
            {
                天线 tx = new 天线();
                tx.StartPosition = FormStartPosition.CenterParent;
                tx.Show();
                isOpenWindow_天线 = true;
                tx.MyEvent += new 天线.MyDelegate(() => isOpenWindow_天线 = false);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (!isOpenWindow_X光发射1)
            {
                X光发射1 obj = new X光发射1();
                obj.StartPosition = FormStartPosition.CenterParent;
                obj.Show();
                isOpenWindow_X光发射1 = true;
                obj.MyEvent += new X光发射1.MyDelegate(() => isOpenWindow_X光发射1 = false);
            }
        }

        private void Button14_Click(object sender, EventArgs e)
        {

            string name = this.name.Text;
            string line1 = this.line1.Text;
            string line2 = this.line2.Text;
            double width = 0.0;

            RadarScreen.Satellite satellite = new RadarScreen.Satellite(name, line1, line2, width);

            雷达幕1.开始时间 = Convert.ToDateTime(textBox1.Text);
            雷达幕1.结束时间 = Convert.ToDateTime(textBox2.Text);
            
            //雷达半轴坐标上限为90
            雷达幕1.当前时刻 = Convert.ToDateTime(textBox3.Text);

            雷达幕1.satellite = satellite;


            雷达幕1.Start();


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="AZ">[-180,180]</param>
        /// <param name="EL">[-90,90]</param>
        /// <returns></returns>
        private float calculateAZ_EL_XY(bool v, double AZ, double EL)
        {
            double x;
            if (v)
            {
                x = (Math.Sin(AZ * Math.PI / 180) * (90 - EL) + 90) / 2;
            }
            else
            {
                x = (90 - (Math.Cos(AZ * Math.PI / 180) * (90 - EL) + 90)) / 2;
            }
            return (float)x;
        }

        public ArrayList 分段(PointF[] pps)
        {
            ArrayList ay = new ArrayList();
            if (pps.Length == 0)
                return ay;
            float temp = pps[0].X;
            for (int i = 1; i < pps.Length; i++)
            {
                if (Math.Abs(pps[i].X - temp) > 100)//跳变需要分段
                {
                    ay.Add(i);
                }
                temp = pps[i].X;
            }

            return ay;//最后得到的是分几段，在哪个序号分段

        }

        private void 雷达慕1_SizeChanged(object sender, EventArgs e)
        {
            雷达幕1.Draw();
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            雷达幕1.close();
        }

        private void 雷达幕1_ImeModeChanged(object sender, EventArgs e)
        {

        }

        private void Button16_Click(object sender, EventArgs e)
        {
            雷达幕1.TimeLag = 10;
            雷达幕1.StartTimer();
        }
    }
}
