using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fractals
{
    /// <summary>
    /// Класс фрактального дерева.
    /// </summary>
    class FractalTree : Fractal
    {
        // Отношение длины отрезка следующей итерации к длине отрезка текущей итерации.
        private double ratio;
        // Угол отклонения левой ветви от вертикали.
        private double leftAngle;
        // Угол отклонения правой ветви от вертикали.
        private double rightAngle;
        
        /// <summary>
        /// Конструктор фрактального дерева.
        /// Вызывает конструктор базового класса, затем устанавливает уникальные параметры.
        /// </summary>
        /// <param name="fractalCanvas"> Ссылка на основной канвас. </param>
        /// <param name="recursionDepth"> Глубина рекурсии. </param>
        /// <param name="ratio"> Отношение длин отрезков. </param>
        /// <param name="leftAngle"> Угол отклонения левой ветви. </param>
        /// <param name="rightAngle"> Угол отклонения правой ветви. </param>
        /// <param name="startColor"> Начальный цвет градиента. </param>
        /// <param name="endColor"> Конечный цвет градиента. </param>
        internal FractalTree(Canvas fractalCanvas, int recursionDepth, double ratio,
                             double leftAngle, double rightAngle,
                             Color startColor, Color endColor) :
                 base(fractalCanvas, recursionDepth, startColor, endColor)
        {
            this.ratio = ratio;
            this.leftAngle = leftAngle;
            this.rightAngle = rightAngle;
        }

        /// <summary>
        /// Инициализирует процесс рисования фрактального дерева.
        /// Задаёт начальные значения и вызывает рекурсивный метод Draw.
        /// </summary>
        public override void InitDrawing()
        {
            // Начальная точка.
            var startPoint = new Coords(fractalCanvas.ActualWidth / 2, fractalCanvas.ActualHeight * 9 / 10);
            // Начальная длина.
            var startLength = fractalCanvas.ActualHeight / 4;
            // Вызываем рекурсивный метод, который непосредственно отрисовывает фрактал.
            Draw(startPoint, 0, startLength, 0);
        }

        /// <summary>
        /// Рекурсивный метод, который отрисовывает фрактальное дерево.
        /// </summary>
        /// <param name="currentPoint"> Текущая точка. </param>
        /// <param name="currentAngle"> Текущий угол отклонения. </param>
        /// <param name="currentLength"> Текущая длина. </param>
        /// <param name="iteration"> Текущая итерация. </param>
        private void Draw(Coords currentPoint, double currentAngle, double currentLength, int iteration)
        {
            // Если текущая итерация достигает заданной глубины, выходим из рекурсии.
            if (iteration >= recursionDepth)
                return;

            // Создаём новую линию, задаём координаты её начала (точки, переданной в кач-ве параметра).
            var myLine = new Line();
            (myLine.X1, myLine.Y1) = (currentPoint.X, currentPoint.Y);

            // Вычисляем координаты второй точки и задаём конец прямой.
            myLine.X2 = currentPoint.X - currentLength * Math.Sin(currentAngle);
            myLine.Y2 = currentPoint.Y - currentLength * Math.Cos(currentAngle);

            // Устанавливаем цвет и толщину кисти, добавляем линию на рабочий канвас.
            myLine.Stroke = GetGradientColor(iteration);
            myLine.StrokeThickness = 2;
            fractalCanvas.Children.Add(myLine);

            // Вызываем этот же метод для двух расходящихся ветвей.
            Draw(new Coords(myLine.X2, myLine.Y2), currentAngle + rightAngle, currentLength * ratio, iteration + 1);
            Draw(new Coords(myLine.X2, myLine.Y2), currentAngle - leftAngle, currentLength * ratio, iteration + 1);
        }

        /// <summary>
        /// Метод устанавливает необходимые значения свойствам некоторых элементов главного окна.
        /// </summary>
        /// <param name="depth"> Slider, устанавливающий глубину рекурсии. </param>
        /// <param name="lAngleTB"> TextBlock для угла отклонения левой ветви. </param>
        /// <param name="rAngleTB"> TextBlock для угла отклонения правой ветви. </param>
        /// <param name="ratioTB"> TextBlock для отношения длин отрезков. </param>
        /// <param name="lAngleSlider"> Slider, устанавливающий угол отклонения левой ветви. </param>
        /// <param name="rAngleSlider"> Slider, устанавливающий угол отклонения правой ветви. </param>
        /// <param name="ratioSlider">  Slider, устанавливающий отношение отрезков. </param>
        public static void SetProperties(Slider depth, TextBlock lAngleTB, TextBlock rAngleTB, TextBlock ratioTB,
                                  Slider lAngleSlider, Slider rAngleSlider, Slider ratioSlider)
        {
            (depth.Minimum, depth.Maximum, depth.Value) = (1, 10, 1);
            (lAngleTB.Text, rAngleTB.Text, ratioTB.Text) = ("L angle: 15°", "R angle: 15°", "Ratio: 0.3");
            (lAngleSlider.Minimum, lAngleSlider.Maximum, lAngleSlider.Value) = (15, 85, 15);
            (rAngleSlider.Minimum, rAngleSlider.Maximum, rAngleSlider.Value) = (15, 85, 15);
            (ratioSlider.Minimum, ratioSlider.Maximum, ratioSlider.Value) = (0.3, 0.7, 0.3);
            (lAngleSlider.Visibility, rAngleSlider.Visibility, ratioSlider.Visibility) = (Visibility.Visible, Visibility.Visible, Visibility.Visible);
        }
    }
}
