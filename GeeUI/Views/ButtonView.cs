using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Structs;

namespace GeeUI.Views
{

    public class ButtonView : View
    {
        public NinePatch normalNinepatch = GeeUI.ninePatch_textFieldDefault;

        public SpriteFont font;

        public string text;


        public Color textColor = Color.Black;

        public override Rectangle BoundBox
        {
            get
            {
                NinePatch patch = normalNinepatch;
                int width = (int)(patch.leftWidth + patch.rightWidth + font.MeasureString(text).X);
                //int height = (int)(patch.topHeight + patch.bottomHeight + font.MeasureString(text).Y);
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public override Rectangle OffsetBoundBox
        {
            get
            {
                Rectangle normalBB = BoundBox;
                normalBB.X = (int)offsetPosition.X;
                normalBB.Y = (int)offsetPosition.Y;
                return normalBB;
            }
        }

        public ButtonView(View rootView, string text, Vector2 position, SpriteFont font)
            : base(rootView)
        {
            normalNinepatch = GeeUI.ninePatch_textFieldDefault;
            this.text = text;
            this.position = position;
            this.font = font;
        }

        protected internal override void onMClick(Vector2 position)
        {
            drawContent = true;
            //base.onMClick(position);
        }
        protected internal override void onMClickAway()
        {
            drawContent = false;
            base.onMClickAway();
        }

        protected internal override void onMOver()
        {
            drawContent = true;
            base.onMOver();
        }
        protected internal override void onMOff()
        {
            drawContent = false;
            base.onMOff();
        }

        protected internal override void Update(GameTime theTime)
        {
            base.Update(theTime);
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            NinePatch patch = normalNinepatch;
            patch.Draw(spriteBatch, offsetPosition, (int)font.MeasureString(text).X, /*(int)font.MeasureString(text).Y*/ height);
            if (drawContent)
            {
                patch.DrawContent(spriteBatch, offsetPosition, (int)font.MeasureString(text).X, /*(int)font.MeasureString(text).Y*/ height, Color.Red);
            }
            //spriteBatch.DrawString(font, text, new Vector2(offsetPosition.X + patch.leftWidth, offsetPosition.Y + patch.topHeight), textColor);
            base.Draw(spriteBatch);
        }
    }
}
