using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CrypTool.Plugins.CommonNMGrammFinder
{
    public class FrequencyToColorConverter : IValueConverter
    {
        public int LowerBoundary { get; set; }
        public int UpperBoundary { get; set; }

        /// <summary>
        /// Converts a frequency value to a color based on the FLIR color palette.
        /// </summary>
        /// <param name="value">The frequency value to convert.</param>
        /// <param name="targetType">The target type of the conversion (not used).</param>
        /// <param name="parameter">Additional parameter for the conversion (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>A SolidColorBrush representing the color corresponding to the frequency value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int frequency)
            {
                if (frequency < LowerBoundary || frequency > UpperBoundary)
                {
                    return Brushes.Transparent;
                }

                double range = UpperBoundary - LowerBoundary;
                double position = (frequency - LowerBoundary) / range;
                double x = 433 * position;

                // Calculate the RGB values based on the FLIR color palette
                double R = 4.18485e-6 * x * x * x - 0.00532377 * x * x + 2.19321 * x - 39.1125;
                double G = 1.28826e-10 * x * x * x * x * x - 1.64251e-7 * x * x * x * x + 6.73208e-5 * x * x * x - 0.00808127 * x * x + 0.280643 * x - 1.61706;
                double B = 9.48804e-12 * x * x * x * x * x - 1.05015e-8 * x * x * x * x + 4.19544e-5 * x * x * x - 0.0232532 * x * x + 3.24907 * x + 30.466;

                // Ensure RGB values are within the valid range
                byte r = (byte)Math.Max(0, Math.Min(255, R));
                byte g = (byte)Math.Max(0, Math.Min(255, G));
                byte b = (byte)Math.Max(0, Math.Min(255, B));

                return new SolidColorBrush(Color.FromRgb(r, g, b));
            }

            return Brushes.Transparent;
        }

        /// <summary>
        /// Converts a color back to a frequency value (not implemented).
        /// </summary>
        /// <param name="value">The color value to convert back.</param>
        /// <param name="targetType">The target type of the conversion (not used).</param>
        /// <param name="parameter">Additional parameter for the conversion (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>Throws a NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}