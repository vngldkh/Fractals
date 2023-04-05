using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fractals
{
    /// <summary>
    /// Класс множества Кантора.
    /// </summary>
    class CantorSet : Fractal
    {
        // Толщина отрезка.
        private readonly double thickness;
        // Расстояние между отрезками.
        private readonly double distance;

        /// <summary>
        /// Конструктор множества Кантора.
        /// Вызывает конструктор базового класса, затем устанавливает уникальные параметры.
        /// </summary>
        /// <param name="fractalCanvas"> Ссылка на основной канвас. </param>
        /// <param name="recursionDepth"> Глубина рекурсии. </param>
        /// <param name="thickness"> Толшина отрезков. </param>
        /// <param name="distance"> Расстояние между отрезками. </param>
        /// <param name="startColor"> Начальный цвет градиента. </param>
        /// <param name="endColor"> Конечный цвет градиента. </param>
        public CantorSet(Canvas fractalCanvas, int recursionDepth,
                         double thickness, double distance,
                         Color startColor, Color endColor) :
               base(fractalCanvas, recursionDepth, startColor, endColor)
        {
            this.thickness = thickness;
            this.distance = distance;
        }

        /// <summary>
        /// Инициализирует процесс рисования множества Кантора.
        /// Задаёт начальные значения и вызывает рекурсивный метод Draw.
        /// </summary>
        public override void InitDrawing()
        {
            Draw(new Coords(50, 50), 85 * fractalCanvas.ActualWidth / 100, 0);
        }

        /// <summary>
        /// Рекурсивный метод, отрисовывающий множество Кантора.
        /// Отрисовывает прямоугольник заданных размеров,
        /// вызывает этот же метод для двух отрезков следующей итерации.
        /// </summary>
        /// <param name="topLeftPoint"> Координаты левой верхней точки. </param>
        /// <param name="length"> Длина отрезка. </param>
        /// <param name="iteration"> Текущая итерация. </param>
        private void Draw(Coords topLeftPoint, double length, int iteration)
        {
            // Если текущая итерация достигает заданной глубины, выходим из рекурсии.
            if (iteration >= recursionDepth)
                return;

            // Создаём отрезок как многоугольник, задаём его точки. 
            var line = new Polygon();
            line.Points.Add(new Point(topLeftPoint.X, topLeftPoint.Y));
            line.Points.Add(new Point(topLeftPoint.X + length, topLeftPoint.Y));
            line.Points.Add(new Point(topLeftPoint.X + length, topLeftPoint.Y + thickness));
            line.Points.Add(new Point(topLeftPoint.X, topLeftPoint.Y + thickness));

            // Устанавливаем цвет заливки и добавляем квадрат на рабочий канвас.
            line.Fill = GetGradientColor(iteration);
            fractalCanvas.Children.Add(line);

            // Вызываем этот метод для двух отрезков следующей итерации.
            Draw(new Coords(topLeftPoint.X, topLeftPoint.Y + distance + thickness), length / 3, iteration + 1);
            Draw(new Coords(topLeftPoint.X + length * 2 / 3, topLeftPoint.Y + distance + thickness), length / 3, iteration + 1);
        }

        /// <summary>
        /// Метод устанавливает необходимые значения свойствам некоторых элементов главного окна.
        /// </summary>
        /// <param name="depth"> Ссылка на Slider, устанавливающий глубину рекурсии. </param>
        /// <param name="thicknessSlider"> Ссылка на Slider, устанавливающий толщину отрезка. </param>
        /// <param name="distanceSlider"> Ссылка на Slider, устанавливающий расстояние между отрезками. </param>
        /// <param name="thicknessTB"> TextBlock для толщины отрезка. </param>
        /// <param name="distanceTB"> TextBlock для расстояния между отрезками. </param>
        public static void SetProperties(Slider depth, Slider thicknessSlider, Slider distanceSlider,
                                         TextBlock thicknessTB, TextBlock distanceTB)
        {
            (depth.Minimum, depth.Maximum, depth.Value) = (1, 8, 1);
            (thicknessSlider.Visibility, distanceSlider.Visibility) = (Visibility.Visible, Visibility.Visible);
            (thicknessTB.Text, distanceTB.Text) = ("Thickness: 5", "Distance: 5");
            (thicknessSlider.Minimum, thicknessSlider.Maximum, thicknessSlider.Value) = (5, 30, 5);
            (distanceSlider.Minimum, distanceSlider.Maximum, distanceSlider.Value) = (5, 20, 5);
        }
    }
}
