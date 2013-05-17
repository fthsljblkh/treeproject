using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
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

namespace Processing
{
    public class Branch
    {
        public int initSize;
        public double x;
        public double y;
        public int level;//0为国家，1为某国某省，2为某国某省某市
        //0为世纪，1为10年，2为1年
        public String country;//0,中国
        public String province;//1,河南省
        public String city;//2,开封
        public Root[] roots;
        public double angle;  //角度
        public String century;//0，世纪
        public String decade; //1，十年
        public String year;//2，一年
        ImageBrush brush = new ImageBrush();
        public Image  img = new Image();

        public Branch()
        {
            this.initSize = 100;
            level = 0;
            roots = new Root[initSize];
           /* img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(
                             new Uri("ms-appx:///Assets/btn.jpg", UriKind.RelativeOrAbsolute));
            img.PointerPressed += img_PointerPressed;*/
        }

        public Branch(int size)
        {
            this.initSize = size;
            level = 0;
            roots = new Root[initSize];
            /*img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(
                             new Uri("ms-appx:///Assets/btn.jpg", UriKind.RelativeOrAbsolute));
            img.PointerPressed += img_PointerPressed;*/
        }

        void img_PointerPressed(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.initSize; ++i) 
            {
                Debug.WriteLine(this.initSize);
                if (this.initSize == 100)
                    return;
                this.roots[i].ellipse.Stroke = new SolidColorBrush(Colors.Red);
            }
        }

        public void display()
        {
            for (int i = 0; i < initSize; ++i)
            {
                roots[i].display();
            }
        }

        public double getAngle() { return angle; }
        public double getX() { return x; }
        public double getY() { return y; }
        public double getLevel() { return level; }
        public String getCentury() { return century; }
        public String getDecade() { if (level > 0)return decade; return ""; }
        public String getYear() { if (level > 1)return year; return ""; }
        public String getCountry() { if (level > 0)return country; return ""; }
        public String getProvince() { if (level > 1)return province; return ""; }
        public String getCity() { return city; }
        public void setDecade(String tempDecade) { this.decade = tempDecade; }
        public void setYear(String tempYear) { this.year = tempYear; }
        public void setCountry(String tempCountry) { this.country = tempCountry; }
        public void setProvince(String tempProvince) { this.province = tempProvince; }
        public void setCity(String tempCity) { this.city = tempCity; }
        public void setAngle(double tempAngle) { this.angle = tempAngle; }
        public void setX(double tempX) { this.x = tempX; }
        public void setY(double tempY) { this.y = tempY; }
        public void setLevel(int tempLevel) { this.level = tempLevel; }
        public Root[] getRoots() { return roots; }
    }
}
