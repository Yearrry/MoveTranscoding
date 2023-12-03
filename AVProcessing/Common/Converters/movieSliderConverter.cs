using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AVProcessing.Common.Converters
{
    public class movieSliderConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(values[0].ToString()) || string.IsNullOrWhiteSpace(values[1].ToString())) return 0D;

            TimeSpan EndTime = TimeSpan.ParseExact(values[1].ToString(),@"hh\:mm\:ss",CultureInfo.CurrentCulture);

            int EndallSecounds = EndTime.Hours * 3600 + EndTime.Minutes * 60 + EndTime.Seconds;

            TimeSpan positionTime =  TimeSpan.ParseExact(values[0].ToString(), @"hh\:mm\:ss", CultureInfo.CurrentCulture);

            int positionAllSecounds = positionTime.Hours * 3600 + positionTime.Minutes * 60 + positionTime.Seconds;

            int slidervalue = System.Convert.ToInt32(System.Convert.ToInt32(values[2]) * positionAllSecounds / EndallSecounds);
            return slidervalue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
