using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //internal void show() { 
            
        //}
    }
}
