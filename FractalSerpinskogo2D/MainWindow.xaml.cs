using System;
using System.Windows;
using Color = System.Drawing.Color;

namespace FractalSerpinskogo2D
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Point(double x, double y)
            {
                this.X = x;
                this.Y = y;
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

        private void RefreshPlot_OnClick(object sender, RoutedEventArgs e)
        {
            FractalPlot.Plot.Clear();
            Random random = new Random();
            Point A = new Point(random.Next(1, 20), random.Next(1, 20));
            Point B = new Point(random.Next(40, 80), random.Next(50, 100));
            Point C = new Point(random.Next(80, 100), random.Next(1, 20));

            var yMin = A.Y < C.Y ? A.Y - 1 : C.Y - 1;
            var yMax = B.Y - 1;

            var xMin = A.X - 1;
            var xMax = C.X - 1;

            Point P = new Point(random.Next(Convert.ToInt32(xMin), Convert.ToInt32(xMax)), random.Next(Convert.ToInt32(yMin), Convert.ToInt32(yMax)));

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
    }
}