using GeeUI.Structs;
using Microsoft.Xna.Framework;

namespace GeeUI.Views
{
    public class PanelView : View
    {
        public NinePatch UnselectedNinepatch = new NinePatch();
        public NinePatch SelectedNinepatch = new NinePatch();

        private const int ChildrenPadding = 1;

        public override Rectangle BoundBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        public override Rectangle ContentBoundBox
        {
            get
            {
                NinePatch curPatch = Selected ? SelectedNinepatch : UnselectedNinepatch;
                return new Rectangle((int)Position.X + curPatch.LeftWidth + ChildrenPadding, (int)Position.Y + curPatch.TopHeight + ChildrenPadding, Width - ChildrenPadding - curPatch.LeftWidth - curPatch.RightWidth, Height - ChildrenPadding - curPatch.TopHeight - curPatch.BottomHeight);
            }
        }

        public PanelView(View rootView, Vector2 position) : base(rootView)
        {
            SelectedNinepatch = GeeUI.NinePatchPanelSelected;
            UnselectedNinepatch = GeeUI.NinePatchPanelUnselected;
            Position = position;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            NinePatch patch = Selected ? SelectedNinepatch : UnselectedNinepatch;
            patch.Draw(spriteBatch, AbsolutePosition, Width - patch.LeftWidth - patch.RightWidth, Height - patch.TopHeight - patch.BottomHeight);
            base.Draw(spriteBatch);
        }



        public override void OnMClick(Vector2 position, bool fromChild = false)
        {
            base.OnMClick(position, true);
        }

        public override void OnMClickAway(bool fromChild = false)
        {
            base.OnMClickAway(true);
        }

        public override void OnMOver(bool fromChild = false)
        {
            base.OnMOver(true);
        }

        public override void OnMOff(bool fromChild = false)
        {
            base.OnMOff(true);
        }
    }
}
