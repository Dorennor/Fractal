using System;
using Fractal.Extensions;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR2;

public partial class IndividualFirstView : UserControl
{
    private Pen p;
    private Graphics g;
    public Bitmap map;

    private int i = 15;

    public IndividualFirstView()
    {
        InitializeComponent();
    }

    private void FirstLine(int x, int y, double a, double b, Graphics g)
    {
        g.DrawLine(p, x, y, (int)Math.Round(x + a * Math.Cos(b)), (int)Math.Round(y - a * Math.Sin(b)));
    }

    private void Draw(int x, int y, double a, double b, Graphics g)
    {
        if (a > 1)
        {
            FirstLine(x, y, a, b, g);
            x = (int)Math.Round(x + a * Math.Cos(b));
            y = (int)Math.Round(y - a * Math.Sin(b));
            Draw(x, y, a * 0.4, b - 14 * Math.PI / 30, g);
            Draw(x, y, a * 0.4, b + 14 * Math.PI / 30, g);
            Draw(x, y, a * 0.7, b + Math.PI / 30, g);
        }
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        p = new Pen(Color.Green, 2);
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        g = Graphics.FromImage(map);
        g.Clear(Color.White);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.FillRectangle(Brushes.White, 0, 0, 600, 600);

        Draw(240, 350, 100, Math.PI / 2, g);

        FractalImage.Source = map.GetImageSource();
    }

    private void DrawLevy(Graphics gr, Pen p, SolidBrush fon, int x1, int x2, int y1, int y2, int i)
    {
        if (i == 0)
        {
            gr.DrawLine(p, x1, y1, x2, y2);
        }
        else
        {
            int x3 = (x1 + x2) / 2 + (y2 - y1) / 2;
            int y3 = (y1 + y2) / 2 - (x2 - x1) / 2;

            DrawLevy(gr, p, fon, x1, x3, y1, y3, i - 1);
            DrawLevy(gr, p, fon, x3, x2, y3, y2, i - 1);
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}