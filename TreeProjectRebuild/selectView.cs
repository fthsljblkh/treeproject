using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeProjectRebuild
{
    class SelectView
    {
        Select selectModel;
        internal List<RootView> selectViewList;
        internal SelectView(Select selectItem) {
            selectModel = selectItem;
            selectViewList = new List<RootView>();
        }
        internal RootView getRootView(int i) {
            return selectViewList[i];
        }

        internal void addRootView(RootView rv) {
		private void get_key(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == control)
			{
			//message.Show("是否获得ctrl");
			//获取圆坐标
			
				selectViewList.add(rv);	//获得一个焦点添加一个对象
			}
	        }
        }

        internal int Count {
            get {
                return selectViewList.Count;
            }
        }

        
    }
}
