using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Ccr.MaterialDesign.MVVM;
using UST.Spatial.GeoJSON;

namespace UST.Spatial.Views
{
	public class MainViewModel
		: ViewModelBase
	{
		public MainViewModel()
		{
			_days = new ObservableCollection<Day>();

			_days.CollectionChanged += onDaysCollectionChanged;
		}

		public static Day FlagStacker(
			params Day[] days)
		{
			var firstDay = days.First();

			return days
				.Skip(1)
				.Aggregate(
					firstDay,
					(c, day) => c | day);
		}

		private void onDaysCollectionChanged(object Sender, NotifyCollectionChangedEventArgs E)
		{
			var stacked = FlagStacker(Days.ToArray());
			BitwiseId = (sbyte) stacked;
		}


		private ObservableCollection<Day> _days;
		public ObservableCollection<Day> Days
		{
			get => _days;
			private set
			{
				_days = value;
				NotifyOfPropertyChange(() => Days);
			}
		}

		private sbyte _bitwiseId;
		public sbyte BitwiseId
		{
			get => _bitwiseId;
			set
			{
				_bitwiseId = value;
				NotifyOfPropertyChange(() => BitwiseId);
			}
		}

		private void updateBitwise()
		{

		}

		private bool _isSundayChecked;
		public bool IsSundayChecked
		{
			get => _isSundayChecked;
			set
			{
				if (value && !_isSundayChecked)
					Days.Add(Day.Sunday);

				if (!value && _isSundayChecked)
					Days.Remove(Day.Sunday);

				_isSundayChecked = value;
				NotifyOfPropertyChange(() => IsSundayChecked);
			}
		}

		private bool _isMondayChecked;
		public bool IsMondayChecked
		{
			get => _isMondayChecked;
			set
			{
				if (value && !_isMondayChecked)
					Days.Add(Day.Monday);

				if (!value && _isMondayChecked)
					Days.Remove(Day.Monday);

				_isMondayChecked = value;
				NotifyOfPropertyChange(() => IsMondayChecked);
			}
		}

		private bool _isTuesdayChecked;
		public bool IsTuesdayChecked
		{
			get => _isTuesdayChecked;
			set
			{
				if (value && !_isTuesdayChecked)
					Days.Add(Day.Tuesday);

				if (!value && _isTuesdayChecked)
					Days.Remove(Day.Tuesday);

				_isTuesdayChecked = value;
				NotifyOfPropertyChange(() => IsTuesdayChecked);
			}
		}

		private bool _isWednesdayChecked;
		public bool IsWednesdayChecked
		{
			get => _isWednesdayChecked;
			set
			{
				if (value && !_isWednesdayChecked)
					Days.Add(Day.Wednesday);

				if (!value && _isWednesdayChecked)
					Days.Remove(Day.Wednesday);

				_isWednesdayChecked = value;
				NotifyOfPropertyChange(() => IsWednesdayChecked);
			}
		}

		private bool _isThursdayChecked;
		public bool IsThursdayChecked
		{
			get => _isThursdayChecked;
			set
			{
				if (value && !_isThursdayChecked)
					Days.Add(Day.Thursday);

				if (!value && _isThursdayChecked)
					Days.Remove(Day.Thursday);

				_isThursdayChecked = value;
				NotifyOfPropertyChange(() => IsThursdayChecked);
			}
		}

		private bool _isFridayChecked;
		public bool IsFridayChecked
		{
			get => _isFridayChecked;
			set
			{
				if (value && !_isFridayChecked)
					Days.Add(Day.Friday);

				if (!value && _isFridayChecked)
					Days.Remove(Day.Friday);

				_isFridayChecked = value;
				NotifyOfPropertyChange(() => IsFridayChecked);
			}
		}

		private bool _isSaturdayChecked;
		public bool IsSaturdayChecked
		{
			get => _isSaturdayChecked;
			set
			{
				if (value && !_isSaturdayChecked)
					Days.Add(Day.Saturday);

				if (!value && _isSaturdayChecked)
					Days.Remove(Day.Saturday);

				_isSaturdayChecked = value;
				NotifyOfPropertyChange(() => IsSaturdayChecked);
			}
		}
	}
}
