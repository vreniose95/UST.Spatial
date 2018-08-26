using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

namespace UST.Spatial.GeoJSON
{
  public class GeoSVGReader
  {

    public static GeometryGroup Read(
      State state)
    {
      var mapKey
        = $"Maps.{state.Abbreviation}";

      var o = System.Windows.Application.Current.FindResource(mapKey);

      var s = (GeometryGroup) o;
      return s;

    }
    
  }
}
