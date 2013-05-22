using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace TreeProjectRebuild
{
    class BranchView
    {
        Branch branchModel;
        internal List<RootView> rootViewList;

        internal BranchView(Branch branch) {
            branchModel = branch;
            rootViewList = new List<RootView>();
        }

        internal BranchView(List<RootView> rvl) {
            rootViewList = new List<RootView>();
            foreach (RootView rv in rvl) {
                rootViewList.Add(rv);
            }
        }

        internal BranchView(RootView rv) {
            rootViewList = new List<RootView>();
            rootViewList.Add(rv);
        }

        internal RootView getRootView(int i) {
            return rootViewList[i];
        }

        internal void addRootView(RootView rv) {
            rootViewList.Add(rv);
        }

        internal int Count {
            get {
                return rootViewList.Count;
            }
        }

        internal static double CalcRadius(int index, List<RootView> rvl) {
            Canvas canvas = RootView.canvas;
            double basicRadius = canvas.ActualWidth / (rvl.Count * 2) * 0.8;
            
            if (rvl[index].rootModel.Level == GeneralLevel.Decade_Country){
                basicRadius += rvl[index].rootModel.GDP / 7;
            }
            else if (rvl[index].rootModel.Level == GeneralLevel.Year_Province) {
                basicRadius += rvl[index].rootModel.GDP * 2;
            }
            else if (rvl[index].rootModel.Level == GeneralLevel.Season_City) {
                basicRadius += rvl[index].rootModel.GDP * 4;
            }

            return basicRadius;
        }

        internal void ShowBranch(GeneralLevel level){
            double centerX = CalcRadius(0, rootViewList);
            double centerY = RootView.canvas.ActualHeight / 4 * 3;

            for(int i = 0; i<rootViewList.Count; ++i){
                double r = CalcRadius(i, rootViewList);
                RootView.SetElli(centerX, centerY, r, Colors.Black, Colors.Gray, rootViewList[i]);
                if (rootViewList[i].rootModel.Level == GeneralLevel.Decade_Country)
                    rootViewList[i].show_notTangent(level + 1, 0.1, 1.2, 4, 2);
                else if (rootViewList[i].rootModel.Level == GeneralLevel.Year_Province)
                    rootViewList[i].show_notTangent(level + 1, 0.2, 1.2, 6, 4);
                else
                    rootViewList[i].show_notTangent(level + 1, 0.3, 1.3, 8, 6);
                centerX += 2 * r;
                /*if (centerX > RootView.canvas.ActualWidth) {
                    centerX = CalcRadius(0, rootViewList);
                    centerY -= RootView.canvas.ActualHeight / 4;
                }*/
            }

            RootView.DataCanvas(this);

            Canvas canvas = RootView.canvas;
            Button returnButton = RootView.mp.returnButton;
            //Canvas.SetLeft(returnButton, 40);
            //Canvas.SetTop(returnButton, 40);
            canvas.Children.Add(returnButton);
        }
    }
}
