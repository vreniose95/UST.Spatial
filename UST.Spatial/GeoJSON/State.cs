using System.Runtime.CompilerServices;
using Ccr.PresentationCore.Collections;

namespace UST.Spatial.GeoJSON
{
	public partial class State
    : ValueEnum<(string Abbreviation, string StateName)>
	{
		public int StateID { get; set; }


	  private State(
			int stateID,
			string abbreviation,
			[CallerMemberName] string memberName = "") 
		    : this(
		      (abbreviation,
		      memberName.Replace('_', ' ')))
		{
			StateID = stateID;
		}
	}
}
