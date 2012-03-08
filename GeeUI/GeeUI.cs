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
        public static NinePatch ninePatch_textFieldSelected = new NinePatch();
        public static NinePatch ninePatch_textFieldRight = new NinePatch();
        public static NinePatch ninePatch_textFieldWrong = new NinePatch();

        public static NinePatch ninePatch_btnDefault = new NinePatch();
        public static NinePatch ninePatch_btnHover = new NinePatch();
        public static NinePatch ninePatch_btnClicked = new NinePatch();

        public static NinePatch ninePatch_windowSelected = new NinePatch();
        public static NinePatch ninePatch_windowUnselected = new NinePatch();

        private static InputManager inputManager = new InputManager();

        public static void Initialize(Game theGame)
        {
            GeeUI.theGame = theGame;
            white = new Texture2D(theGame.GraphicsDevice, 1, 1);

            white.SetData<Color>(new Color[] { Color.White });
            rootView.width = theGame.Window.ClientBounds.Width;
            rootView.height = theGame.Window.ClientBounds.Height;

            Texture2D textFieldDefault = ConversionManager.bitmapToTexture(Resource1.textfield_default_9);
            Texture2D textFieldSelected = ConversionManager.bitmapToTexture(Resource1.textfield_selected_9);
            Texture2D textFieldRight = ConversionManager.bitmapToTexture(Resource1.textfield_selected_right_9);
            Texture2D textFieldWrong = ConversionManager.bitmapToTexture(Resource1.textfield_selected_wrong_9);

            Texture2D windowSelected = ConversionManager.bitmapToTexture(Resource1.window_selected_9);
            Texture2D windowUnselected = ConversionManager.bitmapToTexture(Resource1.window_unselected_9);

            Texture2D btnDefault = ConversionManager.bitmapToTexture(Resource1.btn_default_9);
            Texture2D btnClicked = ConversionManager.bitmapToTexture(Resource1.btn_clicked_9);
            Texture2D btnHover = ConversionManager.bitmapToTexture(Resource1.btn_hover_9);

            ninePatch_textFieldDefault.LoadFromTexture(textFieldDefault);
            ninePatch_textFieldSelected.LoadFromTexture(textFieldSelected);
            ninePatch_textFieldRight.LoadFromTexture(textFieldRight);
            ninePatch_textFieldWrong.LoadFromTexture(textFieldWrong);

            ninePatch_windowSelected.LoadFromTexture(windowSelected);
            ninePatch_windowUnselected.LoadFromTexture(windowUnselected);

            ninePatch_btnDefault.LoadFromTexture(btnDefault);
            ninePatch_btnClicked.LoadFromTexture(btnClicked);
            ninePatch_btnHover.LoadFromTexture(btnHover);

            InputManager.BindMouse(() => handleClick(rootView, InputManager.GetMousePos()), MouseButton.Left);
            InputManager.BindMouse(() => handleMouseMovement(rootView, InputManager.GetMousePos()), MouseButton.Movement);
        }

        internal static void handleClick(View view, Point mousePos)
        {
            if (!view.active)
                return;
            View[] sortedChildren = view.children;
            Array.Sort(sortedChildren, ViewDepthComparer.CompareDepths);
            bool didLower = false;
            for (int i = 0; i < sortedChildren.Length; i++)
            {
                View child = sortedChildren[i];
                if (child.AbsoluteBoundBox.Contains(mousePos) && child.active)
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
            if (!view.active) return;
            View[] sortedChildren = view.children;
            Array.Sort(sortedChildren, ViewDepthComparer.CompareDepths);
            bool didLower = false;
            if (view.parentView == null)
            {
                //The first call
                List<View> allViews = getAllViews(rootView);
                for (int i = 0; i < allViews.Count; i++)
                {
                    allViews[i].mouseOver = false;
                }
            }
            for (int i = 0; i < sortedChildren.Length; i++)
            {
                View child = sortedChildren[i];
                if (child.AbsoluteBoundBox.Contains(mousePos) && !didLower)
                {
                    handleMouseMovement(child, mousePos);
                    didLower = true;
                    child.mouseOver = true;
                    break;
                }
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
                if (!updating.active) continue;
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
                if (!drawing.active) continue;
                drawing.Draw(spriteBatch);
                DrawView(drawing, spriteBatch);
            }
        }

        internal static List<View> getAllViews(View rootView)
        {
            List<View> ret = new List<View>();
            if (!rootView.active) return ret;
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
