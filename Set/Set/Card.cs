using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set
{
    public class Card
    {
        public int Num { get; set; }
        public string Color { get; set; }
        public string Pattern { get; set; }
        public string Shape { get; set; }

        public string GetCardText()
        {
            return Num + "-" + Pattern + "-" + Shape + "-" + Color;
        }

        public string GetCardImage()
        {
            return Pattern[0].ToString() + Color[0].ToString() + Shape[0].ToString() + Num.ToString() + ".png";
        }


    }



}
