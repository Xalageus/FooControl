using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooControl.Settings
{
    public class HBInterval
    {
        public string display { get; set; }
        public int value { get; set; }

        public HBInterval(string display, int value)
        {
            this.display = display;
            this.value = value;
        }
    }
}
