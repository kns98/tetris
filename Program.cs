using System;
using Microsoft.SmallBasic.Library;
using Array = Microsoft.SmallBasic.Library.Array;
using Math = Microsoft.SmallBasic.Library.Math;

namespace Tetris
{
    class Utils
    {
        public static double Remainder(double dividend, double divisor)
        {
            return (double)((double)dividend % (double)divisor);
        }
    }

    internal sealed class Program
    {
        private static int _moveDirection;
        private static int _boxes;
        private static int _bwidth;
        private static int _xoffset;
        private static int _yoffset;
        private static int _cwidth;
        private static int _cheight;
        private static int _startdelay;
        private static int _enddelay;
        private static int _previewXpos;
        private static int _previewYpos;
        private static string _template;
        private static string _nextPiece;
        private static string _h;
        private static int _end;
        private static int _sessionDelay;
        private static int _delay;
        private static string _thisPiece;
        private static int _ypos;
        private static int _done;
        private static int _xpos;
        private static int _yposdelta;
        private static int _delayIndex;
        private static int _invalidMove;
        private static Primitive _basetemplate;
        private static string _rotation;
        private static int _xposbk;
        private static int _xoffsetbk;
        private static int _yoffsetbk;
        private static int _l;
        private static int _i;
        private static Primitive _v;
        private static int _x;
        private static int _y;
        private static int _hcount;
        private static Primitive _s;
        private static int _score;
        private static int _linesCleared;
        private static Primitive _piece;
        private static int _x1;
        private static int _y1;

        public static void Main()
        {
            GraphicsWindow.KeyDown += HandleKey;
            GraphicsWindow.BackgroundColor = GraphicsWindow.GetColorFromRGB(253, 252, 251);
            while (true)
            {
                _boxes = 4;
                _bwidth = 25;
                _xoffset = 40;
                _yoffset = 40;
                _cwidth = 10;
                _cheight = 20;
                _startdelay = 800;
                _enddelay = 175;
                _previewXpos = 13;
                _previewYpos = 2;
                GraphicsWindow.Clear();
                GraphicsWindow.Title = "Another Tetris";
                GraphicsWindow.Height = 580;
                GraphicsWindow.Width = 700;
                GraphicsWindow.Show();
                SetupTemplates();
                SetupCanvas();
                MainLoop();
                GraphicsWindow.ShowMessage("Game Over", "Another Tetris");
            }
        }

        public static void MainLoop()
        {
            _template = Text.Append("template", Math.GetRandomNumber(7));
            CreatePiece();
            _nextPiece = _h;
            _end = 0;
            _sessionDelay = _startdelay;
            while (_end == 0)
            {
                if (_sessionDelay > _enddelay)
                    _sessionDelay -= 1;
                _delay = _sessionDelay;
                _thisPiece = _nextPiece;
                _template = Text.Append("template", Math.GetRandomNumber(7));
                CreatePiece();
                _nextPiece = _h;
                DrawPreviewPiece();
                _h = _thisPiece;
                _ypos = 0;
                _done = 0;
                _xpos = 3;
                CheckStop();
                if (_done == 1)
                {
                    _ypos -= 1;
                    MovePiece();
                    _end = 1;
                }

                _yposdelta = 0;
                while ((_done == 0) | (_yposdelta > 0))
                {
                    MovePiece();
                    _delayIndex = _delay;
                    while ((_delayIndex > 0) & (_delay > 0))
                    {
                        Microsoft.SmallBasic.Library.Program.Delay(10);
                        _delayIndex -= 10;
                    }

                    if (_yposdelta > 0)
                        _yposdelta -= 1;
                    else
                        _ypos += 1;
                    CheckStop();
                }
            }
        }

