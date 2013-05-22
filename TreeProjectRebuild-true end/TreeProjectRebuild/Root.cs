using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace TreeProjectRebuild
{
    class Root
    {
        internal List<Branch> branchlist;

        GeneralLevel level;
        internal TP_Data data;

        //Branch parent;

        internal double GDP { get { return data.gdp; } }

        internal Root(GeneralLevel level, TP_Data pdata) {
            this.level = level;
            branchlist = new List<Branch>();
            if (pdata != null)
                data = new TP_Data { decade = pdata.decade, year = pdata.year, season = pdata.season, country = pdata.country, province = pdata.province, city = pdata.city, gdp = pdata.gdp};
            else
                data = new TP_Data() { decade = -1, year = -1, season = -1, country = -1, city = -1, province = -1, gdp = -1};
        }

        internal GeneralLevel Level
        {
            get
            {
                return level;
            }
        }

        void getSum() {
            data.gdp = 0;
            foreach (Branch branch in branchlist) {
                data.gdp += branch.data.gdp;
            }
        }

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
                TP_Data data_ = null;
                switch (level) { 
                    case GeneralLevel.Genesis:
                        data_ = new TP_Data { country = (sbyte)i };
                        break;
                    case GeneralLevel.Decade_Country:
                        data_ = new TP_Data { country = data.country, province = (sbyte)i, decade = data.decade };
                        break;
                    case GeneralLevel.Year_Province:
                        data_ = new TP_Data { country = data.country, province = data.province, city = (sbyte)i, decade = data.decade, year = data.year };
                        break;
                }
                Branch bran= new Branch(level + 1, data_);
                bran.BuildRoot();
                branchlist.Add(bran);
            }
            getSum();
        }
    }
}
