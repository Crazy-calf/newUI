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
    public partial class 天线 : Form
    {
        public 天线()
        {
            InitializeComponent();
        }

        //定义委托
        public delegate void MyDelegate();
        //定义事件
        public event MyDelegate MyEvent;


        private void 天线_Load(object sender, EventArgs e)
        {

        }

        private void 天线_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyEvent();
        }

    }
}
