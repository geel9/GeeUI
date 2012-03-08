using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GeeUI.Views
{
    public class TextView : View
    {
        public SpriteFont font;

        public string text;

        public Color textColor = Color.Black;

        public override Rectangle BoundBox
        {
            get
            {
                int width = (int)font.MeasureString(text).X;
                int height = (int)font.MeasureString(text).Y;
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public override Rectangle AbsoluteBoundBox
        {
            get
            {
                Rectangle normalBB = BoundBox;
                normalBB.X = (int)absolutePosition.X;
                normalBB.Y = (int)absolutePosition.Y;
                return normalBB;
            }
        }

        public TextView(View rootView, string text, Vector2 position, SpriteFont font)
            : base(rootView)
        {
            this.text = text;
            this.position = position;
            this.font = font;
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
            spriteBatch.DrawString(font, text, absolutePosition, textColor);
            base.Draw(spriteBatch);
        }
    }
}
