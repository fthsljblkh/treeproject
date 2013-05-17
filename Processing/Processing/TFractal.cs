using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace Processing
{
    class TFractal
    {
        
        ImageBrush brush = new ImageBrush();
        public double height;
        public double width;
        public int nSides;
        public double centerX, centerY;
        public int limitNum = 0;
        public double borderR;
        public double initial_l;
        public double initial_r;
        public int threshold_borderR;
        public Root root;
        double StrokeThickness = 1;
        Color BorderColor = Colors.Black, FillColor;
        //int depth = 1;
        Canvas canvas = new Canvas();
        public Ellipse ellipse;

        public TFractal(double tempW, double tempH)
        {
            width = tempW;
            height = tempH;
            nSides = 7;
            centerX = width / 2;
            centerY = height / 2;
            initial_r = 4;
            initial_l = 12;//12
            borderR = 600;
            threshold_borderR = 5;
            root = new Root();177230河北
        }

        public TFractal(int nSides, double centerX, double centerY, double borderR, double initial_r, double initial_l, int threshold_borderR)
        {
            this.nSides = nSides;
            this.centerX = centerX;
            this.centerY = centerY;
            this.borderR = borderR;
            this.initial_r = initial_r;
            this.initial_l = initial_l;
            this.threshold_borderR = threshold_borderR;
            root = new Root(this.nSides);
            root.x = this.centerX;
            root.y = this.centerY;
            root.w = this.borderR;
            root.h = this.borderR;
        }

        /*void ellipse_PointerPressed(object sender, RoutedEventArgs e) 
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.Width = ellipse.Width * 1.2;
            ellipse.Height = ellipse.Height * 1.2;
            Canvas.SetTop(ellipse, this.root.x - ellipse.Width /2);
            Canvas.SetLeft(ellipse, this.root.y - ellipse.Height / 2);
            
            //this.root.x
        }
        void Ellipse_PointerPressed(object sender, RoutedEventArgs e) 
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.Width = ellipse.Width * 1.2;
            ellipse.Height = ellipse.Height * 1.2;
            Canvas.SetTop(ellipse, 100);//this.centerX - this.initial_r - ellipse.Width*0.1);
            Canvas.SetLeft(ellipse, 100);//this.centerY - this.initial_r - ellipse.Height * 0.1);
        }*/

        void img_PointerEntered(object sender, RoutedEventArgs e)
        {
            visibleTrue(1);
            //visibleTrue(1);
            /*for (int i = 0; i < this.nSides; ++i)
            {
                for (int j = 0; j < this.root.branches[i].initSize; ++j) 
                {
                    this.root.branches[i].roots[j].ellipse.Opacity = 1;
                }
            }
            this.root.ellipse.Opacity = 1;
            ellipse.Opacity = 1;*/
        }
        void img_PointerExited(object sender, RoutedEventArgs e) 
        {
            visibleTrue(0);
            /*
            for (int i = 0; i < this.nSides; ++i)
            {
                for (int j = 0; j < this.root.branches[i].initSize; ++j)
                {
                    Debug.WriteLine("size+++: " + this.root.branches[i].roots[j].getW());
                    this.root.branches[i].roots[j].ellipse.Opacity = 0;
                }
            }
            this.root.ellipse.Opacity = 0;
            ellipse.Opacity = 0;*/
        }

        public void img_display(Canvas cas) 
        {
            canvas = cas;
            for (int i = 0; i < this.nSides; ++i)
            {
                double deg = (360.0 / this.nSides) * i;
                double l = 150;
                this.root.branches[i] = new Branch();
                double x = (int)(this.centerX + l * Math.Cos(radians(deg)));
                double y = (int)(this.centerY + l * Math.Sin(radians(deg)));

                Canvas.SetLeft(this.root.branches[i].img, x );
                Canvas.SetTop(this.root.branches[i].img, y );
                //this.root.branches[i].img.PointerEntered += img_PointerEntered;
                //this.root.branches[i].img.PointerExited += img_PointerExited;
                cas.Children.Add(this.root.branches[i].img);
            }
        }

        public void visibleTrue(int opc) 
        {
            for (int i = 0; i < this.root.initSize; ++i)
            {      
                for (int j = 0; j < this.root.branches[i].initSize; ++j) 
                {
                    this.root.branches[i].roots[j].ellipse.Opacity = opc;
                    //visibleTrue(opc);
                }
            }
        }

        public void display(Canvas cas)
        {
            this.root = new Root(this.nSides);
            this.root.setLevel(0);
            this.root.setX(this.centerX);
            this.root.setY(this.centerY);
            /* this.root.setW(this.borderR);
             this.root.setH(this.borderR);*/
            this.root.display();
            Canvas.SetLeft(this.root.ellipse, this.root.x - this.root.ellipse.Width / 2);
            Canvas.SetTop(this.root.ellipse, this.root.y - this.root.ellipse.Height / 2);
            cas.Children.Add(this.root.ellipse);

            ellipse = new Ellipse();
            //ellipse.PointerPressed += Ellipse_PointerPressed;
            ellipse.Width = this.initial_r * 2;
            ellipse.Height = this.initial_r * 2;
            ellipse.StrokeThickness = StrokeThickness;

            ellipse.Stroke = new SolidColorBrush(BorderColor);
            Canvas.SetLeft(ellipse, this.centerX - this.initial_r);
            Canvas.SetTop(ellipse, this.centerY - this.initial_r);
            cas.Children.Add(ellipse);
            if (root.level >= 4)
                return;

            int[] tempPos = new int[2];
            int i;
            for (i = 0; i < this.nSides; ++i)
            {
                double deg = (360.0 / this.nSides) * i;
                double l = this.initial_l;
                //double r = l * 1.35;
                double r = this.initial_r;
                this.root.branches[i] = new Branch();
                tempPos[0] = (int)(this.centerX + l * Math.Cos(radians(deg)));
                tempPos[1] = (int)(this.centerY + l * Math.Sin(radians(deg)));

                int j = 0;
                while (distance(tempPos[0], tempPos[1], this.centerX, this.centerY) <= this.borderR - r)
                {
                    this.root.branches[i].roots[j] = new Root();
                    this.root.branches[i].roots[j].setX(tempPos[0]);
                    this.root.branches[i].roots[j].setY(tempPos[1]);
                    this.root.branches[i].roots[j].setW(r);
                    this.root.branches[i].roots[j].setH(r);
                    this.root.branches[i].roots[j].display();
                    Canvas.SetLeft(this.root.branches[i].roots[j].ellipse, tempPos[0] - this.root.branches[i].roots[j].ellipse.Width / 2);
                    Canvas.SetTop(this.root.branches[i].roots[j].ellipse, tempPos[1] - this.root.branches[i].roots[j].ellipse.Height / 2);
                    cas.Children.Add(this.root.branches[i].roots[j].ellipse);
                    j++;

                    if (r > this.threshold_borderR)
                    {
                        TFractal tempTF = new TFractal(this.nSides, tempPos[0], tempPos[1], r, this.initial_r, this.initial_l, this.threshold_borderR);
                        tempTF.display(cas);
                    }
                    r += 0.2 * r;//0.7
                    //r = l * 1.35;
                    l += 0.9 * r;
                    deg = (deg + 30) % 360;
                    tempPos[0] = (int)(this.centerX + l * Math.Cos(radians(deg)));
                    tempPos[1] = (int)(this.centerY + l * Math.Sin(radians(deg)));
                }
                this.root.branches[i].initSize = j;
            }
            this.root.initSize = i - 1;
        }


        double getRadius(double length, double increase,int all, int n) 
        {
            double sumIncrease = 1;
            for (int i = 0; i < all; ++i)
                sumIncrease *= increase;
            double tempAnswer = length*(1-increase)/(1-sumIncrease);
            for (int i = 0; i < (n-1); ++i) 
            {
                tempAnswer *=  increase; 
            }
            return tempAnswer;
        }


        double distance(double x1, double y1, double x2, double y2)
        {
            double num = sq(x1 - x2) + sq(y1 - y2);
            return Math.Sqrt(num);
        }

        void tfRotate(double[] pos, int[] refo, int degree)
        {

            double cosa = Math.Cos(radians(degree));
            double sina = Math.Sin(radians(degree));
            double newX = (pos[0] - refo[0]) * cosa - (pos[1] - refo[1]) * sina + refo[0];
            double newY = (pos[1] - refo[1]) * cosa + (pos[0] - refo[0]) * sina + refo[1];
            pos[0] = newX;
            pos[1] = newY;
        }

        double radians(double x)
        {
            return (x / 180) * Math.PI;
        }

        double sq(double x)
        {
            return x * x;
        }
    }
}