        public static void HandleKey()
        {
            if (GraphicsWindow.LastKey == "Escape")
                Microsoft.SmallBasic.Library.Program.End();
            if (GraphicsWindow.LastKey == "Left")
            {
                _moveDirection = -1;
                ValidateMove();
                if (_invalidMove == 0)
                    _xpos += _moveDirection;
                MovePiece();
            }

            if (GraphicsWindow.LastKey == "Right")
            {
                _moveDirection = 1;
                ValidateMove();
                if (_invalidMove == 0)
                    _xpos += _moveDirection;
                MovePiece();
            }

            if ((GraphicsWindow.LastKey == "Down") | (GraphicsWindow.LastKey == "Space"))
                _delay = 0;
            if (!(GraphicsWindow.LastKey == "Up"))
                return;
            _basetemplate = Array.GetValue(_h, -1);
            _template = "temptemplate";
            _rotation = "CW";
            CopyPiece();
            Array.SetValue(_h, -1, _template);
            _moveDirection = 0;
            ValidateMove();
            _xposbk = _xpos;
            _yposdelta = 0;
            while ((_yposdelta == 0) & (Math.Abs(_xposbk - _xpos) < 3))
                if (_invalidMove == 0)
                {
                    _basetemplate = _template;
                    _template = "rotatedtemplate";
                    Array.SetValue(_h, -1, _template);
                    _rotation = "COPY";
                    CopyPiece();
                    _yposdelta = 1;
                    MovePiece();
                }
                else if (_invalidMove == 2)
                {
                    _xpos = 99;
                }
                else
                {
                    _xpos -= _invalidMove;
                    ValidateMove();
                }

            if (_invalidMove != 0)
            {
                _xpos = _xposbk;
                Array.SetValue(_h, -1, _basetemplate);
                _template = "";
            }
        }

        public static void DrawPreviewPiece()
        {
            _xpos = _previewXpos;
            _ypos = _previewYpos;
            _h = _nextPiece;
            _xoffsetbk = _xoffset;
            _yoffsetbk = _yoffset;
            _xoffset += Array.GetValue(Array.GetValue(_h, -1), "pviewx");
            _yoffset += Array.GetValue(Array.GetValue(_h, -1), "pviewy");
            MovePiece();
            _xoffset = _xoffsetbk;
            _yoffset = _yoffsetbk;
        }

        public static void CopyPiece()
        {
            _l = Array.GetValue(_basetemplate, "dim");
            if (_rotation == "CW")
            {
                var var1 = 0;
                var var2 = _boxes - 1;
                var var3 = 1;
                bool flag = var3 >= var3 - var3;
                _i = var1;
                while ((flag ? _i <= var2 ? 1 : 0 : _i >= var2 ? 1 : 0) != 0)
                {
                    _v = Array.GetValue(_basetemplate, _i);
                    _x = (int) Utils.Remainder(_v, 10);
                    _y = _l - 1 - Math.Floor(_v / 10);
                    Array.SetValue(_template, _i, _x * 10 + _y);
                    _i += var3;
                }
            }
            else if (_rotation == "CCW")
            {
                var var4 = 0;
                var var5 = _boxes - 1;
                var var6 = 1;
                bool flag = var6 >= var6 - var6;
                _i = var4;
                while ((flag ? _i <= var5 ? 1 : 0 : _i >= var5 ? 1 : 0) != 0)
                {
                    _v = Array.GetValue(_basetemplate, _i);
                    _x = _l - 1 -(int) Utils.Remainder(_v, 10);
                    _y = Math.Floor(_v / 10);
                    Array.SetValue(_template, _i, _x * 10 + _y);
                    _i += var6;
                }
            }
            else if (_rotation == "COPY")
            {
                var var7 = 0;
                var var8 = _boxes - 1;
                var var9 = 1;
                bool flag = var9 >= var9 - var9;
                _i = var7;
                while ((flag ? _i <= var8 ? 1 : 0 : _i >= var8 ? 1 : 0) != 0)
                {
                    Array.SetValue(_template, _i, Array.GetValue(_basetemplate, _i));
                    _i += var9;
                }
            }
            else
            {
                GraphicsWindow.ShowMessage("invalid parameter", "Error");
                Microsoft.SmallBasic.Library.Program.End();
            }

            Array.SetValue(_template, "color", Array.GetValue(_basetemplate, "color"));
            Array.SetValue(_template, "dim", Array.GetValue(_basetemplate, "dim"));
            Array.SetValue(_template, "pviewx", Array.GetValue(_basetemplate, "pviewx"));
            Array.SetValue(_template, "pviewy", Array.GetValue(_basetemplate, "pviewy"));
        }

        public static void CreatePiece()
        {
            _hcount += 1;
            _h = Text.Append("piece", _hcount);
            Array.SetValue(_h, -1, _template);
            GraphicsWindow.PenWidth = 1;
            GraphicsWindow.PenColor = "Black";
            GraphicsWindow.BrushColor = Array.GetValue(_template, "color");
            var var1 = 0;
            var var2 = _boxes - 1;
            var var3 = 1;
            bool flag = var3 >= var3 - var3;
            _i = var1;
            while ((flag ? _i <= var2 ? 1 : 0 : _i >= var2 ? 1 : 0) != 0)
            {
                _s = Shapes.AddRectangle(_bwidth, _bwidth);
                Shapes.Move(_s, -_bwidth, -_bwidth);
                Array.SetValue(_h, _i, _s);
                _i += var3;
            }
        }

