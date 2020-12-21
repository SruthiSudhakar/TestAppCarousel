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
            string readline;
            string DhyanaShlokaText = DhyanaShloka.DhyanaShlokaText;
            // Read the file and display it line by line.  
            List<string> textList = new List<string>(DhyanaShlokaText.Split('\n'));

            //StreamReader file = new StreamReader(filename);
            string chapterName = "Dhyana";
            Chapter chapter = new Chapter();
            int lineNumber = 1;
            int shlokaNumber = 1;
            bool newShloka = false;
            Shloka chapterShloka = new Shloka(shlokaNumber);
            for (int i=0; i < textList.Count; i++)// while ((readline = file.ReadLine()) != null)
            {
                if (textList[i].Equals("\r"))
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
                    shlokaLine.LineWords.Add(new Word(chapterName, shlokaNumber, lineNumber, temp[x]));
                }
                chapterShloka.ShlokaLines.Add(shlokaLine);
                lineNumber += 1;
                if (textList[i].Any(char.IsDigit))
                    newShloka = true;
            }

            //file.Close();
            return chapter;
        }
    }
}
