using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Processing
{
    public class Root
    {
        public int initSize;
        public double x;
        public double y;
        public double w;
        public double h;
        double X1, X2, Y1, Y2, StrokeThickness = 1;
        Color BorderColor = Colors.Black, FillColor;

        public Branch[] branches;

        public int level;
        public String world;
        public String country;
        public String province;
        public String city;
        public String century;
        public String decade;
        public String year;
        public String season;
        public String data;
        public Ellipse ellipse;

        public Root() 
        {
            this.initSize = 7;
            this.level = 0;
            this.branches = new Branch[initSize];
            ellipse = new Ellipse();
        }

        public Root(int tempSize) 
        {
            this.initSize = tempSize;
            this.level = 0;
            this.world = "世界";
            this.branches= new Branch[initSize];
            ellipse = new Ellipse();
        }

        public void display() 
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
        }

        public double getX() { return x; }
        public double getY() { return y; }
        public double getW() { return w; }
        public double getH() { return h; }
        public double getLevel() { return level; }
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
        public void setX(double tempX) { this.x = tempX; }
        public void setY(double tempY) { this.y = tempY; }
        public void setW(double tempW) { this.w = tempW; }
        public void setH(double tempH) { this.h = tempH; }
        public void setLevel(int tempLevel) { this.level = tempLevel; }
        public void setData(String tempData) { this.data = tempData; }
    }
}
