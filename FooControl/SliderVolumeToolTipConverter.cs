using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using FooControl.BeefAPITypes;

namespace FooControl
{
    class SliderVolumeToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            VolumeCurve curve = new VolumeCurve(true, VolumeCurveType.Better);
            double vol = curve.calcVolumeCurve(System.Convert.ToDouble(value));
            return String.Format("{0:0.00} dB", vol);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
