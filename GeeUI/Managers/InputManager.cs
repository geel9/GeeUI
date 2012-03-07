using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GeeUI.Structs;
namespace GeeUI.Managers
{
    public class InputManager
    {
        private static KeyboardState keyboardState;
        private static KeyboardState oldKeyboardState;
        private static MouseState mouseState;
        private static MouseState oldMouseState;
        private int scrollValue = 0;

        private static List<CodeBoundMouse> boundMouse = new List<CodeBoundMouse>();
        private static List<CodeBoundKey> boundKey = new List<CodeBoundKey>();

        private static List<CodeBoundMouse> toBindMouse = new List<CodeBoundMouse>();
        private static List<CodeBoundKey> toBindKey = new List<CodeBoundKey>();

        public static void BindKey(Action lambda, Keys key, bool constant = false, bool press = true)
        {
            toBindKey.Add(new CodeBoundKey(lambda, key, constant, press));
        }

        public static void BindMouse(Action a, MouseButton button, bool press = true, bool constant = false)
        {
            toBindMouse.Add(new CodeBoundMouse(a, button, press, constant));
        }

        public static Point GetMousePos()
        {
            return new Point(mouseState.X, mouseState.Y);
        }

        public static Vector2 GetMousePosV()
        {
            return new Vector2(GetMousePos().X, GetMousePos().Y);
        }

        public void Update(GameTime time)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            int scroll = mouseState.ScrollWheelValue - scrollValue;
            scrollValue = mouseState.ScrollWheelValue;
            if (GeeUI.theGame.IsActive)
            {
                foreach (CodeBoundMouse b in boundMouse)
                {
                    switch (b.boundMouseButton)
                    {
                        case MouseButton.Left:
                            if (mouseState.LeftButton != oldMouseState.LeftButton || b.constant)
                            {
                                if ((b.press && mouseState.LeftButton == ButtonState.Pressed) ||
                                    (!b.press && mouseState.LeftButton == ButtonState.Released))
                                    b.lambda();
                            }
                            break;
                        case MouseButton.Middle:
                            if (mouseState.MiddleButton != oldMouseState.MiddleButton || b.constant)
                            {
                                if ((b.press && mouseState.MiddleButton == ButtonState.Pressed) ||
                                    (!b.press && mouseState.MiddleButton == ButtonState.Released))
                                    b.lambda();
                            }
                            break;

                        case MouseButton.Right:
                            if (mouseState.RightButton != oldMouseState.RightButton || b.constant)
                            {
                                if ((b.press && mouseState.RightButton == ButtonState.Pressed) ||
                                    (!b.press && mouseState.RightButton == ButtonState.Released))
                                    b.lambda();
                            }
                            break;

                        case MouseButton.Scroll:
                            if (scroll != 0)
                                b.lambda();
                            break;

                        case MouseButton.Scrolldown:
                            if (scroll < 0)
                                b.lambda();
                            break;
                        case MouseButton.Scrollup:
                            if (scroll > 0)
                                b.lambda();
                            break;

                        case MouseButton.Movement:
                            if (mouseState.Y != oldMouseState.Y || mouseState.X != oldMouseState.X)
                            {
                                b.lambda();
                            }
                            break;
                    }
                }

                foreach (CodeBoundKey b in boundKey)
                {
                    Keys k = b.boundKey;
                    bool newP = keyboardState.IsKeyDown(k);
                    bool oldP = oldKeyboardState.IsKeyDown(k);

                    if ((newP != oldP || b.constant) && b.press == newP)
                    {
                        b.lambda();
                    }
                }
            }

            foreach (CodeBoundMouse cbm in toBindMouse)
            {
                boundMouse.Add(cbm);
            }
            foreach (CodeBoundKey cbk in toBindKey)
            {
                boundKey.Add(cbk);
            }

            toBindKey.Clear();
            toBindMouse.Clear();

            oldMouseState = mouseState;
            oldKeyboardState = keyboardState;
        }

        public static bool isKeyPressed(Keys k)
        {
            return keyboardState.IsKeyDown(k);
        }
    }
    public enum MouseButton
    {
        Left,
        Middle,
        Right,
        Scrollup,
        Scrolldown,
        Scroll,
        Movement
    }
    public enum TextAlign
    {
        Left,
        Center,
        Right
    }
}
