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
    public delegate void OnKeyPressed(string keyPressed, Keys key);
    public delegate void OnKeyReleased(string keyReleased, Keys key);
    public delegate void OnKeyContinuallyPressed(string keyContinuallyPressed, Keys key);

    public static class GeeUI
    {
        public static event OnKeyPressed OnKeyPressedHandler;
        public static event OnKeyReleased OnKeyReleasedHandler;
        public static event OnKeyContinuallyPressed OnKeyContinuallyPressedHandler;

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

        public static NinePatch ninePatch_panelSelected = new NinePatch();
        public static NinePatch ninePatch_panelUnselected = new NinePatch();

        public static Texture2D texture_checkBoxDefault;
        public static Texture2D texture_checkBoxSelected;
        public static Texture2D texture_checkBoxDefaultChecked;
        public static Texture2D texture_checkBoxSelectedChecked;


        public static Texture2D texture_sliderSelected;
        public static Texture2D texture_sliderDefault;
        public static NinePatch ninePatch_sliderRange = new NinePatch();

        private static InputManager inputManager = new InputManager();

        internal static void InitializeKeybindings()
        {
            string[] toBindUpper = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z ) ! @ # $ % ^ & * ( ? > < \" : } { _ + 0 1 2 3 4 5 6 7 8 9       ".Split(' ');
            string[] toBindLower = "a b c d e f g h i j k l m n o p q r s t u v w x y z 0 1 2 3 4 5 6 7 8 9 / . , ' ; ] [ - = 0 1 2 3 4 5 6 7 8 9       ".Split(' ');
            Keys[] toBind = {Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z
                            , Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.OemQuestion, Keys.OemPeriod, Keys.OemComma, Keys.OemQuotes,
                            Keys.OemSemicolon, Keys.OemCloseBrackets, Keys.OemOpenBrackets, Keys.OemMinus, Keys.OemPlus, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9
                            , Keys.Space, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Enter, Keys.Back};

            for (int i = 0; i < toBindUpper.Length; i++)
            {
                String upper = toBindUpper[i];
                String lower = toBindLower[i];
                Keys bind = toBind[i];

                InputManager.BindKey(() =>
                {
                    bool shiftHeld = InputManager.isKeyPressed(Keys.LeftShift) || InputManager.isKeyPressed(Keys.RightShift);
                    if (OnKeyPressedHandler == null) return;
                    OnKeyPressedHandler(shiftHeld ? upper : lower, bind);
                }, bind);
                InputManager.BindKey(() =>
                {
                    bool shiftHeld = InputManager.isKeyPressed(Keys.LeftShift) || InputManager.isKeyPressed(Keys.RightShift);
                    if (OnKeyReleasedHandler == null) return;
                    OnKeyReleasedHandler(shiftHeld ? upper : lower, bind);
                }, bind, false, false);
                InputManager.BindKey(() =>
                {
                    bool shiftHeld = InputManager.isKeyPressed(Keys.LeftShift) || InputManager.isKeyPressed(Keys.RightShift);
                    if (OnKeyContinuallyPressedHandler == null) return;
                    OnKeyContinuallyPressedHandler(shiftHeld ? upper : lower, bind);
                }, bind, true);
            }
        }

        public static void Initialize(Game theGame)
        {
            GeeUI.theGame = theGame;
            white = new Texture2D(theGame.GraphicsDevice, 1, 1);

            Vector2 test = new Vector2(15, 5);
            Vector2 origin = new Vector2(2, 2);
            Vector2 ret = NinePatch.rotateAroundOrigin(test, origin, 45);

            white.SetData<Color>(new Color[] { Color.White });
            rootView.width = theGame.Window.ClientBounds.Width;
            rootView.height = theGame.Window.ClientBounds.Height;

            Texture2D textFieldDefault = ConversionManager.bitmapToTexture(Resource1.textfield_default_9);
            Texture2D textFieldSelected = ConversionManager.bitmapToTexture(Resource1.textfield_selected_9);
            Texture2D textFieldRight = ConversionManager.bitmapToTexture(Resource1.textfield_selected_right_9);
            Texture2D textFieldWrong = ConversionManager.bitmapToTexture(Resource1.textfield_selected_wrong_9);

            Texture2D windowSelected = ConversionManager.bitmapToTexture(Resource1.window_selected_9);
            Texture2D windowUnselected = ConversionManager.bitmapToTexture(Resource1.window_unselected_9);

            Texture2D panelSelected = ConversionManager.bitmapToTexture(Resource1.panel_selected_9);
            Texture2D panelUnselected = ConversionManager.bitmapToTexture(Resource1.panel_unselected_9);

            Texture2D btnDefault = ConversionManager.bitmapToTexture(Resource1.btn_default_9);
            Texture2D btnClicked = ConversionManager.bitmapToTexture(Resource1.btn_clicked_9);
            Texture2D btnHover = ConversionManager.bitmapToTexture(Resource1.btn_hover_9);

            Texture2D sliderRange = ConversionManager.bitmapToTexture(Resource1.sliderRange_9);
            texture_sliderDefault = ConversionManager.bitmapToTexture(Resource1.slider);
            texture_sliderSelected = ConversionManager.bitmapToTexture(Resource1.sliderSelected);

            ninePatch_sliderRange.LoadFromTexture(sliderRange);

            texture_checkBoxDefault = ConversionManager.bitmapToTexture(Resource1.checkbox_default);
            texture_checkBoxSelected = ConversionManager.bitmapToTexture(Resource1.checkbox_default_selected);
            texture_checkBoxDefaultChecked = ConversionManager.bitmapToTexture(Resource1.checkbox_checked);
            texture_checkBoxSelectedChecked = ConversionManager.bitmapToTexture(Resource1.checkbox_checked_selected);

            ninePatch_textFieldDefault.LoadFromTexture(textFieldDefault);
            ninePatch_textFieldSelected.LoadFromTexture(textFieldSelected);
            ninePatch_textFieldRight.LoadFromTexture(textFieldRight);
            ninePatch_textFieldWrong.LoadFromTexture(textFieldWrong);

            ninePatch_windowSelected.LoadFromTexture(windowSelected);
            ninePatch_windowUnselected.LoadFromTexture(windowUnselected);

            ninePatch_panelUnselected.LoadFromTexture(panelUnselected);
            ninePatch_panelSelected.LoadFromTexture(panelSelected);

            ninePatch_btnDefault.LoadFromTexture(btnDefault);
            ninePatch_btnClicked.LoadFromTexture(btnClicked);
            ninePatch_btnHover.LoadFromTexture(btnHover);

            InitializeKeybindings();

            InputManager.BindMouse(() =>
            {
                handleClick(rootView, InputManager.GetMousePos());
                //When we click, we want to re-evaluate what control the mouse is over.
                handleMouseMovement(rootView, InputManager.GetMousePos());
            }, MouseButton.Left);
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
