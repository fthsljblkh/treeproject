using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TreeProjectRebuild
{
    public enum GeneralLevel {Genesis, Decade_Country, Year_Province, Season_City};    //通用branch/root分类方法：十年与国家，年与省份，季度与城市

    public class Branch
    {
        //Point p;
        GeneralLevel level;
        String location;
        String time;
        internal List<Root> rootlist;
        //double angle;

        public Branch(GeneralLevel level)
        { 
            this.level = level;
            rootlist = new List<Root>();
        }

        /*public void display()
        {
            for (int i = 0; i < initSize; ++i)
            {
                roots[i].display();
            }
        }*/

        //public double getAngle() { return angle; }
        //public GeneralLevel getLevel() { return level; }
        /*public String getCentury() { return century; }
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
        public void setLevel(int tempLevel) { this.level = tempLevel; }*/

        

        internal void BuildRoot(){
            int size = -1;
            if(level == GeneralLevel.Decade_Country)    //root间的间隔为10年
                size = 10;                              //目前设size为10
            else if(level == GeneralLevel.Year_Province)
                size = 9;
            else if(level == GeneralLevel.Season_City)
                size = 4;  

            //level不能为GeneralLevel.Genesis

            for (int i = 0; i < size; ++i) {
                Root root = new Root(level);
                root.BuildBranch();
                rootlist.Add(root);
            }
        }
    }
}
