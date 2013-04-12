using System;
using System.Diagnostics;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

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
        internal Ellipse innerEllipse;
        //List<RootView> rootViewList;
        internal List<BranchView> branchViewList;
        internal List<BranchView> tempBranchViewList;
       // internal  tempRoot;

        internal bool Detected { get; private set; }

        static Canvas canvas;
        //static double amp = 1;
        static List<RootView> relation;
        internal static MainPage mp;
        internal static Stack<RootView> trace = new Stack<RootView>();
        public String status = "null";

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

        internal void show_notTangent(GeneralLevel level, double rIncrease, double lIncrease, double init_l, double init_r) {   //圆之间不相切 
            //SetEllipse(outerEllipse, center.X, center.Y, radius, Colors.Black, Colors.Gray);
            canvas.Children.Add(outerEllipse);
            //SetEllipse(innerEllipse, center.X, center.Y, 3, Colors.Black, Colors.White);
            canvas.Children.Add(innerEllipse);

            if (rootModel.Level >= level)
                return;

            double[] pos = new double[2];
            for (int i = 0; i < branchViewList.Count; ++i) {
                double radians = 2 * Math.PI * i / branchViewList.Count;
                BranchView bv = branchViewList[i];
                double r = init_r;
                double l = init_l;
                for(int j = 0; j < bv.Count; ++j){
                    RootView rv = bv.rootViewList[j];
                    pos[0] = center.X + l * Math.Cos(radians);
                    pos[1] = center.Y + l * Math.Sin(radians);

                    SetElli(pos[0], pos[1], r, Colors.Black, Colors.Gray, rv);
                    //SetElli(pos[0], pos[1], 3, Colors.Black, Colors.White, rv.innerEllipse);
                    rv.show_notTangent(level, rIncrease, lIncrease, l, r);

                    r += rIncrease * r;
                    l += lIncrease * r;
                    double theta = 20.0 / 180 * Math.PI;
                    radians += theta;
                }
            }
        }

        

        internal Point getPos(int index)
        {  
            double rIncrease = 0.3;
            double lIncrease = 1.2;
            double init_l = 10;
            double init_r = 5;
            double radians = 2 * Math.PI * index / branchViewList.Count;
            double[] pos = { 0, 0};
            BranchView bv = branchViewList[index];
            double r = init_r;
            double l = init_l;
            Point result = new Point();
            for (int j = 0; j < bv.Count; ++j)
            {
                RootView rv = bv.rootViewList[j];
                pos[0] = center.X + l * Math.Cos(radians);
                pos[1] = center.Y + l * Math.Sin(radians);
                r += rIncrease * r;
                l += lIncrease * r;
                double theta = 20.0 / 180 * Math.PI;
                radians += theta;
            }
            result.X = pos[0];
            result.Y = pos[1];
            return result;
        }

        internal void showImage()
        {
            for (int i = 0; i < rootModel.branchlist.Count; ++i)
            {
                Image image = new Image();
                image.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(
                                 new Uri("ms-appx:///Assets/btn.jpg", UriKind.RelativeOrAbsolute));

                if (i == 0)
                {
                    Canvas.SetLeft(image, getPos(i).X - 170);//683-220);
                    Canvas.SetTop(image, getPos(i).Y - 40);//661 + 30);
                    image.PointerPressed += image0_PointerPressed;

                }
                else if (i == 1)
                {
                    Canvas.SetLeft(image, getPos(i).X - 220);//466 - 220);
                    Canvas.SetTop(image, getPos(i).Y - 80);//556);
                    image.PointerPressed += image1_PointerPressed;

                }
                else if (i == 2)
                {
                    Canvas.SetLeft(image, getPos(i).X + 100 );//412 - 220);
                    Canvas.SetTop(image, getPos(i).Y - 100);//322);
                    image.PointerPressed += image2_PointerPressed;

                }
                else if (i == 3)
                {
                    Canvas.SetLeft(image, getPos(i).X + 100);//562 - 220);
                    Canvas.SetTop(image, getPos(i).Y - 50);//134 - 30);
                    image.PointerPressed += image3_PointerPressed;
                }
                else if (i == 4)
                {
                    Canvas.SetLeft(image, getPos(i).X + 120);//803 + 120);
                    Canvas.SetTop(image, getPos(i).Y + 30);//134 - 30);
                    image.PointerPressed += image4_PointerPressed;
                }
                else if (i == 5)
                {
                    Canvas.SetLeft(image, getPos(i).X );//953 + 120);
                    Canvas.SetTop(image, getPos(i).Y + 80);//322);
                    image.PointerPressed += image5_PointerPressed;
                }
                else if (i == 6)
                {
                    Canvas.SetLeft(image, getPos(i).X - 190);//899 + 120);
                    Canvas.SetTop(image, getPos(i).Y + 70);//556);
                    image.PointerPressed += image6_PointerPressed;
                }
               // images.Add(image);
                // image.PointerPressed
                image.PointerEntered += image_PointerEntered;
                image.PointerExited += image_PointerExited;
                //image.PointerPressed += image0_PointerPressed;

                canvas.Children.Add(image);
            }
        }

        void image0_PointerPressed(object sender, RoutedEventArgs e)
        {
            if (status == "null")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status != "reclick") 
            {
                canvas.Children.Clear();
                mp.setBox();
                dataCanvas(0, branchViewList);
                status = "reclick";
            }
        }

        void image1_PointerPressed(object sender, RoutedEventArgs e)
        {
            if (status == "null")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status != "reclick")
            {
                canvas.Children.Clear();
                mp.setBox();
                dataCanvas(1, branchViewList);
                status = "reclick";
            }
        }

        void image2_PointerPressed(object sender, RoutedEventArgs e)
        {
            if (status == "null")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status != "reclick")
            {
                canvas.Children.Clear();
                mp.setBox();
                dataCanvas(2, branchViewList);
                status = "reclick";
            }
        }

        void image3_PointerPressed(object sender, RoutedEventArgs e)
        {
            if (status == "null")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status != "reclick")
            {
                canvas.Children.Clear();
                mp.setBox();
                dataCanvas(3, branchViewList);
                status = "reclick";
            }
        }

        void image4_PointerPressed(object sender, RoutedEventArgs e)
        {
            if (status == "null")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status != "reclick")
            {
                canvas.Children.Clear();
                mp.setBox();
                dataCanvas(4, branchViewList);
                status = "reclick";
            }
        }

        void image5_PointerPressed(object sender, RoutedEventArgs e)
        {
            if (status == "null")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status != "reclick")
            {
                canvas.Children.Clear();
                mp.setBox();
                dataCanvas(5, branchViewList);
                status = "reclick";
            }
        }

        void image6_PointerPressed(object sender, RoutedEventArgs e)
        {
            if (status == "null")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status != "reclick")//status == "click" && 
            {
                canvas.Children.Clear();
                mp.setBox();
                dataCanvas(6,branchViewList);
                status = "reclick";
            }
        }
        void getDis(double length, double[] data, ref double[] radius, ref double[] dis)
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

        void setControl(GeneralLevel level,RootView rootView) 
        {
           // if (level == GeneralLevel.Decade_Country) 
            //{
                int length = rootView.branchViewList.Count;
                int size = rootView.branchViewList[0].rootViewList.Count;
                
                for (int i = 0; i < length ; ++i) 
                {
                    Ellipse tempE = new Ellipse();
                    tempE.Stroke = new SolidColorBrush(Colors.Blue);
                    tempE.Fill = new SolidColorBrush(Colors.Blue);
                    tempE.Width = 30;
                    tempE.Height = 30;
                    double left = Canvas.GetLeft( rootView.branchViewList[i].rootViewList[size - 1].outerEllipse);
                    double top = Canvas.GetTop(rootView.branchViewList[i].rootViewList[size - 1].outerEllipse);
                    if (level == GeneralLevel.Decade_Country)
                    {
                        if (i == 0)
                        {
                            Canvas.SetLeft(tempE, left - 60);
                            Canvas.SetTop(tempE, top + 30);
                        }
                        else if (i == 1)
                        {
                            Canvas.SetLeft(tempE, left - 30);
                            Canvas.SetTop(tempE, top - 30);
                        }
                        else if (i == 2)
                        {
                            Canvas.SetLeft(tempE, left + 70);
                            Canvas.SetTop(tempE, top - 40);
                        }
                        else if (i == 3)
                        {
                            Canvas.SetLeft(tempE, left + 120);
                            Canvas.SetTop(tempE, top - 50);
                        }
                        else if (i == 4)
                        {
                            Canvas.SetLeft(tempE, left + 150);
                            Canvas.SetTop(tempE, top + 40);
                        }
                        else if (i == 5)
                        {
                            Canvas.SetLeft(tempE, left + 130);
                            Canvas.SetTop(tempE, top + 100);
                        }
                        else if (i == 6)
                        {
                            Canvas.SetLeft(tempE, left + 50);
                            Canvas.SetTop(tempE, top + 130);
                        }
                        else if (i == 7)
                        {
                            Canvas.SetLeft(tempE, left );
                            Canvas.SetTop(tempE, top + 130);
                        }
                        tempE.PointerPressed += tempE_PointerPressed;
                    }
                    else if (level == GeneralLevel.Year_Province) 
                    {
                        if (i == 0)
                        {
                            Canvas.SetLeft(tempE, left+100);
                            Canvas.SetTop(tempE, top+190);
                        }
                        else if (i == 1)
                        {
                            Canvas.SetLeft(tempE, left + 40);
                            Canvas.SetTop(tempE, top + 190);
                        }
                        else if (i == 2)
                        {
                            Canvas.SetLeft(tempE, left - 50);
                            Canvas.SetTop(tempE, top + 80);
                        }
                        else if (i == 3)
                        {
                            Canvas.SetLeft(tempE, left - 50);
                            Canvas.SetTop(tempE, top);
                        }
                        else if (i == 4)
                        {
                            Canvas.SetLeft(tempE, left + 150);
                            Canvas.SetTop(tempE, top - 30);
                        }
                        else if (i == 5)
                        {
                            Canvas.SetLeft(tempE, left + 200);
                            Canvas.SetTop(tempE, top + 70);
                        }
                        tempE.PointerPressed += tempEY_PointerPressed;
                    }
                   
                    tempBranchViewList = rootView.branchViewList;
                    
                    canvas.Children.Add(tempE);
                }
         //   }
        }

        void tempE_PointerPressed(object sender, RoutedEventArgs e)
        {
            status = "year";
            canvas.Children.Clear();
            mp.setBox();
            dataCanvas(0, tempBranchViewList);
        }

        void tempEY_PointerPressed(object sender, RoutedEventArgs e)
        {
            status = "city";
            canvas.Children.Clear();
            mp.setBox();
            dataCanvas(0, tempBranchViewList);
        }

        public void dataCanvas(int foucus, List<BranchView> branchViewLists)
        {
            BranchView bv = branchViewLists[foucus];
            double[] data = new double[bv.Count];
            for (int i = 0; i < bv.Count; ++i) 
            {
                data[i] = 10 * (i+1);
            }

            List<Ellipse> elip = new List<Ellipse>();
            List<Line> lines = new List<Line>();
            List<TextBlock> blocks = new List<TextBlock>();

            double length = 600;
            double[] radius = new double[bv.Count];
            double[] dis = new double[bv.Count];
            getDis(length, data, ref radius, ref dis);

            for (int i = 0; i < data.Length; ++i)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Stroke = new SolidColorBrush(Colors.Black);
                elip.Add(ellipse);
                ellipse.Width = radius[i];
                ellipse.Height = radius[i];

                Canvas.SetTop(ellipse, 300 - ellipse.Height / 2);
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
                line.Stroke = new SolidColorBrush(Colors.Red);
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
            Button btn = new Button();
            btn.Content = "Back";
            btn.Width = 100;
            btn.Height = 100;
            btn.Foreground = new SolidColorBrush(Colors.White);
            btn.Background = new SolidColorBrush(Colors.Black);

            btn.Tapped += btn_Tapped;
            Canvas.SetLeft(btn, 1200);
            Canvas.SetTop(btn, 100);
            canvas.Children.Add(btn);     

        }

        void btn_Tapped(object sender, RoutedEventArgs e) 
        {
            if (status == "reclick")
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }else if(status == "year")
            {
                canvas.Children.Clear();
                mp.setBox();
                show_notTangent(GeneralLevel.Year_Province, 0.3, 1.2, 10, 5);
                setControl(GeneralLevel.Decade_Country, mp.currentView);
                status = "reclick";
            }
            else if (status == "city") 
            {
                canvas.Children.Clear();
                mp.setBox();
                show_notTangent(GeneralLevel.Season_City, 0.6, 1.3, 30, 20);
                setControl(GeneralLevel.Year_Province, mp.currentView);
                status = "year";
            }
            else if (status == "event") 
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                status = "click";
            }
            else if (status == "enter") 
            {
                canvas.Children.Clear();
                mp.setBox();
                if (rootModel.Level == GeneralLevel.Decade_Country) 
                {
                    showImage();
                    //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                    show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                    status = "reclick";
                }
                else if (rootModel.Level == GeneralLevel.Year_Province)
                {
                    show_notTangent(GeneralLevel.Year_Province, 0.3, 1.2, 10, 5);
                    setControl(GeneralLevel.Decade_Country, mp.currentView);
                    status = "reclick";
                }
                else if (rootModel.Level == GeneralLevel.Season_City) 
                {
                    show_notTangent(GeneralLevel.Season_City, 0.6, 1.3, 30, 20);
                    setControl(GeneralLevel.Year_Province, mp.currentView);
                    status = "reclick";
                }
            }
        }

        void image_PointerEntered(object sender, RoutedEventArgs e)
        {
            if (status == "null") 
            {
                //show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
            }
            
        }

        void image_PointerExited(object sender, RoutedEventArgs e)
        {
            if (status == "null") 
            {
                canvas.Children.Clear();
                mp.setBox();
                showImage();
            }
            
        }

        internal void BuildBranchView(){
            outerEllipse = new Ellipse();
            outerEllipse.Tapped += outerEllipse_Tapped;                             //单击事件
            outerEllipse.PointerEntered += outerEllipse_PointerEntered;             //鼠标移入事件
            outerEllipse.PointerExited += outerEllipse_PointerExited;               //鼠标移出事件

            innerEllipse = new Ellipse();

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

        void outerEllipse_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.Fill = new SolidColorBrush(Colors.Gray);
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

            if (rootModel.Level < GeneralLevel.Season_City && rootModel.Level > mp.currentView.rootModel.Level)
            {
                trace.Push(mp.currentView);
                canvas.Children.Clear();
                mp.setBox();
                SetElli((canvas.ActualWidth - 400) / 2, canvas.ActualHeight / 2, (canvas.ActualHeight - 100) / 2, Colors.Black, Colors.Gray, this);
                if (rootModel.Level == GeneralLevel.Decade_Country)
                    show_notTangent(GeneralLevel.Year_Province, 0.3, 1.2, 10, 5);
                else if (rootModel.Level == GeneralLevel.Year_Province)
                    show_notTangent(GeneralLevel.Season_City, 0.6, 1.3, 30, 20);
                setCurrentView(this);
                setControl(rootModel.Level,this);

            }
            /*else if (rootModel.Level == mp.currentView.rootModel.Level && trace.Count > 0) {
                canvas.Children.Clear();
                mp.setBox();
                RootView parent = trace.Pop();
                SetElli((canvas.ActualWidth - 400) / 2, canvas.ActualHeight / 2, (canvas.ActualHeight - 100) / 2, Colors.Black, Colors.Gray, parent);
                if(rootModel.Level == GeneralLevel.Year_Province)
                    parent.show_notTangent(GeneralLevel.Year_Province, 0.3, 1.2, 10, 5);
                else if (rootModel.Level == GeneralLevel.Decade_Country)
                {
                    //canvas.Children.Clear();
                    parent.showImage();
                    //parent.show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
                    parent.show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                }
                setCurrentView(parent);
                if (rootModel.Level == GeneralLevel.Year_Province)
                {
                    setControl(rootModel.Level, this);
                }
            }*/
        }

        internal static void setCurrentView(RootView rv) {
            if (mp.currentView != null)
            {
                mp.currentView.outerEllipse.PointerMoved -= mp.outerEllipse_PointerMoved;
                mp.currentView.outerEllipse.PointerExited -= mp.outerEllipse_PointerExited;
            }
            mp.currentView = rv;

            mp.currentView.outerEllipse.PointerMoved += mp.outerEllipse_PointerMoved;
            mp.currentView.outerEllipse.PointerExited += mp.outerEllipse_PointerExited;
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

            rv.innerEllipse.Width = 6;
            rv.innerEllipse.Height = 6;
            rv.innerEllipse.Stroke = new SolidColorBrush(Colors.Black);
            rv.innerEllipse.Fill = new SolidColorBrush(Colors.White);
            Canvas.SetLeft(rv.innerEllipse, centerX - 3);
            Canvas.SetTop(rv.innerEllipse, centerY - 3);
        }

        internal static void SetCanvas(Canvas canvas){
            RootView.canvas = canvas;
        }
    }
}