using System.Windows.Media;
using Ccr.Xaml.Markup.Converters.Infrastructure;
using UST.Spatial.GeoJSON;

namespace UST.Spatial.Markup.Converters
{
	public class ZipCodeDeclarationToPathGeometryConverter
		: XamlConverter<
			ZipCodeDeclaration,
			NullParam,
			PathGeometry>
	{
		public override PathGeometry Convert(
			ZipCodeDeclaration zipCodeDeclaration, 
			NullParam param)
		{
			var pathGeometry = PathGeometry
				.CreateFromGeometry(
					zipCodeDeclaration
						.Geometry);

			return pathGeometry;
		}
	}
}
