﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TestAppCarousel.ViewModels;

namespace TestAppCarousel.Models
{
    public class Word : BaseViewModel
    {
        public string Chapter { get; set; }
        public int Shloka { get; set; }
        public int Line { get; set; }
        public string WordText { get; set; }
        public List<string> WordTextList { get; set; } = new List<string>();
        private Color _hilightedWord = Color.White; 
        public Color HilightedWord
        {
            get { return _hilightedWord; }
            set { SetValue(ref _hilightedWord, value); }
        }
        public Word(string chapter, int shloka, int line, string wordtext)
        {
            Chapter = chapter;
            Shloka = shloka;
            Line = line;
            foreach (char c in wordtext)
            {
                WordTextList.Add(c.ToString());
            }
            WordText = wordtext;
        }
    }
}
