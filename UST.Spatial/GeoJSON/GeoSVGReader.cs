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
        = $"Maps.{state.Value.Abbreviation}";

      var resource = System
        .Windows
        .Application
        .Current
        .FindResource(
          mapKey);

      switch (resource)
      {
        case GeometryGroup geometryGroup:
          return geometryGroup;

        case StateGeometry stateGeometry:
          return stateGeometry.GeometryGroup;

        default:
          throw new NotSupportedException();
      }
    }
    
  }
}
