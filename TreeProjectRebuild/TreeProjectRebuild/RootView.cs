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
        internal Point center;
        internal double radius;  //外围圆半径
        double initial_l;
        //double deltaR;
        Color strokeColor, fillColor;
        Root rootModel;
        internal Ellipse outerEllipse;
        internal Ellipse innerEllipse;
        //List<RootView> rootViewList;
        internal List<BranchView> branchViewList;

        internal bool Detected { get; private set; }

        static Canvas canvas;
        static double amp = 1;
        static List<RootView> relation;

        internal RootView(double centerX, double centerY, double r, Root root) {  //此root即为调用Show()时作为最外层圆的root
            center = new Point(centerX, centerY);
            radius = r;
            rootModel = root;
            strokeColor = Colors.Black;
            fillColor = Colors.Gray;
            //rootViewList = new List<RootView>();
            branchViewList = new List<BranchView>();

            Detected = false;
            outerEllipse = MakeElli(center.X, center.Y, radius + (amp - 1) * 3, strokeColor, fillColor);
        }

        internal void SetX(double x){
            center.X = x;
        }

        internal void SetY(double y) {
            center.Y = y;
        }

        internal void Show(GeneralLevel level){
            //canvas.Children.Clear();
            double delta = (amp - 1) * 3;
            initial_l = 25 + delta * 8;

            //outerEllipse = MakeElli(center.X, center.Y, radius + delta, strokeColor, fillColor);
            outerEllipse.Tapped += outerEllipse_Tapped;
            outerEllipse.PointerEntered += outerEllipse_PointerEntered;
            outerEllipse.PointerExited += outerEllipse_PointerExited;
            canvas.Children.Add(outerEllipse);

            canvas.Children.Add(innerEllipse = MakeElli(center.X, center.Y, 3 + delta/3, Colors.Black, Colors.LightGray));

            if (rootModel.Level >= level)
                return;

            double[] pos = new double[2];
            for (int i = 0; i < rootModel.branchlist.Count; ++i) {
                double radians = 2 * Math.PI * i / rootModel.branchlist.Count;
                Branch b = rootModel.branchlist[i];
                double l = initial_l;
                double r = 5 + delta*1.3;
                branchViewList.Add(new BranchView(b));
                for (int j = 0; j < b.rootlist.Count; ++j) {
                    pos[0] = center.X + l * Math.Cos(radians);
                    pos[1] = center.Y + l * Math.Sin(radians);

                    RootView subRoot = new RootView(pos[0], pos[1], r, b.rootlist[j]);
                    subRoot.Show(level);
                    //rootViewList.Add(subRoot);
                    branchViewList[i].addRootView(subRoot);

                    double tempL = 1.35*l;
                    double theta = (Math.PI / 180) * 20;
                    r = LowOfCosines(l, tempL, theta) - r;
                    l = tempL + delta*1.6;
                    radians += theta;
                    pos[0] = center.X + l * Math.Cos(radians);
                    pos[1] = center.Y + l * Math.Sin(radians);
                }
            }
        }

        void outerEllipse_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.Fill = new SolidColorBrush(Colors.Gray);
        }

        internal static double LowOfCosines(double a, double b, double radian) {
            return Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(radian));
        }

        internal static void setAmp(double amp) {
            RootView.amp = amp;
        }

        internal static void setRelation(List<RootView> relation) {
            RootView.relation = relation;
        }

        void outerEllipse_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.Fill = new SolidColorBrush(Colors.LightGray);
            Detected = true;
        }

        void outerEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Ellipse elli = sender as Ellipse;
            elli.Stroke = new SolidColorBrush(Colors.Red);
            Detected = false;
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