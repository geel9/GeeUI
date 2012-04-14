using GeeUI.Structs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class WindowView : View
    {
        public NinePatch NinePatchSelected = new NinePatch();
        public NinePatch NinePatchNormal = new NinePatch();

        public string WindowText = "Hello this is a VIEW!";
        public SpriteFont WindowTextFont;

        protected internal bool SelectedOffChildren;
        protected internal Vector2 LastMousePosition = Vector2.Zero;
        protected internal Vector2 MouseSelectedOffset = Vector2.Zero;

        /// <summary>
        /// If true, the window can be dragged by the user.
        /// </summary>
        public bool Draggable = true;

        public View WindowContentView
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

        public override Rectangle BoundBox
        {
            get
            {
                NinePatch patch = Selected ? NinePatchSelected : NinePatchNormal;
                Rectangle childBoundBox = WindowContentView.BoundBox;
                Vector2 windowTextSize = WindowTextFont.MeasureString(WindowText);
                var height = (int)(patch.TopHeight + patch.BottomHeight + windowTextSize.Y + childBoundBox.Height);

                //The width of the content view is the width of the window title bar
                var width = childBoundBox.Width;
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
            }
        }

        public override Rectangle ContentBoundBox
        {
            get
            {
                return WindowContentView.BoundBox;
            }
        }

        public override Rectangle AbsoluteContentBoundBox
        {
            get
            {
                return WindowContentView.AbsoluteBoundBox;
            }
        }

        public WindowView(View rootView, Vector2 position, SpriteFont windowTextFont)
            : base(rootView)
        {
            Position = position;
            WindowTextFont = windowTextFont;
            NinePatchNormal = GeeUI.NinePatchWindowUnselected;
            NinePatchSelected = GeeUI.NinePatchWindowSelected;
        }

        protected internal void FollowMouse()
        {
            if (!Draggable) return;
            Vector2 newMousePosition = InputManager.GetMousePosV();
            if (SelectedOffChildren && Selected && InputManager.IsMousePressed(MouseButton.Left))
            {
                Position = (newMousePosition - MouseSelectedOffset);
            }
            LastMousePosition = newMousePosition;
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            NinePatch patch = Selected ? NinePatchSelected : NinePatchNormal;
            patch.Draw(spriteBatch, AbsolutePosition, WindowContentView.BoundBox.Width - (patch.LeftWidth + patch.RightWidth), (int)WindowTextFont.MeasureString(WindowText).Y);
            string text = TextView.TruncateString(WindowText, WindowTextFont, WindowContentView.ContentBoundBox.Width);
            spriteBatch.DrawString(WindowTextFont, text, AbsolutePosition + new Vector2(patch.LeftWidth, patch.TopHeight), Color.Black);
            base.Draw(spriteBatch);
        }

        protected internal override void Update(GameTime theTime)
        {
            FollowMouse();
            if (WindowContentView != null)
            {
                NinePatch patch = Selected ? NinePatchSelected : NinePatchNormal;
                Vector2 windowTextSize = WindowTextFont.MeasureString(WindowText);
                var height = (int)(patch.TopHeight + patch.BottomHeight + windowTextSize.Y);
                WindowContentView.Position = new Vector2(0, height);
            }

            base.Update(theTime);
        }

        protected internal override void OnMClick(Vector2 position, bool fromChild = false)
        {
            SelectedOffChildren = !fromChild;
            Selected = true;
            WindowContentView.Selected = true;
            LastMousePosition = position;
            MouseSelectedOffset = position - Position;

            if(ParentView != null)
            ParentView.BringChildToFront(this);
            FollowMouse();
            base.OnMClick(position, true);
        }

        protected internal override void OnMClickAway(bool fromChild = false)
        {
            SelectedOffChildren = false;
            Selected = false;
            WindowContentView.Selected = false;
            base.OnMClickAway(true);
        }
        protected internal override void OnMOff(bool fromChild = false)
        {
            FollowMouse();
            base.OnMOff(true);
        }

        protected internal override void OnMOver(bool fromChild = false)
        {
            FollowMouse();
            base.OnMOver(fromChild);
        }
    }
}
