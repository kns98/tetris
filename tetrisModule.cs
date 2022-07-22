using System;
using Microsoft.SmallBasic.Library;
using Array = Microsoft.SmallBasic.Library.Array;
using Math = Microsoft.SmallBasic.Library.Math;

namespace tetris
{
    internal sealed class tetrisModule
    {
        private static Primitive moveDirection;
        private static Primitive BOXES;
        private static Primitive BWIDTH;
        private static Primitive XOFFSET;
        private static Primitive YOFFSET;
        private static Primitive CWIDTH;
        private static Primitive CHEIGHT;
        private static Primitive STARTDELAY;
        private static Primitive ENDDELAY;
        private static Primitive PREVIEW_xpos;
        private static Primitive PREVIEW_ypos;
        private static Primitive template;
        private static Primitive nextPiece;
        private static Primitive h;
        private static Primitive __end;
        private static Primitive sessionDelay;
        private static Primitive delay;
        private static Primitive thisPiece;
        private static Primitive ypos;
        private static Primitive done;
        private static Primitive xpos;
        private static Primitive yposdelta;
        private static Primitive delayIndex;
        private static Primitive invalidMove;
        private static Primitive basetemplate;
        private static Primitive rotation;
        private static Primitive xposbk;
        private static Primitive XOFFSETBK;
        private static Primitive YOFFSETBK;
        private static Primitive L;
        private static Primitive i;
        private static Primitive v;
        private static Primitive x;
        private static Primitive y;
        private static Primitive hcount;
        private static Primitive s;
        private static Primitive score;
        private static Primitive linesCleared;
        private static Primitive piece;
        private static Primitive x1;
        private static Primitive y1;

        [STAThread]
        public static void Main()
        {
            GraphicsWindow.KeyDown += HandleKey;
            GraphicsWindow.BackgroundColor = GraphicsWindow.GetColorFromRGB(253, 252, 251);
            while (true)
            {
                BOXES = 4;
                BWIDTH = 25;
                XOFFSET = 40;
                YOFFSET = 40;
                CWIDTH = 10;
                CHEIGHT = 20;
                STARTDELAY = 800;
                ENDDELAY = 175;
                PREVIEW_xpos = 13;
                PREVIEW_ypos = 2;
                GraphicsWindow.Clear();
                GraphicsWindow.Title = (Primitive)"Small Basic Tetris";
                GraphicsWindow.Height = 580;
                GraphicsWindow.Width = 700;
                GraphicsWindow.Show();
                SetupTemplates();
                SetupCanvas();
                MainLoop();
                GraphicsWindow.ShowMessage((Primitive)"Game Over", (Primitive)"Small Basic Tetris");
            }
        }

        public static void MainLoop()
        {
            template = Text.Append((Primitive)"template", Math.GetRandomNumber(7));
            CreatePiece();
            nextPiece = h;
            __end = 0;
            sessionDelay = STARTDELAY;
            while (__end == 0)
            {
                if (sessionDelay > ENDDELAY)
                    sessionDelay -= 1;
                delay = sessionDelay;
                thisPiece = nextPiece;
                template = Text.Append((Primitive)"template", Math.GetRandomNumber(7));
                CreatePiece();
                nextPiece = h;
                DrawPreviewPiece();
                h = thisPiece;
                ypos = 0;
                done = 0;
                xpos = 3;
                CheckStop();
                if (done == 1)
                {
                    ypos -= 1;
                    MovePiece();
                    __end = 1;
                }

                yposdelta = 0;
                while ((done == 0) | (yposdelta > 0))
                {
                    MovePiece();
                    delayIndex = delay;
                    while ((delayIndex > 0) & (delay > 0))
                    {
                        Program.Delay(10);
                        delayIndex -= 10;
                    }

                    if (yposdelta > 0)
                        yposdelta -= 1;
                    else
                        ypos += 1;
                    CheckStop();
                }
            }
        }

