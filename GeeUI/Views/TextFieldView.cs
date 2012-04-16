
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GeeUI.Structs;
using GeeUI.Managers;

namespace GeeUI.Views
{
    public class TextFieldView : View
    {
        public NinePatch NinePatchDefault;
        public NinePatch NinePatchSelected;

        public SpriteFont TextInputFont;

        public bool MultiLine = true;
        public bool Editable = true;

        private string _text = "";
        public string Text
        {
            get
            {
                return MultiLine ? _text : _text.Replace("\n", "");
            }
            set
            {
                _text = value;
            }
        }

        private int _offsetX;
        private int _offsetY;
        private int _cursorX;
        private int _cursorY;

        private Vector2 _selectionStart = new Vector2(-1);
        private Vector2 _selectionEnd = new Vector2(-1);

        private int _buttonHeldTime;

        //How long to press before repeating
        private int _buttonHeldTimePreRepeat = 2;
        private string _buttonHeldString = "";
        private Keys _buttonHeld = Keys.None;

        int _delimiterTime;
        private const int DelimiterLimit = 25;
        bool _doingDelimiter;

        public string OffsetText
        {
            get
            {
                var ret = "";
                var lines = TextLines;
                var allowedWidth = Width;
                var allowedHeight = Height;
                for (var iY = _offsetY; iY < lines.Length; iY++)
                {
                    var curLine = lines[iY];
                    var curLineRet = "";
                    for (int iX = _offsetX; iX < curLine.Length; iX++)
                    {
                        var lineWidth = (int)TextInputFont.MeasureString(curLineRet + curLine[iX]).X;
                        if (lineWidth >= allowedWidth)
                        {
                            break;
                        }
                        curLineRet += curLine[iX];
                    }
                    var retTest = ret + curLineRet + (iY + 1 != lines.Length ? "\n" : "");
                    var maxHeight = (int)TextInputFont.MeasureString(retTest).Y;
                    ret += curLineRet + (iY + 1 != lines.Length ? "\n" : "");
                    if (maxHeight >= allowedHeight)
                        break;
                }
                return ret;
            }
        }


        public string[] TextLines
        {
            get
            {
                return Text.Split('\n');
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
                Text = cur;
            }
        }


        public TextFieldView(View rootView, Vector2 position, SpriteFont textFont)
            : base(rootView)
        {
            NinePatchDefault = GeeUI.NinePatchTextFieldDefault;
            NinePatchSelected = GeeUI.NinePatchTextFieldSelected;

            Position = position;
            TextInputFont = textFont;
            NumChildrenAllowed = -1;

            GeeUI.OnKeyPressedHandler += keyPressedHandler;
            GeeUI.OnKeyReleasedHandler += keyReleasedHandler;
        }

        void keyReleasedHandler(string keyReleased, Keys key)
        {
            if (_buttonHeld != key) return;
            _buttonHeldTime = 0;
            _buttonHeld = Keys.None;
            _buttonHeldTimePreRepeat = 0;
            _buttonHeldString = "";
        }


