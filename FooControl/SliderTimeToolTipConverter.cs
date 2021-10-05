using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using FooControl.BeefAPITypes;

namespace FooControl
{
    public class SliderTimeToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Player playerData = new Player();
            playerData.ActiveItem = new ActiveItem();
            playerData.ActiveItem.Position = System.Convert.ToInt32(value);
            return playerData.ActiveItem.convertPositionToFormattedTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
