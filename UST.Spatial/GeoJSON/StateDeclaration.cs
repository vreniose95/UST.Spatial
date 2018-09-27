using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Ccr.PresentationCore.Helpers.DependencyHelpers;

namespace UST.Spatial.GeoJSON
{
	[ContentProperty(nameof(ZipCodeDeclarations))]
	public class StateDeclaration
		: DependencyObject
	{
		public static readonly DependencyProperty ZipCodeDeclarationsProperty = DP.Register(
			new Meta<StateDeclaration, ObservableCollection<ZipCodeDeclaration>>());

		public ObservableCollection<ZipCodeDeclaration> ZipCodeDeclarations
		{
			get
			{
				var previousValueCollection = (ObservableCollection<ZipCodeDeclaration>)
					GetValue(ZipCodeDeclarationsProperty);

				if (previousValueCollection == null)
				{
					SetValue(
						ZipCodeDeclarationsProperty,
						new ObservableCollection<ZipCodeDeclaration>());

					previousValueCollection
						= (ObservableCollection<ZipCodeDeclaration>)
						GetValue(ZipCodeDeclarationsProperty);

					return previousValueCollection;
				}
				return (ObservableCollection<ZipCodeDeclaration>)GetValue(ZipCodeDeclarationsProperty);
			}
			set => SetValue(ZipCodeDeclarationsProperty, value);
		}



		//private GeometryGroup _geometryGroup;
		public GeometryGroup GeometryGroup
		{
			get => _toGeometryGroup();
		}


		public StateDeclaration()
		{
			ZipCodeDeclarations = new ObservableCollection<ZipCodeDeclaration>();
		}


		private GeometryGroup _toGeometryGroup()
		{
			return new GeometryGroup
			{
				Children = new GeometryCollection(
					ZipCodeDeclarations.Select(t => t.Geometry))
			};
		}
	}
}