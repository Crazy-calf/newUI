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
using System.Drawing.Imaging;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class 雷达慕 : UserControl
    {
        //#region 参数
        //public string name = "HAIYANG 2A";
        //public string line1 = "1 37781U 11043A   19084.96741172 -.00000044  00000-0  11863-6 0  9998";
        //public string line2 = "2 37781  99.3212  95.3692 0001741  85.5656 274.5707 13.78718376383104";
        //public double width = 0.0;

        //public Satellite satellite;

        //#region 地面站
        ///// <summary>
        ///// 地面站经度
        ///// </summary>
        //public double x = 116.363;

        ///// <summary>
        ///// 地面站维度
        ///// </summary>
        //public double y = 40.059;

        ///// <summary>
        ///// 地面站高度
        ///// </summary>
        //public double z = 0.4;
        //#endregion

        //#region 起止时间及时区
        //public DateTime 开始时间 = Convert.ToDateTime("2019/5/19 17:28:33");
        //public DateTime 结束时间 = Convert.ToDateTime("2019/5/19 17:42:06");
        //public int 时区 = 8;
        //#endregion

        //#region 画线属性
        //public Color 线颜色 = Color.Red;
        //public float 线宽 = 2;
        //#endregion

        //#region 设置雷达显示范围
        ///// <summary>
        ///// 雷达x轴最大值,默认90
        ///// </summary>
        //public float x_max = 90;

        ///// <summary>
        ///// 雷达y轴最大值,默认90
        ///// </summary>
        //public float y_max = 90;
        //#endregion


        //public PointF[] points;

        //#endregion

        #region 参数
        public string name = "";
        public string line1 = "";
        public string line2 = "";
        public double width = 0.0;

        public Satellite satellite;

        #region 地面站
        /// <summary>
        /// 地面站经度
        /// </summary>
        public double x;

        /// <summary>
        /// 地面站维度
        /// </summary>
        public double y;

        /// <summary>
        /// 地面站高度
        /// </summary>
        public double z;
        #endregion

        #region 起止时间及时区
        public DateTime 开始时间;
        public DateTime 结束时间;
        public int 时区;
        #endregion

        #region 画线属性
        public Color 线颜色 = Color.Red;
        public float 线宽 = 2;
        #endregion

        #region 设置雷达显示范围
        /// <summary>
        /// 雷达x轴最大值,默认90
        /// </summary>
        public float x_max = 90;

        /// <summary>
        /// 雷达y轴最大值,默认90
        /// </summary>
        public float y_max = 90;
        #endregion


        public PointF[] points;

        #endregion
        public 雷达慕()
        {
            InitializeComponent();
            //
        }

        private void 雷达慕_Load(object sender, EventArgs e)
        {
            pictureBox1.Size = this.Size;
        }


        



        /// <summary>
        /// 雷达坐标系转换(中心为原点，y轴向上，x轴向右)
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

        /// <summary>
        /// 画线分段，暂不知道什么用，摘自双流11m
        /// </summary>
        /// <param name="pps"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            Dictionary<string, object> re = Draw();

        }


        public Dictionary<string,object> Draw()
        {
            #region 错误信息
            Exception a = new Exception();
            
            Dictionary<string, object> res = new Dictionary<string, object>();
            res.Add("isError", false);
            res.Add("error", "");
            #endregion
            try
            {
                if (satellite == null)
                {
                    satellite = new Satellite(name, line1, line2, width);
                }

                位置计算 位置计算1 = new 位置计算(satellite.名字, satellite.line1, satellite.line2);
                卫星位置[] 卫星位置 = 位置计算1.计算预计轨迹(x, y, z, 开始时间.AddHours(-时区), 结束时间.AddHours(-时区), 1);

                points = new PointF[卫星位置.Length];

                float dpiX = this.Size.Width / 2;
                float dpiY = this.Size.Height / 2;

                for (int i = 0; i < 卫星位置.Length; i++)
                {
                    points[i].X = calculateAZ_EL_XY(true, 卫星位置[i].AZ, 卫星位置[i].El) * dpiX / x_max + dpiX;
                    points[i].Y = -(calculateAZ_EL_XY(false, 卫星位置[i].AZ, 卫星位置[i].El) * dpiY / y_max) + dpiY;
                }


                Image imgBack = this.BackgroundImage;
                Bitmap destBitmap = new Bitmap(imgBack, Convert.ToInt32(this.Size.Width), Convert.ToInt32(this.Size.Height));//先绘制雷达图。

                Graphics g = Graphics.FromImage(destBitmap);

                Pen pen = new Pen(this.线颜色, this.线宽);

                ArrayList ay = 分段(points);

                //再destBitmap上画图
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

                //将画好的destBitmap设置为picBox的背景（解决改变form界面大小时，重绘背景会覆盖CreateGraphics()画线的bug）
                pictureBox1.BackgroundImage = destBitmap;
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
                res["isError"] = true;
                res["error"] = ex.Message;
            }

            return res;
        }
    }
}
