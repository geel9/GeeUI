using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Structs;
using GeeUI.Managers;

namespace GeeUI.Views
{

    public class ButtonView : View
    {
        public NinePatch NinePatchNormal;
        public NinePatch NinePatchHover;
        public NinePatch NinePatchClicked;

        public View ButtonContentview
        {
            get {
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
                ReOrderChildrenDepth();
            }
        }

        public string Text
        {
            get
            {
                if(Children[0] is TextView)
                {
                    TextView c = (TextView) Children[0];
                    return c.Text;
                }
                return "";
            }
            set
            {
                if (Children[0] is TextView)
                {
                    TextView c = (TextView)Children[0];
                    c.Text = value;
                }
            }
        }

        public override Rectangle BoundBox
        {
            get
            {
                NinePatch patch = CurrentNinepatch;
                int width = patch.LeftWidth + patch.RightWidth + (ButtonContentview != null ? ButtonContentview.BoundBox.Width : 0);
                int height = patch.TopHeight + patch.BottomHeight + (ButtonContentview != null ? ButtonContentview.BoundBox.Height : 0);
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
            }
        }

        public NinePatch CurrentNinepatch
        {
            get
            {
                if (MouseOver)
                {
                    return InputManager.IsMousePressed(MouseButton.Left) ? NinePatchClicked : NinePatchHover;
                }
                return NinePatchNormal;
            }
        }

        public ButtonView(View rootView, string text, Vector2 position, SpriteFont font)
            : base(rootView)
        {
            NinePatchNormal = GeeUI.NinePatchBtnDefault;
            NinePatchHover = GeeUI.NinePatchBtnHover;
            NinePatchClicked = GeeUI.NinePatchBtnClicked;
            Position = position;

            //Make the TextView for the text
            new TextView(this, text, new Vector2(0, 0), font);
        }

        public ButtonView(View rootview, View contentView, Vector2 position) : base(rootview)
        {
            NinePatchNormal = GeeUI.NinePatchBtnDefault;
            NinePatchHover = GeeUI.NinePatchBtnHover;
            NinePatchClicked = GeeUI.NinePatchBtnClicked;
            Position = position;
            ButtonContentview = contentView;
        }

        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            base.OnMClick(position);
        }
        protected internal override void OnMClickAway(bool fromChild = false)
        {
            //base.onMClickAway();
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
            NinePatch patch = CurrentNinepatch;
            int width = ButtonContentview != null ? ButtonContentview.BoundBox.Width : 0;
            int height = ButtonContentview != null ? ButtonContentview.BoundBox.Height : 0;
            patch.Draw(spriteBatch, AbsolutePosition, width, height);

            View childView = ButtonContentview;
            if (childView != null)
            {
                childView.X = patch.LeftWidth;
                childView.Y = patch.TopHeight;
            }

            base.Draw(spriteBatch);
        }
    }
}
