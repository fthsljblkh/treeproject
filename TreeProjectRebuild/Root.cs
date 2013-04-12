using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace TreeProjectRebuild
{
    class Root
    {
        //Point p;
        //double radius;
        //Color strokeColor, fillColor;
        internal List<Branch> branchlist;

        GeneralLevel level; 
        String location;
        String time;
        decimal data;
        //public Ellipse ellipse;

        //internal Canvas canvas;
        //internal RootView fractal;

        /*public Root(Canvas canvas, RootView fractal) 
        {
            this.level = 0;
            branchlist = new List<Branch>();
            ellipse = new Ellipse();
            this.canvas = canvas;
            this.fractal = fractal;
        }*/

        internal Root(GeneralLevel level) {
            this.level = level;
            branchlist = new List<Branch>();
        }

        internal GeneralLevel Level {
            get {
                return level;
            }
        }

        /*public void display() 
        {
            ellipse.Width = this.w*2;
            ellipse.Height = this.h*2;
            ellipse.StrokeThickness = StrokeThickness;
            ellipse.Stroke = new SolidColorBrush(BorderColor);
            ellipse.Fill = new SolidColorBrush(FillColor);
            ellipse.PointerPressed += ellipse_PointerPressed;
        }

        void ellipse_PointerPressed(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.Stroke = new SolidColorBrush(Colors.Red);
            canvas.Children.Clear();
            fractal.root = this;

        }

        public double X
        {
            set
            {
                p.X = value;
            }
            get
            {
                return p.X;
            }
        }

        public double Y
        {
            get
            {
                return p.Y;
            }
            set
            {
                p.Y = value;
            }
        }

        public double Radius
        {
            set
            {
                radius = value;
            }
            get
            {
                return radius;
            }
        }
        /*public double getLevel() { return level; }
        public String getCentury() { return century; }
        public String getDecade() { if (this.level > 0)return decade; return ""; }
        public String getYear() { if (this.level > 1)return year; return ""; }
        public String getSeason() { return season; }
        public String getCountry() { if (this.level > 0)return country; return ""; }
        public String getProvince() { if (this.level > 1)return province; return ""; }
        public String getCity() { return city; }
        public String getData() { if (this.level > 2)return data; return ""; }
        public void setCentury(String tempCentury) { this.century = tempCentury; }
        public void setDecade(String tempDecade) { this.decade = tempDecade; }
        public void setYear(String tempYear) { this.year = tempYear; }
        public void setCountry(String tempCountry) { this.country = tempCountry; }
        public void setProvince(String tempProvince) { this.province = tempProvince; }
        public void setCity(String tempCity) { this.city = tempCity; }

        public void setLevel(int tempLevel) { this.level = tempLevel; }
        public void setData(String tempData) { this.data = tempData; }*/

        internal void BuildBranch() {
            int size;
            if (level == GeneralLevel.Genesis)
                size = 7;   //7国
            else if (level == GeneralLevel.Decade_Country)
                size = 8;   //8省
            else if (level == GeneralLevel.Year_Province)
                size = 6;   //6市
            else
                return;

            for (int i = 0; i < size; ++i) {
                Branch bran= new Branch(level + 1);
                bran.BuildRoot();
                branchlist.Add(bran);
            }
        }
    }
}
