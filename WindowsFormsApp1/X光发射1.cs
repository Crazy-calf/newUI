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
    public partial class X光发射1 : Form
    {
        public X光发射1()
        {
            InitializeComponent();
        }
        
        //定义委托
        public delegate void MyDelegate();
        //定义事件
        public event MyDelegate MyEvent;

        private void X光发射1_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyEvent();
        }
    }
}
