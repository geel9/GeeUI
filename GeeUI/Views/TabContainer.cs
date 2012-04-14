using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeeUI.Views
{
    public class TabContainer : View
    {
        public SpriteFont TabFont;

        public TabView ActiveTabView
        {
            get
            {
                return Children.Where(child => child is TabView).Cast<TabView>().FirstOrDefault(v => v.Selected);
            }
        }

        public int AllTabsWidth
        {
            get {
                if (Children.Length == 0) return 0;
                View last = Children[Children.Length - 1];
                return (int)last.Position.X + last.BoundBox.Width;
            }
        }

        public override Rectangle BoundBox
        {
            get
            {
                TabHost p = (TabHost) ParentView;
                return ActiveTabView == null
                           ? new Rectangle(X, Y, 0, 0)
                           : new Rectangle(X, Y, AllTabsWidth, ActiveTabView.BoundBox.Height);
            }
        }

        internal void TabClicked(TabView child)
        {
            foreach (TabView tab in Children)
            {
                tab.Selected = false;
            }
            child.Selected = true;
            var host = (TabHost) ParentView;
            host.TabClicked(_children.IndexOf(child));
        }

        public TabView AddTab(string tabText, View tabChild)
        {
            var ret = new TabView(this, new Vector2(AllTabsWidth, 0), TabFont) { TabText = tabText };
            ParentView.AddChild(tabChild);
            if (ActiveTabView == null)
                TabClicked(ret);
            else 
                TabClicked(ActiveTabView);
            return ret;
        }

        public TabContainer(View rootView,  SpriteFont font)
            : base(rootView)
        {
            Position = Vector2.Zero;
            TabFont = font;
        }

        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            base.OnMClick(position);
        }
        protected internal override void OnMClickAway(bool fromChild = false)
        {
            //base.OnMClickAway();
        }

        protected internal override void OnMOver(bool fromChild = false)
        {
            base.OnMOver();
        }
        protected internal override void OnMOff(bool fromChild = false)
        {
            base.OnMOff();
        }

        protected internal override void Update(GameTime theTime)
        {
            base.Update(theTime);
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
