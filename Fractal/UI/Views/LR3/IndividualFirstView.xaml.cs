using Fractal.Extensions;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR3;

public partial class IndividualFirstView : UserControl
{
    private Graphics graphics;
    private Pen pen1 = new Pen(Color.BlueViolet, 3);
    private Pen pen2 = new Pen(Color.Yellow, 3);
    public Bitmap map;

    public IndividualFirstView()
    {
        InitializeComponent();
    }

    public void DrawFractal(int w, int h, Graphics g, Pen pen)
    {
        double cRe, cIm;
        double newRe, newIm, oldRe, oldIm;
        double zoom = 1, moveX = 0, moveY = 0;
        int maxIterations = 300;

        cRe = -0.70176;
        cIm = -0.3842;

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                newRe = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                newIm = (y - h / 2) / (0.5 * zoom * h) + moveY;

                int i;

                for (i = 0; i < maxIterations; i++)
                {
                    oldRe = newRe;
                    oldIm = newIm;

                    newRe = oldRe * oldRe - oldIm * oldIm + cRe;
                    newIm = 2 * oldRe * oldIm + cIm;

                    if ((newRe * newRe + newIm * newIm) > 4) break;
                }

                pen.Color = Color.FromArgb(255, (i * 20) % 255, 0, (i * 20) % 255);
                g.DrawRectangle(pen, x, y, 1, 1);
            }
    }

    private void DrawSimilarFractal(int degree, int x, int y, Pen myPen, Graphics g)
    {
        if (degree == 1)
        {
            g.DrawRectangle(myPen, x, y, 1, 1);
        }
        else
        {
            int dist = (int)Math.Pow(3, degree - 1);

            DrawSimilarFractal(degree - 1, x, y, myPen, g);
            DrawSimilarFractal(degree - 1, x + dist, y, myPen, g);
            DrawSimilarFractal(degree - 1, x - dist, y, myPen, g);
            DrawSimilarFractal(degree - 1, x, y - dist, myPen, g);
            DrawSimilarFractal(degree - 1, x, y + dist, myPen, g);
        }
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        DrawFractal(840, 620, graphics, pen1);

        FractalImage.Source = map.GetImageSource();
    }

    private void DrawSimilarButton_OnClick(object sender, RoutedEventArgs e)
    {
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        DrawSimilarFractal(5, 150, 200, pen2, graphics);

        FractalImage.Source = map.GetImageSource();
    }

    private void DrawCombinedButton_OnClick(object sender, RoutedEventArgs e)
    {
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        //DrawFractal(5, 150, 200, pen1, graphics);
        ////DrawSimilarFractal(5, 150, 200, pen2, graphics);

        FractalImage.Source = map.GetImageSource();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}