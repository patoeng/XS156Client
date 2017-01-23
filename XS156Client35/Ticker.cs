using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace XS156Client35
{
    [ComVisible(false)]
    public class Ticker : Timer
    {
        private  Xs156Setting _setting;

        public Ticker()
        {
            _setting = new Xs156Setting();
            Interval = _setting.GetTickerInterval(5);
           
        }
    }
}
