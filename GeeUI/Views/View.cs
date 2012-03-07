using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Structs;

namespace GeeUI.Views
{
    public class View
    {
        public delegate void MouseClickEventHandler(object sender, EventArgs e);
        public delegate void MouseOverEventHandler(object sender, EventArgs e);
        public delegate void MouseOffEventHandler(object sender, EventArgs e);

        public event MouseClickEventHandler onMouseClick;
        public event MouseOverEventHandler onMouseOver;
        public event MouseOffEventHandler onMouseOff;

        public View parentView = null;

        public int childrenDepth = 0;
        public int thisDepth = 0;

        public bool ignoreParentBounds = false;
        public bool selected = false;
        public bool active = true;

        private bool _mouseOver = false;
        public bool mouseOver
        {
            get
            {
                return _mouseOver;
            }
            set
            {
                _mouseOver = value;
                if (value == true)
                    onMOver();
                else
                    onMOff();
            }
        }

        public virtual Rectangle BoundBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public virtual Rectangle OffsetBoundBox
        {
            get
            {
                if (parentView == null) return BoundBox;
                Rectangle curBB = BoundBox;
                return new Rectangle((int)offsetPosition.X, (int)offsetPosition.Y, curBB.Width, curBB.Height);
            }
        }

        public Vector2 position = Vector2.Zero;
        public Vector2 offsetPosition
        {
            get
            {
                if (parentView == null) return position;
                return position + parentView.offsetPosition;
            }
        }

        public int width = 0, height = 0;

        private List<View> _children = new List<View>();

        public View[] children
        {
            get
            {
                return _children.ToArray();
            }
        }

        protected internal View()
        {
        }

        public View(View parentView)
        {
            if (parentView != null)
                parentView.addChild(this);
            else
                throw new Exception("You cannot create a View without a parent; please use GeeUI.rootView for what you want.");
        }

        public void addChild(View child)
        {
            child.parentView = this;
            child.thisDepth = childrenDepth++;
            _children.Add(child);

        }

        public void bringChildToFront(View view)
        {
            _children.Remove(view);
            List<View> sortedChildren = _children;
            sortedChildren.Sort(ViewDepthComparer.CompareDepths);
            sortedChildren.Add(view);
            _children = sortedChildren;
            childrenDepth = 0;
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].thisDepth = i;
                childrenDepth++;
            }

        }

        public void reOrderChildren()
        {
            View[] sortedChildren = children;
            Array.Sort(sortedChildren, ViewDepthComparer.CompareDepths);
            childrenDepth = 0;

            for (int i = 0; i < sortedChildren.Length; i++)
            {
                _children[i].thisDepth = i;
                childrenDepth++;
            }
        }

        protected internal virtual void onMClick(Vector2 position, bool fromChild = false)
        {
            if (onMouseClick != null)
                onMouseClick(this, new EventArgs());
            if (parentView != null) parentView.onMClick(position, true);
        }

        protected internal virtual void onMClickAway(bool fromChild = false)
        {

        }

        protected internal virtual void onMOver(bool fromChild = false)
        {
            if (onMouseOver != null)
                onMouseOver(this, new EventArgs());
            if (parentView != null) parentView.onMOver(true);
        }

        protected internal virtual void onMOff(bool fromChild = false)
        {
            if (onMouseOff != null)
                onMouseOff(this, new EventArgs());
            if (parentView != null) parentView.onMOff(true);
        }

        protected internal virtual void Update(GameTime theTime)
        {
            if (parentView == null || ignoreParentBounds) return;
            Rectangle curBB = OffsetBoundBox;
            Rectangle parentBB = parentView.OffsetBoundBox;
            int xOffset = curBB.Right - parentBB.Right;
            int yOffset = curBB.Bottom - parentBB.Bottom;
            if (xOffset > 0) this.position.X -= xOffset;
            else
            {
                xOffset = curBB.Left - parentBB.Left;
                if (xOffset < 0)
                    this.position.X -= xOffset;
            }
            if (yOffset > 0) this.position.Y -= yOffset;
            else
            {
                yOffset = curBB.Top - parentBB.Top;
                if (yOffset < 0) position.Y -= yOffset;
            }
        }

        protected internal virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
