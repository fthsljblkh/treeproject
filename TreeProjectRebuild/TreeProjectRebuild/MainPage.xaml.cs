using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        Boolean enlarged;
        List<RootView> relation;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            //DoubleTapped += MainPage_DoubleTapped;

            relation = new List<RootView>();
            RootView.setRelation(relation);
        }

        void outerEllipse_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            int index = -1;
            Point cursor = e.GetCurrentPoint(canvas).Position;
            double distance = Dist(cursor, world_view.center);
            BranchView bv = world_view.branchViewList[0];
            for (int i = 0; i < bv.Count; ++i) {
                RootView rv = bv.getRootView(i);
                double dist = Dist(rv.center, world_view.center);
                if (distance > dist - rv.radius && distance < dist + rv.radius) {   //距离
                    index = i;
                    break;
                }
            }

            foreach (RootView rv in relation) {
                rv.outerEllipse.Fill = new SolidColorBrush(Colors.Gray);
            }
            relation.Clear();

            if (index >= 0) {
                foreach (BranchView branchView in world_view.branchViewList) {
                     branchView.getRootView(index).outerEllipse.Fill = new SolidColorBrush(Colors.Aqua);
                     relation.Add(branchView.getRootView(index));
                }
            }
        }

        static double Dist(Point a, Point b) {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /*void MainPage_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!enlarged)
            {
                RootView.setAmp(5);
            }
            else
            {
                RootView.setAmp(1);
            }
            canvas.Children.Clear();
            /*world_root = new Root(GeneralLevel.Genesis);
            world_root.BuildBranch();
            world_view = new RootView(canvas.ActualWidth / 2, canvas.ActualHeight / 2, canvas.ActualHeight * 2, world_root);

            world_view.outerEllipse.PointerMoved += outerEllipse_PointerMoved;
            world_view.outerEllipse.PointerExited += outerEllipse_PointerExited;
            world_view.branchViewList.Clear();
            world_view.Show(GeneralLevel.Decade_Country);
            enlarged = !enlarged;
        }*/

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            world_root = new Root(GeneralLevel.Genesis);
            world_root.BuildBranch();
            
            world_view = new RootView(canvas.ActualWidth / 2, canvas.ActualHeight / 2, canvas.ActualHeight*2, world_root);
            
            //world_view.outerEllipse.PointerMoved += outerEllipse_PointerMoved;
            //world_view.outerEllipse.PointerExited += outerEllipse_PointerExited;

            RootView.SetCanvas(canvas);
            world_view.Show(GeneralLevel.Decade_Country);
            //enlarged = false;

            //world_view.outerEllipse.Tapped += outerEllipse_Tapped;
        }

        void outerEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (relation.Count > 0)
            {
                canvas.Children.Clear();
                double[] pos = new double[2];
                for (int i = 0; i < relation.Count; ++i) {
                    double radians = 2 * Math.PI * i / relation.Count;
                    pos[0] = canvas.ActualWidth / 2 + 300 * Math.Cos(radians);
                    pos[1] = canvas.ActualHeight / 2 + 300 * Math.Sin(radians);

                    relation[i].SetX(pos[0]);
                    relation[i].SetY(pos[1]);
                    relation[i].radius = 300;

                    relation[i].Show(GeneralLevel.Decade_Country);
                }
            }
        }

        void outerEllipse_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            foreach (RootView rv in relation)
            {
                rv.outerEllipse.Fill = new SolidColorBrush(Colors.Gray);
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
