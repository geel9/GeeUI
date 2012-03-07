using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GeeUI.Views;
using GeeUI.Structs;
using GeeUI.Managers;
namespace GeeUI
{
    public static class GeeUI
    {
        public static Texture2D white;
        public static Effect circleShader;

        public static View rootView = new View();

        internal static Game theGame;

        public static NinePatch ninePatch_textFieldDefault = new NinePatch();

        private static InputManager inputManager = new InputManager();

        public static void Initialize(Game theGame)
        {
            GeeUI.theGame = theGame;
            white = new Texture2D(theGame.GraphicsDevice, 1, 1);

            white.SetData<Color>(new Color[] { Color.White });
            rootView.width = theGame.Window.ClientBounds.Width;
            rootView.height = theGame.Window.ClientBounds.Height;

            Texture2D textFieldDefault = ConversionManager.bitmapToTexture(Resource1.textfield_default_9);
            ninePatch_textFieldDefault.LoadFromTexture(textFieldDefault);

            InputManager.BindMouse(() => handleClick(rootView, InputManager.GetMousePos()), MouseButton.Left);
            InputManager.BindMouse(() => handleMouseMovement(rootView, InputManager.GetMousePos()), MouseButton.Movement);
        }

        internal static void handleClick(View view, Point mousePos)
        {
            View[] sortedChildren = view.children;
            Array.Sort(sortedChildren, ViewDepthComparer.CompareDepths);
            bool didLower = false;
            for (int i = 0; i < sortedChildren.Length; i++)
            {
                View child = sortedChildren[i];
                if (child.OffsetBoundBox.Contains(mousePos))
                {
                    handleClick(child, mousePos);
                    didLower = true;
                    break;
                }
            }
            if (!didLower)
            {
                List<View> allOthers = getAllViews(rootView);
                for (int i = 0; i < allOthers.Count; i++)
                {
                    if (allOthers[i] != view)
                        allOthers[i].onMClickAway();
                }
                view.onMClick(ConversionManager.PToV(mousePos));
            }
        }

        internal static void handleMouseMovement(View view, Point mousePos)
        {
            View[] sortedChildren = view.children;
            Array.Sort(sortedChildren, ViewDepthComparer.CompareDepths);
            bool didLower = false;
            for (int i = 0; i < sortedChildren.Length; i++)
            {
                View child = sortedChildren[i];
                if (child.OffsetBoundBox.Contains(mousePos) && !didLower)
                {
                    handleMouseMovement(child, mousePos);
                    didLower = true;
                    child.mouseOver = true;
                }
                else
                    child.mouseOver = false;
            }
            if (!didLower)
            {
                view.mouseOver = true;
            }
        }

        public static void Update(GameTime gameTime)
        {
            inputManager.Update(gameTime);
            UpdateView(rootView, gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            DrawView(rootView, spriteBatch);
        }

        internal static void UpdateView(View toUpdate, GameTime gameTime)
        {
            View[] sortedChildren = toUpdate.children;
            for (int i = 0; i < sortedChildren.Length; i++)
            {
                View updating = sortedChildren[i];
                updating.Update(gameTime);
                UpdateView(updating, gameTime);
            }
        }

        internal static void DrawView(View toDraw, SpriteBatch spriteBatch)
        {
            View[] sortedChildren = toDraw.children;
            Array.Sort(sortedChildren, ViewDepthComparer.CompareDepthsInverse);
            for (int i = 0; i < sortedChildren.Length; i++)
            {
                View drawing = sortedChildren[i];
                drawing.Draw(spriteBatch);
                DrawView(drawing, spriteBatch);
            }
        }

        internal static List<View> getAllViews(View rootView)
        {
            List<View> ret = new List<View>();
            ret.Add(rootView);
            for (int i = 0; i < rootView.children.Length; i++)
            {
                View child = rootView.children[i];
                List<View> childChildren = getAllViews(child);
                for (int j = 0; j < childChildren.Count; j++)
                {
                    ret.Add(childChildren[j]);
                }
            }
            return ret;
        }
    }
}