        public static void MovePiece()
        {
            var var1 = 0;
            var var2 = _boxes - 1;
            var var3 = 1;
            bool flag = var3 >= var3 - var3;
            _i = var1;
            while ((flag ? _i <= var2 ? 1 : 0 : _i >= var2 ? 1 : 0) != 0)
            {
                _v = Array.GetValue(Array.GetValue(_h, -1), _i);
                _x = Math.Floor(_v / 10);
                _y = (int)Utils.Remainder(_v, 10);
                Shapes.Move(Array.GetValue(_h, _i), _xoffset + _xpos * _bwidth + _x * _bwidth,
                    _yoffset + _ypos * _bwidth + _y * _bwidth);
                _i += var3;
            }
        }

        public static void ValidateMove()
        {
            _i = 0;
            _invalidMove = 0;
            while (_i < _boxes)
            {
                _v = Array.GetValue(Array.GetValue(_h, -1), _i);
                _x = Math.Floor(_v / 10);
                _y = (int)Utils.Remainder(_v, 10);
                if (_x + _xpos + _moveDirection < 0)
                {
                    _invalidMove = -1;
                    _i = _boxes;
                }

                if (_x + _xpos + _moveDirection >= _cwidth)
                {
                    _invalidMove = 1;
                    _i = _boxes;
                }

                if (Array.GetValue("c", _x + _xpos + _moveDirection + (_y + _ypos) * _cwidth) != ".")
                {
                    _invalidMove = 2;
                    _i = _boxes;
                }

                _i += 1;
            }
        }

        public static void CheckStop()
        {
            _done = 0;
            _i = 0;
            while (_i < _boxes)
            {
                _v = Array.GetValue(Array.GetValue(_h, -1), _i);
                _x = Math.Floor(_v / 10);
                _y = (int) Utils.Remainder(_v, 10);
                if ((_y + _ypos > _cheight) |
                    (Array.GetValue("c", _x + _xpos + (_y + _ypos) * _cwidth) != "."))
                {
                    _done = 1;
                    _i = _boxes;
                }

                _i += 1;
            }

            if (!(_done == 1))
                return;
            var var1 = 0;
            var var2 = _boxes - 1;
            var var3 = 1;
            bool flag = var3 >= var3 - var3;
            _i = var1;
            while ((flag ? _i <= var2 ? 1 : 0 : _i >= var2 ? 1 : 0) != 0)
            {
                _v = Array.GetValue(Array.GetValue(_h, -1), _i);
                Array.SetValue("c", Math.Floor(_v / 10) + _xpos + (Utils.Remainder(_v, 10) + _ypos - 1) * _cwidth,
                    Array.GetValue(_h, _i));
                _i += var3;
            }

            _score += 1;
            PrintScore();
            DeleteLines();
        }

        public static void DeleteLines()
        {
            _linesCleared = 0;
            var var1 = _cheight - 1;
            var var2 = 0;
            var var3 = -1;
            bool flag1 = var3 >= var3 - var3;
            _y = var1;
            while ((flag1 ? _y <= var2 ? 1 : 0 : _y >= var2 ? 1 : 0) != 0)
            {
                _x = _cwidth;
                while (_x == _cwidth)
                {
                    _x = 0;
                    while (_x < _cwidth)
                    {
                        _piece = Array.GetValue("c", _x + _y * _cwidth);
                        if (_piece == ".")
                            _x = _cwidth;
                        _x += 1;
                    }

                    if (_x == _cwidth)
                    {
                        var var4 = 0;
                        var var5 = _cwidth - 1;
                        var var6 = 1;
                        bool flag2 = var6 >= var6 - var6;
                        _x1 = var4;
                        while ((flag2 ? _x1 <= var5 ? 1 : 0 : _x1 >= var5 ? 1 : 0) != 0)
                        {
                            Shapes.Remove(Array.GetValue("c", _x1 + Program._y * _cwidth));
                            _x1 += var6;
                        }

                        _linesCleared += 1;
                        var y = Program._y;
                        var var7 = 1;
                        var var8 = -1;
                        bool flag3 = var8 >= var8 - var8;
                        _y1 = y;
                        while ((flag3 ? _y1 <= var7 ? 1 : 0 : _y1 >= var7 ? 1 : 0) != 0)
                        {
                            var var9 = 0;
                            var var10 = _cwidth - 1;
                            var var11 = 1;
                            bool flag4 = var11 >= var11 - var11;
                            _x1 = var9;
                            while ((flag4 ? _x1 <= var10 ? 1 : 0 : _x1 >= var10 ? 1 : 0) != 0)
                            {
                                _piece = Array.GetValue("c", _x1 + (_y1 - 1) * _cwidth);
                                Array.SetValue("c", _x1 + _y1 * _cwidth, _piece);
                                Shapes.Move(_piece, Shapes.GetLeft(_piece), Shapes.GetTop(_piece) + _bwidth);
                                _x1 += var11;
                            }

                            _y1 += var8;
                        }
                    }
                }

                _y += var3;
            }

            if (!(_linesCleared > 0))
                return;
            _score += 100 * Math.Round(_linesCleared * 2.15 - 1);
            PrintScore();
        }