        public static void HandleKey()
        {
            if (GraphicsWindow.LastKey == (Primitive)"Escape")
                Program.End();
            if (GraphicsWindow.LastKey == (Primitive)"Left")
            {
                moveDirection = -1;
                ValidateMove();
                if (invalidMove == 0)
                    xpos += moveDirection;
                MovePiece();
            }

            if (GraphicsWindow.LastKey == (Primitive)"Right")
            {
                moveDirection = 1;
                ValidateMove();
                if (invalidMove == 0)
                    xpos += moveDirection;
                MovePiece();
            }

            if ((GraphicsWindow.LastKey == (Primitive)"Down") | (GraphicsWindow.LastKey == (Primitive)"Space"))
                delay = 0;
            if (!(GraphicsWindow.LastKey == (Primitive)"Up"))
                return;
            basetemplate = Array.GetValue(h, -1);
            template = (Primitive)"temptemplate";
            rotation = (Primitive)"CW";
            CopyPiece();
            Array.SetValue(h, -1, template);
            moveDirection = 0;
            ValidateMove();
            xposbk = xpos;
            yposdelta = 0;
            while ((yposdelta == 0) & (Math.Abs(xposbk - xpos) < 3))
                if (invalidMove == 0)
                {
                    basetemplate = template;
                    template = (Primitive)"rotatedtemplate";
                    Array.SetValue(h, -1, template);
                    rotation = (Primitive)"COPY";
                    CopyPiece();
                    yposdelta = 1;
                    MovePiece();
                }
                else if (invalidMove == 2)
                {
                    xpos = 99;
                }
                else
                {
                    xpos -= invalidMove;
                    ValidateMove();
                }

            if (invalidMove != 0)
            {
                xpos = xposbk;
                Array.SetValue(h, -1, basetemplate);
                template = (Primitive)"";
            }
        }

        public static void DrawPreviewPiece()
        {
            xpos = PREVIEW_xpos;
            ypos = PREVIEW_ypos;
            h = nextPiece;
            XOFFSETBK = XOFFSET;
            YOFFSETBK = YOFFSET;
            XOFFSET += Array.GetValue(Array.GetValue(h, -1), (Primitive)"pviewx");
            YOFFSET += Array.GetValue(Array.GetValue(h, -1), (Primitive)"pviewy");
            MovePiece();
            XOFFSET = XOFFSETBK;
            YOFFSET = YOFFSETBK;
        }

        public static void CopyPiece()
        {
            L = Array.GetValue(basetemplate, (Primitive)"dim");
            if (rotation == (Primitive)"CW")
            {
                Primitive primitive1 = 0;
                var primitive2 = BOXES - 1;
                Primitive primitive3 = 1;
                bool flag = primitive3 >= primitive3 - primitive3;
                i = primitive1;
                while ((flag ? i <= primitive2 ? 1 : 0 : i >= primitive2 ? 1 : 0) != 0)
                {
                    v = Array.GetValue(basetemplate, i);
                    x = Math.Remainder(v, 10);
                    y = L - 1 - Math.Floor(v / 10);
                    Array.SetValue(template, i, x * 10 + y);
                    i += primitive3;
                }
            }
            else if (rotation == (Primitive)"CCW")
            {
                Primitive primitive4 = 0;
                var primitive5 = BOXES - 1;
                Primitive primitive6 = 1;
                bool flag = primitive6 >= primitive6 - primitive6;
                i = primitive4;
                while ((flag ? i <= primitive5 ? 1 : 0 : i >= primitive5 ? 1 : 0) != 0)
                {
                    v = Array.GetValue(basetemplate, i);
                    x = L - 1 - Math.Remainder(v, 10);
                    y = Math.Floor(v / 10);
                    Array.SetValue(template, i, x * 10 + y);
                    i += primitive6;
                }
            }
            else if (rotation == (Primitive)"COPY")
            {
                Primitive primitive7 = 0;
                var primitive8 = BOXES - 1;
                Primitive primitive9 = 1;
                bool flag = primitive9 >= primitive9 - primitive9;
                i = primitive7;
                while ((flag ? i <= primitive8 ? 1 : 0 : i >= primitive8 ? 1 : 0) != 0)
                {
                    Array.SetValue(template, i, Array.GetValue(basetemplate, i));
                    i += primitive9;
                }
            }
            else
            {
                GraphicsWindow.ShowMessage((Primitive)"invalid parameter", (Primitive)"Error");
                Program.End();
            }

            Array.SetValue(template, (Primitive)"color", Array.GetValue(basetemplate, (Primitive)"color"));
            Array.SetValue(template, (Primitive)"dim", Array.GetValue(basetemplate, (Primitive)"dim"));
            Array.SetValue(template, (Primitive)"pviewx", Array.GetValue(basetemplate, (Primitive)"pviewx"));
            Array.SetValue(template, (Primitive)"pviewy", Array.GetValue(basetemplate, (Primitive)"pviewy"));
        }

