using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TreeProjectRebuild
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Root world_root;
        RootView world_view;
        internal BranchView currentViews;
        List<RootView> relation;
        internal Button returnButton;

        internal List<Point> point = new List<Point>();
        internal double ras0 = 0;

        internal ComboBox box = new ComboBox();
        internal TextBox text = new TextBox();

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        internal static Color ChangeColor(Color color, double factor){
            double red = (double)color.R;
            double green = (double)color.G;
            double blue = (double)color.B;

            if (factor < 0)
            {
                factor = 1 + factor;
                red *= factor;
                green *= factor;
                blue *= factor;
            }
            else
            {
                red = (255 - red) * factor + red;
                green = (255 - green) * factor + green;
                blue = (255 - blue) * factor + blue;
            }

            if (red < 0) red = 0;

            if (red > 255) red = 255;

            if (green < 0) green = 0;

            if (green > 255) green = 255;

            if (blue < 0) blue = 0;

            if (blue > 255) blue = 255;

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        internal void outerEllipse_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (currentViews.Count == 1)
            {
                RootView currentView = currentViews.rootViewList[0];
                int index = -1;
                Point cursor = e.GetCurrentPoint(canvas).Position;
                double distance = Dist(cursor, currentView.center);
                BranchView bv = currentView.branchViewList[0];
                for (int i = 0; i < bv.Count; ++i)
                {
                    RootView rv = bv.getRootView(i);
                    double dist = Dist(rv.center, currentView.center);
                    if (distance > dist - rv.radius && distance < dist + rv.radius)
                    {   //距离
                        index = i;
                        break;
                    }
                }

                foreach (RootView rv in relation)
                {
                    rv.outerEllipse.Stroke = new SolidColorBrush(RootView.getColor(rv));
                    rv.outerEllipse.StrokeThickness = 1;
                }
                relation.Clear();

                if (index >= 0)
                {
                    foreach (BranchView branchView in currentView.branchViewList)
                    {
                        Ellipse elli = branchView.getRootView(index).outerEllipse;
                        elli.Stroke = new SolidColorBrush(ChangeColor((elli.Fill as SolidColorBrush).Color, -0.5));
                        elli.StrokeThickness = 3;
                        relation.Add(branchView.getRootView(index));
                    }
                }
            }
        }

        internal static double Dist(Point a, Point b) {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            world_root = new Root(GeneralLevel.Genesis, null);
            world_root.BuildBranch();

            relation = new List<RootView>();    //用来处理点击一圈圆
            RootView.setRelation(relation);

            world_view = new RootView(world_root);
            RootView.mp = this;
            RootView.SetCanvas(canvas);
            
            world_view.BuildBranchView();
            RootView.SetElli((canvas.ActualWidth-400) / 2, canvas.ActualHeight / 2, (canvas.ActualHeight-100) / 2, Colors.Black, Colors.Gray, world_view);
            world_view.show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
            RootView.setCurrentView(new BranchView(world_view));

            ComboBoxItem item0 = new ComboBoxItem();
            item0.Content = "时期A";
            box.Items.Add(item0);

            ComboBoxItem item1 = new ComboBoxItem();
            item1.Content = "时期B";
            box.Items.Add(item1);

            ComboBoxItem item2 = new ComboBoxItem();
            item2.Content = "时期C";
            box.Items.Add(item2);

            text.Text = "欢迎使用";
            text.Background = new SolidColorBrush(Colors.Wheat);

            returnButton = new Button{ Content = "Return", FontSize = 36 };
            Canvas.SetLeft(returnButton, 40);
            Canvas.SetTop(returnButton, 40);
            canvas.Children.Add(returnButton);
            returnButton.Tapped += returnButton_Tapped;

            RootView.SetBranchSelectedEllipse();
            
            world_view.outerEllipse.PointerMoved +=outerEllipse_PointerMoved;
            world_view.outerEllipse.PointerExited+=outerEllipse_PointerExited;
            world_view.outerEllipse.Tapped +=outerEllipse_Tapped;

            setBox();
        }

        void returnButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (RootView.trace.Count > 0) {
                canvas.Children.Clear();
                BranchView parent = RootView.trace.Pop();
                if (parent.Count == 1)
                {
                    RootView rv = parent.rootViewList[0];
                    RootView.SetElli((canvas.ActualWidth - 400) / 2, canvas.ActualHeight / 2, (canvas.ActualHeight - 100) / 2, Colors.Black, Colors.Gray, rv);

                    if (rv.rootModel.Level == GeneralLevel.Decade_Country)
                        rv.show_notTangent(GeneralLevel.Year_Province, 0.3, 1.2, 10, 5);
                    else if (rv.rootModel.Level == GeneralLevel.Genesis)
                        rv.show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                    else if (rv.rootModel.Level == GeneralLevel.Year_Province)
                        rv.show_notTangent(GeneralLevel.Season_City, 0.6, 1.3, 30, 20);

                    RootView.setCurrentView(new BranchView(rv));

                    canvas.Children.Add(returnButton);
                    RootView.SetBranchSelectedEllipse();

                    if (rv.rootModel.Level == GeneralLevel.Genesis) {
                        setBox();
                    }
                }
                else {
                    parent.ShowBranch(currentViews.rootViewList[0].rootModel.Level-1);
                    RootView.setCurrentView(parent);
                }
            }
        }

        internal void setBox()
        {
            box.Width = 300;
            box.Height = 100;
            Canvas.SetTop(box, 0);
            Canvas.SetLeft(box, canvas.ActualWidth - 300);
            box.SelectionChanged += box_SelectionChanged;
            canvas.Children.Add(box);
            text.Width = 300;
            text.Height = 300;

            Canvas.SetTop(text, canvas.ActualHeight - 300);
            Canvas.SetLeft(text, canvas.ActualWidth - 300);
            canvas.Children.Add(text);
        }

        internal void RandomJump() {
            RootView currentView = currentViews.rootViewList[0];
            Random rand = new Random();
            BranchView randBranch = currentView.branchViewList[rand.Next(0, currentView.branchViewList.Count)];
            RootView years_rv = randBranch.rootViewList[rand.Next(0, randBranch.rootViewList.Count)];
            BranchView target = years_rv.branchViewList[rand.Next(0, years_rv.branchViewList.Count)];

            RootView.trace.Push(currentViews);
            currentViews = target;

            canvas.Children.Clear();

            target.ShowBranch(GeneralLevel.Season_City);
        }

        private void box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (box.SelectedIndex != -1)
            {
                if (box.SelectedIndex == 0)
                {
                    text.Text = "时期A";
                }
                else if (box.SelectedIndex == 1)
                {
                    text.Text = "时期B";
                }
                else if (box.SelectedIndex == 2)
                {
                    text.Text = "时期C";
                }

                RandomJump();
            }
        }

        internal void outerEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (relation.Count > 0)
            {
                canvas.Children.Clear();

                RootView.trace.Push(new BranchView(currentViews.rootViewList[0]));

                double centerX = BranchView.CalcRadius(0, relation);
                double centerY = RootView.canvas.ActualHeight / 4 * 3;

                for(int i = 0; i<relation.Count; ++i)
                {
                    double r = BranchView.CalcRadius(i, relation);
                    RootView.SetElli(centerX, centerY, r, Colors.Black, Colors.Gray, relation[i]);
                    if (relation[i].rootModel.Level == GeneralLevel.Decade_Country)
                        relation[i].show_notTangent(GeneralLevel.Year_Province, 0.1, 1.2, 4, 2);
                    else if (relation[i].rootModel.Level == GeneralLevel.Year_Province)
                        relation[i].show_notTangent(GeneralLevel.Season_City, 0.2, 1.2, 6, 4);
                    else
                        relation[i].show_notTangent(GeneralLevel.Season_City, 0.3, 1.3, 8, 6); 
                    centerX += 2 * r;
                    /*if (centerX > RootView.canvas.ActualWidth)
                    {
                        centerX = BranchView.CalcRadius(0, relation);
                        centerY -= RootView.canvas.ActualHeight / 4;
                    }*/
                }

                //currentViews.rootViewList = relation;   //等同于设置currentView
                currentViews = new BranchView(relation);

                canvas.Children.Add(returnButton);

                e.Handled = true;
            }
        }

        internal void outerEllipse_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            foreach (RootView rv in relation)
            {
                rv.outerEllipse.Stroke = new SolidColorBrush(RootView.getColor(rv));
                rv.outerEllipse.StrokeThickness = 1;
            }
            relation.Clear();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
