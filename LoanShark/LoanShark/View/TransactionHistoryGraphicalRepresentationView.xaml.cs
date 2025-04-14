using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace LoanShark
{
    public sealed partial class TransactionHistoryGraphicalRepresentationView : Window
    {
        private static Random random = new Random();
        private Dictionary<string, Windows.UI.Color> legendColors = new Dictionary<string, Windows.UI.Color>();

        public TransactionHistoryGraphicalRepresentationView(Dictionary<string, int> transactionTypeCounts)
        {
            this.InitializeComponent();
            ResizeWindow(600, 600);

            LoadPieChart(transactionTypeCounts);
            LoadLegend();
        }

        private void ResizeWindow(int width, int height)
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd));
            if (appWindow != null)
            {
                appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
                var displayArea = DisplayArea.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd), DisplayAreaFallback.Primary);
                if (displayArea != null)
                {
                    var centeredPosition = new Windows.Graphics.PointInt32(
                        (displayArea.WorkArea.Width - width) / 2,
                        (displayArea.WorkArea.Height - height) / 2);
                    appWindow.Move(centeredPosition);
                }
            }
        }

        private void LoadPieChart(Dictionary<string, int> transactionTypeCounts)
        {
            foreach (var kvp in transactionTypeCounts)
            {
                Debug.WriteLine("kvp: " + kvp.Key + " " + kvp.Value);
            }

            if (transactionTypeCounts == null || transactionTypeCounts.Count == 0)
            {
                return;
            }
            var total = transactionTypeCounts.Values.Sum();
            double currentAngle = 0.0;
            double centerX = TransactionPieChart.Width / 2;
            double centerY = TransactionPieChart.Height / 2;
            double radius = Math.Min(centerX, centerY) - 5;

            // Special case: If there's only one type, create a full circle
            if (transactionTypeCounts.Count == 1)
            {
                var kvp = transactionTypeCounts.First();
                Windows.UI.Color sliceColor = GetRandomColor();
                legendColors[kvp.Key] = sliceColor;
                var fullCircle = CreateFullCircle(centerX, centerY, radius, sliceColor, kvp.Key);
                TransactionPieChart.Children.Add(fullCircle);
                return;
            }

            // Normal case: Multiple types
            foreach (var kvp in transactionTypeCounts)
            {
                double sliceAngle = (kvp.Value / (double)total) * 360;
                Windows.UI.Color sliceColor = GetRandomColor();
                legendColors[kvp.Key] = sliceColor;
                var slice = CreatePieSlice(currentAngle, sliceAngle, kvp.Key, centerX, centerY, radius, sliceColor);
                currentAngle += sliceAngle;
                TransactionPieChart.Children.Add(slice);
            }
        }

        private Path CreatePieSlice(double startAngle, double sliceAngle, string name, double centerX, double centerY, double radius, Windows.UI.Color color)
        {
            var startPoint = ComputeCartesianCoordinate(startAngle, radius, centerX, centerY);
            var endPoint = ComputeCartesianCoordinate(startAngle + sliceAngle, radius, centerX, centerY);

            var pathFigure = new PathFigure { StartPoint = new Windows.Foundation.Point(centerX, centerY) };

            pathFigure.Segments.Add(new LineSegment { Point = startPoint });
            pathFigure.Segments.Add(new ArcSegment
            {
                Point = endPoint,
                Size = new Windows.Foundation.Size(radius, radius),
                IsLargeArc = sliceAngle > 180,
                SweepDirection = SweepDirection.Clockwise
            });
            pathFigure.Segments.Add(new LineSegment { Point = new Windows.Foundation.Point(centerX, centerY) });

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            var path = new Path
            {
                Fill = new SolidColorBrush(color),
                Data = pathGeometry
            };

            ToolTipService.SetToolTip(path, name);

            return path;
        }

        private void LoadLegend()
        {
            LegendPanel.Children.Clear();
            foreach (var kvp in legendColors)
            {
                StackPanel legendItem = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
                Rectangle colorBox = new Rectangle
                {
                    Width = 16,
                    Height = 16,
                    Fill = new SolidColorBrush(kvp.Value),
                    Margin = new Thickness(5)
                };

                TextBlock label = new TextBlock
                {
                    Text = kvp.Key,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5, 0, 0, 0)
                };

                legendItem.Children.Add(colorBox);
                legendItem.Children.Add(label);
                LegendPanel.Children.Add(legendItem);
            }
        }

        private Path CreateFullCircle(double centerX, double centerY, double radius, Windows.UI.Color color, string name)
        {
            var ellipse = new EllipseGeometry
            {
                Center = new Windows.Foundation.Point(centerX, centerY),
                RadiusX = radius,
                RadiusY = radius
            };

            var path = new Path
            {
                Fill = new SolidColorBrush(color),
                Data = ellipse
            };

            ToolTipService.SetToolTip(path, name);

            return path;
        }

        private Windows.Foundation.Point ComputeCartesianCoordinate(double angle, double radius, double centerX, double centerY)
        {
            double angleRad = (Math.PI / 180.0) * (angle - 90);
            double x = centerX + (radius * Math.Cos(angleRad));
            double y = centerY + (radius * Math.Sin(angleRad));
            return new Windows.Foundation.Point(x, y);
        }

        private Windows.UI.Color GetRandomColor()
        {
            return Windows.UI.Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
        }
    }
}
