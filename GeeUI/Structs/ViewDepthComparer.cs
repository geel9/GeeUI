using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeeUI.Views;

namespace GeeUI.Structs
{
    public static class ViewDepthComparer
    {
        public static int CompareDepths(View view1, View view2)
        {
            return view2.thisDepth - view1.thisDepth;
        }

        public static int CompareDepthsInverse(View view1, View view2)
        {
            return view1.thisDepth - view2.thisDepth;
        }
    }
}
