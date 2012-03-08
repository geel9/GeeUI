﻿using System;
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
        public NinePatch normalNinepatch;
        public NinePatch hoverNinepatch;
        public NinePatch clickedNinepatch;

        public SpriteFont font;

        public string text;

        public Color textColor = Color.Black;

        public override Rectangle BoundBox
        {
            get
            {
                NinePatch patch = currentNinepatch;
                int width = (int)(patch.leftWidth + patch.rightWidth + font.MeasureString(text).X);
                int height = (int)(patch.topHeight + patch.bottomHeight + font.MeasureString(text).Y);
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

        public NinePatch currentNinepatch
        {
            get
            {
                if (mouseOver)
                {
                    return InputManager.isLeftMousePressed() ? clickedNinepatch : hoverNinepatch;
                }
                else
                {
                    return normalNinepatch;
                }
            }
        }

        public ButtonView(View rootView, string text, Vector2 position, SpriteFont font)
            : base(rootView)
        {
            normalNinepatch = GeeUI.ninePatch_btnDefault;
            hoverNinepatch = GeeUI.ninePatch_btnHover;
            clickedNinepatch = GeeUI.ninePatch_btnClicked;
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
            NinePatch patch = currentNinepatch;
            patch.Draw(spriteBatch, absolutePosition, (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);
            spriteBatch.DrawString(font, text, new Vector2(absolutePosition.X + patch.leftWidth, absolutePosition.Y + patch.topHeight), textColor);
            base.Draw(spriteBatch);
        }
    }
}
