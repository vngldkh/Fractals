using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализация главного окна.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            FractalCanvas.Width = ScrollViewer.ActualWidth;
            FractalCanvas.Height = ScrollViewer.ActualHeight;
            ScaleTextBox.Text = "Scale: 1.00x";
        }

        /// <summary>
        /// Событие изменения значения ползунка масштаба.
        /// Изменяет текст ScaleTextBlock.
        /// </summary>
        /// <param name="sender"> Ползунок Scale. </param>
        /// <param name="e"> Данные о событии. </param>
        private void Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScaleTransform.ScaleX = Scale.Value;
            ScaleTransform.ScaleY = Scale.Value;
            ScaleTextBox.Text = $"Scale: {Scale.Value:f2}x";
        }

        /// <summary>
        /// Событие изменения размера окна.
        /// Если фрактал нарисован, перерисовывает его.
        /// </summary>
        /// <param name="sender"> Главное окно. </param>
        /// <param name="e"> Данные о событии. </param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FractalCanvas.Width = ScrollViewer.ActualWidth;
            FractalCanvas.Height = ScrollViewer.ActualHeight;
            if (fractalIsDrawn)
                Draw();
        }

        // Номер отрисованного фрактала.
        int currentDrawnFractal = -1;
        // Имеет значение true, если фрактал отрисован.
        bool fractalIsDrawn = false;

        /// <summary>
        /// Изменения значения ползунка глубины (Slider Depth).
        /// Меняет текст DepthTextBlock.
        /// Если фрактал отрисован, перерисовывает его.
        /// </summary>
        /// <param name="sender"> Ползунок Depth. </param>
        /// <param name="e"> Данные о событии. </param>
        private void Depth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DepthTextBox.Text = $"Depth: {((int)Depth.Value < 0 ? "" : $"{ (int)Depth.Value}")}";
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Прячет все опциональные элементы.
        /// </summary>
        private void HideAllElements()
        {
            (FirstAdditionTextBlock.Text, SecondAdditionTextBlock.Text) = ("", "");
            (ThirdAdditionTextBlock.Text, FourthAdditionTextBlock.Text) = ("", "");
            (FirstAdditionSlider.Visibility, SecondAdditionSlider.Visibility) = (Visibility.Hidden, Visibility.Hidden);
            (ThirdAdditionSlider.Visibility, FourthAdditionSlider.Visibility) = (Visibility.Hidden, Visibility.Hidden);
            (BackColorR.Visibility, BackColorG.Visibility, BackColorB.Visibility) = (Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
            (LabelR.Visibility, LabelG.Visibility, LabelB.Visibility) = (Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
            (BackColorDisplay.Visibility, BackTextBlock.Visibility) = (Visibility.Hidden, Visibility.Hidden);
        }

        /// <summary>
        /// Событие изменения выбранного типа фрактала.
        /// Изменяет параметры опциональных элементов в зависимости от выбранного типа.
        /// </summary>
        /// <param name="sender"> ComboBox FractalTypeCB. </param>
        /// <param name="e"> Данные о событии. </param>
        private void FractalTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FractalCanvas.Children.Clear();
            fractalIsDrawn = false;
            currentDrawnFractal = -1;
            HideAllElements();

            switch (FractalTypeCB.SelectedIndex)
            {
                case 0:
                    FractalTree.SetProperties(Depth, FirstAdditionTextBlock, SecondAdditionTextBlock, ThirdAdditionTextBlock,
                                              FirstAdditionSlider, SecondAdditionSlider, ThirdAdditionSlider);
                    break;
                case 1:
                    SerpinskyCarpet.SetProperties(Depth, BackColorR, BackColorG, BackColorB, BackTextBlock,
                                                  LabelR, LabelG, LabelB, BackColorDisplay);
                    break;
                case 2:
                    SerpinskyTriangle.SetProperties(Depth);
                    break;
                case 3:
                    CantorSet.SetProperties(Depth, ThirdAdditionSlider, FourthAdditionSlider, 
                                            ThirdAdditionTextBlock, FourthAdditionTextBlock);
                    break;
                case 4:
                    KochCurve.SetProperties(Depth);
                    break;
            }
        }

        /// <summary>
        /// Событие ввода текста.
        /// Проверяет, является ли введённый символ числом.
        /// </summary>
        /// <param name="sender"> Данные об отправителе. </param>
        /// <param name="e"> Данные о событии. </param>
        private void RTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox FirstColorR. </param>
        /// <param name="e"> Данные о событии. </param>
        private void RTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(FirstColorR.Text) && int.Parse(FirstColorR.Text) > 255)
                FirstColorR.Text = "255";
            if (!(FirstColorR == null || FirstColorG == null || FirstColorB == null))
                FirstColorDisplay.Fill = new SolidColorBrush(GetStartColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox FirstColorG. </param>
        /// <param name="e"> Данные о событии. </param>
        private void GTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(FirstColorG.Text) && int.Parse(FirstColorG.Text) > 255)
                FirstColorG.Text = "255";
            if (!(FirstColorR == null || FirstColorG == null || FirstColorB == null))
                FirstColorDisplay.Fill = new SolidColorBrush(GetStartColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox FirstColorB. </param>
        /// <param name="e"> Данные о событии. </param>
        private void BTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(FirstColorB.Text) && int.Parse(FirstColorB.Text) > 255)
                FirstColorB.Text = "255";
            if (!(FirstColorR == null || FirstColorG == null || FirstColorB == null))
                FirstColorDisplay.Fill = new SolidColorBrush(GetStartColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox EndColorR. </param>
        /// <param name="e"> Данные о событии. </param>
        private void EndColorR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(EndColorR.Text) && int.Parse(EndColorR.Text) > 255)
                EndColorR.Text = "255";
            if (!(EndColorR == null || EndColorG == null || EndColorB == null))
                EndColorDisplay.Fill = new SolidColorBrush(GetEndColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox EndColorG. </param>
        /// <param name="e"> Данные о событии. </param>
        private void EndColorG_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(EndColorG.Text) && int.Parse(EndColorG.Text) > 255)
                EndColorG.Text = "255";
            if (!(EndColorR == null || EndColorG == null || EndColorB == null))
                EndColorDisplay.Fill = new SolidColorBrush(GetEndColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox EndColorB. </param>
        /// <param name="e"> Данные о событии. </param>
        private void EndColorB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(EndColorB.Text) && int.Parse(EndColorB.Text) > 255)
                EndColorB.Text = "255";
            if (!(EndColorR == null || EndColorG == null || EndColorB == null))
                EndColorDisplay.Fill = new SolidColorBrush(GetEndColor());
            if (fractalIsDrawn)
                Draw(); 
        }

        /// <summary>
        /// Событие изменения значения слайдера первого опционального параметра.
        /// Работает, если выбран тип "Fractal tree".
        /// </summary>
        /// <param name="sender"> Ползунок FirstAdditionSlider. </param>
        /// <param name="e"> Данные о событии. </param>
        private void FirstAdditionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (FractalTypeCB.SelectedIndex)
            {
                case 0:
                    FirstAdditionTextBlock.Text = $"L Angle: {(int) FirstAdditionSlider.Value}°";
                    if (fractalIsDrawn)
                        Draw();
                    break;
            }
        }

        /// <summary>
        /// Событие изменения значения слайдера второго опционального параметра.
        /// Работает только, если выбран тип "Fractal tree".
        /// </summary>
        /// <param name="sender"> Ползунок SecondAdditionSlider. </param>
        /// <param name="e"> Данные о событии. </param>
        private void SecondAdditionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (FractalTypeCB.SelectedIndex)
            {
                case 0:
                    SecondAdditionTextBlock.Text = $"R Angle: {(int)SecondAdditionSlider.Value}°";
                    if (fractalIsDrawn)
                        Draw();
                    break;
            }
        }

        /// <summary>
        /// Событие изменения значения слайдера третьего опционального параметра.
        /// </summary>
        /// <param name="sender"> Ползунок ThirdAdditionSlider. </param>
        /// <param name="e"> Данные о событии. </param>
        private void ThirdAdditionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (FractalTypeCB.SelectedIndex)
            {
                case 0:
                    ThirdAdditionTextBlock.Text = $"Ratio: {ThirdAdditionSlider.Value:f2}";
                    if (fractalIsDrawn)
                        Draw();
                    break;
                case 3:
                    ThirdAdditionTextBlock.Text = $"Thickness: {(int)ThirdAdditionSlider.Value}";
                    if (fractalIsDrawn)
                        Draw();
                    break;
            }
        }

        /// <summary>
        /// Событие изменения значения слайдера четвёртого опционального параметра.
        /// Работает, если выбран тип "Cantor's set".
        /// </summary>
        /// <param name="sender"> Ползунок FourthAdditionSlider. </param>
        /// <param name="e"> Данные о событии. </param>
        private void FourthAdditionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (FractalTypeCB.SelectedIndex)
            {
                case 3:
                    FourthAdditionTextBlock.Text = $"Distance: {(int)FourthAdditionSlider.Value}";
                    if (fractalIsDrawn)
                        Draw();
                    break;
            }
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox BackColorR. </param>
        /// <param name="e"> Данные о событии. </param>
        private void BackColorR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(BackColorR.Text) && int.Parse(BackColorR.Text) > 255)
                BackColorR.Text = "255";
            if (!(BackColorR == null || BackColorG == null || BackColorB == null))
                BackColorDisplay.Fill = new SolidColorBrush(GetBackColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox BackColorG. </param>
        /// <param name="e"> Данные о событии. </param>
        private void BackColorG_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(BackColorG.Text) && int.Parse(BackColorG.Text) > 255)
                BackColorG.Text = "255";
            if (!(BackColorR == null || BackColorG == null || BackColorB == null))
                BackColorDisplay.Fill = new SolidColorBrush(GetBackColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Событие изменения текста.
        /// Не даёт значению выйти за рамки [0;255].
        /// </summary>
        /// <param name="sender"> TextBox BackColorB. </param>
        /// <param name="e"> Данные о событии. </param>
        private void BackColorB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(BackColorB.Text) && int.Parse(BackColorB.Text) > 255)
                BackColorB.Text = "255";
            if (!(BackColorR == null || BackColorG == null || BackColorB == null))
                BackColorDisplay.Fill = new SolidColorBrush(GetBackColor());
            if (fractalIsDrawn)
                Draw();
        }

        /// <summary>
        /// Метод сохраняющий ОТРЕНДЕРЕННУЮ часть канваса по заданному пути.
        /// </summary>
        /// <param name="path"> Путь. </param>
        private void Save(string path)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)FractalCanvas.RenderSize.Width,
                                                            (int)FractalCanvas.RenderSize.Height, 96d, 96d, 
                                                            PixelFormats.Default);
            rtb.Render(FractalCanvas);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = System.IO.File.OpenWrite(path))
            {
                pngEncoder.Save(fs);
            }
        }

        /// <summary>
        /// Событие нажатия кнопки "Save".
        /// </summary>
        /// <param name="sender"> Безымянная кнопка "Save". </param>
        /// <param name="e"> Данные о событии. </param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sd = new Microsoft.Win32.SaveFileDialog();
            sd.FileName = "Fractal";
            sd.Filter = "PNG-Image (.png)| *.png|JPEG-Image (.jpg)| *.jpg|All files (.*)| *.*";
            try
            {
                sd.ShowDialog();
                Save(sd.FileName);
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка!\nВозможно Вы не имеете права сохранять файл в выбранной директиве.");
            }
        }

        /// <summary>
        /// Метод достаёт значения из ячеек начального цвета типа TextBox и возвращает соответствующий цвет.
        /// </summary>
        /// <returns> Начальный цвет градиента. </returns>
        private Color GetStartColor()
            => Color.FromRgb(String.IsNullOrEmpty(FirstColorR.Text) ? (byte)0 : byte.Parse(FirstColorR.Text),
                             String.IsNullOrEmpty(FirstColorG.Text) ? (byte)0 : byte.Parse(FirstColorG.Text),
                             String.IsNullOrEmpty(FirstColorB.Text) ? (byte)0 : byte.Parse(FirstColorB.Text));

        /// <summary>
        /// Метод достаёт значения из ячеек конечного цвета типа TextBox и возвращает соответствующий цвет.
        /// </summary>
        /// <returns> Конечный цвет градиента. </returns>
        private Color GetEndColor()
            => Color.FromRgb(String.IsNullOrEmpty(EndColorR.Text) ? (byte)0 : byte.Parse(EndColorR.Text),
                             String.IsNullOrEmpty(EndColorG.Text) ? (byte)0 : byte.Parse(EndColorG.Text),
                             String.IsNullOrEmpty(EndColorB.Text) ? (byte)0 : byte.Parse(EndColorB.Text));

        /// <summary>
        /// Метод достаёт значения из ячеек цвета фона ковра типа TextBox и возвращает соответствующий цвет.
        /// </summary>
        /// <returns> Цвет фона ковра. </returns>
        private Color GetBackColor()
            => Color.FromRgb(String.IsNullOrEmpty(BackColorR.Text) ? (byte)0 : byte.Parse(BackColorR.Text),
                             String.IsNullOrEmpty(BackColorG.Text) ? (byte)0 : byte.Parse(BackColorG.Text),
                             String.IsNullOrEmpty(BackColorB.Text) ? (byte)0 : byte.Parse(BackColorB.Text));

        /// <summary>
        /// Отрисовывает фрактал с заданными параметрами.
        /// </summary>
        private void Draw()
        {
            FractalCanvas.Children.Clear();
            switch (currentDrawnFractal)
            {
                case 0:
                    var leftAngle = ((int)FirstAdditionSlider.Value) / 180.0 * Math.PI;
                    var rightAngle = ((int)SecondAdditionSlider.Value) / 180.0 * Math.PI;

                    var tree = new FractalTree(FractalCanvas, (int)Depth.Value, ThirdAdditionSlider.Value, 
                                               leftAngle, rightAngle, GetStartColor(), GetEndColor());
                    tree.InitDrawing();
                    break;
                case 1:
                    var carpet = new SerpinskyCarpet(FractalCanvas, (int)Depth.Value, GetStartColor(), GetEndColor(), GetBackColor());
                    carpet.InitDrawing();
                    break;
                case 2:
                    var triangle = new SerpinskyTriangle(FractalCanvas, (int)Depth.Value, GetStartColor(), GetEndColor());
                    triangle.InitDrawing();
                    break;
                case 3:
                    var set = new CantorSet(FractalCanvas, (int)Depth.Value, (int)ThirdAdditionSlider.Value, 
                                            (int)FourthAdditionSlider.Value, GetStartColor(), GetEndColor());
                    set.InitDrawing();
                    break;
                case 4:
                    var curve = new KochCurve(FractalCanvas, (int)Depth.Value + 1, GetStartColor(), GetEndColor());
                    curve.InitDrawing();
                    break;
            }
        }

        /// <summary>
        /// Событие нажатие кнопки "Draw".
        /// </summary>
        /// <param name="sender"> Кнопка "Draw". </param>
        /// <param name="e"> Данные о событии. </param>
        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            currentDrawnFractal = FractalTypeCB.SelectedIndex;
            fractalIsDrawn = true;
            Draw();           
        }
    }
}
