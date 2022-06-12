using Fractal.Extensions;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR3;

public partial class IndividualSecondView : UserControl
{
    private const int iter = 50;
    private const double min = 1e-6;
    private const double max = 1e+6;

    private Graphics graphics;
    private Pen pen;
    public Bitmap map;

    private struct Complex
    {
        public double x;
        public double y;
    };

    public IndividualSecondView()
    {
        InitializeComponent();
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        pen = new Pen(Color.BlueViolet, 1);

        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        graphics.Clear(Color.Black);

        Draw(int.Parse(Width.Text), int.Parse(Height.Text), graphics, pen);

        FractalImage.Source = map.GetImageSource();
    }

    private void Draw(int mx1, int my1, Graphics g, Pen pen)
    {
        int n, mx, my;
        double p;
        Complex z, t, d = new Complex();

        mx = mx1 / 2;
        my = my1 / 2;

        for (int y = -my; y < my; y++)
            for (int x = -mx; x < mx; x++)
            {
                n = 0;
                z.x = x * 0.005;
                z.y = y * 0.005;
                d = z;

                while ((Math.Pow(z.x, 2) + Math.Pow(z.y, 2) < max) && (Math.Pow(d.x, 2) + Math.Pow(d.y, 2) > min) && (n < iter))
                {
                    t = z;
                    p = Math.Pow(Math.Pow(t.x, 2) + Math.Pow(t.y, 2), 2);
                    z.x = 2 / 3 * t.x + (Math.Pow(t.x, 2) - Math.Pow(t.y, 2)) / (3 * p);
                    z.y = 2 / 3 * t.y * (1 - t.x / p);
                    d.x = Math.Abs(t.x - z.x);
                    d.y = Math.Abs(t.y - z.y);
                    n++;
                }
                pen.Color = Color.FromArgb(255, (n * 9) % 255, 0, (n * 9) % 255);
                g.DrawRectangle(pen, mx + x, my + y, 1, 1);
            }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}