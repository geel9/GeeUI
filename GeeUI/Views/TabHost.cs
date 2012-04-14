using System.Linq;
using GeeUI.ViewLayouts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeeUI.Views
{
    public class TabHost : View
    {

        private TabContainer TabContainerView
        {
            get
            {
                return Children.Length == 0 ? null : (TabContainer)Children[0];
            }
            set
            {
                if (Children.Length == 0)
                {
                    AddChild(value);
                    return;
                }
                _children[0] = value;
                ReOrderChildrenDepth();
            }
        }

        public override Rectangle BoundBox
        {
            get
            {
                View activeTab = TabContainerView.ActiveTabView;
                if(activeTab == null) return new Rectangle(X, Y, 0, 0);

                activeTab = TabViewToView((TabView)activeTab);
                return new Rectangle(X, Y,
                    (int) MathHelper.Max(activeTab.BoundBox.Width, TabContainerView.BoundBox.Width),
                    activeTab.BoundBox.Height + TabContainerView.BoundBox.Height);
            }
        }

        public TabHost(View rootView, Vector2 position, SpriteFont font)
            : base(rootView)
        {
            Position = position;
            TabContainerView = new TabContainer(this, font);

        }

        internal View TabViewToView(TabView v)
        {
            int index = TabContainerView._children.IndexOf(v) + 1;
            return index >= Children.Length ? null : Children[index];
        }

        public void AddTab(string tabText, View newTab)
        {
            TabContainerView.AddTab(tabText, newTab);
            TabContainerView.OrderChildren(new HorizontalViewLayout(1));
        }

        public void SetActiveTab(int index)
        {
            TabContainerView.TabClicked((TabView)TabContainerView.Children[index]);
        }

        internal void TabClicked(int index)
        {
            for(int i = 1; i < Children.Length; i++)
            {
                Children[i].Active = false;
                Children[i].Selected = false;
            }
            Children[index + 1].Active = true;
            Children[index + 1].Selected = true;
        }

        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            base.OnMClick(position);
        }
        protected internal override void OnMClickAway(bool fromChild = false)
        {
            //base.onMClickAway();
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
            for (int i = 1; i < Children.Length; i++  )
            {
                Children[i].Position = new Vector2(0, TabContainerView.BoundBox.Height);
            }
            base.Update(theTime);
        }

        public override void AddChild(View child)
        {
            if (!(child is TabContainer) && TabContainerView.Children.Length != Children.Length) return;
            base.AddChild(child);
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