        public static void CreatePiece()
        {
            hcount += 1;
            h = Text.Append((Primitive)"piece", hcount);
            Array.SetValue(h, -1, template);
            GraphicsWindow.PenWidth = 1;
            GraphicsWindow.PenColor = (Primitive)"Black";
            GraphicsWindow.BrushColor = Array.GetValue(template, (Primitive)"color");
            Primitive primitive1 = 0;
            var primitive2 = BOXES - 1;
            Primitive primitive3 = 1;
            bool flag = primitive3 >= primitive3 - primitive3;
            i = primitive1;
            while ((flag ? i <= primitive2 ? 1 : 0 : i >= primitive2 ? 1 : 0) != 0)
            {
                s = Shapes.AddRectangle(BWIDTH, BWIDTH);
                Shapes.Move(s, -BWIDTH, -BWIDTH);
                Array.SetValue(h, i, s);
                i += primitive3;
            }
        }

        public static void MovePiece()
        {
            Primitive primitive1 = 0;
            var primitive2 = BOXES - 1;
            Primitive primitive3 = 1;
            bool flag = primitive3 >= primitive3 - primitive3;
            i = primitive1;
            while ((flag ? i <= primitive2 ? 1 : 0 : i >= primitive2 ? 1 : 0) != 0)
            {
                v = Array.GetValue(Array.GetValue(h, -1), i);
                x = Math.Floor(v / 10);
                y = Math.Remainder(v, 10);
                Shapes.Move(Array.GetValue(h, i), XOFFSET + xpos * BWIDTH + x * BWIDTH,
                    YOFFSET + ypos * BWIDTH + y * BWIDTH);
                i += primitive3;
            }
        }

        public static void ValidateMove()
        {
            i = 0;
            invalidMove = 0;
            while (i < BOXES)
            {
                v = Array.GetValue(Array.GetValue(h, -1), i);
                x = Math.Floor(v / 10);
                y = Math.Remainder(v, 10);
                if (x + xpos + moveDirection < 0)
                {
                    invalidMove = -1;
                    i = BOXES;
                }

                if (x + xpos + moveDirection >= CWIDTH)
                {
                    invalidMove = 1;
                    i = BOXES;
                }

                if (Array.GetValue((Primitive)"c", x + xpos + moveDirection + (y + ypos) * CWIDTH) != (Primitive)".")
                {
                    invalidMove = 2;
                    i = BOXES;
                }

                i += 1;
            }
        }

        public static void CheckStop()
        {
            done = 0;
            i = 0;
            while (i < BOXES)
            {
                v = Array.GetValue(Array.GetValue(h, -1), i);
                x = Math.Floor(v / 10);
                y = Math.Remainder(v, 10);
                if ((y + ypos > CHEIGHT) |
                    (Array.GetValue((Primitive)"c", x + xpos + (y + ypos) * CWIDTH) != (Primitive)"."))
                {
                    done = 1;
                    i = BOXES;
                }

                i += 1;
            }

            if (!(done == 1))
                return;
            Primitive primitive1 = 0;
            var primitive2 = BOXES - 1;
            Primitive primitive3 = 1;
            bool flag = primitive3 >= primitive3 - primitive3;
            i = primitive1;
            while ((flag ? i <= primitive2 ? 1 : 0 : i >= primitive2 ? 1 : 0) != 0)
            {
                v = Array.GetValue(Array.GetValue(h, -1), i);
                Array.SetValue((Primitive)"c", Math.Floor(v / 10) + xpos + (Math.Remainder(v, 10) + ypos - 1) * CWIDTH,
                    Array.GetValue(h, i));
                i += primitive3;
            }

            score += 1;
            PrintScore();
            DeleteLines();
        }

