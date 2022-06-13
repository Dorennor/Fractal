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
    private Pen pen1 = new Pen(Color.Orange, 3);
    private Pen pen2 = new Pen(Color.Green, 3);
    public Bitmap map;

    public IndividualFirstView()
    {
        InitializeComponent();
    }

    private void DrawFractal(int degree, int x, int y, Pen myPen, Graphics g)
    {
        if (degree == 1)
        {
            graphics.DrawRectangle(myPen, x, y, 1, 1);
        }
        else
        {
            int dist = (int)Math.Pow(3, degree - 1);

            DrawFractal(degree - 1, x, y, myPen, g);
            DrawFractal(degree - 1, x - dist, y - dist, myPen, g);
            DrawFractal(degree - 1, x - dist, y + dist, myPen, g);
            DrawFractal(degree - 1, x + dist, y - dist, myPen, g);
            DrawFractal(degree - 1, x + dist, y + dist, myPen, g);
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
        graphics.Clear(Color.White);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        DrawFractal(5, 150, 200, pen1, graphics);

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

        DrawFractal(5, 150, 200, pen1, graphics);
        DrawSimilarFractal(5, 150, 200, pen2, graphics);

        FractalImage.Source = map.GetImageSource();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}