using System;

namespace UST.Spatial.GeoJSON
{
  public class GeoSVGReader
  {
		//public static GeometryGroup Read(
    //  State state)
    //{
    //  var mapKey
    //    = $"Maps.{state.Value.Abbreviation}";

    //  var resource = System
    //    .Windows
    //    .Application
    //    .Current
    //    .FindResource(
    //      mapKey);

    //  switch (resource)
    //  {
    //    case GeometryGroup geometryGroup:
    //      return geometryGroup;

    //    case StateDeclaration stateGeometry:
    //      return stateGeometry.GeometryGroup;

    //    default:
    //      throw new NotSupportedException();
    //  }
    //}

	  public static StateDeclaration ReadStateDeclaration(
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
			  case StateDeclaration stateGeometry:
				  return stateGeometry;

			  default:
				  throw new NotSupportedException();
		  }
	  }

	}
}