        public static void DeleteLines()
        {
            linesCleared = 0;
            var primitive1 = CHEIGHT - 1;
            Primitive primitive2 = 0;
            Primitive primitive3 = -1;
            bool flag1 = primitive3 >= primitive3 - primitive3;
            y = primitive1;
            while ((flag1 ? y <= primitive2 ? 1 : 0 : y >= primitive2 ? 1 : 0) != 0)
            {
                x = CWIDTH;
                while (x == CWIDTH)
                {
                    x = 0;
                    while (x < CWIDTH)
                    {
                        piece = Array.GetValue((Primitive)"c", x + y * CWIDTH);
                        if (piece == (Primitive)".")
                            x = CWIDTH;
                        x += 1;
                    }

                    if (x == CWIDTH)
                    {
                        Primitive primitive4 = 0;
                        var primitive5 = CWIDTH - 1;
                        Primitive primitive6 = 1;
                        bool flag2 = primitive6 >= primitive6 - primitive6;
                        x1 = primitive4;
                        while ((flag2 ? x1 <= primitive5 ? 1 : 0 : x1 >= primitive5 ? 1 : 0) != 0)
                        {
                            Shapes.Remove(Array.GetValue((Primitive)"c", x1 + tetrisModule.y * CWIDTH));
                            x1 += primitive6;
                        }

                        linesCleared += 1;
                        var y = tetrisModule.y;
                        Primitive primitive7 = 1;
                        Primitive primitive8 = -1;
                        bool flag3 = primitive8 >= primitive8 - primitive8;
                        y1 = y;
                        while ((flag3 ? y1 <= primitive7 ? 1 : 0 : y1 >= primitive7 ? 1 : 0) != 0)
                        {
                            Primitive primitive9 = 0;
                            var primitive10 = CWIDTH - 1;
                            Primitive primitive11 = 1;
                            bool flag4 = primitive11 >= primitive11 - primitive11;
                            x1 = primitive9;
                            while ((flag4 ? x1 <= primitive10 ? 1 : 0 : x1 >= primitive10 ? 1 : 0) != 0)
                            {
                                piece = Array.GetValue((Primitive)"c", x1 + (y1 - 1) * CWIDTH);
                                Array.SetValue((Primitive)"c", x1 + y1 * CWIDTH, piece);
                                Shapes.Move(piece, Shapes.GetLeft(piece), Shapes.GetTop(piece) + BWIDTH);
                                x1 += primitive11;
                            }

                            y1 += primitive8;
                        }
                    }
                }

                y += primitive3;
            }

            if (!(linesCleared > 0))
                return;
            score += 100 * Math.Round(linesCleared * 2.15 - 1);
            PrintScore();
        }

