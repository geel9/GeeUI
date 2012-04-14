using GeeUI.Views;
using System.Collections.Generic;
namespace GeeUI.ViewLayouts
{
    public class ViewLayout
    {
        public List<View> excludedChildren = new List<View>();
        public ViewLayout()
        {

        }

        public virtual void OrderChildren(View parentView)
        {

        }
    }
}
