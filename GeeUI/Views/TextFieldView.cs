using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GeeUI.Structs;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class TextFieldView : View
    {
        public NinePatch ninePatchDefault;
        public NinePatch ninePatchSelected;

        public SpriteFont textInputFont;

        public bool multiLine = true;

        private string _text = "";
        public string text
        {
            get
            {
                if (multiLine) return _text;
                return _text.Replace("\n", "");
            }
            set
            {
                _text = value;
            }
        }

        private int offsetX = 0, offsetY = 0, cursorX = 0, cursorY = 0;

        private int buttonHeldTime = 0;
        //How long to press before repeating
        private int buttonHeldTimePreRepeat = 2;
        private string buttonHeldString = "";
        private Keys buttonHeld = Keys.None;

        int delimiterTime = 0, delimiterLimit = 25;
        bool doingDelimiter = false;

        public string offsetText
        {
            get
            {
                string ret = "";
                string[] lines = textLines;
                NinePatch patch = selected ? ninePatchSelected : ninePatchDefault;
                int allowedWidth = width;
                int allowedHeight = height;
                for (int y = offsetY; y < lines.Length; y++)
                {
                    string curLine = lines[y];
                    string curLineRet = "";
                    for (int x = offsetX; x < curLine.Length; x++)
                    {
                        int lineWidth = (int)textInputFont.MeasureString(curLineRet + curLine[x]).X;
                        if (lineWidth >= allowedWidth)
                        {
                            break;
                        }
                        curLineRet += curLine[x];
                    }
                    string retTest = ret + curLineRet + (y + 1 != lines.Length ? "\n" : "");
                    int maxHeight = (int)textInputFont.MeasureString(retTest).Y;
                    if (maxHeight >= allowedHeight)
                        break;
                    ret += curLineRet + (y + 1 != lines.Length ? "\n" : "");
                }
                return ret;
            }
        }


        public string[] textLines
        {
            get
            {
                return text.Split('\n');
            }
            set
            {
                string cur = "";
                for (int i = 0; i < value.Length; i++)
                {
                    cur += value[i];
                    if (i < value.Length - 1)
                        cur += "\n";
                }
                text = cur;
            }
        }

        public override Microsoft.Xna.Framework.Rectangle BoundBox
        {
            get
            {
                return base.BoundBox;
            }
        }

        public override Microsoft.Xna.Framework.Rectangle ContentBoundBox
        {
            get
            {
                return base.ContentBoundBox;
            }
        }


        public TextFieldView(View rootView, Vector2 position, SpriteFont textFont)
            : base(rootView)
        {
            ninePatchDefault = GeeUI.ninePatch_textFieldDefault;
            ninePatchSelected = GeeUI.ninePatch_textFieldSelected;

            this.position = position;
            this.textInputFont = textFont;
            numChildrenAllowed = -1;

            GeeUI.OnKeyPressedHandler += new OnKeyPressed(keyPressedHandler);
            GeeUI.OnKeyReleasedHandler += new OnKeyReleased(keyReleasedHandler);
        }

        void keyReleasedHandler(string keyReleased, Keys key)
        {
            if (buttonHeld == key)
            {
                buttonHeldTime = 0;
                buttonHeld = Keys.None;
                buttonHeldTimePreRepeat = 0;
                buttonHeldString = "";
            }
        }


        private bool isSpecialKey(Keys key)
        {
            return false;
        }
        void keyPressedHandler(string keyPressed, Keys key)
        {
            if (!selected) return;
            if (buttonHeld != key)
            {
                buttonHeld = key;
                buttonHeldTime = 0;
                buttonHeldTimePreRepeat = 0;
                buttonHeldString = keyPressed;
            }
            bool ctrlPressed = InputManager.isKeyPressed(Keys.LeftControl) || InputManager.isKeyPressed(Keys.RightControl);

            if (ctrlPressed)
            {
                switch (key)
                {
                    case Keys.A:
                        break;

                    case Keys.C:
                        break;

                    case Keys.X:
                        break;

                    case Keys.V:
                        break;

                    case Keys.Back:
                        break;
                }
            }
            else
            {
                switch (key)
                {
                    case Keys.Back:
                        string[] lines = textLines;
                        string line = lines[cursorY];
                        int curPos = cursorX;
                        for (int i = 0; i < cursorY; i++)
                        {
                            string lineL = lines[i] + (i < cursorY ? "\n" : "");
                            curPos += lineL.Length;
                        }
                        if (curPos > 0)
                        {
                            text = text.Remove(curPos - 1, 1);
                            cursorX--;
                        }
                        if (cursorX < 0)
                        {
                            cursorX = lines[cursorY - 1].Length;
                            cursorY--;
                        }

                        break;
                    case Keys.Left:
                        moveCursorX(-1);
                        break;
                    case Keys.Right:
                        moveCursorX(1);
                        break;
                    case Keys.Up:
                        moveCursorY(-1);
                        break;
                    case Keys.Down:
                        moveCursorY(1);
                        break;
                    case Keys.Enter:
                        appendText("\n");
                        break;
                    case Keys.Space:
                        appendText(" ");
                        break;
                    default:
                        appendText(keyPressed);
                        break;
                }
            }
            reEvaluateOffset();
        }

        private void moveCursorX(int xMovement)
        {
            string[] lines = textLines;
            cursorX += xMovement;
            if (cursorX < 0)
            {
                int yMinus = cursorY - 1;
                if (yMinus < 0)
                {
                    cursorX = 0;
                }
                else
                {
                    string line = lines[yMinus];
                    cursorX = line.Length;
                    cursorY = yMinus;
                }
            }
            else if (cursorX > lines[cursorY].Length)
            {
                if (cursorY < lines.Length - 1)
                {
                    cursorY++;
                    cursorX = 0;
                }
                else
                {
                    cursorX = lines[cursorY].Length;
                }
            }

            reEvaluateOffset();
        }

        private void moveCursorY(int yMovement)
        {
            string[] lines = textLines;
            cursorY += yMovement;
            if (cursorY >= lines.Length) cursorY = lines.Length - 1;
            else if (cursorY < 0) cursorY = 0;
            string line = lines[cursorY];
            if (cursorX >= line.Length) cursorX = line.Length;

            reEvaluateOffset();
        }

        private void reEvaluateOffset()
        {
            string ret = "";
            string[] lines = textLines;
            NinePatch patch = selected ? ninePatchSelected : ninePatchDefault;
            int allowedWidth = width;
            int allowedHeight = height;

            int maxCharX = 0;
            int maxCharY = 0;

            int xDiff = cursorX - offsetX;
            int yDiff = cursorY - offsetY;

            if (xDiff < 0) offsetX += xDiff;
            if (yDiff < 0) offsetY += yDiff;

            for (int y = offsetY; y < lines.Length; y++)
            {
                string curLine = lines[y];
                string curLineRet = "";
                for (int x = offsetX; x < curLine.Length; x++)
                {
                    int lineWidth = (int)textInputFont.MeasureString(curLineRet + curLine[x]).X;
                    if (lineWidth >= allowedWidth)
                    {
                        break;
                    }
                    curLineRet += curLine[x];
                    if (y == cursorY)
                        maxCharX++;
                }
                ret += curLineRet + (y + 1 != lines.Length ? "\n" : "");
                int lineHeight = (int)textInputFont.MeasureString(ret).Y;
                if (lineHeight >= allowedHeight)
                {
                    break;
                }
                maxCharY++;
            }
            if (maxCharX < xDiff)
                offsetX += xDiff - maxCharX;
            if (maxCharY < yDiff) offsetY++;
        }

        private void appendText(string text)
        {
            string[] lines = textLines;
            string curLine = lines[cursorY];
            string before = curLine.Substring(0, cursorX);
            string after = (cursorX < curLine.Length) ? curLine.Substring(cursorX) : "";
            lines[cursorY] = before + text + after;
            textLines = lines;
            moveCursorX(text.Length);
        }

        protected internal override void onMClick(Vector2 position, bool fromChild = false)
        {
            selected = true;
            base.onMClick(position);
        }
        protected internal override void onMClickAway(bool fromChild = false)
        {
            selected = false;
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
            if (InputManager.isKeyPressed(buttonHeld))
            {
                if (!(buttonHeldTimePreRepeat++ < 15 || buttonHeldTime++ < 2))
                {
                    buttonHeldTime = 0;
                    keyPressedHandler(buttonHeldString, buttonHeld);
                }
            }
            else
            {
                keyReleasedHandler("", buttonHeld);
            }
            base.Update(theTime);
        }

        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            NinePatch patch = selected ? ninePatchSelected : ninePatchDefault;
            patch.Draw(spriteBatch, absolutePosition, width, height);

            spriteBatch.DrawString(textInputFont, offsetText, absolutePosition + new Vector2(patch.leftWidth, patch.topHeight), Color.Black);

            string[] lines = textLines;

            string totalLine = "";
            for (int i = offsetY; i < cursorY && i < lines.Length; i++)
            {
                string line = lines[i];
                bool addNewline = (i < cursorY - 1) || (i == cursorY && line.Length == 0);
                line += (addNewline ? "\n" : "");
                totalLine += line;
            }

            int yDrawPos = (int)(absolutePosition.Y + patch.topHeight + textInputFont.MeasureString(totalLine).Y);
            string yDrawLine = lines[cursorY];
            string cur = "";
            for (int x = offsetX; x < cursorX && x < yDrawLine.Length; x++)
                cur += yDrawLine[x];
            int xDrawPos = (int)textInputFont.MeasureString(cur).X + (int)(absolutePosition.X + patch.leftWidth);

            if (delimiterTime++ % delimiterLimit == 0)
            {
                doingDelimiter = !doingDelimiter;
            }
            if (doingDelimiter && selected)
                DrawManager.Draw_Box(new Vector2(xDrawPos, yDrawPos + 3), new Vector2(xDrawPos + 1, yDrawPos + 15), Color.Black, spriteBatch);
            //DrawManager.Draw_Circle(new Vector2(xDrawPos, yDrawPos), 3f, Color.Red, Color.Black, spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
