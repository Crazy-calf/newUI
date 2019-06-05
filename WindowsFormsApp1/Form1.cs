using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        #region 声明
        AutoSizeFormClass asc = new AutoSizeFormClass();//分辨率适配

        public bool isOpenWindow_天线 = false;
        public bool isOpenWindow_X光发射1 = false;
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


    }
}
