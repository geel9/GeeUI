using GeeUI.Structs;
using Microsoft.Xna.Framework;

namespace GeeUI.Views
{
    public class PanelView : View
    {
        public NinePatch UnselectedNinepatch = new NinePatch();
        public NinePatch SelectedNinepatch = new NinePatch();

        public override Rectangle BoundBox
        {
            get
            {
                NinePatch curPatch = Selected ? SelectedNinepatch : UnselectedNinepatch;
                int width = curPatch.LeftWidth + curPatch.RightWidth + Width;
                int height = curPatch.TopHeight + curPatch.BottomHeight + Height;
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
            }
        }

        public override Rectangle ContentBoundBox
        {
            get
            {
                NinePatch curPatch = Selected ? SelectedNinepatch : UnselectedNinepatch;
                return new Rectangle((int)Position.X + curPatch.LeftWidth, (int)Position.Y + curPatch.TopHeight, Width, Height);
            }
        }

        public PanelView(View rootView, Vector2 position) : base(rootView)
        {
            SelectedNinepatch = GeeUI.NinePatchPanelSelected;
            UnselectedNinepatch = GeeUI.NinePatchPanelUnselected;
            Position = position;
        }

        protected internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            NinePatch patch = Selected ? SelectedNinepatch : UnselectedNinepatch;
            patch.Draw(spriteBatch, AbsolutePosition, Width, Height);
            base.Draw(spriteBatch);
        }



        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            base.OnMClick(position, true);
        }

        protected internal override void OnMClickAway(bool fromChild = false)
        {
            base.OnMClickAway(true);
        }

        protected internal override void OnMOver(bool fromChild = false)
        {
            base.OnMOver(true);
        }

        protected internal override void OnMOff(bool fromChild = false)
        {
            base.OnMOff(true);
        }
    }
}
