using Ccr.MaterialDesign.MVVM;

namespace UST.Spatial.Views
{
	public class MainViewModel
		: ViewModelBase
	{
		private string _stuff = "hdnasklfdnaskl";
		public string Stuff
		{
			get => _stuff;
			set
			{
				_stuff = value;
				NotifyOfPropertyChange(() => Stuff);
			}
		}
	}
}
 