using Fractal.Extensions;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR1;

public partial class SquareView : UserControl
{
    private const int Level = 5;

    private Bitmap _fractal;

    private Graphics _graph;

    public SquareView()
    {
        InitializeComponent();
    }

    private void DrawCarpet(int level, RectangleF carpet)
    {
        if (level == 0)
        {
            _graph.FillRectangle(Brushes.DarkRed, carpet);
        }
        else
        {
            var width = carpet.Width / 3f;
            var height = carpet.Height / 3f;

            var x1 = carpet.Left;
            var x2 = x1 + width;
            var x3 = x1 + 2f * width;

            var y1 = carpet.Top;
            var y2 = y1 + height;
            var y3 = y1 + 2f * height;

            DrawCarpet(level - 1, new RectangleF(x1, y1, width, height));
            DrawCarpet(level - 1, new RectangleF(x2, y1, width, height));
            DrawCarpet(level - 1, new RectangleF(x3, y1, width, height));
            DrawCarpet(level - 1, new RectangleF(x1, y2, width, height));
            DrawCarpet(level - 1, new RectangleF(x3, y2, width, height));
            DrawCarpet(level - 1, new RectangleF(x1, y3, width, height));
            DrawCarpet(level - 1, new RectangleF(x2, y3, width, height));
            DrawCarpet(level - 1, new RectangleF(x3, y3, width, height));
        }
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        var width = int.Parse(Width.Text);
        var height = int.Parse(Height.Text);
        
        FractalImage.Source = null;

        _fractal = new Bitmap(width, height);

        _graph = Graphics.FromImage(_fractal);

        _graph.Clear(Color.Black);

        _graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        RectangleF carpet = new RectangleF(0, 0, width, height);
        DrawCarpet(Level, carpet);

        FractalImage.Source = _fractal.GetImageSource();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}