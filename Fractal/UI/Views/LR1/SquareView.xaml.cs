using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
            _graph.FillRectangle(Brushes.BlueViolet, carpet);
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
        if (Width.Text == String.Empty || Height.Text == String.Empty)
        {
            MessageBox.Show("Wrong width or height!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var width = int.Parse(Width.Text);
        var height = int.Parse(Height.Text);

        if (width <= 0 || height <= 0)
        {
            MessageBox.Show("Wrong width or height!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        FractalImage.Source = null;

        _fractal = new Bitmap(width, height);

        _graph = Graphics.FromImage(_fractal);

        RectangleF carpet = new RectangleF(0, 0, width, height);
        DrawCarpet(Level, carpet);

        FractalImage.Source = BitmapToImageSource(_fractal);
    }

    private BitmapImage BitmapToImageSource(Bitmap bitmap)
    {
        using (MemoryStream memory = new MemoryStream())
        {
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}