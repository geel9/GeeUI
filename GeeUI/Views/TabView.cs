using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Structs;

namespace GeeUI.Views
{
    public class TabView : View
    {



        public NinePatch NinePatchSelected = new NinePatch();
        public NinePatch NinePatchDefault = new NinePatch();

        public SpriteFont TabFont;

        public string TabText = "Tab";

        public NinePatch CurNinepatch
        {
            get { return Selected ? NinePatchSelected : NinePatchDefault; }
        }

        public override Rectangle BoundBox
        {
            get
            {
                return new Rectangle(X, Y,
                        (int)(CurNinepatch.LeftWidth + TabFont.MeasureString(TabText).X + CurNinepatch.RightWidth),
                        (int)(CurNinepatch.TopHeight + TabFont.MeasureString(TabText).Y + CurNinepatch.BottomHeight));
            }
        }

        public TabView(View rootView, Vector2 position, SpriteFont font)
            : base(rootView)
        {
            Position = position;
            TabFont = font;
            NumChildrenAllowed = 0;

            NinePatchDefault = GeeUI.NinePatchTabDefault;
            NinePatchSelected = GeeUI.NinePatchTabSelected;
        }

        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            var p = (TabContainer)ParentView;
            p.TabClicked(this);

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
            var width = (int)TabFont.MeasureString(TabText).X;
            var height = (int)TabFont.MeasureString(TabText).Y;

            CurNinepatch.Draw(spriteBatch, AbsolutePosition, width, height);
            spriteBatch.DrawString(TabFont, TabText, AbsolutePosition + new Vector2(CurNinepatch.LeftWidth, CurNinepatch.TopHeight), Color.Black);

            base.Draw(spriteBatch);
        }
    }
}
