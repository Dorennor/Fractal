using Fractal.Extensions;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fractal.UI.Views.LR1;

public partial class TriangleView : UserControl
{
    private const int Level = 5;

    private Bitmap _fractal;

    private Graphics _graph;

    public TriangleView()
    {
        InitializeComponent();
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        var width = int.Parse(Width.Text);
        var height = int.Parse(Height.Text);

        _fractal = new Bitmap(width, height);

        _graph = Graphics.FromImage(_fractal);

        _graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        _graph.Clear(Color.Black);

        PointF topPoint = new PointF(width / 2f, 0);
        PointF leftPoint = new PointF(0, height);
        PointF rightPoint = new PointF(width, height);

        DrawTriangle(Level, topPoint, leftPoint, rightPoint);

        FractalImage.Source = _fractal.GetImageSource();
    }

    private PointF MidPoint(PointF p1, PointF p2)
    {
        return new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
    }

    private void DrawTriangle(int level, PointF top, PointF left, PointF right)
    {
        if (level == 0)
        {
            PointF[] points = new PointF[3]
            {
                top, right, left
            };

            _graph.FillPolygon(Brushes.DarkRed, points);
        }
        else
        {
            var leftMid = MidPoint(top, left);
            var rightMid = MidPoint(top, right);
            var topMid = MidPoint(left, right);

            DrawTriangle(level - 1, top, leftMid, rightMid);
            DrawTriangle(level - 1, leftMid, left, topMid);
            DrawTriangle(level - 1, rightMid, topMid, right);
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}