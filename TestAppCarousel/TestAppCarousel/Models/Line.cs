using System;
using System.Collections.Generic;
using System.Text;

namespace TestAppCarousel.Models
{
    public class Line
    {
        public int LineNumber { get; set; } = 0;
        public List<Word> LineWords { get; set; } = new List<Word>();
        public Line(int ln)
        {
            LineNumber = ln;
            LineWords = new List<Word>();
        }
        public Line(int ln, List<Word> lw)
        {
            LineNumber = ln;
            LineWords = lw;
        }
    }
}
