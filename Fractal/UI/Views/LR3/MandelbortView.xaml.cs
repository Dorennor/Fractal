using Fractal.Extensions;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR3;

public partial class MandelbortView : UserControl
{
    private Graphics graphics;
    private Pen p;
    public Bitmap map;

    private struct Complex
    {
        public double x;
        public double y;
    };

    public MandelbortView()
    {
        InitializeComponent();
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        p = new Pen(Color.DarkRed, 2);
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        DrawFractal(int.Parse(Width.Text), int.Parse(Height.Text), p, graphics);

        map.RotateFlip(RotateFlipType.Rotate90FlipNone);

        FractalImage.Source = map.GetImageSource();
    }

    public void DrawFractal(int Width, int Height, Pen pen, Graphics g)
    {
        int iterations = 50, max = 3;
        int xc, yc;
        int x, y, n;
        double p, q;
        Complex z, c;
        xc = (Width - 10) / 2;
        yc = (Height - 10) / 2;

        for (y = -yc; y < yc; y++)
        {
            for (x = -xc; x < xc; x++)
            {
                n = 0;
                c.x = x * 0.01 + 1;
                c.y = y * 0.01;

                z.x = 0.5;
                z.y = 0;

                while ((z.x * z.x + z.y * z.y < max) && (n < iterations))
                {
                    p = z.x - z.x * z.x + z.y * z.y;
                    q = z.y - 2 * z.x * z.y;
                    z.x = c.x * p - c.y * q;
                    z.y = c.x * q + c.y * p;
                    n++;
                }
                if (n < iterations)
                {
                    pen.Color = Color.FromArgb(200, (n * 20) % 255, (n * 2) % 255, 0);
                    g.DrawRectangle(pen, xc + x, yc + y, 1, 1);
                }
            }
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}