        public static void SetupCanvas()
        {
            GraphicsWindow.BrushColor = GraphicsWindow.BackgroundColor;
            GraphicsWindow.FillRectangle(_xoffset, _yoffset, _cwidth * _bwidth, _cheight * _bwidth);
            Microsoft.SmallBasic.Library.Program.Delay(200);
            GraphicsWindow.PenWidth = 1;
            GraphicsWindow.PenColor = "Pink";
            var var1 = 0;
            var var2 = _cwidth - 1;
            var var3 = 1;
            bool flag1 = var3 >= var3 - var3;
            _x = var1;
            while ((flag1 ? _x <= var2 ? 1 : 0 : _x >= var2 ? 1 : 0) != 0)
            {
                var var4 = 0;
                var var5 = _cheight - 1;
                var var6 = 1;
                bool flag2 = var6 >= var6 - var6;
                _y = var4;
                while ((flag2 ? _y <= var5 ? 1 : 0 : _y >= var5 ? 1 : 0) != 0)
                {
                    Array.SetValue("c", _x + _y * _cwidth, ".");
                    GraphicsWindow.DrawRectangle(_xoffset + _x * _bwidth, _yoffset + _y * _bwidth, _bwidth, _bwidth);
                    _y += var6;
                }

                _x += var3;
            }

            GraphicsWindow.PenWidth = 4;
            GraphicsWindow.PenColor = "Black";
            GraphicsWindow.DrawLine(_xoffset, _yoffset, _xoffset, _yoffset + _cheight * _bwidth);
            GraphicsWindow.DrawLine(_xoffset + _cwidth * _bwidth, _yoffset, _xoffset + _cwidth * _bwidth,
                _yoffset + _cheight * _bwidth);
            GraphicsWindow.DrawLine(_xoffset, _yoffset + _cheight * _bwidth, _xoffset + _cwidth * _bwidth,
                _yoffset + _cheight * _bwidth);
            GraphicsWindow.PenColor = "Lime";
            GraphicsWindow.DrawLine(_xoffset - 4, _yoffset, _xoffset - 4, _yoffset + _cheight * _bwidth + 6);
            GraphicsWindow.DrawLine(_xoffset + _cwidth * _bwidth + 4, _yoffset, _xoffset + _cwidth * _bwidth + 4,
                _yoffset + _cheight * _bwidth + 6);
            GraphicsWindow.DrawLine(_xoffset - 4, _yoffset + _cheight * _bwidth + 4, _xoffset + _cwidth * _bwidth + 4,
                _yoffset + _cheight * _bwidth + 4);
            GraphicsWindow.PenColor = "Black";
            GraphicsWindow.BrushColor = "Pink";
            _x = _xoffset + _previewXpos * _bwidth - _bwidth;
            _y = _yoffset + _previewYpos * _bwidth - _bwidth;
            GraphicsWindow.FillRectangle(_x, _y, _bwidth * 5, _bwidth * 6);
            GraphicsWindow.DrawRectangle(_x, _y, _bwidth * 5, _bwidth * 6);
            GraphicsWindow.FillRectangle(_x - 20, _y + 190, 310, 170);
            GraphicsWindow.DrawRectangle(_x - 20, _y + 190, 310, 170);
            GraphicsWindow.BrushColor = "Black";
            GraphicsWindow.FontItalic = false;
            GraphicsWindow.FontName = "Comic Sans MS";
            GraphicsWindow.FontSize = 16;
            GraphicsWindow.DrawText(_x, _y + 200, "Game control keys:");
            GraphicsWindow.DrawText(_x + 25, _y + 220, "Left Arrow = Move piece left");
            GraphicsWindow.DrawText(_x + 25, _y + 240, "Right Arrow = Move piece right");
            GraphicsWindow.DrawText(_x + 25, _y + 260, "Up Arrow = Rotate piece");
            GraphicsWindow.DrawText(_x + 25, _y + 280, "Down Arrow = Drop piece");
            GraphicsWindow.DrawText(_x, _y + 320, "Press to stop game");
            Microsoft.SmallBasic.Library.Program.Delay(200);
            GraphicsWindow.BrushColor = "Black";
            GraphicsWindow.FontName = "Georgia";
            GraphicsWindow.FontItalic = true;
            GraphicsWindow.FontSize = 36;
            GraphicsWindow.DrawText(_x - 20, _y + 400, "Another Tetris");
            Microsoft.SmallBasic.Library.Program.Delay(200);
            GraphicsWindow.FontSize = 16;
            GraphicsWindow.DrawText(_x - 20, _y + 440, "ver.0.1");
            Microsoft.SmallBasic.Library.Program.Delay(200);
            _score = 0;
            PrintScore();
        }

