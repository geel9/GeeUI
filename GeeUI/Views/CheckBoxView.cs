using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class CheckBoxView : View
    {
        public Texture2D textureDefault;
        public Texture2D textureChecked;
        public Texture2D textureDefaultSelected;
        public Texture2D textureCheckedSelected;

        public bool isChecked = false;
        public bool allowLabelClicking = true;

        private int seperationBetweenCBAndText = 3;

        public View checkBoxContentView
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

        public Texture2D curTexture
        {
            get
            {
                if (selected || mouseOver)
                {
                    return isChecked ? textureCheckedSelected : textureDefaultSelected;
                }
                else
                {
                    return isChecked ? textureChecked : textureDefault;
                }
            }
        }

        public override Microsoft.Xna.Framework.Rectangle BoundBox
        {
            get
            {
                View child = checkBoxContentView;
                if (child == null)
                {
                    return curTexture.Bounds;
                }
                return new Rectangle((int)position.X, (int)position.Y,
                    curTexture.Width + seperationBetweenCBAndText + child.BoundBox.Width,

                    Math.Max(curTexture.Height, child.BoundBox.Height));
            }
        }

        public Rectangle checkBoundBox
        {
            get
            {
                return new Rectangle((int)absolutePosition.X, (int)absolutePosition.Y,
                    curTexture.Width,
                    curTexture.Height);
            }
        }

        public override Microsoft.Xna.Framework.Rectangle ContentBoundBox
        {
            get
            {
                View child = checkBoxContentView;
                if (child == null)
                {
                    return curTexture.Bounds;
                }
                return new Rectangle((int)position.X + curTexture.Width + seperationBetweenCBAndText, (int)position.Y, child.BoundBox.Width, child.BoundBox.Height);
            }
        }

        public CheckBoxView(View rootView, Vector2 position, string label, SpriteFont labelFont)
            : base(rootView)
        {
            this.position = position;
            this.numChildrenAllowed = 1;
            TextView labelView = new TextView(this, label, Vector2.Zero, labelFont);

            textureChecked = GeeUI.texture_checkBoxDefaultChecked;
            textureCheckedSelected = GeeUI.texture_checkBoxSelectedChecked;
            textureDefault = GeeUI.texture_checkBoxDefault;
            textureDefaultSelected = GeeUI.texture_checkBoxSelected;
        }

        protected internal override void onMClick(Vector2 position, bool fromChild = false)
        {
            if (allowLabelClicking || fromChild == false)
                isChecked = !isChecked;
            base.onMClick(position);
        }
        protected internal override void onMClickAway(bool fromChild = false)
        {
            //base.onMClickAway();
        }

        protected internal override void onMOver(bool fromChild = false)
        {
            if (!allowLabelClicking && !checkBoundBox.Contains(InputManager.GetMousePos()))
                _mouseOver = false;
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
            spriteBatch.Draw(curTexture, absolutePosition, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
