using System.Windows.Controls;
using System.Windows.Media;

namespace Fractals
{
    /// <summary>
    /// Базовый класс фракталов.
    /// </summary>
    internal abstract class Fractal
    {
        // Координаты точки (Занимает меньше памяти, чем Point).
        internal record Coords(double X, double Y);

        // Глубина рекрусии.
        internal int recursionDepth;
        // Начальный цвет градиента.
        internal Color startColor;
        // Конечный цвет градиента.
        internal Color endColor;
        // Ссылка на основной канвас.
        internal Canvas fractalCanvas;

        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        /// <param name="fractalCanvas"> Ссылка на основной канвас. </param>
        /// <param name="recursionDepth"> Глубина рекурсии. </param>
        /// <param name="startColor"> Начальный цвет градиента. </param>
        /// <param name="endColor"> Конечный цвет градиента. </param>
        public Fractal(Canvas fractalCanvas, int recursionDepth, Color startColor, Color endColor)
        {
            this.recursionDepth = recursionDepth;
            this.startColor = startColor;
            this.endColor = endColor;
            this.fractalCanvas = fractalCanvas;
        }

        /// <summary>
        /// Метод, инициализирующий процесс рисования фрактала.
        /// </summary>
        public abstract void InitDrawing();
        
        /// <summary>
        /// Метод, позволяющий получить цвет из градиента в зависимости от текущей итерации.
        /// </summary>
        /// <param name="iteration"> Текущая итерация. </param>
        /// <returns> Метод элементов текущей итерации. </returns>
        internal SolidColorBrush GetGradientColor(int iteration)
        {
            if (recursionDepth == 0)
                return Brushes.Black;
            var ratio = (double)iteration / (recursionDepth - 1);
            var red = (byte)(ratio * endColor.R + (1 - ratio) * startColor.R);
            var green = (byte)(ratio * endColor.G + (1 - ratio) * startColor.G);
            var blue = (byte)(ratio * endColor.B + (1 - ratio) * startColor.B);
            return new SolidColorBrush(Color.FromRgb(red, green, blue));
        }
    }
}
