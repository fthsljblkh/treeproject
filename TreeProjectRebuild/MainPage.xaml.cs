﻿using System;
using System.Diagnostics;
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
        internal RootView currentView;
        Boolean enlarged;
        internal List<RootView> relation;
	 internal List<RootView> selectItem;
	//for select items in a list
        ComboBox box = new ComboBox();
        TextBox text = new TextBox();
        public String status;

        internal List<Point> point = new List<Point>();
        internal List<Point> point = new List<Point>();

	 internal List<SelectView> selectItem = new List<SelectView>();
	//作为选择出来的一个list
 
        internal Button returnButton = new Button { Content = "Return" };
	 internal Button confirmButton = new Button { content = "Confirm" };

        internal double ras0 = 0;	//

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        internal void outerEllipse_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
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
                rv.outerEllipse.Fill = new SolidColorBrush(Colors.Gray);
            }
            relation.Clear();
            status = "click";

            if (index >= 0)
            {
                status = "enter";
                foreach (BranchView branchView in currentView.branchViewList)
                {
                    branchView.getRootView(index).outerEllipse.Fill = new SolidColorBrush(Colors.Aqua);
                    relation.Add(branchView.getRootView(index));
                }
            }
        }

        internal double Dist(Point a, Point b) {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            status = "null";
            world_root = new Root(GeneralLevel.Genesis);
            world_root.BuildBranch();

            //DoubleTapped += MainPage_DoubleTapped;            
            
            relation = new List<RootView>();
            RootView.setRelation(relation);

            world_view = new RootView(world_root);
            RootView.mp = this;
            RootView.SetCanvas(canvas);
            world_view.BuildBranchView();
            RootView.SetElli((canvas.ActualWidth -400) / 2, canvas.ActualHeight / 2, (canvas.ActualHeight-100) / 2, Colors.Black, Colors.Gray, world_view);
            //RootView.SetElli(canvas.ActualWidth/2, canvas.ActualHeight / 2, 3, Colors.Black, Colors.White, world_view.innerEllipse);

            //world_view.show_notTangent(GeneralLevel.Decade_Country, 0.4, 1.3, 10, 5);
            //world_view.show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);

            RootView.setCurrentView(world_view);
            world_view.showImage();
            ComboBoxItem item0 = new ComboBoxItem();
            item0.Content = "唐山大地震";
            box.Items.Add(item0);

            ComboBoxItem item1 = new ComboBoxItem();
            item1.Content = "北京奥运会";
            box.Items.Add(item1);

            ComboBoxItem item2 = new ComboBoxItem();
            item2.Content = "汶川地震";
            box.Items.Add(item2);
            
            text.Text = "欢迎使用";
            text.Background = new SolidColorBrush(Colors.Wheat);

            returnButton.Content = "Return";
            returnButton.Foreground = new SolidColorBrush(Colors.White);
            returnButton.Background = new SolidColorBrush(Colors.Black);
            returnButton.Tapped += returnButton_Tapped;
            setBox();
            
            confirmButton.Content = "Confirm";
	     confirmButton.Foreground = new SolidColorBrush(Colors.White);
	     confirmButton.Background = new SolidColorBrush(Colors.Black);
	     confirmButton.Tapped += confirmButton_Tapped;

            currentView.outerEllipse.PointerMoved += outerEllipse_PointerMoved;
            currentView.outerEllipse.PointerExited += outerEllipse_PointerExited;
        }

        void returnButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (RootView.trace.Count > 0)
            {
                canvas.Children.Clear();
                
                RootView parent = RootView.trace.Pop();
                
                RootView.SetElli((canvas.ActualWidth - 400) / 2, canvas.ActualHeight / 2, (canvas.ActualHeight - 100) / 2, Colors.Black, Colors.Gray, parent);
                if (parent.rootModel.Level == GeneralLevel.Decade_Country)
                {
                    parent.show_notTangent(GeneralLevel.Year_Province, 0.3, 1.2, 10, 5);
                }
                else if (parent.rootModel.Level == GeneralLevel.Genesis) 
                {
                    parent.showImage();
                    parent.show_notTangent(GeneralLevel.Decade_Country, 0.3, 1.2, 10, 5);
                }
                    
                RootView.setCurrentView(parent);
                Debug.WriteLine(parent.branchViewList.Count);
                if (parent.rootModel.Level == GeneralLevel.Decade_Country)
                {
                    world_view.setControl(GeneralLevel.Decade_Country, parent);
                }
                setBox();
            }
        }
	public void confirmButton_Tapped(object sender, TappedRoutedEventArgs e)
	{
		//从已经获取的 list 里面拿到数据，绘制图片

		
	}
	
        public void setBox() 
        {
            box.Width = 300;
            box.Height = 100;
            Canvas.SetTop(box, 0);
            Canvas.SetLeft(box, canvas.ActualWidth - 300);
            box.SelectionChanged += box_SelectionChanged;
            canvas.Children.Add(box);
            text.Width = 300;
            text.Height = 300;
            
            Canvas.SetTop(text,canvas.ActualHeight - 300);
            Canvas.SetLeft(text,canvas.ActualWidth - 300);
            canvas.Children.Add(text);
           
            returnButton.Width = 100;
            returnButton.Height = 100;
            Canvas.SetLeft(returnButton, 1200);
            Canvas.SetTop(returnButton, 100);
            canvas.Children.Add(returnButton);
        }

        public void setBox0()
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

        void box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (box.SelectedIndex != -1) 
            {
                
                if (box.SelectedIndex == 0) 
                {
                    status = "event";
                    text.Text = "唐山大地震";
                    canvas.Children.Clear();
                    world_view.dataCanvas(0,world_view.branchViewList[0].rootViewList[0].branchViewList);
                }
                else if (box.SelectedIndex == 1) 
                {
                    text.Text = "北京奥运会";
                    status = "event";
                    text.Text = "唐山大地震";
                    canvas.Children.Clear();
                    world_view.dataCanvas(0, world_view.branchViewList[0].rootViewList[0].branchViewList);
                }
                else if (box.SelectedIndex == 2) 
                {
                    text.Text = "汶川地震";
                    status = "event";
                    text.Text = "唐山大地震";
                    canvas.Children.Clear();
                    world_view.dataCanvas(0,world_view.branchViewList[0].rootViewList[0].branchViewList);
                }
            }
        }

        internal void outerEllipse_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            foreach (RootView rv in relation)
            {
                rv.outerEllipse.Fill = new SolidColorBrush(Colors.Gray);
            }
            relation.Clear();

            status = "click";
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
