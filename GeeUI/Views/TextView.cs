using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GeeUI.Views
{
    public class TextView : View
    {
        public SpriteFont Font;

        public string Text;

        public Color TextColor = Color.Black;

        public TextJustification TextJustification = TextJustification.Left;

        public override Rectangle BoundBox
        {
            get
            {
                var width = (int)Font.MeasureString(Text).X;
                var height = (int)Font.MeasureString(Text).Y;
                switch (TextJustification)
                {
                    default:
                        return new Rectangle(X, Y, width, height);

                    case TextJustification.Center:
                        return new Rectangle(X - (width / 2), Y, width, height);

                    case TextJustification.Right:
                        return new Rectangle(X - width, Y, width, height);
                }
            }
        }

        private Vector2 TextOrigin
        {
            get
            {
                var width = (int)Font.MeasureString(Text).X;
                switch (TextJustification)
                {
                    default:
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
            Text = text;
            Position = position;
            Font = font;

        }

        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            base.OnMClick(position);
        }
        protected internal override void OnMClickAway(bool fromChild = false)
        {
        }

        protected internal override void OnMOver(bool fromChild = false)
        {
            base.OnMOver();
        }
        protected internal override void OnMOff(bool fromChild = false)
        {
            base.OnMOff();
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, AbsolutePosition, TextColor, 0f, TextOrigin, 1f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }

        internal static string TruncateString(string input, SpriteFont font, int widthAllowed, string ellipsis = "...")
        {
            string cur = "";
            foreach (char t in input)
            {
                float width = font.MeasureString(cur + t + ellipsis).X;
                if (width > widthAllowed)
                    break;
                cur += t;
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