        public static void PrintScore()
        {
            GraphicsWindow.PenWidth = 4;
            GraphicsWindow.BrushColor = "Pink";
            GraphicsWindow.FillRectangle(500, 65, 153, 50);
            GraphicsWindow.BrushColor = "Black";
            GraphicsWindow.DrawRectangle(500, 65, 153, 50);
            GraphicsWindow.FontItalic = false;
            GraphicsWindow.FontSize = 32;
            GraphicsWindow.FontName = "Impact";
            GraphicsWindow.BrushColor = "Black";
            GraphicsWindow.DrawText(505, 70,
                Text.Append(Text.GetSubText("00000000", 0, 8 - Text.GetLength(_score)), _score));
        }

        public static void SetupTemplates()
        {
            Array.SetValue("template1", 0, 10);
            Array.SetValue("template1", 1, 11);
            Array.SetValue("template1", 2, 12);
            Array.SetValue("template1", 3, 22);
            Array.SetValue("template1", "color", "Yellow");
            Array.SetValue("template1", "dim", 3);
            Array.SetValue("template1", "pviewx", -12);
            Array.SetValue("template1", "pviewy", 12);
            Array.SetValue("template2", 0, 10);
            Array.SetValue("template2", 1, 11);
            Array.SetValue("template2", 2, 12);
            Array.SetValue("template2", 3, 2);
            Array.SetValue("template2", "color", "Magenta");
            Array.SetValue("template2", "dim", 3);
            Array.SetValue("template2", "pviewx", 12);
            Array.SetValue("template2", "pviewy", 12);
            Array.SetValue("template3", 0, 10);
            Array.SetValue("template3", 1, 1);
            Array.SetValue("template3", 2, 11);
            Array.SetValue("template3", 3, 21);
            Array.SetValue("template3", "color", "Gray");
            Array.SetValue("template3", "dim", 3);
            Array.SetValue("template3", "pviewx", 0);
            Array.SetValue("template3", "pviewy", 25);
            Array.SetValue("template4", 0, 0);
            Array.SetValue("template4", 1, 10);
            Array.SetValue("template4", 2, 1);
            Array.SetValue("template4", 3, 11);
            Array.SetValue("template4", "color", "Cyan");
            Array.SetValue("template4", "dim", 2);
            Array.SetValue("template4", "pviewx", 12);
            Array.SetValue("template4", "pviewy", 25);
            Array.SetValue("template5", 0, 0);
            Array.SetValue("template5", 1, 10);
            Array.SetValue("template5", 2, 11);
            Array.SetValue("template5", 3, 21);
            Array.SetValue("template5", "color", "Green");
            Array.SetValue("template5", "dim", 3);
            Array.SetValue("template5", "pviewx", 0);
            Array.SetValue("template5", "pviewy", 25);
            Array.SetValue("template6", 0, 10);
            Array.SetValue("template6", 1, 20);
            Array.SetValue("template6", 2, 1);
            Array.SetValue("template6", 3, 11);
            Array.SetValue("template6", "color", "Blue");
            Array.SetValue("template6", "dim", 3);
            Array.SetValue("template6", "pviewx", 0);
            Array.SetValue("template6", "pviewy", 25);
            Array.SetValue("template7", 0, 10);
            Array.SetValue("template7", 1, 11);
            Array.SetValue("template7", 2, 12);
            Array.SetValue("template7", 3, 13);
            Array.SetValue("template7", "color", "Red");
            Array.SetValue("template7", "dim", 4);
            Array.SetValue("template7", "pviewx", 0);
            Array.SetValue("template7", "pviewy", 0);
        }
    }
}