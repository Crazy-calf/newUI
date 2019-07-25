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
using System.Threading;

namespace RadarScreen
{
    public partial class 雷达幕 : UserControl
    {
        #region 参数
        private Satellite _satellite = new Satellite();
        Thread 线程1;

        private bool IsStart = false;
        private bool IsStartTimer = false;

        /// <summary>
        /// 开启计时器之后的时间间隔(second)
        /// </summary>
        [Description("计时间隔(模拟计时，计时速率:TimeLag秒/秒)")]
        public int TimeLag = 1;

        #region 地面站
        /// <summary>
        /// 地面站经度,默认104
        /// </summary>
        public double 地面站经度 { get; set; } = 104;

        /// <summary>
        /// 地面站维度,默认30
        /// </summary>
        public double 地面站纬度 { get; set; } = 30;

        /// <summary>
        /// 地面站高度,默认0
        /// </summary>
        public double 地面站高度 { get; set; } = 0;
        #endregion

        #region 天线方位俯仰
        /// <summary>
        /// 是否显示天线位置
        /// </summary>
        bool IsShowANT = false;

        /// <summary>
        /// 天线方位坐标
        /// </summary>
        double ANT_AZ;

        /// <summary>
        /// 天线俯仰坐标
        /// </summary>
        double ANT_El;
        #endregion

        #region 起止时间及时区
        public DateTime 开始时间;
        public DateTime 结束时间;
        private DateTime _当前时刻 = new DateTime();
        public int 时区 = 8;
        #endregion

        #region 画线属性
        public Color 线颜色 { get; set; } = Color.Red ;
        public float 线宽 { get; set; } = 2;

        public Color 点颜色 { get; set; } = Color.Yellow;
        public float 点大小 { get; set; } = 4;

        Pen pen;
        #endregion

        #region 设置雷达显示范围
        /// <summary>
        /// 雷达幕x轴最大值,默认90
        /// </summary>
        [Description("雷达幕x轴最大值")]
        public float x_max { get; set; } = 90;

        /// <summary>
        /// 雷达幕y轴最大值,默认90
        /// </summary>
        [Description("雷达幕y轴最大值")]
        public float y_max { get; set; } = 90;
        #endregion

        #region 功能开关
        public void Start()
        {
            this.IsStart = true;
            Draw();
        }

        public void StartTimer()
        {
            if (!this.IsStartTimer)
            {
                this.IsStartTimer = true;
                线程1 = new Thread(new ThreadStart(更新时间));
                线程1.IsBackground = true;
                线程1.Start();
            }
        }

        public void close()
        {
            this.satellite = null;
            Draw();
            if (this.IsStartTimer)
            {
                this.IsStartTimer = false;
                this.IsStart = false;
                this.线程1.Abort();
            }
        }
        #endregion
        //private PointF[] points;
        public PointF point;

        #region 控件属性值监听
        public event PropertyChangedEventHandler SatellitePropertyChanged;
        public event PropertyChangedEventHandler 当前时刻PropertyChanged;

        public Satellite satellite
        {
            get => _satellite;
            set
            {
                _satellite = value;
                if (IsStart)
                {
                    SatellitePropertyChanged(this, new PropertyChangedEventArgs("Satellite"));
                }
            }
        }

        [Description("当前时间以传入的时间为准，随后的计时以此为起点，模拟计时")]
        public DateTime 当前时刻
        {
            get => this._当前时刻;
            set 
            {
                this._当前时刻 = value;
                if(IsStart)
                {
                    当前时刻PropertyChanged(this, new PropertyChangedEventArgs("当前时刻"));
                }
            }
        }

        #endregion

        #endregion

        public 雷达幕()
        {
            InitializeComponent();
        }
        //INotifyPropertyChanged inpc;

        private void 雷达幕_Load(object sender, EventArgs e)
        {
            //初始化雷达幕背景
            #region picBox背景设置
            this.雷达幕picBox.Size = this.Size;
            this.雷达幕picBox.BackgroundImage = this.panel1.BackgroundImage;
            this.雷达幕picBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            #endregion

            this.SatellitePropertyChanged += satellite_PropertyChanged;
            this.当前时刻PropertyChanged += 当前时刻_PropertyChanged;

        }

        void 更新时间()
        {
            while (true)
            {
                //this.当前时刻 = new DateTime();
                this.当前时刻 = this.当前时刻.AddSeconds(1 * TimeLag);
                Thread.Sleep(1000);
            }
        }

        private void 当前时刻_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Draw();
        }

