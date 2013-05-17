using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TreeProjectRebuild
{
    class RootView
    {
        int nSides;
        Point center;
        double radius;  //外围圆半径
        double initial_l;
        double deltaR;
        Color strokeColor, fillColor;
        Root rootModel;
        Ellipse outerEllipse;
        Ellipse innerEllipse;
        List<RootView> rootViewList;

        static Canvas canvas;

        internal RootView(double centerX, double centerY, double r, Root root) {  //此root即为调用Show()时作为最外层圆的root
            center = new Point(centerX, centerY);
            radius = r;
            rootModel = root;
            strokeColor = Colors.Black;
            fillColor = Colors.Gray;
            rootViewList = new List<RootView>();
        }

        internal void SetX(double x){
            center.X = x;
        }

        internal void SetY(double y) {
            center.Y = y;
        }

        internal void Show(GeneralLevel level){
            //canvas.Children.Clear();
            initial_l = 4;

            outerEllipse = MakeElli(center.X, center.Y, radius, strokeColor, fillColor);
            outerEllipse.Tapped += outerEllipse_Tapped;
            outerEllipse.PointerEntered += outerEllipse_PointerEntered;
            canvas.Children.Add(outerEllipse);

            canvas.Children.Add(innerEllipse = MakeElli(center.X, center.Y, 3, Colors.Black, Colors.LightGray));

            if (rootModel.Level >= level)
                return;

            double[] pos = new double[2];
            for (int i = 0; i < rootModel.branchlist.Count; ++i) {
                double radians = 2 * Math.PI * i / rootModel.branchlist.Count;
                Branch b = rootModel.branchlist[i];
                double l = initial_l;
                double r = 6;
                for (int j = 0; j < b.rootlist.Count; ++j) {
                    pos[0] = center.X + l * Math.Cos(radians);
                    pos[1] = center.Y + l * Math.Sin(radians);

                    RootView subRoot = new RootView(pos[0], pos[1], r, b.rootlist[j]);
                    subRoot.Show(level);
                    rootViewList.Add(subRoot);

                    r += 0.4 * r;
                    l += 0.8 * r;
                    radians += (Math.PI / 180) * 20;
                    pos[0] = center.X + l * Math.Cos(radians);
                    pos[1] = center.Y + l * Math.Sin(radians);
                }
            }
        }

        void outerEllipse_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            canvas.Children.Remove(outerEllipse);
            canvas.Children.Remove(innerEllipse);
            this.Show(GeneralLevel.Year_Province);
        }

        void outerEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Ellipse elli = sender as Ellipse;
            elli.Stroke = new SolidColorBrush(Colors.Red);
        }

        Ellipse MakeElli(double centerX, double centerY, double r, Color strokeColor, Color fillColor)
        {
            Ellipse e = new Ellipse
            {
                Width = r * 2,
                Height = r * 2,
                Stroke = new SolidColorBrush(strokeColor),
                Fill = new SolidColorBrush(fillColor)
            };
            Canvas.SetLeft(e, centerX - r);
            Canvas.SetTop(e, centerY - r);
            return e;
        }

        internal static void SetCanvas(Canvas canvas){
            RootView.canvas = canvas;
        }
    }
}