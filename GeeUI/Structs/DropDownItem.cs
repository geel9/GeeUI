using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeeUI.Structs
{
    public class DropDownItem
    {
        public List<DropDownItem> children = new List<DropDownItem>();
        public bool hasChildren = false;
        public Action onClick = null;
        public string text = "";

        public DropDownItem(string text, Action onClick = null)
        {
            this.text = text;
            this.onClick = onClick;
        }

        public void AddChild(DropDownItem d)
        {
            children.Add(d);
            hasChildren = true;
        }

    }
}
