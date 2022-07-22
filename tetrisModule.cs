using Microsoft.SmallBasic.Library;
using Microsoft.VisualBasic.CompilerServices;
using System;

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
      GraphicsWindow.KeyDown += new SmallBasicCallback(tetrisModule.HandleKey);
      GraphicsWindow.BackgroundColor = GraphicsWindow.GetColorFromRGB((Primitive) 253, (Primitive) 252, (Primitive) 251);
      while (true)
      {
        tetrisModule.BOXES = (Primitive) 4;
        tetrisModule.BWIDTH = (Primitive) 25;
        tetrisModule.XOFFSET = (Primitive) 40;
        tetrisModule.YOFFSET = (Primitive) 40;
        tetrisModule.CWIDTH = (Primitive) 10;
        tetrisModule.CHEIGHT = (Primitive) 20;
        tetrisModule.STARTDELAY = (Primitive) 800;
        tetrisModule.ENDDELAY = (Primitive) 175;
        tetrisModule.PREVIEW_xpos = (Primitive) 13;
        tetrisModule.PREVIEW_ypos = (Primitive) 2;
        GraphicsWindow.Clear();
        GraphicsWindow.Title = (Primitive) "Small Basic Tetris";
        GraphicsWindow.Height = (Primitive) 580;
        GraphicsWindow.Width = (Primitive) 700;
        GraphicsWindow.Show();
        tetrisModule.SetupTemplates();
        tetrisModule.SetupCanvas();
        tetrisModule.MainLoop();
        GraphicsWindow.ShowMessage((Primitive) "Game Over", (Primitive) "Small Basic Tetris");
      }
    }

    public static void MainLoop()
    {
      tetrisModule.template = Text.Append((Primitive) "template", Microsoft.SmallBasic.Library.Math.GetRandomNumber((Primitive) 7));
      tetrisModule.CreatePiece();
      tetrisModule.nextPiece = tetrisModule.h;
      tetrisModule.__end = (Primitive) 0;
      tetrisModule.sessionDelay = tetrisModule.STARTDELAY;
      while ((bool) (tetrisModule.__end == (Primitive) 0))
      {
        if ((bool) (tetrisModule.sessionDelay > tetrisModule.ENDDELAY))
          tetrisModule.sessionDelay -= (Primitive) 1;
        tetrisModule.delay = tetrisModule.sessionDelay;
        tetrisModule.thisPiece = tetrisModule.nextPiece;
        tetrisModule.template = Text.Append((Primitive) "template", Microsoft.SmallBasic.Library.Math.GetRandomNumber((Primitive) 7));
        tetrisModule.CreatePiece();
        tetrisModule.nextPiece = tetrisModule.h;
        tetrisModule.DrawPreviewPiece();
        tetrisModule.h = tetrisModule.thisPiece;
        tetrisModule.ypos = (Primitive) 0;
        tetrisModule.done = (Primitive) 0;
        tetrisModule.xpos = (Primitive) 3;
        tetrisModule.CheckStop();
        if ((bool) (tetrisModule.done == (Primitive) 1))
        {
          tetrisModule.ypos -= (Primitive) 1;
          tetrisModule.MovePiece();
          tetrisModule.__end = (Primitive) 1;
        }
        tetrisModule.yposdelta = (Primitive) 0;
        while ((bool) (tetrisModule.done == (Primitive) 0 | tetrisModule.yposdelta > (Primitive) 0))
        {
          tetrisModule.MovePiece();
          tetrisModule.delayIndex = tetrisModule.delay;
          while ((bool) (tetrisModule.delayIndex > (Primitive) 0 & tetrisModule.delay > (Primitive) 0))
          {
            Program.Delay((Primitive) 10);
            tetrisModule.delayIndex -= (Primitive) 10;
          }
          if ((bool) (tetrisModule.yposdelta > (Primitive) 0))
            tetrisModule.yposdelta -= (Primitive) 1;
          else
            tetrisModule.ypos += (Primitive) 1;
          tetrisModule.CheckStop();
        }
      }
    }

    public static void HandleKey()
    {
      if ((bool) (GraphicsWindow.LastKey == (Primitive) "Escape"))
        Program.End();
      if ((bool) (GraphicsWindow.LastKey == (Primitive) "Left"))
      {
        tetrisModule.moveDirection = (Primitive)(-1);
        tetrisModule.ValidateMove();
        if ((bool) (tetrisModule.invalidMove == (Primitive) 0))
          tetrisModule.xpos += tetrisModule.moveDirection;
        tetrisModule.MovePiece();
      }
      if ((bool) (GraphicsWindow.LastKey == (Primitive) "Right"))
      {
        tetrisModule.moveDirection = (Primitive) 1;
        tetrisModule.ValidateMove();
        if ((bool) (tetrisModule.invalidMove == (Primitive) 0))
          tetrisModule.xpos += tetrisModule.moveDirection;
        tetrisModule.MovePiece();
      }
      if ((bool) (GraphicsWindow.LastKey == (Primitive) "Down" | GraphicsWindow.LastKey == (Primitive) "Space"))
        tetrisModule.delay = (Primitive) 0;
      if (!(bool) (GraphicsWindow.LastKey == (Primitive) "Up"))
        return;
      tetrisModule.basetemplate = Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, (Primitive) (-1));
      tetrisModule.template = (Primitive) "temptemplate";
      tetrisModule.rotation = (Primitive) "CW";
      tetrisModule.CopyPiece();
      Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.h, (Primitive) (-1), tetrisModule.template);
      tetrisModule.moveDirection = (Primitive) 0;
      tetrisModule.ValidateMove();
      tetrisModule.xposbk = tetrisModule.xpos;
      tetrisModule.yposdelta = (Primitive) 0;
      while ((bool) (tetrisModule.yposdelta == (Primitive) 0 & Microsoft.SmallBasic.Library.Math.Abs(tetrisModule.xposbk - tetrisModule.xpos) < (Primitive) 3))
      {
        if ((bool) (tetrisModule.invalidMove == (Primitive) 0))
        {
          tetrisModule.basetemplate = tetrisModule.template;
          tetrisModule.template = (Primitive) "rotatedtemplate";
          Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.h, (Primitive)(-1), tetrisModule.template);
          tetrisModule.rotation = (Primitive) "COPY";
          tetrisModule.CopyPiece();
          tetrisModule.yposdelta = (Primitive) 1;
          tetrisModule.MovePiece();
        }
        else if ((bool) (tetrisModule.invalidMove == (Primitive) 2))
        {
          tetrisModule.xpos = (Primitive) 99;
        }
        else
        {
          tetrisModule.xpos -= tetrisModule.invalidMove;
          tetrisModule.ValidateMove();
        }
      }
      if ((bool) (tetrisModule.invalidMove != (Primitive) 0))
      {
        tetrisModule.xpos = tetrisModule.xposbk;
        Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.h, (Primitive)(-1), tetrisModule.basetemplate);
        tetrisModule.template = (Primitive) "";
      }
    }

    public static void DrawPreviewPiece()
    {
      tetrisModule.xpos = tetrisModule.PREVIEW_xpos;
      tetrisModule.ypos = tetrisModule.PREVIEW_ypos;
      tetrisModule.h = tetrisModule.nextPiece;
      tetrisModule.XOFFSETBK = tetrisModule.XOFFSET;
      tetrisModule.YOFFSETBK = tetrisModule.YOFFSET;
      tetrisModule.XOFFSET += Microsoft.SmallBasic.Library.Array.GetValue(Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, (Primitive)(-1)), (Primitive) "pviewx");
      tetrisModule.YOFFSET += Microsoft.SmallBasic.Library.Array.GetValue(Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, (Primitive)(-1)), (Primitive) "pviewy");
      tetrisModule.MovePiece();
      tetrisModule.XOFFSET = tetrisModule.XOFFSETBK;
      tetrisModule.YOFFSET = tetrisModule.YOFFSETBK;
    }

    public static void CopyPiece()
    {
      tetrisModule.L = Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, (Primitive) "dim");
      if ((bool) (tetrisModule.rotation == (Primitive) "CW"))
      {
        Primitive primitive1 = (Primitive) 0;
        Primitive primitive2 = tetrisModule.BOXES - (Primitive) 1;
        Primitive primitive3 = (Primitive) 1;
        bool flag = (bool) (primitive3 >= primitive3 - primitive3);
        tetrisModule.i = primitive1;
        while ((flag ? ((bool) (tetrisModule.i <= primitive2) ? 1 : 0) : ((bool) (tetrisModule.i >= primitive2) ? 1 : 0)) != 0)
        {
          tetrisModule.v = Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, tetrisModule.i);
          tetrisModule.x = Microsoft.SmallBasic.Library.Math.Remainder(tetrisModule.v, (Primitive) 10);
          tetrisModule.y = tetrisModule.L - (Primitive) 1 - Microsoft.SmallBasic.Library.Math.Floor(tetrisModule.v / (Primitive) 10);
          Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.template, tetrisModule.i, tetrisModule.x * (Primitive) 10 + tetrisModule.y);
          tetrisModule.i += primitive3;
        }
      }
      else if ((bool) (tetrisModule.rotation == (Primitive) "CCW"))
      {
        Primitive primitive4 = (Primitive) 0;
        Primitive primitive5 = tetrisModule.BOXES - (Primitive) 1;
        Primitive primitive6 = (Primitive) 1;
        bool flag = (bool) (primitive6 >= primitive6 - primitive6);
        tetrisModule.i = primitive4;
        while ((flag ? ((bool) (tetrisModule.i <= primitive5) ? 1 : 0) : ((bool) (tetrisModule.i >= primitive5) ? 1 : 0)) != 0)
        {
          tetrisModule.v = Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, tetrisModule.i);
          tetrisModule.x = tetrisModule.L - (Primitive) 1 - Microsoft.SmallBasic.Library.Math.Remainder(tetrisModule.v, (Primitive) 10);
          tetrisModule.y = Microsoft.SmallBasic.Library.Math.Floor(tetrisModule.v / (Primitive) 10);
          Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.template, tetrisModule.i, tetrisModule.x * (Primitive) 10 + tetrisModule.y);
          tetrisModule.i += primitive6;
        }
      }
      else if ((bool) (tetrisModule.rotation == (Primitive) "COPY"))
      {
        Primitive primitive7 = (Primitive) 0;
        Primitive primitive8 = tetrisModule.BOXES - (Primitive) 1;
        Primitive primitive9 = (Primitive) 1;
        bool flag = (bool) (primitive9 >= primitive9 - primitive9);
        tetrisModule.i = primitive7;
        while ((flag ? ((bool) (tetrisModule.i <= primitive8) ? 1 : 0) : ((bool) (tetrisModule.i >= primitive8) ? 1 : 0)) != 0)
        {
          Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.template, tetrisModule.i, Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, tetrisModule.i));
          tetrisModule.i += primitive9;
        }
      }
      else
      {
        GraphicsWindow.ShowMessage((Primitive) "invalid parameter", (Primitive) "Error");
        Program.End();
      }
      Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.template, (Primitive) "color", Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, (Primitive) "color"));
      Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.template, (Primitive) "dim", Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, (Primitive) "dim"));
      Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.template, (Primitive) "pviewx", Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, (Primitive) "pviewx"));
      Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.template, (Primitive) "pviewy", Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.basetemplate, (Primitive) "pviewy"));
    }

    public static void CreatePiece()
    {
      tetrisModule.hcount += (Primitive) 1;
      tetrisModule.h = Text.Append((Primitive) "piece", tetrisModule.hcount);
      Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.h, (Primitive)(-1), tetrisModule.template);
      GraphicsWindow.PenWidth = (Primitive) 1;
      GraphicsWindow.PenColor = (Primitive) "Black";
      GraphicsWindow.BrushColor = Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.template, (Primitive) "color");
      Primitive primitive1 = (Primitive) 0;
      Primitive primitive2 = tetrisModule.BOXES - (Primitive) 1;
      Primitive primitive3 = (Primitive) 1;
      bool flag = (bool) (primitive3 >= primitive3 - primitive3);
      tetrisModule.i = primitive1;
      while ((flag ? ((bool) (tetrisModule.i <= primitive2) ? 1 : 0) : ((bool) (tetrisModule.i >= primitive2) ? 1 : 0)) != 0)
      {
        tetrisModule.s = Shapes.AddRectangle(tetrisModule.BWIDTH, tetrisModule.BWIDTH);
        Shapes.Move(tetrisModule.s, -tetrisModule.BWIDTH, -tetrisModule.BWIDTH);
        Microsoft.SmallBasic.Library.Array.SetValue(tetrisModule.h, tetrisModule.i, tetrisModule.s);
        tetrisModule.i += primitive3;
      }
    }

    public static void MovePiece()
    {
      Primitive primitive1 = (Primitive) 0;
      Primitive primitive2 = tetrisModule.BOXES - (Primitive) 1;
      Primitive primitive3 = (Primitive) 1;
      bool flag = (bool) (primitive3 >= primitive3 - primitive3);
      tetrisModule.i = primitive1;
      while ((flag ? ((bool) (tetrisModule.i <= primitive2) ? 1 : 0) : ((bool) (tetrisModule.i >= primitive2) ? 1 : 0)) != 0)
      {
        tetrisModule.v = Microsoft.SmallBasic.Library.Array.GetValue(Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, (Primitive)(-1)), tetrisModule.i);
        tetrisModule.x = Microsoft.SmallBasic.Library.Math.Floor(tetrisModule.v / (Primitive) 10);
        tetrisModule.y = Microsoft.SmallBasic.Library.Math.Remainder(tetrisModule.v, (Primitive) 10);
        Shapes.Move(Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, tetrisModule.i), tetrisModule.XOFFSET + tetrisModule.xpos * tetrisModule.BWIDTH + tetrisModule.x * tetrisModule.BWIDTH, tetrisModule.YOFFSET + tetrisModule.ypos * tetrisModule.BWIDTH + tetrisModule.y * tetrisModule.BWIDTH);
        tetrisModule.i += primitive3;
      }
    }

    public static void ValidateMove()
    {
      tetrisModule.i = (Primitive) 0;
      tetrisModule.invalidMove = (Primitive) 0;
      while ((bool) (tetrisModule.i < tetrisModule.BOXES))
      {
        tetrisModule.v = Microsoft.SmallBasic.Library.Array.GetValue(Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, (Primitive)(-1)), tetrisModule.i);
        tetrisModule.x = Microsoft.SmallBasic.Library.Math.Floor(tetrisModule.v / (Primitive) 10);
        tetrisModule.y = Microsoft.SmallBasic.Library.Math.Remainder(tetrisModule.v, (Primitive) 10);
        if ((bool) (tetrisModule.x + tetrisModule.xpos + tetrisModule.moveDirection < (Primitive) 0))
        {
          tetrisModule.invalidMove = (Primitive)(-1);
          tetrisModule.i = tetrisModule.BOXES;
        }
        if ((bool) (tetrisModule.x + tetrisModule.xpos + tetrisModule.moveDirection >= tetrisModule.CWIDTH))
        {
          tetrisModule.invalidMove = (Primitive) 1;
          tetrisModule.i = tetrisModule.BOXES;
        }
        if ((bool) (Microsoft.SmallBasic.Library.Array.GetValue((Primitive) "c", tetrisModule.x + tetrisModule.xpos + tetrisModule.moveDirection + (tetrisModule.y + tetrisModule.ypos) * tetrisModule.CWIDTH) != (Primitive) "."))
        {
          tetrisModule.invalidMove = (Primitive) 2;
          tetrisModule.i = tetrisModule.BOXES;
        }
        tetrisModule.i += (Primitive) 1;
      }
    }

    public static void CheckStop()
    {
      tetrisModule.done = (Primitive) 0;
      tetrisModule.i = (Primitive) 0;
      while ((bool) (tetrisModule.i < tetrisModule.BOXES))
      {
        tetrisModule.v = Microsoft.SmallBasic.Library.Array.GetValue(Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, (Primitive)(-1)), tetrisModule.i);
        tetrisModule.x = Microsoft.SmallBasic.Library.Math.Floor(tetrisModule.v / (Primitive) 10);
        tetrisModule.y = Microsoft.SmallBasic.Library.Math.Remainder(tetrisModule.v, (Primitive) 10);
        if ((bool) (tetrisModule.y + tetrisModule.ypos > tetrisModule.CHEIGHT | Microsoft.SmallBasic.Library.Array.GetValue((Primitive) "c", tetrisModule.x + tetrisModule.xpos + (tetrisModule.y + tetrisModule.ypos) * tetrisModule.CWIDTH) != (Primitive) "."))
        {
          tetrisModule.done = (Primitive) 1;
          tetrisModule.i = tetrisModule.BOXES;
        }
        tetrisModule.i += (Primitive) 1;
      }
      if (!(bool) (tetrisModule.done == (Primitive) 1))
        return;
      Primitive primitive1 = (Primitive) 0;
      Primitive primitive2 = tetrisModule.BOXES - (Primitive) 1;
      Primitive primitive3 = (Primitive) 1;
      bool flag = (bool) (primitive3 >= primitive3 - primitive3);
      tetrisModule.i = primitive1;
      while ((flag ? ((bool) (tetrisModule.i <= primitive2) ? 1 : 0) : ((bool) (tetrisModule.i >= primitive2) ? 1 : 0)) != 0)
      {
        tetrisModule.v = Microsoft.SmallBasic.Library.Array.GetValue(Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, (Primitive)(-1)), tetrisModule.i);
        Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "c", Microsoft.SmallBasic.Library.Math.Floor(tetrisModule.v / (Primitive) 10) + tetrisModule.xpos + (Microsoft.SmallBasic.Library.Math.Remainder(tetrisModule.v, (Primitive) 10) + tetrisModule.ypos - (Primitive) 1) * tetrisModule.CWIDTH, Microsoft.SmallBasic.Library.Array.GetValue(tetrisModule.h, tetrisModule.i));
        tetrisModule.i += primitive3;
      }
      tetrisModule.score += (Primitive) 1;
      tetrisModule.PrintScore();
      tetrisModule.DeleteLines();
    }

    public static void DeleteLines()
    {
      tetrisModule.linesCleared = (Primitive) 0;
      Primitive primitive1 = tetrisModule.CHEIGHT - (Primitive) 1;
      Primitive primitive2 = (Primitive) 0;
      Primitive primitive3 = (Primitive)(-1);
      bool flag1 = (bool) (primitive3 >= primitive3 - primitive3);
      tetrisModule.y = primitive1;
      while ((flag1 ? ((bool) (tetrisModule.y <= primitive2) ? 1 : 0) : ((bool) (tetrisModule.y >= primitive2) ? 1 : 0)) != 0)
      {
        tetrisModule.x = tetrisModule.CWIDTH;
        while ((bool) (tetrisModule.x == tetrisModule.CWIDTH))
        {
          tetrisModule.x = (Primitive) 0;
          while ((bool) (tetrisModule.x < tetrisModule.CWIDTH))
          {
            tetrisModule.piece = Microsoft.SmallBasic.Library.Array.GetValue((Primitive) "c", tetrisModule.x + tetrisModule.y * tetrisModule.CWIDTH);
            if ((bool) (tetrisModule.piece == (Primitive) "."))
              tetrisModule.x = tetrisModule.CWIDTH;
            tetrisModule.x += (Primitive) 1;
          }
          if ((bool) (tetrisModule.x == tetrisModule.CWIDTH))
          {
            Primitive primitive4 = (Primitive) 0;
            Primitive primitive5 = tetrisModule.CWIDTH - (Primitive) 1;
            Primitive primitive6 = (Primitive) 1;
            bool flag2 = (bool) (primitive6 >= primitive6 - primitive6);
            tetrisModule.x1 = primitive4;
            while ((flag2 ? ((bool) (tetrisModule.x1 <= primitive5) ? 1 : 0) : ((bool) (tetrisModule.x1 >= primitive5) ? 1 : 0)) != 0)
            {
              Shapes.Remove(Microsoft.SmallBasic.Library.Array.GetValue((Primitive) "c", tetrisModule.x1 + tetrisModule.y * tetrisModule.CWIDTH));
              tetrisModule.x1 += primitive6;
            }
            tetrisModule.linesCleared += (Primitive) 1;
            Primitive y = tetrisModule.y;
            Primitive primitive7 = (Primitive) 1;
            Primitive primitive8 = (Primitive)(-1);
            bool flag3 = (bool) (primitive8 >= primitive8 - primitive8);
            tetrisModule.y1 = y;
            while ((flag3 ? ((bool) (tetrisModule.y1 <= primitive7) ? 1 : 0) : ((bool) (tetrisModule.y1 >= primitive7) ? 1 : 0)) != 0)
            {
              Primitive primitive9 = (Primitive) 0;
              Primitive primitive10 = tetrisModule.CWIDTH - (Primitive) 1;
              Primitive primitive11 = (Primitive) 1;
              bool flag4 = (bool) (primitive11 >= primitive11 - primitive11);
              tetrisModule.x1 = primitive9;
              while ((flag4 ? ((bool) (tetrisModule.x1 <= primitive10) ? 1 : 0) : ((bool) (tetrisModule.x1 >= primitive10) ? 1 : 0)) != 0)
              {
                tetrisModule.piece = Microsoft.SmallBasic.Library.Array.GetValue((Primitive) "c", tetrisModule.x1 + (tetrisModule.y1 - (Primitive) 1) * tetrisModule.CWIDTH);
                Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "c", tetrisModule.x1 + tetrisModule.y1 * tetrisModule.CWIDTH, tetrisModule.piece);
                Shapes.Move(tetrisModule.piece, Shapes.GetLeft(tetrisModule.piece), Shapes.GetTop(tetrisModule.piece) + tetrisModule.BWIDTH);
                tetrisModule.x1 += primitive11;
              }
              tetrisModule.y1 += primitive8;
            }
          }
        }
        tetrisModule.y += primitive3;
      }
      if (!(bool) (tetrisModule.linesCleared > (Primitive) 0))
        return;
      tetrisModule.score += (Primitive) 100 * Microsoft.SmallBasic.Library.Math.Round(tetrisModule.linesCleared * (Primitive) 2.15 - (Primitive) 1);
      tetrisModule.PrintScore();
    }

    public static void SetupCanvas()
    {
      GraphicsWindow.BrushColor = GraphicsWindow.BackgroundColor;
      GraphicsWindow.FillRectangle(tetrisModule.XOFFSET, tetrisModule.YOFFSET, tetrisModule.CWIDTH * tetrisModule.BWIDTH, tetrisModule.CHEIGHT * tetrisModule.BWIDTH);
      Program.Delay((Primitive) 200);
      GraphicsWindow.PenWidth = (Primitive) 1;
      GraphicsWindow.PenColor = (Primitive) "Pink";
      Primitive primitive1 = (Primitive) 0;
      Primitive primitive2 = tetrisModule.CWIDTH - (Primitive) 1;
      Primitive primitive3 = (Primitive) 1;
      bool flag1 = (bool) (primitive3 >= primitive3 - primitive3);
      tetrisModule.x = primitive1;
      while ((flag1 ? ((bool) (tetrisModule.x <= primitive2) ? 1 : 0) : ((bool) (tetrisModule.x >= primitive2) ? 1 : 0)) != 0)
      {
        Primitive primitive4 = (Primitive) 0;
        Primitive primitive5 = tetrisModule.CHEIGHT - (Primitive) 1;
        Primitive primitive6 = (Primitive) 1;
        bool flag2 = (bool) (primitive6 >= primitive6 - primitive6);
        tetrisModule.y = primitive4;
        while ((flag2 ? ((bool) (tetrisModule.y <= primitive5) ? 1 : 0) : ((bool) (tetrisModule.y >= primitive5) ? 1 : 0)) != 0)
        {
          Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "c", tetrisModule.x + tetrisModule.y * tetrisModule.CWIDTH, (Primitive) ".");
          GraphicsWindow.DrawRectangle(tetrisModule.XOFFSET + tetrisModule.x * tetrisModule.BWIDTH, tetrisModule.YOFFSET + tetrisModule.y * tetrisModule.BWIDTH, tetrisModule.BWIDTH, tetrisModule.BWIDTH);
          tetrisModule.y += primitive6;
        }
        tetrisModule.x += primitive3;
      }
      GraphicsWindow.PenWidth = (Primitive) 4;
      GraphicsWindow.PenColor = (Primitive) "Black";
      GraphicsWindow.DrawLine(tetrisModule.XOFFSET, tetrisModule.YOFFSET, tetrisModule.XOFFSET, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH);
      GraphicsWindow.DrawLine(tetrisModule.XOFFSET + tetrisModule.CWIDTH * tetrisModule.BWIDTH, tetrisModule.YOFFSET, tetrisModule.XOFFSET + tetrisModule.CWIDTH * tetrisModule.BWIDTH, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH);
      GraphicsWindow.DrawLine(tetrisModule.XOFFSET, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH, tetrisModule.XOFFSET + tetrisModule.CWIDTH * tetrisModule.BWIDTH, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH);
      GraphicsWindow.PenColor = (Primitive) "Lime";
      GraphicsWindow.DrawLine(tetrisModule.XOFFSET - (Primitive) 4, tetrisModule.YOFFSET, tetrisModule.XOFFSET - (Primitive) 4, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH + (Primitive) 6);
      GraphicsWindow.DrawLine(tetrisModule.XOFFSET + tetrisModule.CWIDTH * tetrisModule.BWIDTH + (Primitive) 4, tetrisModule.YOFFSET, tetrisModule.XOFFSET + tetrisModule.CWIDTH * tetrisModule.BWIDTH + (Primitive) 4, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH + (Primitive) 6);
      GraphicsWindow.DrawLine(tetrisModule.XOFFSET - (Primitive) 4, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH + (Primitive) 4, tetrisModule.XOFFSET + tetrisModule.CWIDTH * tetrisModule.BWIDTH + (Primitive) 4, tetrisModule.YOFFSET + tetrisModule.CHEIGHT * tetrisModule.BWIDTH + (Primitive) 4);
      GraphicsWindow.PenColor = (Primitive) "Black";
      GraphicsWindow.BrushColor = (Primitive) "Pink";
      tetrisModule.x = tetrisModule.XOFFSET + tetrisModule.PREVIEW_xpos * tetrisModule.BWIDTH - tetrisModule.BWIDTH;
      tetrisModule.y = tetrisModule.YOFFSET + tetrisModule.PREVIEW_ypos * tetrisModule.BWIDTH - tetrisModule.BWIDTH;
      GraphicsWindow.FillRectangle(tetrisModule.x, tetrisModule.y, tetrisModule.BWIDTH * (Primitive) 5, tetrisModule.BWIDTH * (Primitive) 6);
      GraphicsWindow.DrawRectangle(tetrisModule.x, tetrisModule.y, tetrisModule.BWIDTH * (Primitive) 5, tetrisModule.BWIDTH * (Primitive) 6);
      GraphicsWindow.FillRectangle(tetrisModule.x - (Primitive) 20, tetrisModule.y + (Primitive) 190, (Primitive) 310, (Primitive) 170);
      GraphicsWindow.DrawRectangle(tetrisModule.x - (Primitive) 20, tetrisModule.y + (Primitive) 190, (Primitive) 310, (Primitive) 170);
      GraphicsWindow.BrushColor = (Primitive) "Black";
      GraphicsWindow.FontItalic = (Primitive) false;
      GraphicsWindow.FontName = (Primitive) "Comic Sans MS";
      GraphicsWindow.FontSize = (Primitive) 16;
      GraphicsWindow.DrawText(tetrisModule.x, tetrisModule.y + (Primitive) 200, (Primitive) "Game control keys:");
      GraphicsWindow.DrawText(tetrisModule.x + (Primitive) 25, tetrisModule.y + (Primitive) 220, (Primitive) "Left Arrow = Move piece left");
      GraphicsWindow.DrawText(tetrisModule.x + (Primitive) 25, tetrisModule.y + (Primitive) 240, (Primitive) "Right Arrow = Move piece right");
      GraphicsWindow.DrawText(tetrisModule.x + (Primitive) 25, tetrisModule.y + (Primitive) 260, (Primitive) "Up Arrow = Rotate piece");
      GraphicsWindow.DrawText(tetrisModule.x + (Primitive) 25, tetrisModule.y + (Primitive) 280, (Primitive) "Down Arrow = Drop piece");
      GraphicsWindow.DrawText(tetrisModule.x, tetrisModule.y + (Primitive) 320, (Primitive) "Press to stop game");
      Program.Delay((Primitive) 200);
      GraphicsWindow.BrushColor = (Primitive) "Black";
      GraphicsWindow.FontName = (Primitive) "Georgia";
      GraphicsWindow.FontItalic = (Primitive) true;
      GraphicsWindow.FontSize = (Primitive) 36;
      GraphicsWindow.DrawText(tetrisModule.x - (Primitive) 20, tetrisModule.y + (Primitive) 400, (Primitive) "Small Basic Tetris");
      Program.Delay((Primitive) 200);
      GraphicsWindow.FontSize = (Primitive) 16;
      GraphicsWindow.DrawText(tetrisModule.x - (Primitive) 20, tetrisModule.y + (Primitive) 440, (Primitive) "ver.0.1");
      Program.Delay((Primitive) 200);
      tetrisModule.score = (Primitive) 0;
      tetrisModule.PrintScore();
    }

    public static void PrintScore()
    {
      GraphicsWindow.PenWidth = (Primitive) 4;
      GraphicsWindow.BrushColor = (Primitive) "Pink";
      GraphicsWindow.FillRectangle((Primitive) 500, (Primitive) 65, (Primitive) 153, (Primitive) 50);
      GraphicsWindow.BrushColor = (Primitive) "Black";
      GraphicsWindow.DrawRectangle((Primitive) 500, (Primitive) 65, (Primitive) 153, (Primitive) 50);
      GraphicsWindow.FontItalic = (Primitive) false;
      GraphicsWindow.FontSize = (Primitive) 32;
      GraphicsWindow.FontName = (Primitive) "Impact";
      GraphicsWindow.BrushColor = (Primitive) "Black";
      GraphicsWindow.DrawText((Primitive) 505, (Primitive) 70, Text.Append(Text.GetSubText((Primitive) "00000000", (Primitive) 0, (Primitive) 8 - Text.GetLength(tetrisModule.score)), tetrisModule.score));
    }

    public static void SetupTemplates()
    {
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template1", (Primitive) 0, (Primitive) 10);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template1", (Primitive) 1, (Primitive) 11);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template1", (Primitive) 2, (Primitive) 12);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template1", (Primitive) 3, (Primitive) 22);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template1", (Primitive) "color", (Primitive) "Yellow");
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template1", (Primitive) "dim", (Primitive) 3);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive)"template1", (Primitive)"pviewx", (Primitive)(-12));
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template1", (Primitive) "pviewy", (Primitive) 12);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) 0, (Primitive) 10);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) 1, (Primitive) 11);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) 2, (Primitive) 12);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) 3, (Primitive) 2);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) "color", (Primitive) "Magenta");
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) "dim", (Primitive) 3);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) "pviewx", (Primitive) 12);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template2", (Primitive) "pviewy", (Primitive) 12);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) 0, (Primitive) 10);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) 1, (Primitive) 1);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) 2, (Primitive) 11);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) 3, (Primitive) 21);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) "color", (Primitive) "Gray");
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) "dim", (Primitive) 3);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) "pviewx", (Primitive) 0);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template3", (Primitive) "pviewy", (Primitive) 25);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) 0, (Primitive) 0);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) 1, (Primitive) 10);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) 2, (Primitive) 1);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) 3, (Primitive) 11);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) "color", (Primitive) "Cyan");
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) "dim", (Primitive) 2);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) "pviewx", (Primitive) 12);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template4", (Primitive) "pviewy", (Primitive) 25);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) 0, (Primitive) 0);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) 1, (Primitive) 10);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) 2, (Primitive) 11);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) 3, (Primitive) 21);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) "color", (Primitive) "Green");
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) "dim", (Primitive) 3);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) "pviewx", (Primitive) 0);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template5", (Primitive) "pviewy", (Primitive) 25);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) 0, (Primitive) 10);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) 1, (Primitive) 20);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) 2, (Primitive) 1);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) 3, (Primitive) 11);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) "color", (Primitive) "Blue");
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) "dim", (Primitive) 3);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) "pviewx", (Primitive) 0);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template6", (Primitive) "pviewy", (Primitive) 25);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) 0, (Primitive) 10);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) 1, (Primitive) 11);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) 2, (Primitive) 12);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) 3, (Primitive) 13);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) "color", (Primitive) "Red");
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) "dim", (Primitive) 4);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) "pviewx", (Primitive) 0);
      Microsoft.SmallBasic.Library.Array.SetValue((Primitive) "template7", (Primitive) "pviewy", (Primitive) 0);
    }
  }
}
