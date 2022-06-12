using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fractal.Extensions;

namespace Fractal.UI.Views.LR2;

public partial class IndividualFirstView : UserControl
{
    private Pen p;
    private SolidBrush background;
    private Graphics graphics;
    public Bitmap map;

    private int i = 15;

    public IndividualFirstView()
    {
        InitializeComponent();
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        p = new Pen(Color.BlueViolet, 2);
        background = new SolidBrush(Color.Black);
        map = new Bitmap(int.Parse(Width.Text), int.Parse(Height.Text));
        graphics = Graphics.FromImage(map);
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        graphics.FillRectangle(background, 0, 0, int.Parse(Width.Text), int.Parse(Height.Text));

        Draw_Levy(graphics, p, background, 250, 400, 160, 160, i);
        Draw_Levy(graphics, p, background, 400, 400, 160, 310, i);
        Draw_Levy(graphics, p, background, 400, 250, 310, 310, i);
        Draw_Levy(graphics, p, background, 250, 250, 310, 160, i);

        FractalImage.Source = map.GetImageSource();
    }

    private void Draw_Levy(Graphics gr, Pen p, SolidBrush fon, int x1, int x2, int y1, int y2, int i)
    {
        if (i == 0)
        {
            gr.DrawLine(p, x1, y1, x2, y2);
        }
        else
        {
            int x3 = (x1 + x2) / 2 + (y2 - y1) / 2;
            int y3 = (y1 + y2) / 2 - (x2 - x1) / 2;

            Draw_Levy(gr, p, fon, x1, x3, y1, y3, i - 1);
            Draw_Levy(gr, p, fon, x3, x2, y3, y2, i - 1);
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}