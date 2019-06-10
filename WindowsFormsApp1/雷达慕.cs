using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 轨道计算;
using System.Collections;

namespace WindowsFormsApp1
{
    public partial class 雷达慕 : UserControl
    {
        #region 参数
        public string name = "HAIYANG 2A";
        public string line1 = "1 37781U 11043A   19084.96741172 -.00000044  00000-0  11863-6 0  9998";
        public string line2 = "2 37781  99.3212  95.3692 0001741  85.5656 274.5707 13.78718376383104";
        public double width = 0.0;

        public Satellite satellite;

        #region 地面站
        /// <summary>
        /// 地面站经度
        /// </summary>
        public double x = 116.363;

        /// <summary>
        /// 地面站维度
        /// </summary>
        public double y = 40.059;

        /// <summary>
        /// 地面站高度
        /// </summary>
        public double z = 0.4;
        #endregion

        #region 起止时间及时区
        DateTime 开始时间 = Convert.ToDateTime("2019/5/19 17:28:33");
        DateTime 结束时间 = Convert.ToDateTime("2019/5/19 17:42:06");
        public int 时区 = 8;
        #endregion

        #region 画线属性
        public Color 线颜色 = Color.Red;
        public float 线宽 = 2;
        #endregion

        #region 设置雷达显示范围
        /// <summary>
        /// 雷达x轴最大值,默认90
        /// </summary>
        float x_max = 90;

        /// <summary>
        /// 雷达y轴最大值,默认90
        /// </summary>
        float y_max = 90;
        #endregion

        #endregion


        public 雷达慕()
        {
            InitializeComponent();
             string home = Application.StartupPath;
            this.BackgroundImage= Image.FromFile( @"D:\project\winform\newUI\WindowsFormsApp1\WindowsFormsApp1\leida.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

        }

        private void 雷达慕_Paint(object sender, PaintEventArgs e)
        {
        }

        private void 雷达慕_Load(object sender, EventArgs e)
        {
        }

        private void 雷达慕_Click(object sender, EventArgs e)
        {

            Graphics g = this.CreateGraphics();

            float dpiX = this.Size.Width / 2;
            float dpiY = this.Size.Height / 2;
            if(satellite == null)
            {
                satellite = new Satellite(name, line1, line2, width);
            }

            位置计算 位置计算1 = new 位置计算(satellite.名字, satellite.line1, satellite.line2);
            卫星位置[] 卫星位置 = 位置计算1.计算预计轨迹(x, y, z, 开始时间.AddHours(-时区), 结束时间.AddHours(-时区), 1);

            PointF[] points = new PointF[卫星位置.Length];


            for (int i = 0; i < 卫星位置.Length; i++)
            {
                points[i].X = calculateAZ_EL_XY(true, 卫星位置[i].AZ, 卫星位置[i].El) * dpiX / x_max + dpiX;
                points[i].Y = -(calculateAZ_EL_XY(false, 卫星位置[i].AZ, 卫星位置[i].El) * dpiY / y_max) + dpiY;
            }

            Pen pen = new Pen(this.线颜色, this.线宽);

            ArrayList ay = 分段(points);
            if (ay.Count == 0)//不需要分段
            {
                g.DrawCurve(pen, points);
            }
            else
            {
                int 上次结尾 = 0;
                for (int i = 0; i < ay.Count; i++)
                {
                    int 点数 = (int)ay[i] - 上次结尾;
                    PointF[] points_1 = new PointF[点数];
                    for (int j = 0; j < 点数; j++)
                    {
                        points_1[j] = points[上次结尾 + j];
                    }
                    上次结尾 = 上次结尾 + 点数;
                    try
                    {
                        g.DrawCurve(pen, points_1);
                    }
                    catch
                    {
                    }
                }

            }
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
    }
}
