using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fractals
{
    /// <summary>
    /// Класс треугольника Серпинского.
    /// </summary>
    class SerpinskyTriangle : Fractal
    {
        /// <summary>
        /// Конструктор треугольника Серпинского.
        /// Вызывает конструктор базового класса.
        /// </summary>
        /// <param name="fractalCanvas"> Ссылка на рабочий канвас. </param>
        /// <param name="recursionDepth"> Глубина рекурсии. </param>
        /// <param name="startColor"> Начальный цвет градиента. </param>
        /// <param name="endColor"> Конечный цвет градиента. </param>
        public SerpinskyTriangle(Canvas fractalCanvas, int recursionDepth, Color startColor, Color endColor) :
               base(fractalCanvas, recursionDepth, startColor, endColor) { }

        /// <summary>
        /// Метод инициализирующий отрисовку треугольника Серпинского.
        /// Отрисовывает внешний треугольник, задаёт начальные значения и вызывает рекурсивный метод Draw.
        /// </summary>
        public override void InitDrawing()
        {
            // Длина стороны внешнего равностороннего треугольника.
            var sideLength = fractalCanvas.ActualWidth > fractalCanvas.ActualHeight ? 
                             8 * fractalCanvas.ActualHeight / 10 :
                             7 * fractalCanvas.ActualWidth / 10;

            // Вычисляем координаты точек равностороннего тругольника.
            var point1 = new Coords((fractalCanvas.ActualWidth - sideLength) / 2, fractalCanvas.ActualHeight * 8 / 10);
            var point2 = new Coords(point1.X + sideLength, point1.Y);
            var point3 = new Coords((point1.X + point2.X) / 2, point1.Y - sideLength * Math.Sqrt(3) / 2);

            // Создаём треугольник как многоугольник, добавляем вычисленные точки.
            var outerTriangle = new Polygon();
            outerTriangle.Points.Add(new Point(point1.X, point1.Y));
            outerTriangle.Points.Add(new Point(point2.X, point2.Y));
            outerTriangle.Points.Add(new Point(point3.X, point3.Y));

            // Устанавливаем цвет и глубину кисти, добавляем треугольник на рабочий канвас.
            outerTriangle.Stroke = GetGradientColor(0);
            outerTriangle.StrokeThickness = 2;
            fractalCanvas.Children.Add(outerTriangle);

            // Вызываем рекурсивный метод для отрисовки внутреннего треугольника.
            Draw(point1, point2, point3, 1);
        }

        /// <summary>
        /// Рекурсивный метод, отрисовывающий треугольник Серпинского.
        /// </summary>
        /// <param name="point1"> Первая точка внешнего треугольника. </param>
        /// <param name="point2"> Вторая точка внешнего треугольника. </param>
        /// <param name="point3"> Третья точка внешнего треугольника. </param>
        /// <param name="iteration"> Текущая итерация. </param>
        private void Draw(Coords point1, Coords point2, Coords point3, int iteration)
        {
            // Если текущая итерация достигает заданной глубины, выходим из рекурсии.
            if (iteration >= recursionDepth)
                return;

            // Вычисляем точки середин сторон.
            var side1Middle = new Coords((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
            var side2Middle = new Coords((point1.X + point3.X) / 2, (point1.Y + point3.Y) / 2);
            var side3Middle = new Coords((point2.X + point3.X) / 2, (point2.Y + point3.Y) / 2);

            // Создаём внутренний треугольник как многоугольник, добавляем вычисленные точки.
            var innerTriangle = new Polygon();
            innerTriangle.Points.Add(new Point(side1Middle.X, side1Middle.Y));
            innerTriangle.Points.Add(new Point(side2Middle.X, side2Middle.Y));
            innerTriangle.Points.Add(new Point(side3Middle.X, side3Middle.Y));

            // Устанавливаем цвет и глубину кисти, добавляем треугольник на рабочий канвас.
            innerTriangle.Stroke = GetGradientColor(iteration);
            innerTriangle.StrokeThickness = 2;
            fractalCanvas.Children.Add(innerTriangle);

            // Вызываем этот же метод для каждого из трёх получившихся треугольников.
            Draw(point1, side1Middle, side2Middle, iteration + 1);
            Draw(point2, side1Middle, side3Middle, iteration + 1);
            Draw(point3, side2Middle, side3Middle, iteration + 1);
        }
        
        /// <summary>
        /// Устанавливает необходимые ограничения на Slider глубины рекурсии.
        /// </summary>
        /// <param name="depth"> Ссылка на Slider, устанавливающий глубину рекурсии. </param>
        public static void SetProperties(Slider depth)
            => (depth.Minimum, depth.Maximum, depth.Value) = (0, 5, 0);
    }
}
