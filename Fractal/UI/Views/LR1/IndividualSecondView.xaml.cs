using Fractal.Extensions;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR1;

public partial class IndividualSecondView : UserControl
{
    public Graphics graphics;
    public Bitmap map;
    public Pen p;

    public IndividualSecondView()
    {
        InitializeComponent();
    }

    private void DrawDragon(int x1, int y1, int x2, int y2, int n, Graphics g, Pen pen)
    {
        int xn, yn;

        if (n > 0)
        {
            xn = (x1 + x2) / 2 + (y2 - y1) / 2;
            yn = (y1 + y2) / 2 - (x2 - x1) / 2;

            DrawDragon(x2, y2, xn, yn, n - 1, g, pen);
            DrawDragon(x1, y1, xn, yn, n - 1, g, pen);
        }

        var point1 = new System.Drawing.Point(x1, y1);
        var point2 = new System.Drawing.Point(x2, y2);
        g.DrawLine(pen, point1, point2);
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        p = new Pen(Color.DarkRed);

        graphics.Clear(Color.Black);

        int x1, y1, x2, y2, k;

        x1 = 200;
        y1 = 200;
        x2 = 390;
        y2 = 400;
        k = 15;

        DrawDragon(x1, y1, x2, y2, k, graphics, p);

        FractalImage.Source = map.GetImageSource();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}