        void keyPressedHandler(string keyPressed, Keys key)
        {
            if (!Selected || !Editable) return;
            if (_buttonHeld != key)
            {
                _buttonHeld = key;
                _buttonHeldTime = 0;
                _buttonHeldTimePreRepeat = 0;
                _buttonHeldString = keyPressed;
            }
            bool ctrlPressed = InputManager.IsKeyPressed(Keys.LeftControl) || InputManager.IsKeyPressed(Keys.RightControl);

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
                        var lines = TextLines;
                        var curPos = _cursorX;
                        for (var i = 0; i < _cursorY; i++)
                        {
                            var lineL = lines[i] + (i < _cursorY ? "\n" : "");
                            curPos += lineL.Length;
                        }
                        if (curPos > 0)
                        {
                            Text = Text.Remove(curPos - 1, 1);
                            _cursorX--;
                        }
                        if (_cursorX < 0)
                        {
                            _cursorX = lines[_cursorY - 1].Length;
                            _cursorY--;
                        }

                        break;
                    case Keys.Left:
                        MoveCursorX(-1);
                        break;
                    case Keys.Right:
                        MoveCursorX(1);
                        break;
                    case Keys.Up:
                        MoveCursorY(-1);
                        break;
                    case Keys.Down:
                        MoveCursorY(1);
                        break;
                    case Keys.Enter:
                        AppendTextCursor("\n");
                        break;
                    case Keys.Space:
                        AppendTextCursor(" ");
                        break;
                    default:
                        AppendTextCursor(keyPressed);
                        break;
                }
            }
            ReEvaluateOffset();
        }

        private void MoveCursorX(int xMovement)
        {
            string[] lines = TextLines;
            _cursorX += xMovement;
            if (_cursorX < 0)
            {
                int yMinus = _cursorY - 1;
                if (yMinus < 0)
                {
                    _cursorX = 0;
                }
                else
                {
                    string line = lines[yMinus];
                    _cursorX = line.Length;
                    _cursorY = yMinus;
                }
            }
            else if (_cursorX > lines[_cursorY].Length)
            {
                if (_cursorY < lines.Length - 1)
                {
                    _cursorY++;
                    _cursorX = 0;
                }
                else
                {
                    _cursorX = lines[_cursorY].Length;
                }
            }

            ReEvaluateOffset();
        }

        private void MoveCursorY(int yMovement)
        {
            string[] lines = TextLines;
            _cursorY += yMovement;
            if (_cursorY >= lines.Length) _cursorY = lines.Length - 1;
            else if (_cursorY < 0) _cursorY = 0;
            string line = lines[_cursorY];
            if (_cursorX >= line.Length) _cursorX = line.Length;

            ReEvaluateOffset();
        }

        private void ReEvaluateOffset()
        {
            if(_selectionStart == _selectionEnd)
                _selectionEnd = _selectionStart = Vector2.Zero;
            var ret = "";
            var lines = TextLines;
            var allowedWidth = Width;
            var allowedHeight = Height;

            var maxCharX = 0;
            var maxCharY = 0;

            var xDiff = _cursorX - _offsetX;
            var yDiff = _cursorY - _offsetY;

            if (xDiff < 0) _offsetX += xDiff;
            if (yDiff < 0) _offsetY += yDiff;

            for (var iY = _offsetY; iY < lines.Length; iY++)
            {
                var curLine = lines[iY];
                var curLineRet = "";
                for (var iX = _offsetX; iX < curLine.Length; iX++)
                {
                    var lineWidth = (int)TextInputFont.MeasureString(curLineRet + curLine[iX]).X;
                    if (lineWidth >= allowedWidth)
                    {
                        break;
                    }
                    curLineRet += curLine[iX];
                    if (iY == _cursorY)
                        maxCharX++;
                }
                ret += curLineRet + (iY + 1 != lines.Length ? "\n" : "");
                var lineHeight = (int)TextInputFont.MeasureString(ret).Y;
                if (lineHeight >= allowedHeight)
                {
                    break;
                }
                maxCharY++;
            }
            if (maxCharX < xDiff)
                _offsetX += xDiff - maxCharX;
            if (maxCharY < yDiff) _offsetY++;
        }

        private void AppendTextCursor(string text)
        {
            string[] lines = TextLines;
            string curLine = lines[_cursorY];
            string before = curLine.Substring(0, _cursorX);
            string after = (_cursorX < curLine.Length) ? curLine.Substring(_cursorX) : "";
            lines[_cursorY] = before + text + after;
            TextLines = lines;
            MoveCursorX(text.Length);
        }

        public void AppendText(string text)
        {
            Text += text;
        }

        public Vector2 GetMouseTextPos(Vector2 pos)
        {
            var lines = TextLines;

            var patch = Selected ? NinePatchSelected : NinePatchDefault;

            var topLeftContentPos = AbsolutePosition + new Vector2(patch.LeftWidth, patch.TopHeight);
            var actualClickPos = pos - topLeftContentPos;

            var ret = new Vector2();

            var actualText = "";

            for (var iY = _offsetY; iY < lines.Length; iY++)
            {
                var textHeight = (int)TextInputFont.MeasureString(actualText + lines[iY]).Y;
                if (textHeight >= actualClickPos.Y)
                {
                    ret.Y = iY;

                    var line = lines[iY];

                    //No need to make another variable
                    actualText = "";

                    bool setX = false;

                    for (int iX = _offsetX; iX < line.Length; iX++)
                    {
                        actualText += line[iX];
                        var textWidth = (int)TextInputFont.MeasureString(actualText).X;
                        if (textWidth < actualClickPos.X) continue;
                        ret.X = iX;
                        setX = true;
                        break;
                    }

                    if (!setX)
                        ret.X = line.Length;

                    break;
                }
                actualText += lines[iY] + "\n";
            }
            return ret;
        }

        protected internal override void OnMClick(Vector2 mousePosition, bool fromChild = false)
        {
            Selected = true;

            var clickPos = GetMouseTextPos(mousePosition);
            _cursorX = (int)clickPos.X;
            _cursorY = (int)clickPos.Y;

            _selectionStart = clickPos;

            base.OnMClick(mousePosition);
        }

        protected internal override void OnMClickAway(bool fromChild = false)
        {
            Selected = false;
            _selectionEnd = _selectionStart = new Vector2(-1);
        }

        protected internal override void OnMOver(bool fromChild = false)
        {
            if (Selected && InputManager.IsMousePressed(MouseButton.Left))
            {
                var clickPos = GetMouseTextPos(InputManager.GetMousePosV());
                _selectionEnd = clickPos;
            }
            base.OnMOver();
        }
        protected internal override void OnMOff(bool fromChild = false)
        {
            base.OnMOff();
        }

        private Vector2 GetDrawPosForCursorPos(int cursorX, int cursorY)
        {
            var patch = Selected ? NinePatchSelected : NinePatchDefault;
            var lines = TextLines;

            var totalLine = "";
            for (int i = _offsetY; i < cursorY && i < lines.Length; i++)
            {
                var line = lines[i];
                var addNewline = (i < cursorY - 1) || (i == cursorY && line.Length == 0);
                var addSpace = (line.Length == 0);
                line += (addNewline ? "\n" : "");
                line += (addSpace ? " " : "");
                totalLine += line;
            }

            var yDrawPos = (int)(AbsoluteY + patch.TopHeight + TextInputFont.MeasureString(totalLine).Y);
            var yDrawLine = lines[cursorY];
            var cur = "";
            for (var x = _offsetX; x < cursorX && x < yDrawLine.Length; x++)
                cur += yDrawLine[x];
            var xDrawPos = (int)TextInputFont.MeasureString(cur).X + (AbsoluteX + patch.LeftWidth);

            return new Vector2(xDrawPos, yDrawPos);
        }

        protected internal override void Update(GameTime theTime)
        {
            if (InputManager.IsKeyPressed(_buttonHeld))
            {
                if (!(_buttonHeldTimePreRepeat++ < 15 || _buttonHeldTime++ < 2))
                {
                    _buttonHeldTime = 0;
                    keyPressedHandler(_buttonHeldString, _buttonHeld);
                }
            }
            else
            {
                keyReleasedHandler("", _buttonHeld);
            }
            base.Update(theTime);
        }


        protected internal override void Draw(SpriteBatch spriteBatch)
        {
            var patch = Selected ? NinePatchSelected : NinePatchDefault;
            patch.Draw(spriteBatch, AbsolutePosition, Width, Height);

            spriteBatch.DrawString(TextInputFont, OffsetText, AbsolutePosition + new Vector2(patch.LeftWidth, patch.TopHeight), Color.Black);

            var drawPos = GetDrawPosForCursorPos(_cursorX, _cursorY);
            var xDrawPos = drawPos.X;
            var yDrawPos = drawPos.Y;

            if (_selectionStart != new Vector2(-1) && _selectionEnd != new Vector2(-1))
            {
                var start = _selectionStart;
                var end = _selectionEnd;
                if (_selectionStart.Y > _selectionEnd.Y || (_selectionStart.Y == _selectionEnd.Y && _selectionStart.X > _selectionEnd.X))
                {
                    //Need to swap the variables.
                    var store = start;
                    start = end;
                    end = store;
                }
                var drawSS = GetDrawPosForCursorPos((int)start.X, (int)start.Y);
                var drawSE = GetDrawPosForCursorPos((int)end.X, (int)end.Y);
                spriteBatch.DrawString(TextInputFont, "|", new Vector2(drawSS.X - 1, drawSS.Y), Color.Red);
                spriteBatch.DrawString(TextInputFont, "|", new Vector2(drawSE.X - 1, drawSE.Y), Color.Green);
            }

            if (_delimiterTime++ % DelimiterLimit == 0)
            {
                _doingDelimiter = !_doingDelimiter;
            }
            if (_doingDelimiter && Selected)
                spriteBatch.DrawString(TextInputFont, "|", new Vector2(xDrawPos - 1, yDrawPos), Color.Black);
            base.Draw(spriteBatch);
        }
    }
}
