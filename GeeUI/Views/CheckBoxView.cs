using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class CheckBoxView : View
    {
        public Texture2D TextureDefault;
        public Texture2D TextureChecked;
        public Texture2D TextureDefaultSelected;
        public Texture2D TextureCheckedSelected;

        public bool IsChecked;
        public bool AllowLabelClicking = true;

        private const int SeperationBetweenCbAndText = 3;

        public View CheckBoxContentView
        {
            get
            {
                return Children.Length == 0 ? null : Children[0];
            }
            set
            {
                if (Children.Length == 0)
                {
                    AddChild(value);
                    return;
                }
                _children[0] = value;
                ReOrderChildren();
            }
        }

        public Texture2D CurTexture
        {
            get
            {
                if (Selected || MouseOver)
                    return IsChecked ? TextureCheckedSelected : TextureDefaultSelected;
                return IsChecked ? TextureChecked : TextureDefault;
            }
        }

        public override Rectangle BoundBox
        {
            get
            {
                View child = CheckBoxContentView;
                if (child == null)
                {
                    return CurTexture.Bounds;
                }
                return new Rectangle((int)Position.X, (int)Position.Y,
                    CurTexture.Width + SeperationBetweenCbAndText + child.BoundBox.Width,

                    Math.Max(CurTexture.Height, child.BoundBox.Height));
            }
        }

        public Rectangle CheckBoundBox
        {
            get
            {
                return new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y,
                    CurTexture.Width,
                    CurTexture.Height);
            }
        }

        public override Rectangle ContentBoundBox
        {
            get
            {
                View child = CheckBoxContentView;
                if (child != null)
                {
                    return new Rectangle((int)Position.X + CurTexture.Width + SeperationBetweenCbAndText,
                                         (int)Position.Y, child.BoundBox.Width, child.BoundBox.Height);
                }
                return CurTexture.Bounds;
            }
        }

        public CheckBoxView(View rootView, Vector2 position, string label, SpriteFont labelFont)
            : base(rootView)
        {
            Position = position;
            NumChildrenAllowed = 1;

            new TextView(this, label, Vector2.Zero, labelFont);

            TextureChecked = GeeUI.TextureCheckBoxDefaultChecked;
            TextureCheckedSelected = GeeUI.TextureCheckBoxSelectedChecked;
            TextureDefault = GeeUI.TextureCheckBoxDefault;
            TextureDefaultSelected = GeeUI.TextureCheckBoxSelected;
        }

        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            if (AllowLabelClicking || fromChild == false)
                IsChecked = !IsChecked;
            base.OnMClick(position);
        }
        protected internal override void OnMClickAway(bool fromChild = false)
        {
            //base.onMClickAway();
        }

        protected internal override void OnMOver(bool fromChild = false)
        {
            if (!AllowLabelClicking && !CheckBoundBox.Contains(InputManager.GetMousePos()))
                _mouseOver = false;
            base.OnMOver();
        }
        protected internal override void OnMOff(bool fromChild = false)
        {
            base.OnMOff();
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurTexture, AbsolutePosition, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
