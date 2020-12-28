using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestAppCarousel.Models
{
    public static class ParseAndCreateChapterList
    {
        public static Chapter praseJson(string filename)
        {
            string DhyanaShlokaText = DhyanaShloka.DhyanaShlokaText;
            List<string> textList = new List<string>(DhyanaShlokaText.Split('\n'));
            string chapterName = "Dhyana";
            Chapter chapter = new Chapter();
            int lineNumber = 1;
            int shlokaNumber = 1;
            bool newShloka = false;
            Shloka chapterShloka = new Shloka(shlokaNumber);

            for (int i=0; i < textList.Count; i++)
            { 
                if (textList[i].Equals("\r") | textList[i].Equals(" "))
                    continue;
                if (newShloka)
                {
                    chapter.ChapterShlokas.Add(chapterShloka);
                    newShloka = false;
                    shlokaNumber++;
                    lineNumber = 1;
                    chapterShloka = new Shloka(shlokaNumber);
                }
                List<string> temp = new List<string>(textList[i].Split(' '));
                Line shlokaLine = new Line(lineNumber);
                for (int x = 0; x < temp.Count; x++)
                {
                    string str = temp[x];
                    shlokaLine.LineWords.Add(new Word(chapterName, shlokaNumber, lineNumber, str));
                }
                chapterShloka.ShlokaLines.Add(shlokaLine);
                lineNumber += 1;
                if (textList[i].Any(char.IsDigit))
                    newShloka = true;
            }
            return chapter;
        }
    }
}
