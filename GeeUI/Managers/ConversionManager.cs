using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using SDI = System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace GeeUI.Managers
{
    public static class ConversionManager
    {
        public static Texture2D bitmapToTexture(Bitmap bmp)
        {
            using (MemoryStream s = new MemoryStream())
            {
                bmp.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                Texture2D tx = Texture2D.FromStream(GeeUI.theGame.GraphicsDevice, s);
                return tx;
            }
        }

        public static Vector2 MidPoint(Vector2 point1, Vector2 point2)
        {
            return new Vector2((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
        }

        public static string GetSafeName(string path)
        {
            Regex r = new Regex("(.+?\\\\)+(.+?)\\.(?=.+)");
            Match mc = r.Match(path);
            return mc.Groups[2].Value;
        }

        public static TimeSpan FloatToTime(float minutes)
        {
            double seconds = minutes * 60;
            return TimeSpan.FromSeconds(seconds);
        }

        public static double DegreeToRadians(double degree)
        {
            return degree * (Math.PI / 180);
        }


        public static double RadianToDegrees(double radian)
        {
            return radian * (180 / Math.PI);
        }

        public static Vector2 PToV(Microsoft.Xna.Framework.Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static Microsoft.Xna.Framework.Point VToP(Vector2 v)
        {
            return new Microsoft.Xna.Framework.Point((int)v.X, (int)v.Y);
        }
    }
}
