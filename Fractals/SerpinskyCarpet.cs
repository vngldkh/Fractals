using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fractals
{
    /// <summary>
    /// Класс ковра Серпинского.
    /// </summary>
    class SerpinskyCarpet : Fractal
    {
        // Цвет фона ковра.
        private readonly Color backgroundColor;

        /// <summary>
        /// Конструктор ковра Серпинского.
        /// Вызывает конструктор базового класса, затем устанавливает цвет фона.
        /// </summary>
        /// <param name="fractalCanvas"> Ссылка на рабочий канвас. </param>
        /// <param name="recursionDepth"> Глубина рекурсии. </param>
        /// <param name="startColor"> Начальный цвет градиента. </param>
        /// <param name="endColor"> Конечный цвет градиента. </param>
        /// <param name="backgroundColor"> Цвет фона ковра. </param>
        public SerpinskyCarpet(Canvas fractalCanvas, int recursionDepth,
                               Color startColor, Color endColor, Color backgroundColor) :
               base(fractalCanvas, recursionDepth, startColor, endColor)
            => this.backgroundColor = backgroundColor;

        /// <summary>
        /// Метод, инициализирующий процесс отрисовки ковра Серпинского.
        /// Отрисовывает фон и вызывает рекурсивный метод Draw.
        /// </summary>
        public override void InitDrawing()
        {
            // Длина стороны квадрата, представляющего собой фон.
            var length = Math.Min(fractalCanvas.ActualWidth, fractalCanvas.ActualHeight) - 20;

            // Создаём фоновый квадрат как многоугольник и задаём его точки.
            var outerSquare = new Polygon();
            outerSquare.Points.Add(new Point((fractalCanvas.ActualWidth - length) / 2, 10));
            outerSquare.Points.Add(new Point((fractalCanvas.ActualWidth - length) / 2, 10 + length));
            outerSquare.Points.Add(new Point((fractalCanvas.ActualWidth - length) / 2 + length, 10 + length));
            outerSquare.Points.Add(new Point((fractalCanvas.ActualWidth - length) / 2 + length, 10));

            // Устанавливаем цвет заливки, добавляем квадрат на рабочий канвас.
            outerSquare.Fill = new SolidColorBrush(backgroundColor);
            fractalCanvas.Children.Add(outerSquare);

            // Вызываем рекурсивный метод, который непосредственно отрисовывает фрактал.
            Draw(new Coords((fractalCanvas.ActualWidth - length) / 2 + length / 3, 10 + length / 3), length / 3, 0);
        }
        
        /// <summary>
        /// Метод, отрисовывающий ковёр Серпинского.
        /// Визуально делим квадрат на 9 равных квадратов,
        /// центральный заливаем, для 8 оставшихся вызываем этот же метод.
        /// </summary>
        /// <param name="topLeftPoint"> Координаты левой верхней точки. </param>
        /// <param name="currentLength"> Текущая длина стороны. </param>
        /// <param name="iteration"> Итерация. </param>
        private void Draw(Coords topLeftPoint, double currentLength, int iteration)
        {
            // Если текущая итерация достигает заданной глубины, выходим из рекурсии.
            if (iteration >= recursionDepth)
                return;

            // Создаём квадрат как многоугольник, добавляем его точки.
            var innerSquare = new Polygon();
            innerSquare.Points.Add(new Point(topLeftPoint.X, topLeftPoint.Y));
            innerSquare.Points.Add(new Point(topLeftPoint.X, topLeftPoint.Y + currentLength));
            innerSquare.Points.Add(new Point(topLeftPoint.X + currentLength, topLeftPoint.Y + currentLength));
            innerSquare.Points.Add(new Point(topLeftPoint.X + currentLength, topLeftPoint.Y));

            // Устанавливаем цвет заливки и добавляем квадрат на рабочий канвас.
            innerSquare.Fill = GetGradientColor(iteration);
            fractalCanvas.Children.Add(innerSquare);

            // Вызываем этот же метод для 8 "незаполненных клеток".
            Draw(new Coords(topLeftPoint.X - 2 * currentLength / 3, topLeftPoint.Y - 2 * currentLength / 3),
                 currentLength / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X + currentLength / 3, topLeftPoint.Y - 2 * currentLength / 3),
                 currentLength / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X + 4 * currentLength / 3, topLeftPoint.Y - 2 * currentLength / 3),
                 currentLength / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X - 2 * currentLength / 3, topLeftPoint.Y + currentLength / 3),
                 currentLength / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X + 4 * currentLength / 3, topLeftPoint.Y + currentLength / 3),
                 currentLength / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X - 2 * currentLength / 3, topLeftPoint.Y + 4 * currentLength / 3),
                 currentLength / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X + currentLength / 3, topLeftPoint.Y + 4 * currentLength / 3),
                 currentLength / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X + 4 * currentLength / 3, topLeftPoint.Y + 4 * currentLength / 3),
                 currentLength / 3, iteration + 1);
        }

        /// <summary>
        /// необходимые значения свойствам некоторых элементов главного окна.
        /// </summary>
        /// <param name="depth"> Ссылка на Slider, устанавливающий глубину рекурсии. </param>
        /// <param name="rTB"> Ссылка на TextBox, устанавливающий значение канала R для цвета фона. </param>
        /// <param name="gTB"> Ссылка на TextBox, устанавливающий значение канала G для цвета фона. </param>
        /// <param name="bTB"> Ссылка на TextBox, устанавливающий значение канала B для цвета фона. </param>
        /// <param name="backTB"> Ссылка на Label "Background color". </param>
        /// <param name="rLabel"> Ссылка на Label "R". </param>
        /// <param name="gLabel"> Ссылка на Label "G". </param>
        /// <param name="bLabel"> Ссылка на Label "B". </param>
        /// <param name="display"> Ссылка на эллипс, показывающий цвет фона. </param>
        public static void SetProperties(Slider depth, TextBox rTB, TextBox gTB, TextBox bTB, TextBlock backTB,
                                         Label rLabel, Label gLabel, Label bLabel, Ellipse display)
        {
            (depth.Minimum, depth.Maximum, depth.Value) = (0, 4, 0);
            (rTB.Visibility, gTB.Visibility, bTB.Visibility) = (Visibility.Visible, Visibility.Visible, Visibility.Visible);
            (rLabel.Visibility, gLabel.Visibility, bLabel.Visibility) = (Visibility.Visible, Visibility.Visible, Visibility.Visible);
            (display.Visibility, backTB.Visibility) = (Visibility.Visible, Visibility.Visible);
        }
    }
}
