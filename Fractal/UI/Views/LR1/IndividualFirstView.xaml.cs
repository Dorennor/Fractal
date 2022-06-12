using Fractal.Extensions;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Fractal.UI.Views.LR1;

public partial class IndividualFirstView : UserControl
{
    private const float MinX = -6;
    private const float MaxX = 6;
    private const float MinY = 0.1f;
    private const float MaxY = 10;

    private const int PointNumber = 200000;

    private float[] _probability = new float[4]
    {
            0.01f,
            0.06f,
            0.08f,
            0.85f
    };

    private float[,] _funcCoef = new float[4, 6]
    {
            {0, 0, 0, 0.16f, 0, 0},
            {-0.15f, 0.28f, 0.26f, 0.24f, 0, 0.44f},
            {0.2f, -0.26f, 0.23f, 0.22f, 0, 1.6f},
            {0.85f, 0.04f, -0.04f, 0.85f, 0, 1.6f}
    };

    private Bitmap _fern;

    private Graphics _graph;

    public IndividualFirstView()
    {
        InitializeComponent();
    }

    private void DrawFern(int width, int height, double imageBoxWidth)
    {
        Random random = new Random();

        float xtemp = 0, ytemp = 0;

        int func_num = 0;

        for (int i = 1; i <= PointNumber; i++)
        {
            var num = random.NextDouble();

            for (int j = 0; j <= 3; j++)
            {
                num = num - _probability[j];
                if (num <= 0)
                {
                    func_num = j;
                    break;
                }
            }

            var x = _funcCoef[func_num, 0] * xtemp + _funcCoef[func_num, 1] * ytemp + _funcCoef[func_num, 4];
            var y = _funcCoef[func_num, 2] * xtemp + _funcCoef[func_num, 3] * ytemp + _funcCoef[func_num, 5];

            xtemp = x;
            ytemp = y;

            x = (int)(xtemp * width + imageBoxWidth / 2);
            y = (int)(ytemp * height);

            try
            {
                _fern.SetPixel((int)x, (int)y, Color.Orange);
            }
            catch
            {
                break;
            }
        }

        FractalImage.Source = _fern.GetImageSource();
    }

    private void DrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (Size.Text == String.Empty)
        {
            MessageBox.Show("Wrong size!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var imageBoxSize = int.Parse(Size.Text);

        var width = (int)(double.Parse(Size.Text) / (MaxX - MinX));
        var height = (int)(double.Parse(Size.Text) / (MaxY - MinY));

        if (imageBoxSize > 980 || imageBoxSize <= 11)
        {
            MessageBox.Show("Wrong size! Size must be a number from 12 to 980", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (width == 0 || height == 0)
        {
            MessageBox.Show("Wrong size!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _fern = new Bitmap(imageBoxSize, imageBoxSize);

        _graph = Graphics.FromImage(_fern);

        _graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        _graph.Clear(Color.White);

        DrawFern(width, height, Convert.ToDouble(imageBoxSize));
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void StagedDrawButton_OnClick(object sender, RoutedEventArgs e)
    {
        Task task = Task.Factory.StartNew(() => StagedDraw(MaxX, MinX, MaxY, MinY));
    }

    private void StagedDraw(float MaxX, float MinX, float MaxY, float MinY)
    {
        for (double i = 12; i < 981; i += 0.5)
        {
            var imageBoxSize = Convert.ToInt32(i);

            var width = (int)((double)i / (MaxX - MinX));
            var height = (int)((double)i / (MaxY - MinY));

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _fern = new Bitmap(imageBoxSize, imageBoxSize);

                _graph = Graphics.FromImage(_fern);

                _graph.Clear(Color.Black);

                DrawFern(width, height, Convert.ToDouble(imageBoxSize));

                DrawButton.IsEnabled = false;
                StagedDrawButton.IsEnabled = false;
                Size.IsEnabled = false;
            }), DispatcherPriority.Background);

            Thread.Sleep(20);
        }

        Dispatcher.BeginInvoke(new Action(() =>
        {
            DrawButton.IsEnabled = true;
            StagedDrawButton.IsEnabled = true;
            Size.IsEnabled = true;
        }), DispatcherPriority.Background);
    }
}