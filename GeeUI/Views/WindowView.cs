using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeeUI.Structs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class WindowView : View
    {
        public NinePatch ninePatchSelected = new NinePatch();
        public NinePatch ninePatchNormal = new NinePatch();

        public string windowText = "Hello this is a VIEW!";
        public SpriteFont windowTextFont = null;

        protected internal bool selectedOffChildren = false;
        protected internal Vector2 lastMousePosition = Vector2.Zero;
        protected internal Vector2 mouseSelectedOffset = Vector2.Zero;

        public View windowContentView
        {
            get
            {
                if (children.Length == 0)
                {
                    return null;
                }
                else
                {
                    return children[0];
                }
            }
            set
            {
                if (this.children.Length == 0)
                {
                    addChild(value);
                    return;
                }
                _children[0] = value;
                reOrderChildren();
            }
        }

        public override Rectangle BoundBox
        {
            get
            {
                NinePatch patch = selected ? ninePatchSelected : ninePatchNormal;
                Rectangle childBoundBox = windowContentView.BoundBox;
                Vector2 windowTextSize = windowTextFont.MeasureString(windowText);
                int height = (int)(patch.topHeight + patch.bottomHeight + windowTextSize.Y + childBoundBox.Height);

                //The width of the content view is the width of the window title bar
                int width = (int)(childBoundBox.Width);
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public override Rectangle ContentBoundBox
        {
            get
            {
                return windowContentView.BoundBox;
            }
        }

        public override Rectangle OffsetContentBoundBox
        {
            get
            {
                return windowContentView.OffsetBoundBox;
            }
        }

        public WindowView(View rootView, Vector2 position, SpriteFont windowTextFont)
            : base(rootView)
        {
            this.position = position;
            this.windowTextFont = windowTextFont;
            ninePatchNormal = GeeUI.ninePatch_textFieldDefault;
            ninePatchSelected = GeeUI.ninePatch_textFieldSelected;
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            NinePatch patch = selected ? ninePatchSelected : ninePatchNormal;
            patch.Draw(spriteBatch, offsetPosition, windowContentView.BoundBox.Width - (patch.leftWidth + patch.rightWidth), (int)windowTextFont.MeasureString(windowText).Y);
            spriteBatch.DrawString(windowTextFont, windowText, offsetPosition + new Vector2(patch.leftWidth, patch.topHeight), Color.Black);
         //   DrawManager.Draw_Box(new Vector2(OffsetBoundBox.Left, OffsetBoundBox.Top), new Vector2(OffsetBoundBox.Right, OffsetBoundBox.Bottom), Color.Red, spriteBatch);
            base.Draw(spriteBatch);
        }

        protected internal override void Update(GameTime theTime)
        {
            if (windowContentView != null)
            {
                NinePatch patch = selected ? ninePatchSelected : ninePatchNormal;
                Vector2 windowTextSize = windowTextFont.MeasureString(windowText);
                int height = (int)(patch.topHeight + patch.bottomHeight + windowTextSize.Y);
                windowContentView.position.Y = height;
                windowContentView.position.X = 0;
            }
            Vector2 newMousePosition = InputManager.GetMousePosV();
            if (selectedOffChildren && selected && InputManager.isLeftMousePressed())
            {
                position = (newMousePosition - mouseSelectedOffset);
            }
            lastMousePosition = newMousePosition;
            base.Update(theTime);
        }

        protected internal override void onMClick(Microsoft.Xna.Framework.Vector2 position, bool fromChild = false)
        {
            selectedOffChildren = !fromChild;
            selected = true;
            windowContentView.selected = true;
            lastMousePosition = position;
            mouseSelectedOffset = position - this.position;
            parentView.bringChildToFront(this);
            base.onMClick(position, true);
        }

        protected internal override void onMClickAway(bool fromChild = false)
        {
            selectedOffChildren = false;
            selected = false;
            windowContentView.selected = false;
            base.onMClickAway(true);
        }
    }
}
