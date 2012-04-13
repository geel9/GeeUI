using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GeeUI.Managers;

namespace GeeUI.Structs
{
    public class NinePatch
    {
        public int leftMostPatch;
        public int rightMostPatch;
        public int topMostPatch;
        public int bottomMostPatch;

        public int leftWidth
        {
            get
            {
                return leftMostPatch - 1;
            }
        }
        public int rightWidth
        {
            get
            {
                if (texture != null)
                    return texture.Width - (rightMostPatch + 1);
                return 0;
            }
        }
        public int topHeight
        {
            get
            {
                return topMostPatch - 1;
            }
        }
        public int bottomHeight
        {
            get
            {
                if (texture != null)
                    return texture.Height - (bottomMostPatch + 1);
                return 0;
            }
        }

        public Texture2D texture;

        public NinePatch()
        {
            leftMostPatch = -1;
            rightMostPatch = -1;
            topMostPatch = -1;
            bottomMostPatch = -1;
            texture = null;
        }

        /// <summary>
        /// Method to determine if a texture has ninepatch data inside of it
        /// </summary>
        /// <param name="texture">The texture to test against</param>
        /// <returns>True if the texture is compatible with ninepatches, false otherwise</returns>
        public static bool isAlreadyNinepatch(Texture2D texture)
        {
            Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int i = 0; i < texture.Width; i++)
            {
                Color curPixel = data[i];
                int a = curPixel.A;
                int r = curPixel.R;
                int g = curPixel.G;
                int b = curPixel.B;

                if (a != 0 && (r != 0 || g != 0 || b != 0))
                {
                    //Is not black and is not transparent.
                    return false;
                }
            }
            for (int i = data.Length - (texture.Width + 1); i < data.Length; i++)
            {
                Color curPixel = data[i];
                int a = curPixel.A;
                int r = curPixel.R;
                int g = curPixel.G;
                int b = curPixel.B;
                if (a != 0 && (r != 0 || g != 0 || b != 0))
                {
                    //Is not black and is not transparent.
                    return false;
                }
            }
            for (int i = 0; i < data.Length; i += texture.Width)
            {
                Color curPixel = data[i];
                int a = curPixel.A;
                int r = curPixel.R;
                int g = curPixel.G;
                int b = curPixel.B;
                if (a != 0 && (r != 0 || g != 0 || b != 0))
                {
                    //Is not black and is not transparent.
                    return false;
                }
            }
            for (int i = texture.Width - 1; i < data.Length; i += texture.Width)
            {
                Color curPixel = data[i];
                int a = curPixel.A;
                int r = curPixel.R;
                int g = curPixel.G;
                int b = curPixel.B;
                if (a != 0 && (r != 0 || g != 0 || b != 0))
                {
                    //Is not black and is not transparent.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Load the NinePatch data from a texture.
        /// </summary>
        /// <param name="texture">The NinePatch-compatible texture to load from.</param>
        public void LoadFromTexture(Texture2D texture)
        {
            leftMostPatch = -1;
            rightMostPatch = -1;
            topMostPatch = -1;
            bottomMostPatch = -1;
            this.texture = texture;
            Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[texture.Width * texture.Height];
            texture.GetData(data);
            for (int i = 0; i < texture.Width; i++)
            {
                Color curPixel = data[i];
                if (curPixel.A != 0)
                {
                    if (leftMostPatch == -1) leftMostPatch = i;
                }
                if (curPixel.A != 0 && leftMostPatch != -1)
                {
                    rightMostPatch = i;
                }
                if (curPixel.A == 0 && leftMostPatch != -1 && rightMostPatch != -1)
                    break;
            }
            for (int i = 0; i < data.Length; i += texture.Width)
            {
                Color curPixel = data[i];
                if (curPixel.A != 0)
                {
                    if (topMostPatch == -1) topMostPatch = i / texture.Width;
                }
                if (curPixel.A != 0 && topMostPatch != -1)
                {
                    bottomMostPatch = (i / texture.Width);
                }
                if (curPixel.A == 0 && topMostPatch != -1 && bottomMostPatch != -1)
                    break;
            }
        }

        /// <summary>
        /// Draws the ninepatch at the specified point
        /// </summary>
        /// <param name="sb">The spritebatch to use for drawing</param>
        /// <param name="position">The position to draw it at (top left)</param>
        /// <param name="contentWidth">The width of the content inside the Ninepatch</param>
        /// <param name="contentHeight">The height of the content inside the Ninepatch</param>
        /// <param name="angle">The angle in degrees to rotate the ninepatch.</param>
        public void Draw(SpriteBatch sb, Vector2 position, int contentWidth, int contentHeight, float angle = 0)
        {
            Rectangle topLeft = new Rectangle(1, 1, leftMostPatch - 1, topMostPatch - 1);
            Rectangle topMiddle = new Rectangle(leftMostPatch, 1, (rightMostPatch - leftMostPatch), topMostPatch - 1);
            Rectangle topRight = new Rectangle(rightMostPatch + 1, 1, (texture.Width - 1) - rightMostPatch, topMostPatch - 1);

            Rectangle Left = new Rectangle(1, topMostPatch, leftMostPatch - 1, (bottomMostPatch - topMostPatch));
            Rectangle Middle = new Rectangle(leftMostPatch, topMostPatch, (rightMostPatch - leftMostPatch), (bottomMostPatch - topMostPatch));
            Rectangle Right = new Rectangle(rightMostPatch + 1, topMostPatch, (texture.Width - 1) - rightMostPatch, (bottomMostPatch - topMostPatch));

            Rectangle bottomLeft = new Rectangle(1, bottomMostPatch, leftMostPatch - 1, (texture.Height - 1) - bottomMostPatch);
            Rectangle bottomMiddle = new Rectangle(leftMostPatch, bottomMostPatch, (rightMostPatch - leftMostPatch), (texture.Height - 1) - bottomMostPatch);
            Rectangle bottomRight = new Rectangle(rightMostPatch + 1, bottomMostPatch, (texture.Width - 1) - rightMostPatch, (texture.Height - 1) - bottomMostPatch);

            int topMiddleWidth = topMiddle.Width;
            int leftMiddleHeight = Left.Height;
            float scaleMiddleByHorizontally = ((float)contentWidth / (float)topMiddleWidth);
            float scaleMiddleByVertically = ((float)contentHeight / (float)leftMiddleHeight);

            Vector2 drawTL = position;
            Vector2 drawT = drawTL + new Vector2(topLeft.Width, 0);
            Vector2 drawTR = drawT + new Vector2(topMiddle.Width * scaleMiddleByHorizontally, 0);

            Vector2 drawL = drawTL + new Vector2(0, topLeft.Height);
            Vector2 drawM = drawT + new Vector2(0, topMiddle.Height);
            Vector2 drawR = drawTR + new Vector2(0, topRight.Height);

            Vector2 drawBL = drawL + new Vector2(0, leftMiddleHeight * scaleMiddleByVertically);
            Vector2 drawBM = drawM + new Vector2(0, leftMiddleHeight * scaleMiddleByVertically);
            Vector2 drawBR = drawR + new Vector2(0, leftMiddleHeight * scaleMiddleByVertically);

            drawTL = rotateAroundOrigin(drawTL, position, angle);
            drawT = rotateAroundOrigin(drawT, position, angle);
            drawTR = rotateAroundOrigin(drawTR, position, angle);

            drawL = rotateAroundOrigin(drawL, position, angle);
            drawM = rotateAroundOrigin(drawM, position, angle);
            drawR = rotateAroundOrigin(drawR, position, angle);

            drawBL = rotateAroundOrigin(drawBL, position, angle);
            drawBM = rotateAroundOrigin(drawBM, position, angle);
            drawBR = rotateAroundOrigin(drawBR, position, angle);

            float angR = (float)ConversionManager.DegreeToRadians(angle);

            sb.Draw(texture, drawTL, topLeft, Color.White, angR, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            sb.Draw(texture, drawT, topMiddle, Color.White, angR, Vector2.Zero, new Vector2(scaleMiddleByHorizontally, 1), SpriteEffects.None, 0f);
            sb.Draw(texture, drawTR, topRight, Color.White, angR, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            sb.Draw(texture, drawL, Left, Color.White, angR, Vector2.Zero, new Vector2(1, scaleMiddleByVertically), SpriteEffects.None, 0f);
            sb.Draw(texture, drawM, Middle, Color.White, angR, Vector2.Zero, new Vector2(scaleMiddleByHorizontally, scaleMiddleByVertically), SpriteEffects.None, 0f);
            sb.Draw(texture, drawR, Right, Color.White, angR, Vector2.Zero, new Vector2(1, scaleMiddleByVertically), SpriteEffects.None, 0f);

            sb.Draw(texture, drawBL, bottomLeft, Color.White, angR, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            sb.Draw(texture, drawBM, bottomMiddle, Color.White, angR, Vector2.Zero, new Vector2(scaleMiddleByHorizontally, 1), SpriteEffects.None, 0f);
            sb.Draw(texture, drawBR, bottomRight, Color.White, angR, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draws a box where the content of a NinePatch will be placed.
        /// Does not draw the NinePatch.
        /// </summary>
        /// <param name="sb">The drawing SpriteBatch</param>
        /// <param name="position">The position to draw (top left)</param>
        /// <param name="contentWidth">The width of the content inside the Ninepatch</param>
        /// <param name="contentHeight">The height of the content inside the Ninepatch</param>
        /// <param name="drawColor">The color to draw </param>
        public void DrawContent(SpriteBatch sb, Vector2 position, int contentWidth, int contentHeight, Color drawColor)
        {
            Rectangle topLeft = new Rectangle(1, 1, leftMostPatch - 1, topMostPatch - 1);
            Rectangle topMiddle = new Rectangle(leftMostPatch, 1, (rightMostPatch - leftMostPatch), topMostPatch - 1);

            Rectangle Left = new Rectangle(1, topMostPatch, leftMostPatch - 1, (bottomMostPatch - topMostPatch));
            Rectangle Middle = new Rectangle(leftMostPatch, topMostPatch, (rightMostPatch - leftMostPatch), (bottomMostPatch - topMostPatch));

            int topMiddleWidth = topMiddle.Width;
            int leftMiddleHeight = Left.Height;
            float scaleMiddleByHorizontally = ((float)contentWidth / (float)topMiddleWidth);
            float scaleMiddleByVertically = ((float)contentHeight / (float)leftMiddleHeight);
            // if (scaleMiddleByVertically < 1) scaleMiddleByVertically = 1;
            // if (scaleMiddleByHorizontally < 1) scaleMiddleByHorizontally = 1;

            Vector2 drawTL = position;
            Vector2 drawT = drawTL + new Vector2(topLeft.Width, 0);

            Vector2 drawM = drawT + new Vector2(0, topMiddle.Height);

            Vector2 bottomRight = new Vector2(drawM.X + (Middle.Width * scaleMiddleByHorizontally), drawM.Y + (Middle.Height * scaleMiddleByVertically));
            DrawManager.Draw_Box(drawM, bottomRight, drawColor, sb, 0f, 150);
        }

        /// <summary>
        /// Gets the center of a NinePatch with the defined width and height
        /// </summary>
        /// <param name="contentWidth">The width of the NinePatch content</param>
        /// <param name="contentHeight">The height of the NinePatch content</param>
        /// <returns></returns>
        public Vector2 getCenter(int contentWidth, int contentHeight)
        {
            Rectangle topLeft = new Rectangle(1, 1, leftMostPatch - 1, topMostPatch - 1);
            Rectangle topRight = new Rectangle(rightMostPatch + 1, 1, (texture.Width - 1) - rightMostPatch, topMostPatch - 1);
            Rectangle topMiddle = new Rectangle(leftMostPatch, 1, (rightMostPatch - leftMostPatch), topMostPatch - 1);
            Rectangle Left = new Rectangle(1, topMostPatch, leftMostPatch - 1, (bottomMostPatch - topMostPatch));
            Rectangle Middle = new Rectangle(leftMostPatch, topMostPatch, (rightMostPatch - leftMostPatch), (bottomMostPatch - topMostPatch));


            int topMiddleWidth = topMiddle.Width;
            int leftMiddleHeight = Left.Height;
            float scaleMiddleByHorizontally = ((float)contentWidth / (float)topMiddleWidth);
            float scaleMiddleByVertically = ((float)contentHeight / (float)leftMiddleHeight);
            if (scaleMiddleByVertically < 1) scaleMiddleByVertically = 1;
            if (scaleMiddleByHorizontally < 1) scaleMiddleByHorizontally = 1;

            Vector2 drawMMiddle = new Vector2(topLeft.Width, topLeft.Height);
            drawMMiddle += new Vector2(topMiddleWidth * (scaleMiddleByHorizontally / 2), leftMiddleHeight * (scaleMiddleByVertically / 2));
            return drawMMiddle;
        }

        /// <summary>
        /// Rotates a point around a specified origin with the specified angle
        /// </summary>
        /// <param name="point">The point to rotate</param>
        /// <param name="origin">The rotation point of the point</param>
        /// <param name="angle">The angle in degrees of the angle to rotate the point by.</param>
        /// <returns></returns>
        public static Vector2 rotateAroundOrigin(Vector2 point, Vector2 origin, double angle)
        {
            Vector2 real = point - origin;
            Vector2 ret = Vector2.Zero;

            //We need to use radians for Math.* functions
            angle = ConversionManager.DegreeToRadians(angle);
            ret.X = (float)((real.X * Math.Cos(angle)) - (real.Y * Math.Sin(angle)));
            ret.Y = (float)((real.X * Math.Sin(angle)) + (real.Y * Math.Cos(angle)));

            ret += origin;
            return ret;
        }

        /// <summary>
        /// Rotates a point with the specified angle
        /// </summary>
        /// <param name="point">The point to rotate</param>
        /// <param name="angle">The angle in degrees of the angle to rotate the point by.</param>
        /// <returns></returns>
        public static Vector2 rotatePoint(Vector2 point, double angle)
        {
            Vector2 real = point;
            Vector2 ret = Vector2.Zero;

            //We need to use radians for Math.* functions
            angle = ConversionManager.DegreeToRadians(angle);
            ret.X = (float)((real.X * Math.Cos(angle)) - (real.Y * Math.Sin(angle)));
            ret.Y = (float)((real.X * Math.Sin(angle)) + (real.Y * Math.Cos(angle)));

            return ret;
        }
    }
}
