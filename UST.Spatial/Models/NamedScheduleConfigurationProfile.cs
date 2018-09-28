using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using UST.Spatial.GeoJSON;

namespace UST.Spatial.Models
{
	public class NamedScheduleConfigurationProfile
	{
		public ZipCodeDeliverySchedule DeliverySchedule { get; }

		public string DisplayName { get; }

		public Brush ProfileBrush { get; }

		public sbyte ProfileId
		{
			get => DeliverySchedule.ScheduleConfigurationId;
		}


		public IEnumerable<Day> DaysDelivered
		{
			get => DeliverySchedule
				.AggregateDayStates()
				.Where(t => t.shouldDeliver)
				.Select(t => t.day);
		}

		public IEnumerable<Day> DaysNotDelivered
		{
			get => DeliverySchedule
				.AggregateDayStates()
				.Where(t => !t.shouldDeliver)
				.Select(t => t.day);
		}
		

		public NamedScheduleConfigurationProfile(
			sbyte scheduleConfigurationId,
			string displayName,
			Brush profileBrush)
		{
			DeliverySchedule = ZipCodeDeliverySchedule.FromBitwiseSByte(scheduleConfigurationId);
			DisplayName = displayName;
			ProfileBrush = profileBrush;
		}
	}
}
