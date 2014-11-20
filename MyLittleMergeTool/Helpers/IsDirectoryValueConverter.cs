using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MyLittleMergeTool.Helpers
{
    public sealed class IsDirectoryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            Boolean myValue = (Boolean)value;
            if (myValue)
                return "/Images/folder.png";
            else
                return "/Images/script.png";

        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
