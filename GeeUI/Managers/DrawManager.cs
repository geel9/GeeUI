using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GeeUI.Managers
{
    public static class DrawManager
    {
        public static void Draw_Bezier(Vector2 point1, Vector2 point2, Color c, SpriteBatch b, byte alpha = 255)
        {
            Vector2 midpoint = ConversionManager.MidPoint(point1, point2);


            Vector2 c1 = new Vector2(midpoint.X, point1.Y);
            Vector2 c2 = new Vector2(midpoint.X, point2.Y);

            Vector2 lastPoint = point1;
            for (double t1 = 0; t1 <= 1; t1 += (double)((double)1 / (double)30))
            {
                float t = (float)t1;

                Vector2 point = ((1 - t) * (1 - t) * (1 - t)) * point1 + 3 * ((1 - t) * (1 - t)) * t * c1 + 3 * (1 - t) * (t * t) * c2 + (t * t * t) * point2;

                Draw_Line(point, lastPoint, c, b, alpha);

                lastPoint = point;
            }
        }

        public static void Draw_Line(Vector2 point1, Vector2 point2, Color c, SpriteBatch b, byte alpha = 255)
        {
            Vector2 midpoint = ConversionManager.MidPoint(point1, point2);
            float XD = midpoint.X - point2.X;
            float YD = midpoint.Y - point2.Y;
            double rotation = Math.Atan2(YD, XD);
            float dist = Vector2.Distance(midpoint, point2) * 2;
            Texture2D t = GeeUI.white;
            c.A = alpha;
            b.Draw(t, point2, null, c, (float)rotation, Vector2.Zero, new Vector2(dist, 1), SpriteEffects.None, 0);
        }

        public static void Draw_Box(Vector2 position, float width, float height, Color c, SpriteBatch b, float rotation = 0, byte alpha = 255, Effect e = null, bool wrong = false)
        {
            Vector2 topLeft = new Vector2(position.X - (width / 2), position.Y - (height / 2));
            Vector2 bottomRight = new Vector2(position.X + (width / 2), position.Y + (height / 2));

            Draw_Box(topLeft, bottomRight, c, b, rotation, alpha, e, wrong);
        }

        public static void Draw_Box(Vector2 topLeft, Vector2 bottomRight, Color c, SpriteBatch b, float rotation = 0, byte alpha = 255, Effect e = null, bool wrong = false)
        {
            if (e != null)
            {
                b.End();
                b.Begin(0, BlendState.NonPremultiplied, null, null, null, e);
            }
            Vector2 midpoint = ConversionManager.MidPoint(topLeft, bottomRight);
            float height = topLeft.Y - bottomRight.Y;
            float width = topLeft.X - bottomRight.X;
            Texture2D t = GeeUI.white;
            c.A = alpha;
            Vector2 pos = bottomRight;
            b.Draw(t, pos, null, c, rotation, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, 0);

            if (e != null)
            {
                b.End();
                b.Begin();
            }
        }

        public static void Draw_Circle(Vector2 position, float radius, Color c, Color cOutline, SpriteBatch b, byte alpha = 255, byte cutoff = 0)
        {
            Effect circleShader = GeeUI.circleShader;

            b.End();
            b.Begin(0, BlendState.NonPremultiplied, null, null, null, circleShader);
            float diameter = radius * 2;
            position.X -= radius;
            position.Y -= radius;
            circleShader.Parameters["aspect"].SetValue(diameter / diameter);
            circleShader.Parameters["cutoff"].SetValue((int)cutoff);
            circleShader.Parameters["outlineColor"].SetValue(new float[4] { (float)cOutline.R / 255, (float)cOutline.G / 255, (float)cOutline.B / 255, (float)cOutline.A / 255 });
            Texture2D t = GeeUI.white;
            c.A = alpha;
            b.Draw(t, position, null, c, 0f, Vector2.Zero, diameter, SpriteEffects.None, 0);
            b.End();
            b.Begin();
        }

        public static void Draw_Outline(Vector2 position, float width, float height, Color c, SpriteBatch b, byte alpha = 255, bool up = true, bool down = true, bool left = true, bool right = true)
        {
            Vector2 topLeft = new Vector2(position.X - (width / 2), position.Y - (height / 2));
            Vector2 bottomRight = new Vector2(position.X + (width / 2), position.Y + (height / 2));

            Draw_Outline(topLeft, bottomRight, c, b, alpha, up, down, left, right);
        }

        public static void Draw_Outline(Vector2 topLeft, Vector2 bottomRight, Color c, SpriteBatch b, byte alpha = 255, bool up = true, bool down = true, bool left = true, bool right = true)
        {
            Vector2 midpoint = ConversionManager.MidPoint(topLeft, bottomRight);
            Vector2 topRight = new Vector2(bottomRight.X, topLeft.Y);
            Vector2 bottomLeft = new Vector2(topLeft.X, bottomRight.Y);
            c.A = alpha;
            if(up)
            Draw_Line(topLeft, new Vector2(topRight.X + 1, topRight.Y), c, b, alpha);
            if(left)
            Draw_Line(topLeft, bottomLeft, c, b, alpha);
            if(right)
            Draw_Line(topRight, bottomRight, c, b, alpha);
            if(down)
            Draw_Line(bottomLeft, bottomRight, c, b, alpha);
        }

        public static bool PointInCircle(Vector2 origin, float radius, Vector2 point)
        {

            return ((point.X - origin.X) * (point.X - origin.X)) + ((point.Y - origin.Y) * (point.Y - origin.Y)) <= (radius * radius);
        }

        public static bool PointOnLine(Vector2 linePoint1, Vector2 linePoint2, Vector2 point)
        {
            float y1 = linePoint1.Y;
            float y2 = linePoint2.Y;
            float x1 = linePoint1.X;
            float x2 = linePoint2.X;
            float Slope = (y1 - y2) / (x1 - x2);
            float Intersect = -Slope * x1 / y2;
            if (x1 - x2 == 0)
            {
                return point.Y == x1 || point.Y == x2;
            }
            else
            {
                return point.Y == (Slope * point.X + Intersect);
            }
        }
    }
}
