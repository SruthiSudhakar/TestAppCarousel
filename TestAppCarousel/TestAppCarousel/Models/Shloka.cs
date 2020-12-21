using System;
using System.Collections.Generic;
using System.Text;

namespace TestAppCarousel.Models
{
    public class Shloka
    {
        public int ShlokaNum { get; set; } = 0;
        public List<Line> ShlokaLines { get; set; } = new List<Line>();
        public Shloka(int sn)
        {
            ShlokaNum = sn;
        }
    }
}
