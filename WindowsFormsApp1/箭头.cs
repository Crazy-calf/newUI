using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class 箭头 : UserControl
    {
        public 箭头()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.FromArgb(0, 0, 0, 0);

            this.DoubleBuffered = true;
        }
        private bool 横向a = true;
        private string 文字a = "";
        private Color 线颜色a = Color.Red;
        private Color temp = Color.Red;
        private float 线宽a = 2;
        private Font 字体a = new Font("宋体", 9);
        private 箭头方向 箭头设置a = 箭头方向.正向;
        private bool 闪烁a = false;
        private float 偏移a = 0;
        public enum 箭头方向
        {
            正向 = 0,
            反向 = 1,
            双向 = 2,
            无 = 3,
        }
        /// <summary>
        /// 箭头横竖方向
        /// </summary>
        /// <remarks></remarks>
        /// <value>m/s</value>
        [DescriptionAttribute("控件中箭头横竖方向"), Category("AAA")]
        public bool 横向
        {
            get
            {
                return 横向a;
            }
            set
            {
                横向a = value;
                偏移a = 0;//换方向要清除偏移a
            }
        }
        /// <summary>
        /// 箭头闪烁
        /// </summary>
        /// <remarks></remarks>
        /// <value>m/s</value>
        [DescriptionAttribute("箭头闪烁"), Category("AAA")]
        public bool 闪烁
        {
            get
            {
                return 闪烁a;
            }
            set
            {
                闪烁a = value;
                temp = 线颜色a;
                timer1.Enabled = 闪烁a;
            }
        }
        /// <summary>
        /// 箭头正反方向
        /// </summary>
        /// <remarks></remarks>
        /// <value>m/s</value>
        [DescriptionAttribute("控件中箭头正反方向"), Category("AAA")]
        public 箭头方向 箭头设置
        {
            get
            {
                return 箭头设置a;
            }
            set
            {
                箭头设置a = value;
            }
        }
        /// <summary>
        /// 文字
        /// </summary>
        /// <remarks></remarks>
        /// <value>m/s</value>
        [DescriptionAttribute("显示值"), Category("AAA")]
        public string 文字
        {
            get
            {
                return 文字a;
            }
            set
            {
                文字a = value;
            }
        }
        /// <summary>
        /// Color
        /// </summary>
        /// <remarks></remarks>
        /// <value>m/s</value>
        [DescriptionAttribute("线颜色"), Category("AAA")]
        public Color 线颜色
        {
            get
            {
                return 线颜色a;
            }
            set
            {
                线颜色a = value;
            }
        }
        /// <summary>
        /// 线宽
        /// </summary>
        /// <remarks></remarks>
        /// <value>m/s</value>
        [DescriptionAttribute("线宽,INT"), Category("AAA")]
        public float 线宽
        {
            get
            {
                return 线宽a;
            }
            set
            {
                线宽a = value;
            }
        }
        /// <summary>
        /// FONT
        /// </summary>
        /// <remarks></remarks>
        /// <value>m/s</value>
        [DescriptionAttribute("FONT"), Category("AAA")]
        public Font 字体
        {
            get
            {
                return 字体a;
            }
            set
            {
                字体a = value;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            Pen pen1 = new Pen(线颜色a, 线宽a);
            Pen pen2 = new Pen(Color.Black, 2);
            if (闪烁a)
            {
                pen1 = new Pen(temp, 线宽a);
            }

            if (箭头设置a == 箭头方向.正向)
            {
                pen1.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 8, true);
            }
            else if (箭头设置a == 箭头方向.反向)
            {
                pen1.CustomStartCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 8, true);
            }
            else if (箭头设置a == 箭头方向.双向)
            {
                pen1.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 8, true);
                pen1.CustomStartCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 8, true);
            }
            else if (箭头设置a == 箭头方向.无)
            {

            }
            if (横向a)
            {
                e.Graphics.DrawLine(pen1, 0, this.Height / 2, this.Width, this.Height / 2);
                e.Graphics.DrawString(文字a, 字体a, pen2.Brush, this.Width / 2 - 字体a.Size + 偏移a, 0);
            }
            else
            {
                e.Graphics.DrawLine(pen1, Width / 2, 0, this.Width / 2, Height);
                e.Graphics.DrawString(文字a, 字体a, pen2.Brush, /*this.Width / 2 - 字体a.Size*/0, this.Height / 2 - 字体a.Size + 偏移a);
            }

            base.OnPaint(e);
            pen1.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Color color1 = Color.FromArgb(temp.G, temp.B, temp.R);
            temp = color1;
            if (横向a)
            {
                偏移a = 偏移a + this.Width / 5;
                if (this.Width / 2 + 偏移a > this.Width)
                {
                    偏移a = 字体a.Size - this.Width / 2;
                }
            }
            else
            {
                偏移a = 偏移a + this.Height / 5;
                if (this.Height / 2 + 偏移a > this.Height)
                {
                    偏移a = 字体a.Size - this.Height / 2;
                }
            }
            this.Refresh();
        }
    }
}
