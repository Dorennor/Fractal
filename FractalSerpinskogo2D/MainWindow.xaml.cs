using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Color = System.Drawing.Color;

namespace FractalSerpinskogo2D
{
    public partial class MainWindow : Window
    {
        private Point A;
        private Point B;
        private Point C;
        private Point P;
        private Random random;
        private bool isPaused;
        private int currentCycleState;

        public MainWindow()
        {
            InitializeComponent();
            isPaused = false;
            currentCycleState = 0;
            random = new Random();
        }

        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        private bool IsPointInTriangle(Point A, Point B, Point C, Point P)
        {
            var areaABC = Math.Abs((((A.X * (B.Y - C.Y)) + (B.X * (C.Y - A.Y)) + (C.X * (A.Y - B.Y)))) / 2);
            var areaPAB = Math.Abs((((P.X * (A.Y - B.Y)) + (A.X * (B.Y - P.Y)) + (B.X * (P.Y - A.Y)))) / 2);
            var areaPBC = Math.Abs((((P.X * (B.Y - C.Y)) + (B.X * (C.Y - P.Y)) + (C.X * (P.Y - B.Y)))) / 2);
            var areaPAC = Math.Abs((((P.X * (A.Y - C.Y)) + (A.X * (C.Y - P.Y)) + (C.X * (P.Y - A.Y)))) / 2);

            return (areaPAB + areaPBC + areaPAC) == areaABC;
        }

        private void DrawPointsAsync(Point A, Point B, Point C, Point P, Random random)
        {
            for (int i = 0; i < 10000; i++)
            {
                int rollResult = random.Next(1, 6);

                if (rollResult == 1 || rollResult == 2)
                {
                    Point newPoint = new Point((P.X + A.X) / 2, (P.Y + A.Y) / 2);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        FractalPlot.Plot.AddPoint(newPoint.X, newPoint.Y, Color.BlueViolet);
                        FractalPlot.Refresh();
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
                    }), DispatcherPriority.Background);
                    P = newPoint;
                }
            }
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => DrawPointsAsync(A, B, C, P, random));
        }

        private void RefreshPlot_OnClick(object sender, RoutedEventArgs e)
        {
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

            FractalPlot.Plot.AddPoint(P.X, P.Y, Color.Black);

            FractalPlot.Refresh();
        }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            source.Cancel();
        }
    }
}