        public static void SetupCanvas()
        {
            GraphicsWindow.BrushColor = GraphicsWindow.BackgroundColor;
            GraphicsWindow.FillRectangle(XOFFSET, YOFFSET, CWIDTH * BWIDTH, CHEIGHT * BWIDTH);
            Program.Delay(200);
            GraphicsWindow.PenWidth = 1;
            GraphicsWindow.PenColor = (Primitive)"Pink";
            Primitive primitive1 = 0;
            var primitive2 = CWIDTH - 1;
            Primitive primitive3 = 1;
            bool flag1 = primitive3 >= primitive3 - primitive3;
            x = primitive1;
            while ((flag1 ? x <= primitive2 ? 1 : 0 : x >= primitive2 ? 1 : 0) != 0)
            {
                Primitive primitive4 = 0;
                var primitive5 = CHEIGHT - 1;
                Primitive primitive6 = 1;
                bool flag2 = primitive6 >= primitive6 - primitive6;
                y = primitive4;
                while ((flag2 ? y <= primitive5 ? 1 : 0 : y >= primitive5 ? 1 : 0) != 0)
                {
                    Array.SetValue((Primitive)"c", x + y * CWIDTH, (Primitive)".");
                    GraphicsWindow.DrawRectangle(XOFFSET + x * BWIDTH, YOFFSET + y * BWIDTH, BWIDTH, BWIDTH);
                    y += primitive6;
                }

                x += primitive3;
            }

            GraphicsWindow.PenWidth = 4;
            GraphicsWindow.PenColor = (Primitive)"Black";
            GraphicsWindow.DrawLine(XOFFSET, YOFFSET, XOFFSET, YOFFSET + CHEIGHT * BWIDTH);
            GraphicsWindow.DrawLine(XOFFSET + CWIDTH * BWIDTH, YOFFSET, XOFFSET + CWIDTH * BWIDTH,
                YOFFSET + CHEIGHT * BWIDTH);
            GraphicsWindow.DrawLine(XOFFSET, YOFFSET + CHEIGHT * BWIDTH, XOFFSET + CWIDTH * BWIDTH,
                YOFFSET + CHEIGHT * BWIDTH);
            GraphicsWindow.PenColor = (Primitive)"Lime";
            GraphicsWindow.DrawLine(XOFFSET - 4, YOFFSET, XOFFSET - 4, YOFFSET + CHEIGHT * BWIDTH + 6);
            GraphicsWindow.DrawLine(XOFFSET + CWIDTH * BWIDTH + 4, YOFFSET, XOFFSET + CWIDTH * BWIDTH + 4,
                YOFFSET + CHEIGHT * BWIDTH + 6);
            GraphicsWindow.DrawLine(XOFFSET - 4, YOFFSET + CHEIGHT * BWIDTH + 4, XOFFSET + CWIDTH * BWIDTH + 4,
                YOFFSET + CHEIGHT * BWIDTH + 4);
            GraphicsWindow.PenColor = (Primitive)"Black";
            GraphicsWindow.BrushColor = (Primitive)"Pink";
            x = XOFFSET + PREVIEW_xpos * BWIDTH - BWIDTH;
            y = YOFFSET + PREVIEW_ypos * BWIDTH - BWIDTH;
            GraphicsWindow.FillRectangle(x, y, BWIDTH * 5, BWIDTH * 6);
            GraphicsWindow.DrawRectangle(x, y, BWIDTH * 5, BWIDTH * 6);
            GraphicsWindow.FillRectangle(x - 20, y + 190, 310, 170);
            GraphicsWindow.DrawRectangle(x - 20, y + 190, 310, 170);
            GraphicsWindow.BrushColor = (Primitive)"Black";
            GraphicsWindow.FontItalic = false;
            GraphicsWindow.FontName = (Primitive)"Comic Sans MS";
            GraphicsWindow.FontSize = 16;
            GraphicsWindow.DrawText(x, y + 200, (Primitive)"Game control keys:");
            GraphicsWindow.DrawText(x + 25, y + 220, (Primitive)"Left Arrow = Move piece left");
            GraphicsWindow.DrawText(x + 25, y + 240, (Primitive)"Right Arrow = Move piece right");
            GraphicsWindow.DrawText(x + 25, y + 260, (Primitive)"Up Arrow = Rotate piece");
            GraphicsWindow.DrawText(x + 25, y + 280, (Primitive)"Down Arrow = Drop piece");
            GraphicsWindow.DrawText(x, y + 320, (Primitive)"Press to stop game");
            Program.Delay(200);
            GraphicsWindow.BrushColor = (Primitive)"Black";
            GraphicsWindow.FontName = (Primitive)"Georgia";
            GraphicsWindow.FontItalic = true;
            GraphicsWindow.FontSize = 36;
            GraphicsWindow.DrawText(x - 20, y + 400, (Primitive)"Small Basic Tetris");
            Program.Delay(200);
            GraphicsWindow.FontSize = 16;
            GraphicsWindow.DrawText(x - 20, y + 440, (Primitive)"ver.0.1");
            Program.Delay(200);
            score = 0;
            PrintScore();
        }

        public static void PrintScore()
        {
            GraphicsWindow.PenWidth = 4;
            GraphicsWindow.BrushColor = (Primitive)"Pink";
            GraphicsWindow.FillRectangle(500, 65, 153, 50);
            GraphicsWindow.BrushColor = (Primitive)"Black";
            GraphicsWindow.DrawRectangle(500, 65, 153, 50);
            GraphicsWindow.FontItalic = false;
            GraphicsWindow.FontSize = 32;
            GraphicsWindow.FontName = (Primitive)"Impact";
            GraphicsWindow.BrushColor = (Primitive)"Black";
            GraphicsWindow.DrawText(505, 70,
                Text.Append(Text.GetSubText((Primitive)"00000000", 0, 8 - Text.GetLength(score)), score));
        }

