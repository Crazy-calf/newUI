using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class FormBaseDeleg
    {
        //定义委托
        public delegate void MyDelegate();
        //定义事件
        public event MyDelegate MyEvent;
    }
}
