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
        //static double amp = 1;
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
            //outerEllipse = MakeElli(center.X, center.Y, radius + (amp - 1) * 3, strokeColor, fillColor);
            
        }

        internal void SetX(double x){
            center.X = x;
        }

        internal void SetY(double y) {
            center.Y = y;
        }

        internal void SetInitial_l(double l){
            initial_l = l;
        }

        internal void update() { 
            
        }

        internal void Show(GeneralLevel level){
            //canvas.Children.Clear();
            //double delta = (amp - 1) * 3;
            //initial_l = 25 + delta * 8;

            //outerEllipse = MakeElli(center.X, center.Y, radius, strokeColor, fillColor);
            //outerEllipse.Tapped += outerEllipse_Tapped;                             //单击事件
            //outerEllipse.PointerEntered += outerEllipse_PointerEntered;             //鼠标移入事件
            //outerEllipse.PointerExited += outerEllipse_PointerExited;               //鼠标移出事件
            canvas.Children.Add(outerEllipse);                                      //将外围圆添加至canvas

            canvas.Children.Add(innerEllipse);  //将中心圆添加至canvas

            if (rootModel.Level >= level)                                           //决定是否显示细节
                return;

            double[] pos = new double[2];                                           //子root的坐标
            for (int i = 0; i < rootModel.branchlist.Count; ++i) {
                double radians = 2 * Math.PI * i / rootModel.branchlist.Count;      //弧度
                Branch b = rootModel.branchlist[i];                                 //获取root model的一条branch
                double l = initial_l;                                               //l为innerEllipse与pos间直线距离
                //double r = 5 + delta*1.3;
                double r = 5;
                branchViewList.Add(new BranchView(b));                              //建立一个branchView并将
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
                    //l = tempL + delta*1.6;
                    l = tempL;
                    radians += theta;
                    pos[0] = center.X + l * Math.Cos(radians);
                    pos[1] = center.Y + l * Math.Sin(radians);
                }
            }
        }

        static void SetEllipse(Ellipse e, double centerX, double centerY, double radius, Color strokeColor, Color fillColor) {
            Canvas.SetLeft(e, centerX);
            Canvas.SetTop(e, centerY);
            e.Height = radius;
            e.Width = radius;
            e.Stroke = new SolidColorBrush(strokeColor);
            e.Fill = new SolidColorBrush(fillColor);
        }

        internal void show_notTangent(GeneralLevel level) {   //圆之间不相切 
            SetEllipse(outerEllipse, center.X, center.Y, radius, Colors.Black, Colors.Gray);
            canvas.Children.Add(outerEllipse);
            SetEllipse(innerEllipse, center.X, center.Y, 3, Colors.Black, Colors.White);
            canvas.Children.Add(innerEllipse);

            if (rootModel.Level >= level)
                return;

            double[] pos = new double[2];
            for (int i = 0; i < branchViewList.Count; ++i) { 
                
            }
        }

        internal void BuildBranchView(){
            outerEllipse = MakeElli(center.X, center.Y, radius, strokeColor, fillColor);
            outerEllipse.Tapped += outerEllipse_Tapped;                             //单击事件
            outerEllipse.PointerEntered += outerEllipse_PointerEntered;             //鼠标移入事件
            outerEllipse.PointerExited += outerEllipse_PointerExited;               //鼠标移出事件

            innerEllipse = MakeElli(center.X, center.Y, 3, Colors.Black, Colors.LightGray);

            foreach (Branch branch in rootModel.branchlist) {
                BranchView bv = new BranchView(branch);
                branchViewList.Add(bv);
                foreach (Root r in branch.rootlist) {
                    RootView subRoot = new RootView(10, 10, 10, r);
                    bv.addRootView(subRoot);
                    subRoot.BuildBranchView();
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

        /*internal static void setAmp(double amp) {
            RootView.amp = amp;
        }*/

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