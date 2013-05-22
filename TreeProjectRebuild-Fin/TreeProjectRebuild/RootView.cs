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
        //double initial_l;
        //double deltaR;
        Color strokeColor, fillColor;
        internal Root rootModel;
        internal Ellipse outerEllipse;
        //internal Ellipse innerEllipse;
        //List<RootView> rootViewList;
        internal List<BranchView> branchViewList;

        internal bool Detected { get; private set; }

        internal static Canvas canvas;
        //static double amp = 1;
        static List<RootView> relation;
        internal static BranchView branchRelation;
        internal static MainPage mp;
        internal static Stack<BranchView> trace = new Stack<BranchView>();
        internal static List<Ellipse> branchSelected = new List<Ellipse>();

        internal Line line = new Line();
        internal Line line1 = new Line();

        internal RootView(Root r) {
            rootModel = r;
            branchViewList = new List<BranchView>();
            Detected = false;
            //initial_l = 10;
        }

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

        internal void update() { 
            
        }

        static double GetFactor(RootView rv){
            switch (rv.rootModel.Level) { 
                case GeneralLevel.Genesis:
                    return 1;
                case GeneralLevel.Decade_Country:
                    return 1 - rv.rootModel.GDP / 350.0;
                case GeneralLevel.Year_Province:
                    return 1 - rv.rootModel.GDP / 6.2;
                case GeneralLevel.Season_City:
                    return 1 - rv.rootModel.GDP / 0.8;
                default:
                    return -1;
            }    
        }

        static internal Color getColor(RootView rv) {
            int index;
            if (rv.rootModel.Level == GeneralLevel.Decade_Country)
            {
                index = rv.rootModel.data.country;
            }
            else if (rv.rootModel.Level == GeneralLevel.Year_Province)
            {
                index = rv.rootModel.data.province;
            }
            else if (rv.rootModel.Level == GeneralLevel.Season_City)
            {
                index = rv.rootModel.data.city;
            }
            else
                index = 100;    //赋值100是为了保证之后的switch不被提前捕获

            switch (index)
            {
                case 0:
                    return MainPage.ChangeColor(Colors.Red, GetFactor(rv));
                case 1:
                    return MainPage.ChangeColor(Colors.Blue, GetFactor(rv));
                case 2:
                    return MainPage.ChangeColor(Colors.Violet, GetFactor(rv));
                case 3:
                    return MainPage.ChangeColor(Colors.Brown, GetFactor(rv));
                case 4:
                    return MainPage.ChangeColor(Colors.Green, GetFactor(rv));
                case 5:
                    return MainPage.ChangeColor(Colors.Yellow, GetFactor(rv));
                case 6:
                    return MainPage.ChangeColor(Colors.SteelBlue, GetFactor(rv));
                case 7:
                    return MainPage.ChangeColor(Colors.Salmon, GetFactor(rv));
                default:
                    return MainPage.ChangeColor(Colors.Black, GetFactor(rv));
            }
        }

        /*internal void Show(GeneralLevel level){
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
                //double l = initial_l;                                               //l为innerEllipse与pos间直线距离
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
        }*/

        internal void show_notTangent(GeneralLevel level, double rIncrease, double lIncrease, double init_l, double init_r) {   //圆之间不相切 
            //SetEllipse(outerEllipse, center.X, center.Y, radius, Colors.Black, Colors.Gray);
            canvas.Children.Add(outerEllipse);
            //SetEllipse(innerEllipse, center.X, center.Y, 3, Colors.Black, Colors.White);
            //canvas.Children.Add(innerEllipse);

            if (rootModel.Level >= level)
                return;

            double[] pos = new double[2];
            double r = 0, l = 0;
            for (int i = 0; i < branchViewList.Count; ++i) {
                double radians = 2 * Math.PI * i / branchViewList.Count;
                BranchView bv = branchViewList[i];

                l = init_l;
                r = init_r;
                for(int j = 0; j < bv.Count; ++j){
                    RootView rv = bv.rootViewList[j];

                    pos[0] = center.X + l * Math.Cos(radians);
                    pos[1] = center.Y + l * Math.Sin(radians);

                    Color colour = getColor(rv);

                    SetElli(pos[0], pos[1], r, colour, colour, rv);
                    rv.show_notTangent(level, rIncrease, lIncrease, l, r);

                    r += rIncrease * r;
                    l += lIncrease * r;

                    double theta = 20.0 / 180 * Math.PI;
                    radians += theta;                 
                }
            }
        }

        internal static void SetBranchSelectedEllipse() {
            RootView rv = mp.currentViews.rootViewList[0];

            int c = rv.branchViewList[0].rootViewList.Count;    //获得一条branch的root的数量

            foreach (Ellipse e in branchSelected)
                canvas.Children.Remove(e);

            branchSelected.Clear();

            Point pos = new Point();
            for (int i = 0; i < rv.branchViewList.Count; ++i) {

                Point p = new Point(rv.branchViewList[i].rootViewList[c - 1].center.X - rv.center.X,
                                    rv.branchViewList[i].rootViewList[c - 1].center.Y - rv.center.Y);//当作向量来用
                //double l = MainPage.Dist(p, rv.center);

                pos.X = rv.center.X + 1.4 * p.X;
                pos.Y = rv.center.Y + 1.4 * p.Y;

                Ellipse e = new Ellipse { Width = 60, Height = 60, Stroke = new SolidColorBrush(Colors.Blue), Fill = new SolidColorBrush(Colors.Gray) };
                e.Tapped += selectBranch;
                Canvas.SetLeft(e, pos.X);
                Canvas.SetTop(e, pos.Y);
                canvas.Children.Add(e);
                branchSelected.Add(e);
            }
        }

        private static void selectBranch(object sender, TappedRoutedEventArgs e)
        {
            trace.Push(mp.currentViews);

            int index = branchSelected.IndexOf(sender as Ellipse);

            RootView main = mp.currentViews.rootViewList[0];    //应该只有一个

            branchRelation = main.branchViewList[index];

            mp.currentViews = new BranchView(branchRelation.rootViewList);

            canvas.Children.Clear();

            branchRelation.ShowBranch(main.rootModel.Level+1);
        }

        internal static void getDis(double length, double[] data, ref double[] radius, ref double[] dis)
        {
            double sumData = 0;
            for (int i = 0; i < data.Length; ++i)
            {
                sumData += data[i];
            }

            for (int i = 0; i < radius.Length; ++i)
            {
                radius[i] = 0;
                radius[i] = length * (data[i] / sumData);
            }

            for (int i = 0; i < radius.Length; i++)
            {
                dis[i] = 0;
                for (int j = 0; j < i; ++j)
                {
                    dis[i] += radius[j];
                }
            }
        }

        internal static void DataCanvas(BranchView bv)    //BranchView bv = branchViewLists[foucus];
        {
            double[] data = new double[bv.Count];
            for (int i = 0; i < bv.Count; ++i)
            {
                //data[i] = 10 * (i + 1);
                data[i] = bv.rootViewList[i].rootModel.GDP + 50;
            }

            List<Ellipse> elip = new List<Ellipse>();
            List<Line> lines = new List<Line>();
            List<TextBlock> blocks = new List<TextBlock>();

            double length = 1440;    //
            double[] radius = new double[bv.Count];
            double[] dis = new double[bv.Count];
            getDis(length, data, ref radius, ref dis);

            for (int i = 0; i < data.Length; ++i)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Stroke = new SolidColorBrush(Colors.White);
                elip.Add(ellipse);
                ellipse.Width = radius[i];
                ellipse.Height = radius[i];

                Canvas.SetTop(ellipse, 300 - ellipse.Height / 2);   //300
                Canvas.SetLeft(ellipse, dis[i]);
                canvas.Children.Add(ellipse);

                TextBlock text = new TextBlock();
                text.Foreground = new SolidColorBrush(Colors.Blue);
                text.Width = 50;
                text.Height = 50;
                text.Text = data[i].ToString();
                Canvas.SetTop(text, 300 - ellipse.Height / 2);
                Canvas.SetLeft(text, dis[i] + radius[i] / 2);
                canvas.Children.Add(text);
            }

            for (int i = 0; i < (elip.Count - 1); ++i)
            {
                Line line = new Line();
                lines.Add(line);
                line.Stroke = new SolidColorBrush(Colors.Yellow);
                line.X1 = dis[i] + radius[i] / 2;
                line.Y1 = 300 - elip[i].Height / 2;
                line.X2 = dis[i + 1] + radius[i + 1] / 2;
                line.Y2 = 300 - elip[i + 1].Height / 2;

                canvas.Children.Add(line);
            }

            if (data.Length >= 2)
            {
                Line yLine = new Line();
                yLine.X1 = dis[0];
                yLine.Y1 = 300;
                yLine.X2 = dis[elip.Count - 1] + radius[elip.Count - 1] + 100;
                yLine.Y2 = 300;

                yLine.Stroke = new SolidColorBrush(Colors.Red);
                canvas.Children.Add(yLine);

                Line xLine = new Line();
                xLine.X1 = radius[0] / 2;
                xLine.Y1 = 0;
                xLine.X2 = radius[0] / 2;
                xLine.Y2 = 300;

                xLine.Stroke = new SolidColorBrush(Colors.Red);
                canvas.Children.Add(xLine);
            }
        }

        internal void BuildBranchView(){
            outerEllipse = new Ellipse();

            if (rootModel.Level == GeneralLevel.Decade_Country) {
                outerEllipse.RightTapped += outerEllipse_RightTapped;
            }

            outerEllipse.Tapped += outerEllipse_Tapped;                             //单击事件
            outerEllipse.PointerEntered += outerEllipse_PointerEntered;             //鼠标移入事件
            outerEllipse.PointerExited += outerEllipse_PointerExited;               //鼠标移出事件

            //innerEllipse = new Ellipse();

            if (rootModel.Level == GeneralLevel.Season_City)
                return;

            foreach (Branch branch in rootModel.branchlist) {
                BranchView bv = new BranchView(branch);
                branchViewList.Add(bv);
                foreach (Root r in branch.rootlist) {
                    RootView subRoot = new RootView(r);
                    bv.addRootView(subRoot);
                    subRoot.BuildBranchView();
                }
            }
        }
        
        void outerEllipse_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (mp.point.Count < 2) {
                Point tempPoint = new Point();
                tempPoint.X = Canvas.GetLeft(this.outerEllipse) + this.outerEllipse.Width / 2;
                tempPoint.Y = Canvas.GetTop(this.outerEllipse) + this.outerEllipse.Height / 2;

                mp.point.Add(tempPoint);

                if (mp.point.Count == 1) {
                    mp.ras0 = this.outerEllipse.Width;
                }
            }
            if (mp.point.Count == 2) {
                if (mp.ras0.Equals(this.outerEllipse.Width)) {
                    relationCanvas(mp.point);
                }
                mp.point.Clear();
            }
        }

        void relationCanvas(List<Point> tempPoint)
        {
            line.Stroke = new SolidColorBrush(Colors.Red);

            line1.Stroke = new SolidColorBrush(Colors.Blue);
            double multiIndex = 2;

            line.X1 = tempPoint[0].X;
            line.Y1 = tempPoint[0].Y;
            line.X2 = (tempPoint[1].X - tempPoint[0].X) * (multiIndex / (multiIndex + 1)) + tempPoint[0].X;
            line.Y2 = (tempPoint[1].Y - tempPoint[0].Y) * (multiIndex / (multiIndex + 1)) + tempPoint[0].Y;


            line1.X1 = (tempPoint[1].X - tempPoint[0].X) * (multiIndex / (multiIndex + 1)) + tempPoint[0].X;
            line1.Y1 = (tempPoint[1].Y - tempPoint[0].Y) * (multiIndex / (multiIndex + 1)) + tempPoint[0].Y;
            line1.X2 = tempPoint[1].X;
            line1.Y2 = tempPoint[1].Y;

            if (!canvas.Children.Contains(line))
            {
                canvas.Children.Add(line);
                canvas.Children.Add(line1);
            }
        }

        void outerEllipse_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;

            ellipse.Fill = new SolidColorBrush(RootView.getColor(this));
            Detected = false;
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
            //Ellipse elli = sender as Ellipse;
            //elli.Stroke = new SolidColorBrush(Colors.Red);

            BranchView bv = mp.currentViews;
            RootView rv = bv.rootViewList[0];

            if (rootModel.Level < GeneralLevel.Season_City && rootModel.Level > rv.rootModel.Level)
            {
                trace.Push(bv);
                canvas.Children.Clear();
                SetElli((canvas.ActualWidth - 400) / 2, canvas.ActualHeight / 2, (canvas.ActualHeight - 100) / 2, Colors.Black, Colors.Gray, this);
                if(rootModel.Level == GeneralLevel.Decade_Country)
                    show_notTangent(GeneralLevel.Year_Province, 0.3, 1.2, 10, 5);
                else if (rootModel.Level == GeneralLevel.Year_Province)
                    show_notTangent(GeneralLevel.Season_City, 0.6, 1.3, 30, 20);
                setCurrentView(new BranchView(this));
                canvas.Children.Add(mp.returnButton);

                SetBranchSelectedEllipse();
            }
            /*else if (rootModel.Level == mp.currentView.rootModel.Level && trace.Count > 0) {
                canvas.Children.Clear();
                RootView parent = trace.Pop();
                SetElli(canvas.ActualWidth / 2, canvas.ActualHeight / 2, canvas.ActualHeight / 2, Colors.Black, Colors.Gray, parent);
                if(rootModel.Level == GeneralLevel.Year_Province)
                    parent.show_notTangent(GeneralLevel.Year_Province, 0.4, 1.3, 10, 5);
                else if(rootModel.Level == GeneralLevel.Decade_Country)
                    parent.show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                setCurrentView(parent);
            }*/
        }

        internal static void setCurrentView(BranchView bv)
        {
            if (mp.currentViews != null && mp.currentViews.Count == 1)//(mp.currentViews != null)
            {
                RootView currentView = mp.currentViews.rootViewList[0];
                currentView.outerEllipse.PointerMoved -= mp.outerEllipse_PointerMoved;
                currentView.outerEllipse.PointerExited -= mp.outerEllipse_PointerExited;
                currentView.outerEllipse.Tapped -= mp.outerEllipse_Tapped;
            }

            mp.currentViews = bv;

            if (mp.currentViews.Count == 1)
            {
                RootView currentView = mp.currentViews.rootViewList[0];
                currentView.outerEllipse.PointerMoved += mp.outerEllipse_PointerMoved;
                currentView.outerEllipse.PointerExited += mp.outerEllipse_PointerExited;
                currentView.outerEllipse.Tapped += mp.outerEllipse_Tapped;
            }
        }

        static internal void SetElli(double centerX, double centerY, double r, Color strokeColor, Color fillColor, RootView rv)
        {
            rv.center = new Point(centerX, centerY);
            rv.radius = r;
            rv.outerEllipse.Width = r * 2;
            rv.outerEllipse.Height = r * 2;
            rv.outerEllipse.Stroke = new SolidColorBrush(strokeColor);
            rv.outerEllipse.Fill = new SolidColorBrush(fillColor);
            Canvas.SetLeft(rv.outerEllipse, centerX - r);
            Canvas.SetTop(rv.outerEllipse, centerY - r);

            //rv.innerEllipse.Width = 6;
            //rv.innerEllipse.Height = 6;
            //rv.innerEllipse.Stroke = new SolidColorBrush(Colors.Black);
            //rv.innerEllipse.Fill = new SolidColorBrush(Colors.White);
            //Canvas.SetLeft(rv.innerEllipse, centerX - 3);
            //Canvas.SetTop(rv.innerEllipse, centerY - 3);
        }

        internal static void SetCanvas(Canvas canvas){
            RootView.canvas = canvas;
        }
    }
}