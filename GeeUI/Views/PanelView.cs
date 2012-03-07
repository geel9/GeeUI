using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeeUI.Structs;
using Microsoft.Xna.Framework;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class PanelView : View
    {
        public NinePatch unselectedNinepatch = new NinePatch();
        public NinePatch selectedNinepatch = new NinePatch();

        protected internal bool selectedOffChildren = false;
        protected internal Vector2 lastMousePosition = Vector2.Zero;
        protected internal Vector2 mouseSelectedOffset = Vector2.Zero;

        public override Rectangle BoundBox
        {
            get
            {
                NinePatch curPatch = selected ? selectedNinepatch : unselectedNinepatch;
                int width = curPatch.leftWidth + curPatch.rightWidth + this.width;
                int height = curPatch.topHeight + curPatch.bottomHeight + this.height;
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public PanelView(View rootView, Vector2 position) : base(rootView)
        {
            selectedNinepatch = GeeUI.ninePatch_windowSelected;
            unselectedNinepatch = GeeUI.ninePatch_windowUnselected;
            this.position = position;
        }

        protected internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            NinePatch patch = selected ? selectedNinepatch : unselectedNinepatch;
            patch.Draw(spriteBatch, offsetPosition, width, height);
            base.Draw(spriteBatch);
        }

        protected internal override void Update(GameTime theTime)
        {
            Vector2 newMousePosition = InputManager.GetMousePosV();
            if (selectedOffChildren && selected && InputManager.isLeftMousePressed())
            {
                position = (newMousePosition - mouseSelectedOffset) ;
            }
            lastMousePosition = newMousePosition;
            base.Update(theTime);
        }

        protected internal override void onMClick(Microsoft.Xna.Framework.Vector2 position, bool fromChild = false)
        {
            selectedOffChildren = !fromChild;
            selected = true;
            lastMousePosition = position;
            mouseSelectedOffset = position - this.position;
            parentView.bringChildToFront(this);
            base.onMClick(position, true);
        }

        protected internal override void onMClickAway(bool fromChild = false)
        {
            selectedOffChildren = false;
            selected = false;
            base.onMClickAway(true);
        }

        protected internal override void onMOver(bool fromChild = false)
        {
            base.onMOver(true);
        }

        protected internal override void onMOff(bool fromChild = false)
        {
            base.onMOff(true);
        }
    }
}
