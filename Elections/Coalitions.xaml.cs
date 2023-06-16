using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Elections
{
    /// <summary>
    /// Interaction logic for Coalitions.xaml
    /// </summary>
    public partial class Coalitions : Window
    {
        private Dictionary<string, int> representation = new();
        private Dictionary<string, int> coals = new();
        private Dictionary<string, Brush> colors = new();
        private bool type = false;
        public Coalitions(Dictionary<string, int> results)
        {
            InitializeComponent();
            MakeRepresentation(results);
            Representation();
        }
        private void MakeRepresentation(Dictionary<string, int> results)
        {
            int votes = results.Values.Sum();
            foreach (var item in results)
            {
                if (item.Key.Contains("Koalícia"))
                {
                    if (item.Value > (0.07 * votes))
                    {
                        representation.Add(item.Key, item.Value);
                    }
                }
                else
                {
                    if (item.Value > (0.05 * votes))
                    {
                        representation.Add(item.Key, item.Value);
                    }
                }
            }

            votes = representation.Values.Sum();
            representation = representation.ToDictionary(pair => pair.Key, pair => (int)(150 * pair.Value / votes));
            var rep = representation.ToDictionary(pair => pair.Key, pair => pair.Value);
            representation.Clear();
            int num = rep.Values.Sum();
            foreach (var item in rep.OrderByDescending(pair => pair.Value))
            {
                if (num != 150)
                {
                    representation.Add(item.Key, item.Value + 1);
                    num++;
                }
                else
                {
                    representation.Add(item.Key, item.Value);
                }
            }
        }
        private void Next(object sender, EventArgs e)
        {
            canvas.Children.Clear();
            if (!type)
            {
                Representation();
            }
            else
            {
                Coalition();
            }
        }
        private void Coalition()
        {
            type = false;
            double pos_x = 60;
            double pos_y = 200;
            foreach (var item in representation)
            {
                coals.Add(item.Key, 1);
                Button button = new()
                {
                    Content = item.Key,
                    Width = 70,
                    Background = colors[item.Key]
                };
                button.Click += Add;
                Canvas.SetLeft(button, pos_x);
                Canvas.SetTop(button, pos_y);
                pos_x += 80;
                canvas.Children.Add(button);
            }
            Update();
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string key = (string)clickedButton.Content;
            if (clickedButton.Background == null)
            {
                clickedButton.Background = colors[key];
                coals[key] = 1;
            }
            else
            {
                clickedButton.Background = null;
                coals[key] = 0;
            }
            Update();
        }
        private void Update()
        {
            List<UIElement> toRemove = new();

            foreach (var child in canvas.Children)
            {
                if (child is System.Windows.Shapes.Rectangle || child is Line || child is TextBlock)
                {
                    toRemove.Add((UIElement)child);
                }
            }
            foreach (var obj in toRemove)
            {
                canvas.Children.Remove(obj);
            }
            double pos_x = 60;
            double pos_y = 100;

            Rectangle rectangle = new()
            {
                Width = 606,
                Height = 56,
                StrokeThickness = 3,
                Stroke = Brushes.Black,
                Fill = Brushes.White
            };
            Canvas.SetLeft(rectangle, pos_x - 3);
            Canvas.SetTop(rectangle, pos_y - 3);
            TextBlock txt = new()
            {
                Text = "75"
            };
            Canvas.SetLeft(txt, 355);
            Canvas.SetTop(txt, 65);
            canvas.Children.Add(txt);
            canvas.Children.Add(rectangle);
            int suma = 0;
            foreach (var item in coals)
            {
                if (item.Value == 1)
                {
                    rectangle = new System.Windows.Shapes.Rectangle
                    {
                        Width = representation[item.Key] * 4,
                        Height = 50,
                        Fill = colors[item.Key]
                    };
                    Canvas.SetLeft(rectangle, pos_x);
                    Canvas.SetTop(rectangle, pos_y);
                    pos_x += rectangle.Width;
                    canvas.Children.Add(rectangle);
                    suma += representation[item.Key];
                }
            }

            TextBlock textBlock = new();
            Canvas.SetLeft(textBlock, pos_x - 10);
            Canvas.SetTop(textBlock, pos_y + 60);
            canvas.Children.Add(textBlock);
            textBlock.Text = "" + suma;

            Line line = new()
            {
                X1 = 359,
                Y1 = 80,
                X2 = 361,
                Y2 = 170,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection() { 2, 2 }
            };
            canvas.Children.Add(line);
        }
        private void Representation()
        {
            type = true;
            colors.Clear();
            coals.Clear();
            foreach (var item in representation)
            {
                Random random = new();
                byte r = (byte)random.Next(256);
                byte g = (byte)random.Next(256);
                byte b = (byte)random.Next(256);
                System.Windows.Media.Color randomColor = System.Windows.Media.Color.FromRgb(r, g, b);
                Brush brushColor = new SolidColorBrush(randomColor);
                colors.Add(item.Key, brushColor);
            }
            int pos_x = 100;
            int pos_y = 50;
            int number = 0;
            int countRectangles = 0;
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 16; j++)
                {
                    Rectangle rectangle = new()
                    {
                        Width = 20,
                        Height = 20
                    };
                    if (countRectangles >= representation.ElementAt(number).Value && number + 1 < representation.Count)
                    {
                        number++;
                        countRectangles = 1;
                    }
                    else
                    {
                        countRectangles++;
                    }
                    rectangle.Fill = colors.ElementAt(number).Value;

                    Canvas.SetLeft(rectangle, j * 21 + pos_x);
                    if (j == 5 || j == 10)
                    {
                        pos_x += 10;
                    }
                    if (j < 6)
                    {
                        pos_y -= 10;
                    }
                    else if (j > 11)
                    {
                        pos_y += 10;
                    }
                    Canvas.SetTop(rectangle, i * 21 + pos_y);
                    canvas.Children.Add(rectangle);
                }
                pos_y = 50 + i * 10;
                pos_x = 100;
            }
            pos_x = 500;
            pos_y = 100;
            int it = 0;
            foreach (var item in colors)
            {
                Rectangle rectangle = new()
                {
                    Width = 10,
                    Height = 10,
                    Fill = item.Value
                };
                Canvas.SetLeft(rectangle, pos_x);
                Canvas.SetTop(rectangle, pos_y + it * 20);
                TextBlock textBlock = new()
                {
                    Text = item.Key + ": " + representation.GetValueOrDefault(item.Key)
                };
                Canvas.SetLeft(textBlock, pos_x + 20);
                Canvas.SetTop(textBlock, pos_y + it * 20);
                it++;
                canvas.Children.Add(rectangle);
                canvas.Children.Add(textBlock);
            }
        }
    }
}
