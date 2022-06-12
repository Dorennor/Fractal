using Fractal.Extensions;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR2;

public partial class IndividualSecondView : UserControl
{
    private Graphics graphics;
    private Pen p;
    public Bitmap map;

    public IndividualSecondView()
    {
        InitializeComponent();
    }

    private void Draw(double x, double y, double l, double u, int t, int q)
    {
        if (t > 0)
        {
            if (q == 1)
            {
                x += l * Math.Cos(u);
                y -= l * Math.Sin(u);
                u += Math.PI;
            }
            u -= 2 * Math.PI / 19;
            l /= Math.Sqrt(7);

            Paint(ref x, ref y, l, u, t - 1, 0);
            Paint(ref x, ref y, l, u + Math.PI / 3, t - 1, 1);
            Paint(ref x, ref y, l, u + Math.PI, t - 1, 1);
            Paint(ref x, ref y, l, u + 2 * Math.PI / 3, t - 1, 0);
            Paint(ref x, ref y, l, u, t - 1, 0);
            Paint(ref x, ref y, l, u, t - 1, 0);
            Paint(ref x, ref y, l, u - Math.PI / 3, t - 1, 1);
        }
        else graphics.DrawLine(p, (float)Math.Round(x), (float)Math.Round(y), (float)Math.Round(x + Math.Cos(u) * l), (float)Math.Round(y - Math.Sin(u) * l));
    }

    private void Paint(ref double x, ref double y, double l, double u, int t, int q)
    {
        Draw(x, y, l, u, t, q);
        x += l * Math.Cos(u);
        y -= l * Math.Sin(u);
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        p = new Pen(Color.BlueViolet, 2);
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        Draw(150, 350, 300, 0, 3, 0);

        FractalImage.Source = map.GetImageSource();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}