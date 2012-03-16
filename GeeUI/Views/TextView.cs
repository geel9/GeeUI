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

        public TextJustification textJustification = TextJustification.Left;

        public override Rectangle BoundBox
        {
            get
            {
                int width = (int)font.MeasureString(text).X;
                int height = (int)font.MeasureString(text).Y;
                switch (textJustification)
                {
                    default:
                    case TextJustification.Left:
                        return new Rectangle(x, y, width, height);

                    case TextJustification.Center:
                        return new Rectangle(x - (width / 2), y, width, height);

                    case TextJustification.Right:
                        return new Rectangle(x - width, y, width, height);
                }
            }
        }

        private Vector2 textOrigin
        {
            get
            {
                int width = (int)font.MeasureString(text).X;
                int height = (int)font.MeasureString(text).Y;
                switch (textJustification)
                {
                    default:
                    case TextJustification.Left:
                        return new Vector2(0, 0);

                    case TextJustification.Center:
                        return new Vector2(width / 2, 0);

                    case TextJustification.Right:
                        return new Vector2(width, 0);
                }
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
            spriteBatch.DrawString(font, text, absolutePosition, textColor, 0f, textOrigin, 1f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }

        internal static string TruncateString(string input, SpriteFont font, int widthAllowed, string ellipsis = "...")
        {
            string cur = "";
            for (int i = 0; i < input.Length; i++)
            {
                float width = font.MeasureString(cur + input[i] + ellipsis).X;
                if (width > widthAllowed)
                    break;
                cur += input[i];
            }
            return cur + (cur.Length != input.Length ? ellipsis : "");
        }
    }
    public enum TextJustification
    {
        Left,
        Center,
        Right
    }
}
