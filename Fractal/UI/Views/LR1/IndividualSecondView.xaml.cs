using Fractal.Extensions;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR1;

public partial class IndividualSecondView : UserControl
{
    public Graphics g;
    public Bitmap map;
    public Pen p;
    public double angle = Math.PI / 2;
    public double ang1 = Math.PI / 4;
    public double ang2 = Math.PI / 6;

    public IndividualSecondView()
    {
        InitializeComponent();
    }

    public int DrawTree(double x, double y, double a, double angle)
    {
        if (a > 2)
        {
            a *= 0.7;

            double xnew = Math.Round(x + a * Math.Cos(angle)), ynew = Math.Round(y - a * Math.Sin(angle));

            g.DrawLine(p, (float)x, (float)y, (float)xnew, (float)ynew);

            x = xnew;
            y = ynew;

            DrawTree(x, y, a, angle + ang1);
            DrawTree(x, y, a, angle - ang2);
        }
        return 0;
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        g = Graphics.FromImage(map);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        p = new Pen(Color.BlueViolet);

        DrawTree(300, 450, int.Parse(IterationsNumber.Text), angle);

        FractalImage.Source = map.GetImageSource();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}