        public static void SetupTemplates()
        {
            Array.SetValue((Primitive)"template1", 0, 10);
            Array.SetValue((Primitive)"template1", 1, 11);
            Array.SetValue((Primitive)"template1", 2, 12);
            Array.SetValue((Primitive)"template1", 3, 22);
            Array.SetValue((Primitive)"template1", (Primitive)"color", (Primitive)"Yellow");
            Array.SetValue((Primitive)"template1", (Primitive)"dim", 3);
            Array.SetValue((Primitive)"template1", (Primitive)"pviewx", -12);
            Array.SetValue((Primitive)"template1", (Primitive)"pviewy", 12);
            Array.SetValue((Primitive)"template2", 0, 10);
            Array.SetValue((Primitive)"template2", 1, 11);
            Array.SetValue((Primitive)"template2", 2, 12);
            Array.SetValue((Primitive)"template2", 3, 2);
            Array.SetValue((Primitive)"template2", (Primitive)"color", (Primitive)"Magenta");
            Array.SetValue((Primitive)"template2", (Primitive)"dim", 3);
            Array.SetValue((Primitive)"template2", (Primitive)"pviewx", 12);
            Array.SetValue((Primitive)"template2", (Primitive)"pviewy", 12);
            Array.SetValue((Primitive)"template3", 0, 10);
            Array.SetValue((Primitive)"template3", 1, 1);
            Array.SetValue((Primitive)"template3", 2, 11);
            Array.SetValue((Primitive)"template3", 3, 21);
            Array.SetValue((Primitive)"template3", (Primitive)"color", (Primitive)"Gray");
            Array.SetValue((Primitive)"template3", (Primitive)"dim", 3);
            Array.SetValue((Primitive)"template3", (Primitive)"pviewx", 0);
            Array.SetValue((Primitive)"template3", (Primitive)"pviewy", 25);
            Array.SetValue((Primitive)"template4", 0, 0);
            Array.SetValue((Primitive)"template4", 1, 10);
            Array.SetValue((Primitive)"template4", 2, 1);
            Array.SetValue((Primitive)"template4", 3, 11);
            Array.SetValue((Primitive)"template4", (Primitive)"color", (Primitive)"Cyan");
            Array.SetValue((Primitive)"template4", (Primitive)"dim", 2);
            Array.SetValue((Primitive)"template4", (Primitive)"pviewx", 12);
            Array.SetValue((Primitive)"template4", (Primitive)"pviewy", 25);
            Array.SetValue((Primitive)"template5", 0, 0);
            Array.SetValue((Primitive)"template5", 1, 10);
            Array.SetValue((Primitive)"template5", 2, 11);
            Array.SetValue((Primitive)"template5", 3, 21);
            Array.SetValue((Primitive)"template5", (Primitive)"color", (Primitive)"Green");
            Array.SetValue((Primitive)"template5", (Primitive)"dim", 3);
            Array.SetValue((Primitive)"template5", (Primitive)"pviewx", 0);
            Array.SetValue((Primitive)"template5", (Primitive)"pviewy", 25);
            Array.SetValue((Primitive)"template6", 0, 10);
            Array.SetValue((Primitive)"template6", 1, 20);
            Array.SetValue((Primitive)"template6", 2, 1);
            Array.SetValue((Primitive)"template6", 3, 11);
            Array.SetValue((Primitive)"template6", (Primitive)"color", (Primitive)"Blue");
            Array.SetValue((Primitive)"template6", (Primitive)"dim", 3);
            Array.SetValue((Primitive)"template6", (Primitive)"pviewx", 0);
            Array.SetValue((Primitive)"template6", (Primitive)"pviewy", 25);
            Array.SetValue((Primitive)"template7", 0, 10);
            Array.SetValue((Primitive)"template7", 1, 11);
            Array.SetValue((Primitive)"template7", 2, 12);
            Array.SetValue((Primitive)"template7", 3, 13);
            Array.SetValue((Primitive)"template7", (Primitive)"color", (Primitive)"Red");
            Array.SetValue((Primitive)"template7", (Primitive)"dim", 4);
            Array.SetValue((Primitive)"template7", (Primitive)"pviewx", 0);
            Array.SetValue((Primitive)"template7", (Primitive)"pviewy", 0);
        }
    }
}