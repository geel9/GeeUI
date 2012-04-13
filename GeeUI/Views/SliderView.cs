using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeeUI.Structs;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class SliderView : View
    {
        public delegate void SliderValueChangedHandler(object sender, EventArgs e);

        public event SliderValueChangedHandler onSliderValueChanged;

        public Texture2D sliderDefault;
        public Texture2D sliderSelected;

        public NinePatch sliderRange = new NinePatch();

        private int min = 0, max = 0;

        private bool clicked = false;

        private bool _drawText = false;
        public bool drawText
        {
            get
            {
                return _drawText && textFont != null;
            }
            set
            {
                //Only let text be drawn if the user has set the font.
                _drawText = textFont != null && value;
                if (textFont == null)
                {
                    throw new Exception("Cannot set SliderView.drawText to true unless textFont is set.");
                }       
            }
        }

        public SpriteFont textFont = null;
        public Color textColor = Color.Black;

        public int currentValue
        {
            get
            {
                float percent = (float)(sliderPosition) / (float)width;
                return (int)(min + ((float)(max - min) * percent));
            }
        }

        public int sliderPosition = 0;

        public Texture2D curSliderTexture
        {
            get
            {
                return mouseOver || selected || clicked ? sliderSelected : sliderDefault;
            }
        }

        public override Microsoft.Xna.Framework.Rectangle BoundBox
        {
            get
            {
                return new Rectangle(x, y, sliderRange.leftWidth + sliderRange.rightWidth + width, (int)MathHelper.Max(sliderRange.texture.Height, sliderDefault.Height));
            }
        }

        public SliderView(View rootView, Vector2 position, int min, int max)
            : base(rootView)
        {
            sliderRange = GeeUI.ninePatch_sliderRange;
            sliderDefault = GeeUI.texture_sliderDefault;
            sliderSelected = GeeUI.texture_sliderSelected;

            this.min = min;
            this.max = max;

            this.position = position;
        }

        protected internal override void onMClick(Vector2 position, bool fromChild = false)
        {
            sliderCalc(position);
            clicked = true;
            base.onMClick(position);
        }
        protected internal override void onMClickAway(bool fromChild = false)
        {
            clicked = false;
            //base.onMClickAway();
        }

        protected internal override void onMOver(bool fromChild = false)
        {
            Vector2 position = InputManager.GetMousePosV();
            sliderCalc(position);
            base.onMOver();
        }
        protected internal override void onMOff(bool fromChild = false)
        {
            Vector2 position = InputManager.GetMousePosV();
            sliderCalc(position);
            base.onMOff();
        }

        private void sliderCalc(Vector2 position)
        {
            if (clicked)
            {
                if (InputManager.isLeftMousePressed())
                {
                    sliderPosition = (int)MathHelper.Clamp((int)(position.X - absoluteX + sliderRange.leftWidth), 0, width);
                    if (onSliderValueChanged != null)
                        onSliderValueChanged(null, null);
                }
                else
                {
                    clicked = false;
                }
            }
        }


        protected internal override void Update(GameTime theTime)
        {
            if (min > max)
            {
                throw new Exception("The minimum value of a slider cannot be above the maximum.");
            }
            if (sliderPosition > width) sliderPosition = width;
            if (sliderPosition < 0) sliderPosition = 0;
            base.Update(theTime);
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            //We want to preserve the slider skin's original height.
            sliderRange.Draw(spriteBatch, absolutePosition, width, sliderRange.bottomMostPatch - sliderRange.topMostPatch);
            spriteBatch.Draw(curSliderTexture, new Vector2(absoluteX + sliderRange.leftWidth - (curSliderTexture.Width) + sliderPosition, absoluteY), null, Color.White, 0f, new Vector2(curSliderTexture.Width / -2, 0), 1f, SpriteEffects.None, 0f);
            if (drawText)
            {
                int drawX = absoluteX + (sliderRange.leftWidth + width + sliderRange.rightWidth) / 2;
                int drawY = absoluteY;
                Vector2 offset = textFont.MeasureString(currentValue.ToString());
                offset.X = (int)(offset.X / 2);
                spriteBatch.DrawString(textFont, currentValue.ToString(), new Vector2(drawX, drawY), textColor, 0f, offset, 1f, SpriteEffects.None, 0f);
            }
            base.Draw(spriteBatch);
        }
    }
}
