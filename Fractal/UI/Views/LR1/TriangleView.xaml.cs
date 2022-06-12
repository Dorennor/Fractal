using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Point = Fractal.Models.Point;

namespace Fractal.UI.Views.LR1;

public partial class TriangleView : UserControl
{
    private Point A;
    private Point B;
    private Point C;
    private Point P;
    private Random random;
    private int currentCycleState = default;
    private int maxIterations = default;
    private CancellationTokenSource source;
    private CancellationToken token;

    public TriangleView()
    {
        InitializeComponent();

        IterationsNumber.Text = "10000";
        currentCycleState = 0;

        random = new Random();
        source = new CancellationTokenSource();

        token = source.Token;

        PauseButton.IsEnabled = false;
        ContinueButton.IsEnabled = false;
        StartButton.IsEnabled = false;
    }

    private bool IsPointInTriangle(Point A, Point B, Point C, Point P)
    {
        var areaABC = Math.Abs((((A.X * (B.Y - C.Y)) + (B.X * (C.Y - A.Y)) + (C.X * (A.Y - B.Y)))) / 2);
        var areaPAB = Math.Abs((((P.X * (A.Y - B.Y)) + (A.X * (B.Y - P.Y)) + (B.X * (P.Y - A.Y)))) / 2);
        var areaPBC = Math.Abs((((P.X * (B.Y - C.Y)) + (B.X * (C.Y - P.Y)) + (C.X * (P.Y - B.Y)))) / 2);
        var areaPAC = Math.Abs((((P.X * (A.Y - C.Y)) + (A.X * (C.Y - P.Y)) + (C.X * (P.Y - A.Y)))) / 2);

        return (areaPAB + areaPBC + areaPAC) == areaABC;
    }

    private void DrawPointsAsync(Point A, Point B, Point C, Point P, Random random, CancellationToken token)
    {
        if (A != null && B != null && C != null && P != null)
        {
            for (int i = currentCycleState; i < maxIterations - currentCycleState; i++)
            {
                currentCycleState = i;

                if (token.IsCancellationRequested)
                {
                    return;
                }

                int rollResult = random.Next(1, 6);

                if (rollResult == 1 || rollResult == 2)
                {
                    Point newPoint = new Point((P.X + A.X) / 2, (P.Y + A.Y) / 2);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        FractalPlot.Plot.AddPoint(newPoint.X, newPoint.Y, Color.BlueViolet);
                        FractalPlot.Refresh();
                        Iteration.Content = $"Iteration №{currentCycleState}.";
                    }), DispatcherPriority.Background);

                    P = newPoint;
                }

                if (rollResult == 3 || rollResult == 4)
                {
                    Point newPoint = new Point((P.X + B.X) / 2, (P.Y + B.Y) / 2);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        FractalPlot.Plot.AddPoint(newPoint.X, newPoint.Y, Color.BlueViolet);
                        FractalPlot.Refresh();
                        Iteration.Content = $"Iteration №{currentCycleState}.";
                    }), DispatcherPriority.Background);

                    P = newPoint;
                }

                if (rollResult == 5 || rollResult == 6)
                {
                    Point newPoint = new Point((P.X + C.X) / 2, (P.Y + C.Y) / 2);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        FractalPlot.Plot.AddPoint(newPoint.X, newPoint.Y, Color.BlueViolet);
                        FractalPlot.Refresh();
                        Iteration.Content = $"Iteration №{currentCycleState}.";
                    }), DispatcherPriority.Background);

                    P = newPoint;
                }

                Thread.Sleep(50);
            }
        }
    }

    private void StartButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (IterationsNumber.Text != string.Empty)
        {
            maxIterations = Convert.ToInt32(IterationsNumber.Text);
            Task task = Task.Factory.StartNew(() => DrawPointsAsync(A, B, C, P, random, token));

            ContinueButton.IsEnabled = false;
            StartButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
        }
    }

    private void RefreshPlot_OnClick(object sender, RoutedEventArgs e)
    {
        source.Cancel();
        source.Dispose();

        source = new CancellationTokenSource();
        token = source.Token;

        ContinueButton.IsEnabled = false;
        PauseButton.IsEnabled = false;
        StartButton.IsEnabled = true;

        currentCycleState = 0;
        Iteration.Content = null;

        FractalPlot.Plot.Clear();

        A = new Point(random.Next(1, 20), random.Next(1, 20));
        B = new Point(random.Next(40, 80), random.Next(50, 100));
        C = new Point(random.Next(80, 100), random.Next(1, 20));

        var yMin = A.Y < C.Y ? A.Y - 1 : C.Y - 1;
        var yMax = B.Y - 1;

        var xMin = A.X - 1;
        var xMax = C.X - 1;

        P = new Point(random.Next(Convert.ToInt32(xMin), Convert.ToInt32(xMax)), random.Next(Convert.ToInt32(yMin), Convert.ToInt32(yMax)));

        while (!IsPointInTriangle(A, B, C, P))
        {
            P = new Point(random.Next(Convert.ToInt32(xMin), Convert.ToInt32(xMax)), random.Next(Convert.ToInt32(yMin), Convert.ToInt32(yMax)));
        }

        double[] dataX = { A.X, B.X, C.X, A.X };
        double[] dataY = { A.Y, B.Y, C.Y, A.Y };

        FractalPlot.Plot.AddScatter(dataX, dataY, Color.Red);

        FractalPlot.Plot.AddPoint(P.X, P.Y, Color.Red);

        FractalPlot.Refresh();
    }

    private void PauseButton_OnClick(object sender, RoutedEventArgs e)
    {
        source.Cancel();
        source.Dispose();

        source = new CancellationTokenSource();
        token = source.Token;

        ContinueButton.IsEnabled = true;
        PauseButton.IsEnabled = false;
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void ContinueButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (IterationsNumber.Text != string.Empty)
        {
            Task task = Task.Factory.StartNew(() => DrawPointsAsync(A, B, C, P, random, token));

            ContinueButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
        }
    }
}