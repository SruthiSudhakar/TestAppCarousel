using System;
using System.Collections.Generic;
using System.Text;

namespace TestAppCarousel.Models
{
    public class Chapter
    {
        public string ChapterName { get; set; } = "";
        public List<Shloka> ChapterShlokas { get; set; } = new List<Shloka>();
        public Chapter()
        {

        }
        public Chapter(string cn, List<Shloka> cs)
        {
            ChapterName = cn;
            ChapterShlokas = cs;
        }
    }
}
