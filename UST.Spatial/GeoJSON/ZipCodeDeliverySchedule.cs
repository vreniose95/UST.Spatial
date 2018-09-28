using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using Ccr.Core.Extensions;
using Ccr.PresentationCore.Helpers.DependencyHelpers;

namespace UST.Spatial.GeoJSON
{
	[Flags]
	public enum Day
		: sbyte
	{
		Sunday = 0b0000_0001,
		Monday = 0b0000_0010,
		Tuesday = 0b0000_0100,
		Wednesday = 0b0000_1000,
		Thursday = 0b0001_0000,
		Friday = 0b0010_0000,
		Saturday = 0b010_0000
	}


	/// <summary>
	/// heressummary
	/// </summary>
	[DebuggerDisplay("ToString()")]
  public class ZipCodeDeliverySchedule
		: DependencyObject
	{
		//protected class DependencyObjectSetterDispatcher
		//{
		//	private ZipCodeDeliverySchedule _parent;
		//	private bool _supressCallbackIdStacking = false;

		//	public DependencyObjectSetterDispatcher(
		//		ZipCodeDeliverySchedule parent)
		//	{

		//	}

		//	public void SetMultiple(
		//		Action<ZipCodeDeliverySchedule> action)
		//	{
		//		_supressCallbackIdStacking = true;

		//		action.Invoke(_parent);

		//		_supressCallbackIdStacking = false;
		//	}
		//}


    private static readonly IDictionary<Day, Expression<Func<ZipCodeDeliverySchedule, bool>>> _mappings
      = new Dictionary<Day, Expression<Func<ZipCodeDeliverySchedule, bool>>>
      {
        [Day.Sunday] = t => t.Sunday,
        [Day.Monday] = t => t.Monday,
        [Day.Tuesday] = t => t.Tuesday,
        [Day.Wednesday] = t => t.Wednesday,
        [Day.Thursday] = t => t.Thursday,
        [Day.Friday] = t => t.Friday,
        [Day.Saturday] = t => t.Saturday
      };


	  public IEnumerable<(Day day, bool shouldDeliver)> AggregateDayStates()
	  {
		  foreach (var day in _mappings.Keys)
		  {
			  var shouldDeliver = ShouldDeliverOnDay(day);
				yield return (day, shouldDeliver);
		  }
	  }


	  private static readonly DependencyPropertyKey ScheduleConfigurationIdPropertyKey = DP.RegisterReadOnly(
		  new Meta<ZipCodeDeliverySchedule, sbyte>(sbyte.MaxValue));

	  public static readonly DependencyProperty ScheduleConfigurationIdProperty 
		  = ScheduleConfigurationIdPropertyKey.DependencyProperty;

	  public sbyte ScheduleConfigurationId
	  {
		  get => (sbyte) GetValue(ScheduleConfigurationIdProperty);
		  protected set => SetValue(ScheduleConfigurationIdPropertyKey, value);
	  }



	  /// <summary>
		///   Indicates whether delivery should occur on Sunday.
		/// </summary>
	  public static readonly DependencyProperty SundayProperty = DP.Register(
		  new Meta<ZipCodeDeliverySchedule, bool>(false, onDayPropertyChanged));

	  public bool Sunday
		{
		  get => (bool) GetValue(SundayProperty);
		  set => SetValue(SundayProperty, value);
	  }

    /// <summary>
    ///   Indicates whether delivery should occur on Monday.
    /// </summary>
	  public static readonly DependencyProperty MondayProperty = DP.Register(
		  new Meta<ZipCodeDeliverySchedule, bool>(false, onDayPropertyChanged));
		public bool Monday
		{
		  get => (bool) GetValue(MondayProperty);
		  set => SetValue(MondayProperty, value);
	  }

    /// <summary>
    ///   Indicates whether delivery should occur on Tuesday.
    /// </summary>
	  public static readonly DependencyProperty TuesdayProperty = DP.Register(
		  new Meta<ZipCodeDeliverySchedule, bool>(false, onDayPropertyChanged));
		public bool Tuesday
		{
		  get => (bool) GetValue(TuesdayProperty);
		  set => SetValue(TuesdayProperty, value);
	  }

    /// <summary>
    ///   Indicates whether delivery should occur on Wednesday.
    /// </summary>
	  public static readonly DependencyProperty WednesdayProperty = DP.Register(
		  new Meta<ZipCodeDeliverySchedule, bool>(false, onDayPropertyChanged));
		public bool Wednesday
		{
		  get => (bool) GetValue(WednesdayProperty);
		  set => SetValue(WednesdayProperty, value);
	  }

    /// <summary>
    ///   Indicates whether delivery should occur on Thursday.
    /// </summary>
	  public static readonly DependencyProperty ThursdayProperty = DP.Register(
		  new Meta<ZipCodeDeliverySchedule, bool>(false, onDayPropertyChanged));
		public bool Thursday
		{
		  get => (bool) GetValue(ThursdayProperty);
		  set => SetValue(ThursdayProperty, value);
	  }

    /// <summary>
    ///   Indicates whether delivery should occur on Friday.
    /// </summary>
	  public static readonly DependencyProperty FridayProperty = DP.Register(
		  new Meta<ZipCodeDeliverySchedule, bool>(false, onDayPropertyChanged));
		public bool Friday
		{
		  get => (bool) GetValue(FridayProperty);
		  set => SetValue(FridayProperty, value);
	  }

    /// <summary>
    ///   Indicates whether delivery should occur on Saturday.
    /// </summary>
	  public static readonly DependencyProperty SaturdayProperty = DP.Register(
		  new Meta<ZipCodeDeliverySchedule, bool>(false, onDayPropertyChanged));
		public bool Saturday
		{
		  get => (bool) GetValue(SaturdayProperty);
		  set => SetValue(SaturdayProperty, value);
	  }


	  public ZipCodeDeliverySchedule()
	  {
	  }

		public static ZipCodeDeliverySchedule FromBitwiseSByte(
			sbyte value)
		{
			var dayStacked = (Day)value;

			var deliveryDayStates = _mappings
				.Select(
					t => (dayStacked & t.Key) == t.Key)
				.ToArray();

			return new ZipCodeDeliverySchedule(deliveryDayStates);
		}


		/// <summary>
		///   Creates an instance of the <see cref="ZipCodeDeliverySchedule"/> class.
		/// </summary>
		/// <param name="sunday">
		///  <see cref="bool"/> value indicating whether or not Sunday delivery is scheduled.
		/// </param>
		/// <param name="monday">
		///  <see cref="bool"/> value indicating whether or not Monday delivery is scheduled.
		/// </param>
		/// <param name="tuesday">
		///  <see cref="bool"/> value indicating whether or not Tuesday delivery is scheduled.
		/// </param>
		/// <param name="wednesday">
		///  <see cref="bool"/> value indicating whether or not Wednesday delivery is scheduled.
		/// </param>
		/// <param name="thursday">
		///<see cref="bool"/> value indicating whether or not Thursday delivery is scheduled.
		/// </param>
		/// <param name="friday">
		///<see cref="bool"/> value indicating whether or not Friday delivery is scheduled.
		/// </param>
		/// <param name="saturday">
		///<see cref="bool"/> value indicating whether or not Saturday delivery is scheduled.
		/// </param>
		public ZipCodeDeliverySchedule(
      bool sunday,
      bool monday,
      bool tuesday,
      bool wednesday,
      bool thursday,
      bool friday,
      bool saturday)
				: this()
		{
			Sunday = sunday;
      Monday = monday;
      Tuesday = tuesday;
      Wednesday = wednesday;
      Thursday = thursday;
      Friday = friday;
      Saturday = saturday;
		}

		
    public ZipCodeDeliverySchedule(
      params bool[] deliveryDays)
				: this()
    {
      if (deliveryDays.Length != _mappings.Count)
        throw new NotSupportedException();

	    var index = 0;
      foreach (var mapping in _mappings)
      {
        var setterExpression = mapping
          .Value
          .ToSetterExpression();

	      var shouldDeliver = deliveryDays[index];

        setterExpression.Compile()(this, shouldDeliver);
	      index++;
      }
    }
		

    public bool ShouldDeliverOnDay(
      Day Day)
    {
      if (!_mappings.TryGetValue(Day, out var func))
        throw new KeyNotFoundException(
          $"There is no declared \'Day\' entry in the dictionary.");

      var result = func.Compile()(this);
      return result;
    }

	  public bool ShouldDeliverOnDays(
		  params Day[] daysOfWeek)
	  {
		  return daysOfWeek.All(ShouldDeliverOnDay);
	  }

	  public bool ShouldDeliverOnAnyOfDays(
		  params Day[] daysOfWeek)
	  {
		  return daysOfWeek.Any(ShouldDeliverOnDay);
	  }
		

	  public void SetShouldDeliverOnDay(
		  bool shouldDeliver,
			Day Day)
	  {
		  if (!_mappings.TryGetValue(Day, out var func))
			  throw new KeyNotFoundException(
				  $"There is no declared \'Day\' entry in the dictionary.");

		  var setterExpression = func.ToSetterExpression();
		  setterExpression.Compile()(this, shouldDeliver);
	  }

	  public void SetShouldDeliverOnDays(
		  bool shouldDeliver,
		  params Day[] daysOfWeek)
	  {
			daysOfWeek.ForEach(
				t => SetShouldDeliverOnDay(shouldDeliver, t));
		}


	  public void InvertShouldDeliverOnDay(
		  Day Day)
	  {
		  if (!_mappings.TryGetValue(Day, out var func))
			  throw new KeyNotFoundException(
				  $"There is no declared \'Day\' entry in the dictionary.");

		  var compiledFunc = func.Compile();

		  var lastValue = compiledFunc(this);

		  var setterExpression = func.ToSetterExpression();
		  setterExpression.Compile()(this, !lastValue);
	  }

	  public void InvertShouldDeliverOnDays(
		  params Day[] daysOfWeek)
	  {
		  daysOfWeek.ForEach(InvertShouldDeliverOnDay);
	  }


		private static void onDayPropertyChanged(
		  ZipCodeDeliverySchedule @this,
		  DPChangedEventArgs<bool> args)
		{
			var dayStates = @this
				.AggregateDayStates()
				.Where(t => t.shouldDeliver)
				.Select(t => t.day)
				.ToArray();

			var stackedDays = FlagStacker(dayStates);
			var integralValue = (sbyte) stackedDays;

			@this.ScheduleConfigurationId = integralValue;
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

		public override string ToString()
    {
      var sb = new StringBuilder();

      if (Sunday)
        sb.Append("Sunday, ");

      if (Monday)
        sb.Append("Monday, ");

      if (Tuesday)
        sb.Append("Tuesday, ");

      if (Wednesday)
        sb.Append("Wednesday, ");

      if (Thursday)
        sb.Append("Thurday, ");

      if (Friday)
        sb.Append("Friday, ");

      if (Saturday)
        sb.Append("Saturday, ");

      var str = sb.ToString();

      if (str.EndsWith(", "))
        str = str.Substring(str.Length - 2);

      return str;
    }
  }
}