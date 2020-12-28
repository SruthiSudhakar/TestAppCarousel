using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TestAppCarousel.Models;
using TestAppCarousel.Services;
using TestAppCarousel.TouchTracking;
using TestAppCarousel.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestAppCarousel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentPage : ContentPage
    {
        Dictionary<Guid, Tuple<Dictionary<long, SKPath>, List<SKPath>>> dictOfPaths = new Dictionary<Guid, Tuple<Dictionary<long, SKPath>, List<SKPath>>>();
        List<SKCanvasView> mostRecentDrawings = new List<SKCanvasView>();
        int memoryLimit = 10;
        bool MistakeRegistred = false;
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue.WithAlpha(0x80),
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        public AssessmentPage()
        {
            BindingContext = new AssessmentViewModel(SessionConfiguration.numberOfStudentsInSession, SessionConfiguration.numberOfStudentsPerPage, SessionConfiguration.chapters, new PageService());
            InitializeComponent();
            MessagingCenter.Subscribe<AssessmentViewModel>(this, "UndoDrawing", (sender) =>
            {
                undoLastDrawing();
            });
            MessagingCenter.Subscribe<AssessmentViewModel>(this, "MistakeRegistered", (sender) =>
            {
                MistakeRegistred = true;
            });
            PrevPageButtonId.IsEnabled = false;
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (((BindingContext as AssessmentViewModel).studentsPerPage != SessionConfiguration.numberOfStudentsPerPage) || ((BindingContext as AssessmentViewModel).numOfStudents != SessionConfiguration.numberOfStudentsInSession))
            {
                BindingContext = new AssessmentViewModel(SessionConfiguration.numberOfStudentsInSession, SessionConfiguration.numberOfStudentsPerPage, SessionConfiguration.chapters, new PageService());
            }
        }
        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {


            SKCanvasView canvasView = (SKCanvasView)((Grid)sender).Children[0];
            if (!dictOfPaths.ContainsKey(canvasView.Id))
            {
                Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
                List<SKPath> completedPaths = new List<SKPath>();
                dictOfPaths.Add(canvasView.Id, new Tuple<Dictionary<long, SKPath>, List<SKPath>>(inProgressPaths, completedPaths));
            }
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!dictOfPaths[canvasView.Id].Item1.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(canvasView, args.Location));
                        dictOfPaths[canvasView.Id].Item1.Add(args.Id, path);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (dictOfPaths[canvasView.Id].Item1.ContainsKey(args.Id))
                    {
                        SKPath path = dictOfPaths[canvasView.Id].Item1[args.Id];
                        path.LineTo(ConvertToPixel(canvasView, args.Location));
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (MistakeRegistred == false)
                    {
                        undoLastDrawing();
                    }
                    MistakeRegistred = false;
                    if (dictOfPaths[canvasView.Id].Item1.ContainsKey(args.Id))
                    {
                        dictOfPaths[canvasView.Id].Item2.Add(dictOfPaths[canvasView.Id].Item1[args.Id]);
                        pushIntoMemory(canvasView);

                        SKPoint[] pathpoints = dictOfPaths[canvasView.Id].Item1[args.Id].Points;
                        float minx = (float)Int32.MaxValue;
                        float maxx = 0;
                        foreach (SKPoint point in pathpoints)
                        {
                            minx = Math.Min(minx, point.X);
                            maxx = Math.Max(maxx, point.X);
                        }
                        StackLayout sl = (StackLayout)((Grid)sender).Children[1];
                        double space = sl.Spacing;
                        double pos = 0;
                        int firstChar = 0;
                        int lastChar = sl.Children.Count - 1;
                        bool firstCharSet = false;

                        for (int i = 0; i < sl.Children.Count; i++)
                        {
                            if (i == 0)
                            {
                                pos += ((Label)(sl.Children[i])).Width + space / 2;
                            } else
                            {
                                pos += ((Label)(sl.Children[i])).Width + space;
                            }
                            if ((!firstCharSet) && (minx <= pos))
                            {
                                firstChar = i;
                                firstCharSet = true;
                            }
                            if ((firstCharSet) && (maxx <= pos))
                            {
                                lastChar = i;
                                break;
                            }
                        }

                        Word word = (Word)((Grid)sender).BindingContext;
                        StudentAndList studentAndList = (StudentAndList)(canvasView.BindingContext);

                        MessagingCenter.Send<AssessmentPage, Tuple<StudentAndList, Word, string>>(this, "MostRecentView", new Tuple<StudentAndList, Word, string>(studentAndList, word, word.WordText.Substring(firstChar, lastChar - firstChar + 1)));

                        dictOfPaths[canvasView.Id].Item1.Remove(args.Id);

                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (MistakeRegistred == false)
                    {
                        undoLastDrawing();
                    }
                    MistakeRegistred = false;
                    if (dictOfPaths[canvasView.Id].Item1.ContainsKey(args.Id))
                    {
                        dictOfPaths[canvasView.Id].Item1.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            System.Guid id = ((SKCanvasView)sender).Id;
            if (dictOfPaths.ContainsKey(id))
            {
                foreach (SKPath path in dictOfPaths[id].Item2)
                {
                    canvas.DrawPath(path, paint);
                }

                foreach (SKPath path in dictOfPaths[id].Item1.Values)
                {
                    canvas.DrawPath(path, paint);
                }
            }
        }

        SKPoint ConvertToPixel(SKCanvasView canvasView, Point pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                                (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }
        void pushIntoMemory(SKCanvasView id)
        {
            if (mostRecentDrawings.Count > memoryLimit)
            {
                mostRecentDrawings.RemoveAt(0);
            }
            mostRecentDrawings.Add(id);
        }

        void undoLastDrawing()
        {
            if (mostRecentDrawings.Count != 0)
            {
                SKCanvasView tempCV = mostRecentDrawings[mostRecentDrawings.Count - 1];
                if (dictOfPaths[tempCV.Id].Item2.Count != 0)
                {
                    dictOfPaths[tempCV.Id].Item2.RemoveAt(dictOfPaths[tempCV.Id].Item2.Count - 1);
                    tempCV.InvalidateSurface();
                }
                mostRecentDrawings.RemoveAt(mostRecentDrawings.Count - 1);
            }
        }

        private void NextPage_Clicked(object sender, EventArgs e)
        {
            CarouselViewID.Position += 1;
            PrevPageButtonId.IsEnabled = true;
            if (CarouselViewID.Position == (BindingContext as AssessmentViewModel).StudentGroups.Count - 1)
            {
                NextPageButtonId.IsEnabled = false;
            }
        }
        private void PrevPage_Clicked(object sender, EventArgs e)
        {
            CarouselViewID.Position -= 1;
            NextPageButtonId.IsEnabled = true;
            if (CarouselViewID.Position == 0)
            {
                PrevPageButtonId.IsEnabled = false;
            }
        }
    }

}