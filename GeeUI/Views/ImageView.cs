using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeeUI.Views
{
    public class ImageView : View
    {
        public Texture2D texture;

        public View buttonContentview
        {
            get
            {
                if (children.Length == 0)
                {
                    return null;
                }
                else
                {
                    return children[0];
                }
            }
            set
            {
                if (this.children.Length == 0)
                {
                    addChild(value);
                    return;
                }
                _children[0] = value;
                reOrderChildren();
            }
        }

        public override Rectangle BoundBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)scaledImageSize.X, (int)scaledImageSize.Y);
            }
        }

        public Vector2 scaleVector
        {
            get
            {
                return new Vector2((float)width / (float)texture.Width, (float)height / (float)texture.Height);
            }
            set
            {
                width = (int)((float)texture.Width * value.X);
                height = (int)((float)texture.Height * value.Y);
            }
        }

        public Vector2 scaledImageSize
        {
            get
            {
                return new Vector2((float)width * scaleVector.X, (float)height * scaleVector.Y);
            }
        }


        public ImageView(View rootView, Texture2D texture)
            : base(rootView)
        {
            this.position = position;
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;
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

            spriteBatch.Draw(texture, absolutePosition, null, Color.White, 0f, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);

            base.Draw(spriteBatch);
        }
    }
}
