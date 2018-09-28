using System;
using System.Windows;
using System.Windows.Controls;
using Ccr.PresentationCore.Helpers.DependencyHelpers;
using UST.Spatial.Primitives.BehavioralInjection.Services;

namespace UST.Spatial.Primitives.BehavioralInjection.Assists
{
	public static class ItemsControlAssist
	{
		public static readonly Type _type = typeof(ItemsControlAssist);


		public static readonly DependencyProperty ZIndexManagerServiceProperty = DP.Attach(
			_type, new MetaBase<ItemsControlContainerZIndexManagerService>(onZIndexManagerServiceChanged));

		
		public static ItemsControlContainerZIndexManagerService GetZIndexManagerService(DependencyObject @this)
		{
			return @this.Get<ItemsControlContainerZIndexManagerService>(ZIndexManagerServiceProperty);
		}

		public static void SetZIndexManagerService(DependencyObject @this, ItemsControlContainerZIndexManagerService value)
		{
			@this.Set(ZIndexManagerServiceProperty, value);
		}


		private static void onZIndexManagerServiceChanged(
			DependencyObject @this, 
			DPChangedEventArgs<ItemsControlContainerZIndexManagerService> args)
		{
			args.OldValue?.DetachHost();

			if (!(@this is ItemsControl itemsControl))
				throw new ArgumentException(
					$"The service \'ItemsControlAssist.ZIndexManagerService\' can only be used with elements " +
					$"that derive from \'ItemsControl\'.");
			
			args.NewValue?.AttachHost(itemsControl);
		}

		//private static void onTextFieldInputVisualStateServiceChanged(
		//	DependencyObject @this,
		//	DPChangedEventArgs<TextFieldInputVisualStateService> args)
		//{
		//	var visualStateManagerService = args.NewValue;

		//	var frameworkInputElement = @this.AsOrDefault<IFrameworkInputElement>();
		//	if (frameworkInputElement == null)
		//		throw new ArgumentException(
		//			$"The service \'HintAssist.TextFieldInputVisualStateService\' can only be used " +
		//			$"with elements that implement the \'IFrameworkInputElement\' interface.");

		//	visualStateManagerService
		//		.AttachElement(
		//			frameworkInputElement);
		//}
	}
}
