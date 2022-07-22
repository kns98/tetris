using System;
using Microsoft.SmallBasic.Library;
using Array = Microsoft.SmallBasic.Library.Array;
using Math = Microsoft.SmallBasic.Library.Math;

namespace tetris
{
    internal sealed class tetrisModule
    {
        private static int moveDirection;
        private static int BOXES;
        private static int BWIDTH;
        private static int XOFFSET;
        private static int YOFFSET;
        private static int CWIDTH;
        private static int CHEIGHT;
        private static int STARTDELAY;
        private static int ENDDELAY;
        private static int PREVIEW_xpos;
        private static int PREVIEW_ypos;
        private static string template;
        private static string nextPiece;
        private static string h;
        private static int __end;
        private static int sessionDelay;
        private static int delay;
        private static string thisPiece;
        private static int ypos;
        private static int done;
        private static int xpos;
        private static int yposdelta;
        private static int delayIndex;
        private static int invalidMove;
        private static Primitive basetemplate;
        private static string rotation;
        private static int xposbk;
        private static int XOFFSETBK;
        private static int YOFFSETBK;
        private static int L;
        private static int i;
        private static Primitive v;
        private static int x;
        private static int y;
        private static int hcount;
        private static Primitive s;
        private static int score;
        private static int linesCleared;
        private static Primitive piece;
        private static int x1;
        private static int y1;

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
                GraphicsWindow.Title = "Small Basic Tetris";
                GraphicsWindow.Height = 580;
                GraphicsWindow.Width = 700;
                GraphicsWindow.Show();
                SetupTemplates();
                SetupCanvas();
                MainLoop();
                GraphicsWindow.ShowMessage("Game Over", "Small Basic Tetris");
            }
        }

        public static void MainLoop()
        {
            template = Text.Append("template", Math.GetRandomNumber(7));
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
                template = Text.Append("template", Math.GetRandomNumber(7));
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
            if (GraphicsWindow.LastKey == "Escape")
                Program.End();
            if (GraphicsWindow.LastKey == "Left")
            {
                moveDirection = -1;
                ValidateMove();
                if (invalidMove == 0)
                    xpos += moveDirection;
                MovePiece();
            }

            if (GraphicsWindow.LastKey == "Right")
            {
                moveDirection = 1;
                ValidateMove();
                if (invalidMove == 0)
                    xpos += moveDirection;
                MovePiece();
            }

            if ((GraphicsWindow.LastKey == "Down") | (GraphicsWindow.LastKey == "Space"))
                delay = 0;
            if (!(GraphicsWindow.LastKey == "Up"))
                return;
            basetemplate = Array.GetValue(h, -1);
            template = "temptemplate";
            rotation = "CW";
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
                    template = "rotatedtemplate";
                    Array.SetValue(h, -1, template);
                    rotation = "COPY";
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
                template = "";
            }
        }

        public static void DrawPreviewPiece()
        {
            xpos = PREVIEW_xpos;
            ypos = PREVIEW_ypos;
            h = nextPiece;
            XOFFSETBK = XOFFSET;
            YOFFSETBK = YOFFSET;
            XOFFSET += Array.GetValue(Array.GetValue(h, -1), "pviewx");
            YOFFSET += Array.GetValue(Array.GetValue(h, -1), "pviewy");
            MovePiece();
            XOFFSET = XOFFSETBK;
            YOFFSET = YOFFSETBK;
        }

        public static void CopyPiece()
        {
            L = Array.GetValue(basetemplate, "dim");
            if (rotation == "CW")
            {
                var var1 = 0;
                var var2 = BOXES - 1;
                var var3 = 1;
                bool flag = var3 >= var3 - var3;
                i = var1;
                while ((flag ? i <= var2 ? 1 : 0 : i >= var2 ? 1 : 0) != 0)
                {
                    v = Array.GetValue(basetemplate, i);
                    x = Math.Remainder(v, 10);
                    y = L - 1 - Math.Floor(v / 10);
                    Array.SetValue(template, i, x * 10 + y);
                    i += var3;
                }
            }
            else if (rotation == "CCW")
            {
                var var4 = 0;
                var var5 = BOXES - 1;
                var var6 = 1;
                bool flag = var6 >= var6 - var6;
                i = var4;
                while ((flag ? i <= var5 ? 1 : 0 : i >= var5 ? 1 : 0) != 0)
                {
                    v = Array.GetValue(basetemplate, i);
                    x = L - 1 - Math.Remainder(v, 10);
                    y = Math.Floor(v / 10);
                    Array.SetValue(template, i, x * 10 + y);
                    i += var6;
                }
            }
            else if (rotation == "COPY")
            {
                var var7 = 0;
                var var8 = BOXES - 1;
                var var9 = 1;
                bool flag = var9 >= var9 - var9;
                i = var7;
                while ((flag ? i <= var8 ? 1 : 0 : i >= var8 ? 1 : 0) != 0)
                {
                    Array.SetValue(template, i, Array.GetValue(basetemplate, i));
                    i += var9;
                }
            }
            else
            {
                GraphicsWindow.ShowMessage("invalid parameter", "Error");
                Program.End();
            }

            Array.SetValue(template, "color", Array.GetValue(basetemplate, "color"));
            Array.SetValue(template, "dim", Array.GetValue(basetemplate, "dim"));
            Array.SetValue(template, "pviewx", Array.GetValue(basetemplate, "pviewx"));
            Array.SetValue(template, "pviewy", Array.GetValue(basetemplate, "pviewy"));
        }

        public static void CreatePiece()
        {
            hcount += 1;
            h = Text.Append("piece", hcount);
            Array.SetValue(h, -1, template);
            GraphicsWindow.PenWidth = 1;
            GraphicsWindow.PenColor = "Black";
            GraphicsWindow.BrushColor = Array.GetValue(template, "color");
            var var1 = 0;
            var var2 = BOXES - 1;
            var var3 = 1;
            bool flag = var3 >= var3 - var3;
            i = var1;
            while ((flag ? i <= var2 ? 1 : 0 : i >= var2 ? 1 : 0) != 0)
            {
                s = Shapes.AddRectangle(BWIDTH, BWIDTH);
                Shapes.Move(s, -BWIDTH, -BWIDTH);
                Array.SetValue(h, i, s);
                i += var3;
            }
        }

        public static void MovePiece()
        {
            var var1 = 0;
            var var2 = BOXES - 1;
            var var3 = 1;
            bool flag = var3 >= var3 - var3;
            i = var1;
            while ((flag ? i <= var2 ? 1 : 0 : i >= var2 ? 1 : 0) != 0)
            {
                v = Array.GetValue(Array.GetValue(h, -1), i);
                x = Math.Floor(v / 10);
                y = Math.Remainder(v, 10);
                Shapes.Move(Array.GetValue(h, i), XOFFSET + xpos * BWIDTH + x * BWIDTH,
                    YOFFSET + ypos * BWIDTH + y * BWIDTH);
                i += var3;
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

                if (Array.GetValue("c", x + xpos + moveDirection + (y + ypos) * CWIDTH) != ".")
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
                    (Array.GetValue("c", x + xpos + (y + ypos) * CWIDTH) != "."))
                {
                    done = 1;
                    i = BOXES;
                }

                i += 1;
            }

            if (!(done == 1))
                return;
            var var1 = 0;
            var var2 = BOXES - 1;
            var var3 = 1;
            bool flag = var3 >= var3 - var3;
            i = var1;
            while ((flag ? i <= var2 ? 1 : 0 : i >= var2 ? 1 : 0) != 0)
            {
                v = Array.GetValue(Array.GetValue(h, -1), i);
                Array.SetValue("c", Math.Floor(v / 10) + xpos + (Math.Remainder(v, 10) + ypos - 1) * CWIDTH,
                    Array.GetValue(h, i));
                i += var3;
            }

            score += 1;
            PrintScore();
            DeleteLines();
        }

        public static void DeleteLines()
        {
            linesCleared = 0;
            var var1 = CHEIGHT - 1;
            var var2 = 0;
            var var3 = -1;
            bool flag1 = var3 >= var3 - var3;
            y = var1;
            while ((flag1 ? y <= var2 ? 1 : 0 : y >= var2 ? 1 : 0) != 0)
            {
                x = CWIDTH;
                while (x == CWIDTH)
                {
                    x = 0;
                    while (x < CWIDTH)
                    {
                        piece = Array.GetValue("c", x + y * CWIDTH);
                        if (piece == ".")
                            x = CWIDTH;
                        x += 1;
                    }

                    if (x == CWIDTH)
                    {
                        var var4 = 0;
                        var var5 = CWIDTH - 1;
                        var var6 = 1;
                        bool flag2 = var6 >= var6 - var6;
                        x1 = var4;
                        while ((flag2 ? x1 <= var5 ? 1 : 0 : x1 >= var5 ? 1 : 0) != 0)
                        {
                            Shapes.Remove(Array.GetValue("c", x1 + tetrisModule.y * CWIDTH));
                            x1 += var6;
                        }

                        linesCleared += 1;
                        var y = tetrisModule.y;
                        var var7 = 1;
                        var var8 = -1;
                        bool flag3 = var8 >= var8 - var8;
                        y1 = y;
                        while ((flag3 ? y1 <= var7 ? 1 : 0 : y1 >= var7 ? 1 : 0) != 0)
                        {
                            var var9 = 0;
                            var var10 = CWIDTH - 1;
                            var var11 = 1;
                            bool flag4 = var11 >= var11 - var11;
                            x1 = var9;
                            while ((flag4 ? x1 <= var10 ? 1 : 0 : x1 >= var10 ? 1 : 0) != 0)
                            {
                                piece = Array.GetValue("c", x1 + (y1 - 1) * CWIDTH);
                                Array.SetValue("c", x1 + y1 * CWIDTH, piece);
                                Shapes.Move(piece, Shapes.GetLeft(piece), Shapes.GetTop(piece) + BWIDTH);
                                x1 += var11;
                            }

                            y1 += var8;
                        }
                    }
                }

                y += var3;
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
            GraphicsWindow.PenColor = "Pink";
            var var1 = 0;
            var var2 = CWIDTH - 1;
            var var3 = 1;
            bool flag1 = var3 >= var3 - var3;
            x = var1;
            while ((flag1 ? x <= var2 ? 1 : 0 : x >= var2 ? 1 : 0) != 0)
            {
                var var4 = 0;
                var var5 = CHEIGHT - 1;
                var var6 = 1;
                bool flag2 = var6 >= var6 - var6;
                y = var4;
                while ((flag2 ? y <= var5 ? 1 : 0 : y >= var5 ? 1 : 0) != 0)
                {
                    Array.SetValue("c", x + y * CWIDTH, ".");
                    GraphicsWindow.DrawRectangle(XOFFSET + x * BWIDTH, YOFFSET + y * BWIDTH, BWIDTH, BWIDTH);
                    y += var6;
                }

                x += var3;
            }

            GraphicsWindow.PenWidth = 4;
            GraphicsWindow.PenColor = "Black";
            GraphicsWindow.DrawLine(XOFFSET, YOFFSET, XOFFSET, YOFFSET + CHEIGHT * BWIDTH);
            GraphicsWindow.DrawLine(XOFFSET + CWIDTH * BWIDTH, YOFFSET, XOFFSET + CWIDTH * BWIDTH,
                YOFFSET + CHEIGHT * BWIDTH);
            GraphicsWindow.DrawLine(XOFFSET, YOFFSET + CHEIGHT * BWIDTH, XOFFSET + CWIDTH * BWIDTH,
                YOFFSET + CHEIGHT * BWIDTH);
            GraphicsWindow.PenColor = "Lime";
            GraphicsWindow.DrawLine(XOFFSET - 4, YOFFSET, XOFFSET - 4, YOFFSET + CHEIGHT * BWIDTH + 6);
            GraphicsWindow.DrawLine(XOFFSET + CWIDTH * BWIDTH + 4, YOFFSET, XOFFSET + CWIDTH * BWIDTH + 4,
                YOFFSET + CHEIGHT * BWIDTH + 6);
            GraphicsWindow.DrawLine(XOFFSET - 4, YOFFSET + CHEIGHT * BWIDTH + 4, XOFFSET + CWIDTH * BWIDTH + 4,
                YOFFSET + CHEIGHT * BWIDTH + 4);
            GraphicsWindow.PenColor = "Black";
            GraphicsWindow.BrushColor = "Pink";
            x = XOFFSET + PREVIEW_xpos * BWIDTH - BWIDTH;
            y = YOFFSET + PREVIEW_ypos * BWIDTH - BWIDTH;
            GraphicsWindow.FillRectangle(x, y, BWIDTH * 5, BWIDTH * 6);
            GraphicsWindow.DrawRectangle(x, y, BWIDTH * 5, BWIDTH * 6);
            GraphicsWindow.FillRectangle(x - 20, y + 190, 310, 170);
            GraphicsWindow.DrawRectangle(x - 20, y + 190, 310, 170);
            GraphicsWindow.BrushColor = "Black";
            GraphicsWindow.FontItalic = false;
            GraphicsWindow.FontName = "Comic Sans MS";
            GraphicsWindow.FontSize = 16;
            GraphicsWindow.DrawText(x, y + 200, "Game control keys:");
            GraphicsWindow.DrawText(x + 25, y + 220, "Left Arrow = Move piece left");
            GraphicsWindow.DrawText(x + 25, y + 240, "Right Arrow = Move piece right");
            GraphicsWindow.DrawText(x + 25, y + 260, "Up Arrow = Rotate piece");
            GraphicsWindow.DrawText(x + 25, y + 280, "Down Arrow = Drop piece");
            GraphicsWindow.DrawText(x, y + 320, "Press to stop game");
            Program.Delay(200);
            GraphicsWindow.BrushColor = "Black";
            GraphicsWindow.FontName = "Georgia";
            GraphicsWindow.FontItalic = true;
            GraphicsWindow.FontSize = 36;
            GraphicsWindow.DrawText(x - 20, y + 400, "Small Basic Tetris");
            Program.Delay(200);
            GraphicsWindow.FontSize = 16;
            GraphicsWindow.DrawText(x - 20, y + 440, "ver.0.1");
            Program.Delay(200);
            score = 0;
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
                Text.Append(Text.GetSubText("00000000", 0, 8 - Text.GetLength(score)), score));
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