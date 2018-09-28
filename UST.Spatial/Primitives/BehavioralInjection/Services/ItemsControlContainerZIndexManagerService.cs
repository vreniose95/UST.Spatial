using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Ccr.Core.Extensions;
using Ccr.MaterialDesign.Primitives.Behaviors.Services;
using UST.Spatial.Controls;

namespace UST.Spatial.Primitives.BehavioralInjection.Services
{
	public class ItemsControlContainerZIndexManagerService
		: HostedElement<ItemsControl>
	{
		private const int LowZIndex = 100;
		private const int HighZIndex = 200;

		protected override void OnHostAttached(DependencyObject host)
		{
			base.OnHostAttached(host);

			if (!(host is ItemsControl itemsControl))
				throw new NotSupportedException(
					$"");

			itemsControl.SourceUpdated += onItemsControlSourceUpdated;
			itemsControl.TargetUpdated += onItemsControlTargetUpdated;
			itemsControl.Items.CurrentChanged += onItemsControlCurrentChanged;

			itemsControl.ItemContainerGenerator.ItemsChanged
				+= ItemContainerGeneratorOnItemsChanged;

			itemsControl.ItemContainerGenerator.StatusChanged
				+= ItemContainerGeneratorOnStatusChanged;



			//var canvases = itemsControl.FindVisualChildren<Canvas>().ToArray();
			//var canvas = canvases.First();

			//foreach (var canvasChild in canvas.Children)
			//{

			//}

		}

		private void ItemContainerGeneratorOnStatusChanged(object Sender, EventArgs args)
		{

		}

		private void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
					{
						foreach (var item in HostElement.Items)
						{
							if (item is ZipCodeGeometryPresenter presenter)
							{
								Panel.SetZIndex(presenter, LowZIndex);

								presenter.MouseEnter += onPresenterMouseEnter;
								presenter.MouseLeave += onPresenterMouseLeave;
							}
						}
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					{
						foreach (var item in HostElement.Items)
						{
							if (item is ZipCodeGeometryPresenter presenter)
							{
								Panel.SetZIndex(presenter, LowZIndex);

								presenter.MouseEnter -= onPresenterMouseEnter;
								presenter.MouseLeave -= onPresenterMouseLeave;
							}
						}
					}
					break;
				case NotifyCollectionChangedAction.Replace:
					break;
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Reset:
					{
						foreach (var item in HostElement.Items)
						{
							if (item is ZipCodeGeometryPresenter presenter)
							{
								Panel.SetZIndex(presenter, LowZIndex);

								presenter.MouseEnter -= onPresenterMouseEnter;
								presenter.MouseLeave -= onPresenterMouseLeave;
							}
						}
					}
					break;
			}
		}

		private void onPresenterMouseLeave(object sender, MouseEventArgs args)
		{
			if (!(args.OriginalSource is ZipCodeGeometryPresenter presenter))
			{
				Panel.SetZIndex(args.OriginalSource.As<UIElement>(), LowZIndex);
			}
			else
				Panel.SetZIndex(presenter, LowZIndex);
		}

		private void onPresenterMouseEnter(object sender, MouseEventArgs args)
		{
			if (!(args.OriginalSource is ZipCodeGeometryPresenter presenter))
			{
				Panel.SetZIndex(args.OriginalSource.As<UIElement>(), HighZIndex);
			}
			else
				Panel.SetZIndex(presenter, LowZIndex);
		}

		private void onItemsControlTargetUpdated(object Sender, DataTransferEventArgs E)
		{

		}

		private void onItemsControlSourceUpdated(object Sender, DataTransferEventArgs E)
		{

		}

		private void onItemsControlCurrentChanged(object Sender, EventArgs E)
		{

		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
		}

		private void OnMouseDown(object Sender, MouseButtonEventArgs MouseButtonEventArgs)
		{
			var mousePosition = new Point(2, 2);

			HostElement.LayoutTransform
				= new TranslateTransform(
					-mousePosition.X,
					-mousePosition.Y);
		}
	}
}