        #region 配置监听
        private void satellite_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Draw();
        }

        #endregion



        #region method
        #region private
        /// <summary>
        /// 通过俯仰，进行picBox上的坐标轴转换，原点在左上角x向右，y向左
        /// </summary>
        /// <param name="AZ">方位</param>
        /// <param name="EL">俯仰</param>
        /// <returns></returns>
        private PointF Calculate(double AZ, double EL)
        {
            double len = (90 - EL) / 90;

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
        /// 画卫星图标
        /// </summary>
        /// <param name="destBitmap"></param>
        /// <param name="g"></param>
        /// <param name="卫星当前位置"></param>
        /// <returns></returns>
        private Bitmap DrawPoint(Bitmap destBitmap, Graphics g, 卫星位置 卫星当前位置)
        {
            if (当前时刻 != null)
            {


                point = Calculate(卫星当前位置.AZ, 卫星当前位置.El);
                g = Graphics.FromImage(destBitmap);

                //Image icon = Image.FromFile(@".\dot.jpg");
                Image icon = this.panel_dot.BackgroundImage;
                //卫星图标坐标(左上角)
                float iconx = point.X - icon.Size.Width / 2;
                float icony = point.Y - icon.Size.Height / 2;

                g.DrawImage(icon, iconx, icony);

                float strX = point.X + icon.Size.Width / 2;
                float strY = point.Y;

                #region 卫星名称格式
                Font font = new Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                StringFormat 格式 = new StringFormat();
                格式.Alignment = StringAlignment.Near; //右对齐
                格式.LineAlignment = StringAlignment.Center;

                float len = g.MeasureString(satellite.名字, font).Width;

                //做判断，字符串是否超出右边界，若超出，默认显示在左边
                if (strX + len > 雷达幕picBox.Size.Width)
                {
                    strX = iconx;
                    格式.Alignment = StringAlignment.Far;//左对齐
                }
                #endregion

                g.DrawString(satellite.名字, font, Brushes.Yellow, new PointF(strX, strY), 格式);
            }

            return destBitmap;
        }

        /// <summary>
        /// 卫星画线
        /// </summary>
        /// <param name="destBitmap"></param>
        /// <param name="g"></param>
        /// <param name="pointsList"></param>
        /// <returns></returns>
        private Bitmap DrawLine(Bitmap destBitmap, Graphics g, List<List<PointF>> pointsList)
        {
            //再destBitmap上画图
            foreach (var item in pointsList)
            {
                g.DrawCurve(pen, item.ToArray());
            }
            return destBitmap;
        }

        private Bitmap DrawANT(Bitmap destBitmap, Graphics g, PointF 天线坐标)
        {
            g = Graphics.FromImage(destBitmap);

            Image ANT = this.panel2.BackgroundImage;
                //Image.FromFile(@".\pic\天线图标.png");
            //卫星图标坐标(左上角)
            float ANTx = point.X - ANT.Size.Width / 2;
            float ANTy = point.Y - ANT.Size.Height / 2;

            g.DrawImage(ANT, ANTx, ANTy);

            return destBitmap;
        }
        /// <summary>
        /// 通过卫星位置计算雷达幕坐标(含分段)
        /// </summary>
        /// <param name="卫星位置"></param>
        /// <returns></returns>
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
                    pointFs[lineIndex].Add(Calculate(卫星位置[i].AZ, 卫星位置[i].El));
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

        #endregion

        #region public
        /// <summary>
        /// 通过方位俯仰，获取天线坐标
        /// </summary>
        /// <param name="AZ">方位</param>
        /// <param name="EL">俯仰</param>
        /// <returns></returns>
        public PointF 天线坐标(double AZ, double EL)
        {
            return Calculate(AZ, EL);
        }

        public Dictionary<string, object> Draw()
        {
            #region 错误信息
            Exception a = new Exception();

            Dictionary<string, object> res = new Dictionary<string, object>();
            res.Add("isError", false);
            res.Add("error", "");
            #endregion
            try
            {
                if (satellite is null)
                {
                    Image imgBack = this.panel1.BackgroundImage;
                    Bitmap destBitmap = new Bitmap(imgBack, Convert.ToInt32(this.Size.Width), Convert.ToInt32(this.Size.Height));//先绘制雷达图。
                    雷达幕picBox.BackgroundImage = destBitmap;
                    雷达幕picBox.BackgroundImageLayout = ImageLayout.Stretch;
                    res["isError"] = false;
                    res["error"] = "卫星参数";
                }
                else
                {
                    #region 卫星轨迹计算
                    位置计算 位置计算1 = new 位置计算(satellite.名字, satellite.line1, satellite.line2);
                    卫星位置[] 卫星位置 = 位置计算1.计算预计轨迹(地面站经度, 地面站纬度, 地面站高度, 开始时间, 结束时间, 1);
                    卫星位置 卫星当前位置 = 位置计算1.计算实时位置(地面站经度, 地面站纬度, 地面站高度, 当前时刻);
                    #endregion

                    //将卫星轨迹点系列转化为 线条的集合。
                    //主要依据：俯仰>=0
                    List<List<PointF>> pointFs = Conversion(卫星位置);

                    point = new PointF();

                    #region 创建画布
                    Image imgBack = this.panel1.BackgroundImage;
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
                    雷达幕picBox.BackgroundImage = destBitmap;
                    雷达幕picBox.BackgroundImageLayout = ImageLayout.Stretch;

                    if (IsShowANT)
                    {
                        destBitmap = DrawANT(destBitmap, g, 天线坐标(ANT_AZ, ANT_El));
                        //destBitmap = DrawANT(destBitmap, g, 天线坐标(卫星当前位置.AZ, 卫星当前位置.El));
                    }
                }

            }
            catch (Exception ex)
            {
                res["isError"] = true;
                res["error"] = ex.Message;
            }
            return res;
        }
        #endregion

        #endregion

        private void 雷达幕_SizeChanged(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
