using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UST.Spatial.Markup.Converters
{
  public class PathFigureToPathFigureGeometryConverter : IValueConverter //XamlConverter<PathFigure, NullParam, PathFigureCollection>
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is PathFigure pathFigure))
        throw new NotSupportedException();
      
      return new PathFigureCollection
      {
        pathFigure
      };

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
