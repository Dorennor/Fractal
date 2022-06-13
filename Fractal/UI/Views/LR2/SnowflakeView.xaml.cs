using Fractal.Extensions;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR2;

public partial class SnowflakeView : UserControl
{
    public Pen pen1;
    public Graphics graphics;
    public Pen pen2;
    public Bitmap map;

    public SnowflakeView()
    {
        InitializeComponent();
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        pen1 = new Pen(Color.Green, 1);
        pen2 = new Pen(Color.DarkGreen, 1);

        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        graphics.Clear(Color.White);

        var point1 = new PointF(200, 200);
        var point2 = new PointF(500, 200);
        var point3 = new PointF(350, 400);

        graphics.DrawLine(pen1, point1, point2);
        graphics.DrawLine(pen1, point2, point3);
        graphics.DrawLine(pen1, point3, point1);

        Fractal(point1, point2, point3, int.Parse(IterationsNumber.Text));
        Fractal(point2, point3, point1, int.Parse(IterationsNumber.Text));
        Fractal(point3, point1, point2, int.Parse(IterationsNumber.Text));

        FractalImage.Source = map.GetImageSource();
    }

    public int Fractal(PointF p1, PointF p2, PointF p3, int iter)
    {
        if (iter > 0)
        {
            var p4 = new PointF((p2.X + 2 * p1.X) / 3, (p2.Y + 2 * p1.Y) / 3);
            var p5 = new PointF((2 * p2.X + p1.X) / 3, (p1.Y + 2 * p2.Y) / 3);

            var ps = new PointF((p2.X + p1.X) / 2, (p2.Y + p1.Y) / 2);
            var pn = new PointF((4 * ps.X - p3.X) / 3, (4 * ps.Y - p3.Y) / 3);

            graphics.DrawLine(pen1, p4, pn);
            graphics.DrawLine(pen1, p5, pn);
            graphics.DrawLine(pen2, p4, p5);

            Fractal(p4, pn, p5, iter - 1);
            Fractal(pn, p5, p4, iter - 1);
            Fractal(p1, p4, new PointF((2 * p1.X + p3.X) / 3, (2 * p1.Y + p3.Y) / 3), iter - 1);
            Fractal(p5, p2, new PointF((2 * p2.X + p3.X) / 3, (2 * p2.Y + p3.Y) / 3), iter - 1);
        }
        return iter;
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}