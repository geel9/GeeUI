using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Structs;
using GeeUI.Managers;

namespace GeeUI.Views
{

    public class ButtonView : View
    {
        public NinePatch ninePatchNormal;
        public NinePatch ninePatchHover;
        public NinePatch ninePatchClicked;

        public View buttonContentview
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
                NinePatch patch = currentNinepatch;
                int width = (int)(patch.leftWidth + patch.rightWidth + (buttonContentview != null ? buttonContentview.BoundBox.Width : 0));
                int height = (int)(patch.topHeight + patch.bottomHeight + (buttonContentview != null ? buttonContentview.BoundBox.Height : 0));
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public NinePatch currentNinepatch
        {
            get
            {
                if (mouseOver)
                {
                    return InputManager.isLeftMousePressed() ? ninePatchClicked : ninePatchHover;
                }
                else
                {
                    return ninePatchNormal;
                }
            }
        }

        public ButtonView(View rootView, string text, Vector2 position, SpriteFont font)
            : base(rootView)
        {
            ninePatchNormal = GeeUI.ninePatch_btnDefault;
            ninePatchHover = GeeUI.ninePatch_btnHover;
            ninePatchClicked = GeeUI.ninePatch_btnClicked;
            this.position = position;
            TextView view = new TextView(this, text, new Vector2(0, 0), font);
        }

        public ButtonView(View rootview, View contentView, Vector2 position) : base(rootview)
        {
            ninePatchNormal = GeeUI.ninePatch_btnDefault;
            ninePatchHover = GeeUI.ninePatch_btnHover;
            ninePatchClicked = GeeUI.ninePatch_btnClicked;
            this.position = position;
            buttonContentview = contentView;
        }

        protected internal override void onMClick(Vector2 position, bool fromChild = false)
        {
            base.onMClick(position);
        }
        protected internal override void onMClickAway(bool fromChild = false)
        {
            //base.onMClickAway();
        }

        protected internal override void onMOver(bool fromChild = false)
        {
            base.onMOver();
        }
        protected internal override void onMOff(bool fromChild = false)
        {
            base.onMOff();
        }

        protected internal override void Update(GameTime theTime)
        {
            base.Update(theTime);
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            NinePatch patch = currentNinepatch;
            int width = (int)(buttonContentview != null ? buttonContentview.BoundBox.Width : 0);
            int height = (int)(buttonContentview != null ? buttonContentview.BoundBox.Height : 0);
            patch.Draw(spriteBatch, absolutePosition, width, height);

            View childView = buttonContentview;
            if (childView != null)
            {
                childView.x = patch.leftWidth;
                childView.y = patch.topHeight;
            }

            base.Draw(spriteBatch);
        }
    }
}
