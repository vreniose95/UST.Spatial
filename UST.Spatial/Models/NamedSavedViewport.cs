using UST.Spatial.GeoJSON;

namespace UST.Spatial.Models
{
	public class NamedSavedViewport
	{
		public string ViewportName { get; set; }

		public string CityName { get; set; }

		public State State { get; set; }

		public double PanPositionX { get; set; }

		public double PanPositionY { get; set; }

		public double ScaleZoom { get; set; }

	}
}
