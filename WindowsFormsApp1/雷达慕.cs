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
        public DateTime 当前时刻;
        public int 时区;
        #endregion

        #region 画线属性
        public Color 线颜色 = Color.Red;
        public float 线宽 = 2;

        public Color 点颜色 = Color.Yellow;
        public float 点大小 = 4;

        Pen pen;
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


        //private PointF[] points;
        public PointF point;

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
        /// 坐标轴转换，原点在左上角x向右，y向左
        /// </summary>
        /// <param name="AZ"></param>
        /// <param name="EL"></param>
        /// <returns></returns>
        private PointF calculate(double AZ, double EL)
        {
            double len = (90 - EL)/90;

            Double X;
            Double Y;

            #region 俯仰坐标转换雷达坐标系
            X = len * Math.Cos(Math.PI / 2 - AZ * Math.PI / 180) * this.Size.Width / 2;
            Y = len * Math.Sin(Math.PI / 2 - AZ * Math.PI / 180) * this.Size.Height / 2;
            #endregion

            #region 雷达坐标系转换控件坐标
            X += this.Size.Width / 2;
            Y = -Y + this.Size.Height / 2;
            #endregion

            return new PointF((float)X, (float)Y);
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

            float temp = pps[0].Y;
            for (int i = 0; i < pps.Length; i++)
            {
                if (Math.Abs(pps[i].Y - temp) > 100)//跳变需要分段
                {
                    ay.Add(i);
                }
                temp = pps[i].Y;
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



        public Dictionary<string,object> Draw(bool isClear = false)
        {
            #region 错误信息
            Exception a = new Exception();
            
            Dictionary<string, object> res = new Dictionary<string, object>();
            res.Add("isError", false);
            res.Add("error", "");
            #endregion
            try
            {
                if (!isClear)
                {
                    #region 卫星轨迹计算
                    位置计算 位置计算1 = new 位置计算(satellite.名字, satellite.line1, satellite.line2);
                    卫星位置[] 卫星位置 = 位置计算1.计算预计轨迹(x, y, z, 开始时间, 结束时间, 1);
                    卫星位置 卫星当前位置 = 位置计算1.计算实时位置(x, y, z, 当前时刻);
                    #endregion

                    //将卫星轨迹点系列转化为 线条的集合。
                    //主要依据：俯仰>=0
                    List<List<PointF>> pointFs = Conversion(卫星位置);

                    point = new PointF();

                    #region 创建画布
                    Image imgBack = this.BackgroundImage;
                    Bitmap destBitmap = new Bitmap(imgBack, Convert.ToInt32(this.Size.Width), Convert.ToInt32(this.Size.Height));//先绘制雷达图。
                    Graphics g = Graphics.FromImage(destBitmap);
                    #endregion

                    //画笔
                    pen = new Pen(线颜色, 线宽);

                    //雷达画线
                    destBitmap = DrawLine(destBitmap, g, pointFs);

                    //卫星画点
                    destBitmap = DrawPoint(destBitmap, g, 卫星当前位置);

                    //将画好的destBitmap设置为picBox的背景（解决改变form界面大小时，重绘背景会覆盖CreateGraphics()画线的bug）
                    pictureBox1.BackgroundImage = destBitmap;
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    Image imgBack = this.BackgroundImage;
                    Bitmap destBitmap = new Bitmap(imgBack, Convert.ToInt32(this.Size.Width), Convert.ToInt32(this.Size.Height));//先绘制雷达图。
                    pictureBox1.BackgroundImage = destBitmap;
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                }

            }
            catch (Exception ex)
            {
                res["isError"] = true;
                res["error"] = ex.Message;
            }
            return res;
        }

        private List<List<PointF>> Conversion(卫星位置[] 卫星位置)
        {
            List<List<PointF>> pointFs = new List<List<PointF>>();
            int lineIndex = 0;

            bool breakMark = true;
            for (int i = 0; i < 卫星位置.Length; i++)
            {
                if (卫星位置[i].El >= 0)
                {
                    if (breakMark)
                    {
                        pointFs.Add(new List<PointF>());
                    }
                    pointFs[lineIndex].Add(calculate(卫星位置[i].AZ, 卫星位置[i].El));
                    breakMark = false;
                }
                else
                {
                    if (!breakMark)
                    {
                        lineIndex++;
                        breakMark = true;
                    }
                }
            }
            return pointFs;
        }

        private Bitmap DrawLine(Bitmap destBitmap, Graphics g, List<List<PointF>> pointsList)
        {
            //再destBitmap上画图
            foreach (var item in pointsList)
            {
                g.DrawCurve(pen, item.ToArray());
            }
            return destBitmap;
        }


        private Bitmap DrawPoint(Bitmap destBitmap, Graphics g, 卫星位置 卫星当前位置)
        {
            if (当前时刻 != null)
            {
                point = calculate(卫星当前位置.AZ, 卫星当前位置.El);
                g = Graphics.FromImage(destBitmap);

                Image icon = Image.FromFile(@".\dot.jpg");
                //卫星图标坐标(左上角)
                float iconx = point.X - icon.Size.Width / 2;
                float icony = point.Y - icon.Size.Height / 2;

                g.DrawImage(icon, iconx, icony);

                Font font = new Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                float strX = point.X + icon.Size.Width / 2;
                float strY = point.Y;
                StringFormat 格式 = new StringFormat();
                格式.Alignment = StringAlignment.Near; //右对齐
                格式.LineAlignment = StringAlignment.Center;

                float len = g.MeasureString(satellite.名字, font).Width;

                //做判断，字符串是否超出右边界，若超出，默认显示在左边
                if (strX + len > pictureBox1.Size.Width)
                {
                    strX = iconx;
                    格式.Alignment = StringAlignment.Far;//左对齐
                }

                g.DrawString(satellite.名字, font, Brushes.Yellow, new PointF(strX, strY), 格式);
            }

            return destBitmap;
        }

        private void 雷达慕_SizeChanged(object sender, EventArgs e)
        {
            Draw();
        }
    }
}



