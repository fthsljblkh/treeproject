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
        GeneralLevel level;
        internal TP_Data data;
        internal List<Root> rootlist;
        static Random rand;

        static Branch() {
            rand = new Random();
        }

        void getSum()
        {
            data.gdp = 0;
            foreach (Root root in rootlist)
            {
                data.gdp += root.data.gdp;
            }
        }

        double GDP { get { return data.gdp; } }

        internal Branch(GeneralLevel level, TP_Data pdata)
        { 
            this.level = level;
            rootlist = new List<Root>();
            if (pdata != null)
                data = new TP_Data { decade = pdata.decade, year = pdata.year, season = pdata.season, country = pdata.country, province = pdata.province, city = pdata.city, gdp = pdata.gdp };
            else
                data = new TP_Data { decade = -1, year = -1, season = -1, country = -1, province = -1, city = -1, gdp = -1 };
        }

        internal int SeasonNumber(TP_Data data_){
            return data_.decade * data_.year * data_.season;
        }

        internal void BuildRoot(){
            int size = -1;
            if(level == GeneralLevel.Decade_Country)    //root间的间隔为10年
                size = 10;                              //目前设size为10
            else if(level == GeneralLevel.Year_Province)
                size = 10;
            else if(level == GeneralLevel.Season_City)
                size = 4;  

            //level不能为GeneralLevel.Genesis

            for (int i = 0; i < size; ++i) {
                TP_Data data_ = null;
                switch (level) { 
                    case GeneralLevel.Decade_Country:
                        data_ = new TP_Data { country = data.country, decade = (sbyte)i };
                        break;
                    case GeneralLevel.Year_Province:
                        data_ = new TP_Data { country = data.country, province = data.province, decade = data.decade, year = (sbyte)i };
                        break;
                    case GeneralLevel.Season_City:
                        data_ = new TP_Data { country = data.country, province = data.province, city = data.city, decade = data.decade, year = data.year, season = (sbyte)i};
                        data_.gdp = (SeasonNumber(data_) + rand.Next(-100, 100))/500.0;
                        break;
                }
                
                Root root = new Root(level, data_);
                root.BuildBranch();
                rootlist.Add(root);
            }
            getSum();
        }
    }
}
