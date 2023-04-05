using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fractals
{
    /// <summary>
    /// Класс кривой Коха.
    /// </summary>
    class KochCurve : Fractal
    {
        // Запись отрезка.
        record Segment(Coords P1, Coords P2, SolidColorBrush ColorBrush);

        /// <summary>
        /// Конструктор кривой Коха.
        /// Вызывает конструктор базового класса.
        /// </summary>
        /// <param name="fractalCanvas"> Ссылка на рабочий канвас. </param>
        /// <param name="recursionDepth"> Глубина рекурсии. </param>
        /// <param name="startColor"> Начальный цвет градиента. </param>
        /// <param name="endColor"> Конечный цвет градиента. </param>
        public KochCurve(Canvas fractalCanvas, int recursionDepth, Color startColor, Color endColor) :
               base(fractalCanvas, recursionDepth, startColor, endColor) { }

        /// <summary>
        /// Метод, инициализирующий отрисовку кривой Коха.
        /// Получает список отрезков и вызывает метод Draw для их отрисовки.
        /// </summary>
        public override void InitDrawing()
        {
            Draw(GetSegmentList(85 * fractalCanvas.ActualWidth / 100));
        }

        /// <summary>
        /// Метод генерирует список отрезков кривой (вместе с цветом).
        /// </summary>
        /// <param name="mainLength"> Длина отрезка 0 итерации. </param>
        /// <returns> Список цветных отрезков. </returns>
        private List<Segment> GetSegmentList(double mainLength)
        {
            // Создаем список отрезков. Добавляем туда главный отрезок (нулевой итерации).
            var segments = new List<Segment>();
            segments.Add(new Segment(new Coords(fractalCanvas.ActualWidth / 10, 7 * fractalCanvas.ActualHeight / 10),
                         new Coords(fractalCanvas.ActualWidth / 10 + mainLength, 7 * fractalCanvas.ActualHeight / 10),
                         GetGradientColor(0)));

            // На каждой итерации (рекурсии) "поднимаем" каждый отрезок.
            for (var depth = 0; depth < recursionDepth - 1; depth++)
            {
                for (var i = 0; i < segments.Count; i++)
                {
                    var p1 = segments[i].P1;
                    var p2 = segments[i].P2;
                    // Вычисляем длину отрезка.
                    var length = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
                    // Первая точка изгиба отрезка.
                    var curveBending1 = new Coords((2 * p1.X + p2.X) / 3, (2 * p1.Y + p2.Y) / 3);
                    // Арктангенс текущего угла отклонения отрезка.
                    var atan = Math.Atan((p2.X - p1.X) / (p2.Y - p1.Y));
                    // Угол отклонения изгиба кривой.
                    var angle = Math.PI / 3 + (p2.Y - p1.Y < 0 ? atan + Math.PI : atan);
                    // Вершина изгиба (треугольника).
                    var newPoint = new Coords(curveBending1.X + Math.Sin(angle) * length / 3, curveBending1.Y + Math.Cos(angle) * length / 3);
                    // Вторая точка изгиба отрезка.
                    var curveBending2 = new Coords((p1.X + 2 * p2.X) / 3, (p1.Y + 2 * p2.Y) / 3);
                    // Запоминаем текущий цвет отрезка.
                    var temp = segments[i].ColorBrush;
                    // Заменяем один отрезок на два его остатка и изгиб.
                    segments.RemoveAt(i);
                    segments.Insert(i, new Segment(p1, curveBending1, temp));
                    segments.Insert(i + 1, new Segment(curveBending1, newPoint, GetGradientColor(depth + 1)));
                    segments.Insert(i + 2, new Segment(newPoint, curveBending2, GetGradientColor(depth + 1)));
                    segments.Insert(i + 3, new Segment(curveBending2, p2, temp));
                    i += 3;
                }
            }

            return segments;
        }

        /// <summary>
        /// Непосредственно отрисовывает кривую Коха.
        /// </summary>
        /// <param name="segments"> Список отрезков. </param>
        private void Draw(List<Segment> segments)
        {
            foreach (var el in segments)
            {
                var line = new Line();
                (line.X1, line.X2, line.Y1, line.Y2) = (el.P1.X, el.P2.X, el.P1.Y, el.P2.Y);
                line.Stroke = el.ColorBrush;
                line.StrokeThickness = 2;
                fractalCanvas.Children.Add(line);
            }
        }

        /// <summary>
        /// Устанавливает возможную глуюину рекурсии.
        /// </summary>
        /// <param name="depth"> Ссылка на Slider, устанавливающий глубину рекурсии. </param>
        public static void SetProperties(Slider depth)
            => (depth.Minimum, depth.Maximum, depth.Value) = (0, 4, 0);
    }
}
