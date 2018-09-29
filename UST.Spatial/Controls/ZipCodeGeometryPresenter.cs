using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Ccr.Core.Extensions;
using Ccr.PresentationCore.Helpers.DependencyHelpers;
using Ccr.PresentationCore.Helpers.EventHelpers;
using UST.Spatial.GeoJSON;

namespace UST.Spatial.Controls
{
	public class ZipCodeGeometryPresenter
		: Control
	{
		private Path PART_ZipCodeBoundaryPath;

		private static readonly Type _type = typeof(ZipCodeGeometryPresenter);


		public static readonly DependencyProperty ZipCodeDeclarationProperty = DP.Register(
			new Meta<ZipCodeGeometryPresenter, ZipCodeDeclaration>());

		public static readonly DependencyProperty DeliveryScheduleProperty = DP.Register(
			new Meta<ZipCodeGeometryPresenter, ZipCodeDeliverySchedule>());

		public static readonly DependencyProperty DeliveryConfigurationCurrentlyPaintingIdProperty = DP.Register(
			new Meta<ZipCodeGeometryPresenter, sbyte>());

		private static readonly DependencyPropertyKey IsPressedPropertyKey = DP.RegisterReadOnly(
			new Meta<ZipCodeGeometryPresenter, bool>(false, OnIsPressedChanged));

		public static readonly DependencyProperty IsPressedProperty
			= IsPressedPropertyKey.DependencyProperty;


		public ZipCodeDeclaration ZipCodeDeclaration
		{
			get => (ZipCodeDeclaration)GetValue(ZipCodeDeclarationProperty);
			set => SetValue(ZipCodeDeclarationProperty, value);
		}

		public ZipCodeDeliverySchedule DeliverySchedule
		{
			get => (ZipCodeDeliverySchedule)GetValue(DeliveryScheduleProperty);
			set => SetValue(DeliveryScheduleProperty, value);
		}

		public sbyte DeliveryConfigurationCurrentlyPaintingId
		{
			get => (sbyte)GetValue(DeliveryConfigurationCurrentlyPaintingIdProperty);
			set => SetValue(DeliveryConfigurationCurrentlyPaintingIdProperty, value);
		}

		public bool IsPressed
		{
			get => (bool)GetValue(IsPressedProperty);
			protected set => SetValue(IsPressedPropertyKey, value);
		}


		public static readonly RoutedEvent ZipCodeMouseEnterEvent
			= EM.Register<ZipCodeGeometryPresenter, MouseEventHandler>(RoutingStrategy.Bubble);

		public static readonly RoutedEvent ZipCodeMouseLeaveEvent
			= EM.Register<ZipCodeGeometryPresenter, MouseEventHandler>(RoutingStrategy.Bubble);

		public static readonly RoutedEvent ZipCodeMouseUpEvent
			= EM.Register<ZipCodeGeometryPresenter, MouseButtonEventHandler>(RoutingStrategy.Bubble);

		public static readonly RoutedEvent ZipCodeMouseDownEvent
			= EM.Register<ZipCodeGeometryPresenter, MouseButtonEventHandler>(RoutingStrategy.Bubble);

		public static readonly RoutedEvent ZipCodeClickEvent
			= EM.Register<ZipCodeGeometryPresenter, MouseButtonEventHandler>(RoutingStrategy.Bubble);



		public event MouseEventHandler ZipCodeMouseEnter
		{
			add { AddHandler(ZipCodeMouseEnterEvent, value); }
			remove { RemoveHandler(ZipCodeMouseEnterEvent, value); }
		}

		public event MouseEventHandler ZipCodeMouseLeave
		{
			add { AddHandler(ZipCodeMouseLeaveEvent, value); }
			remove { RemoveHandler(ZipCodeMouseLeaveEvent, value); }
		}

		public event MouseButtonEventHandler ZipCodeMouseUp
		{
			add { AddHandler(ZipCodeMouseUpEvent, value); }
			remove { RemoveHandler(ZipCodeMouseUpEvent, value); }
		}

		public event MouseButtonEventHandler ZipCodeMouseDown
		{
			add { AddHandler(ZipCodeMouseDownEvent, value); }
			remove { RemoveHandler(ZipCodeMouseDownEvent, value); }
		}

		public event MouseButtonEventHandler ZipCodeClick
		{
			add { AddHandler(ZipCodeClickEvent, value); }
			remove { RemoveHandler(ZipCodeClickEvent, value); }
		}



		#region Click
		/// <summary>Gets or sets when the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event occurs.  </summary>
		/// <returns>When the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event occurs. The default value is <see cref="F:System.Windows.Controls.ClickMode.Release" />. </returns>
		public static readonly DependencyProperty ClickModeProperty = DP.Register(
			new Meta<ZipCodeGeometryPresenter, ClickMode>(ClickMode.Release), ValidateClickModeProperty);

		public ClickMode ClickMode
		{
			get => (ClickMode)GetValue(ClickModeProperty);
			set => SetValue(ClickModeProperty, value);
		}

		private static readonly DependencyPropertyKey IsSpaceKeyDownPropertyKey = DP.RegisterReadOnly(
			new Meta<ZipCodeGeometryPresenter, bool>(false));
		public static readonly DependencyProperty IsSpaceKeyDownProperty = IsSpaceKeyDownPropertyKey.DependencyProperty;

		public bool IsSpaceKeyDown
		{
			get => (bool)GetValue(IsSpaceKeyDownProperty);
			protected set => SetValue(IsSpaceKeyDownPropertyKey, value);
		}

		private static bool ValidateClickModeProperty(
			ClickMode clickMode)
		{
			switch (clickMode)
			{
				case ClickMode.Hover:
				case ClickMode.Release:
				case ClickMode.Press:
					return true;

				default:
					return false;
			}
		}

		private bool IsInMainFocusScope
		{
			get
			{
				var focusScope = FocusManager.GetFocusScope(this) as Visual;

				if (focusScope != null)
					return VisualTreeHelper.GetParent(focusScope) == null;

				return true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> routed event. </summary>
		protected virtual void OnClick()
		{
			var forwardedEventArgs = new MouseButtonEventArgs(
				Mouse.PrimaryDevice,
				Environment.TickCount,
				MouseButton.Left)
			{
				RoutedEvent = ZipCodeClickEvent,
			};

			forwardedEventArgs.Source = this;
			RaiseEvent(forwardedEventArgs);
			//CommandHelpers.ExecuteCommandSource((ICommandSource)this);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.IsPressed" /> property changes.</summary>
		/// <param name="e">The data for <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />.</param>
		protected virtual void OnIsPressedChanged(
			DependencyPropertyChangedEventArgs e)
		{
			OnVisualStatePropertyChanged(this, e);
		}

		internal static void OnVisualStatePropertyChanged(
			ZipCodeGeometryPresenter @this,
			DependencyPropertyChangedEventArgs e)
		{
			@this.ChangeVisualState();
		}

		internal virtual void ChangeVisualState(
			bool useTransitions = true)
		{
			if (!IsEnabled)
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			else if (IsPressed)
				VisualStateManager.GoToState(this, "Pressed", useTransitions);
			else if (IsMouseOver)
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			else
				VisualStateManager.GoToState(this, "Normal", useTransitions);


			if (IsKeyboardFocused)
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			else
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
		}


		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event that occurs when the left mouse button is pressed while the mouse pointer is over this control.</summary>
		/// <param name="e">The event data. </param>
		protected override void OnMouseLeftButtonDown(
			MouseButtonEventArgs e)
		{
			if (ClickMode != ClickMode.Hover)
			{
				e.Handled = true;
				Focus();

				if (e.ButtonState == MouseButtonState.Pressed)
				{
					CaptureMouse();
					if (IsMouseCaptured)
					{
						if (e.ButtonState == MouseButtonState.Pressed)
						{
							if (!IsPressed)
								SetIsPressed(true);
						}
						else
							ReleaseMouseCapture();
					}
				}
				if (ClickMode == ClickMode.Press)
				{
					var flag = true;
					try
					{
						OnClick();
						flag = false;
					}
					finally
					{
						if (flag)
						{
							SetIsPressed(false);
							ReleaseMouseCapture();
						}
					}
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		private void UpdateIsPressed()
		{
			var position = Mouse.PrimaryDevice.GetPosition(this);

			if (position.X >= 0.0
					&& position.X <= ActualWidth
					&& position.Y >= 0.0
					&& position.Y <= ActualHeight)
			{
				if (IsPressed)
					return;

				SetIsPressed(true);
			}
			else
			{
				if (!IsPressed)
					return;

				SetIsPressed(false);
			}
		}


		private static void OnIsPressedChanged(
			ZipCodeGeometryPresenter @this,
			DPChangedEventArgs<bool> e)
		{
			@this.OnIsPressedChanged(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> routed event that occurs when the left mouse button is released while the mouse pointer is over this control. </summary>
		/// <param name="e">The event data.</param>
		protected override void OnMouseLeftButtonUp(
			MouseButtonEventArgs e)
		{
			if (ClickMode != ClickMode.Hover)
			{
				e.Handled = true;

				bool flag = !IsSpaceKeyDown
										&& IsPressed
										&& ClickMode == ClickMode.Release;

				if (IsMouseCaptured && !IsSpaceKeyDown)
					ReleaseMouseCapture();

				if (flag)
					OnClick();
			}
			base.OnMouseLeftButtonUp(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> routed event that occurs when the mouse pointer moves while over this element.</summary>
		/// <param name="e">The event data.</param>
		protected override void OnMouseMove(
			MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (ClickMode == ClickMode.Hover
					|| !IsMouseCaptured
					|| Mouse.PrimaryDevice.LeftButton != MouseButtonState.Pressed
					|| IsSpaceKeyDown)
				return;

			UpdateIsPressed();
			e.Handled = true;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> routed event that occurs when this control is no longer receiving mouse event messages. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.LostMouseCapture" /> event.</param>
		protected override void OnLostMouseCapture(
			MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);

			if (e.OriginalSource != this
					|| ClickMode == ClickMode.Hover
					|| IsSpaceKeyDown)
				return;

			if (IsKeyboardFocused
					&& !IsInMainFocusScope)
				Keyboard.Focus(null);

			SetIsPressed(false);
		}


		private void SetIsPressed(
			bool pressed)
		{
			if (pressed)
				SetValue(IsPressedPropertyKey, true);

			else
				ClearValue(IsPressedPropertyKey);
		}


		private bool HandleIsMouseOverChanged()
		{
			if (ClickMode != ClickMode.Hover)
				return false;

			if (IsMouseOver)
			{
				SetIsPressed(true);
				OnClick();
			}
			else
				SetIsPressed(false);
			return true;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event that occurs when the user presses a key while this control has focus.</summary>
		/// <param name="e">The event data.</param>
		protected override void OnKeyDown(
			KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (ClickMode == ClickMode.Hover)
				return;

			if (e.Key == Key.Space)
			{
				if ((Keyboard.Modifiers
						 & (ModifierKeys.Alt | ModifierKeys.Control)) == ModifierKeys.Alt
						|| IsMouseCaptured
						|| e.OriginalSource != this)
					return;

				IsSpaceKeyDown = true;
				SetIsPressed(true);

				CaptureMouse();

				if (ClickMode == ClickMode.Press)
					OnClick();

				e.Handled = true;
			}
			else if (e.Key == Key.Return
							 && (bool)GetValue(
								 KeyboardNavigation.AcceptsReturnProperty))
			{
				if (e.OriginalSource != this)
					return;

				IsSpaceKeyDown = false;
				SetIsPressed(false);

				if (IsMouseCaptured)
					ReleaseMouseCapture();

				OnClick();

				e.Handled = true;
			}
			else
			{
				if (!IsSpaceKeyDown)
					return;

				SetIsPressed(false);
				IsSpaceKeyDown = false;

				if (!IsMouseCaptured)
					return;

				ReleaseMouseCapture();
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyUp" /> routed event that occurs when the user releases a key while this control has focus.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.KeyUp" /> event.</param>
		protected override void OnKeyUp(
			KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (ClickMode == ClickMode.Hover
					|| e.Key != Key.Space
					|| !IsSpaceKeyDown
					|| (Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) == ModifierKeys.Alt)
				return;

			IsSpaceKeyDown = false;

			if (GetMouseLeftButtonReleased())
			{
				bool flag = IsPressed
										&& ClickMode == ClickMode.Release;

				if (IsMouseCaptured)
					ReleaseMouseCapture();

				if (flag)
					OnClick();
			}
			else if (IsMouseCaptured)
				UpdateIsPressed();

			e.Handled = true;
		}

		/// <summary> Called when an element loses keyboard focus. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.IInputElement.LostKeyboardFocus" /> event.</param>
		protected override void OnLostKeyboardFocus(
			KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);

			if (ClickMode == ClickMode.Hover
					|| e.OriginalSource != this)
				return;

			if (IsPressed)
				SetIsPressed(false);

			if (IsMouseCaptured)
				ReleaseMouseCapture();

			IsSpaceKeyDown = false;
		}

		/// <summary>Responds when the <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for this control is called. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.AccessKeyManager.AccessKeyPressed" /> event.</param>
		protected override void OnAccessKey(
			AccessKeyEventArgs e)
		{
			if (e.IsMultiple)
				base.OnAccessKey(e);

			else
				OnClick();
		}

		private bool GetMouseLeftButtonReleased()
		{
			return InputManager
							 .Current
							 .PrimaryMouseDevice
							 .LeftButton == MouseButtonState.Released;
		}

		#endregion



		//protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs args)
		//{
		//	args.RoutedEvent = ZipCodeClickEvent;

		//	RaiseEvent(args);
		//}


		private void RaiseMouseDoubleClick(MouseButtonEventArgs args)
		{
			OnMouseDoubleClick(args);
			RaiseEvent(args);
		}



		private void RaisePreviewMouseDown(MouseButtonEventArgs args)
		{
			OnPreviewMouseDown(args);
			RaiseEvent(args);
		}
		//protected override void OnPreviewMouseDown(MouseButtonEventArgs args)
		//{
		//	base.OnMouseDoubleClick(args);
		//}

		[SecurityCritical]
		private static void OnPreviewMouseDownThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseDown(args);
		}
		//UIElement.CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);


		[SecurityCritical]
		private static void OnMouseDownThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseDown(args);
		}
		//UIElement.CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
		//CommandManager.TranslateInput((IInputElement)sender, (InputEventArgs)e);

		[SecurityCritical]
		private static void OnPreviewMouseUpThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseUp(args);
		}
		//UIElement.CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);


		[SecurityCritical]
		private static void OnMouseUpThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseUp(args);
		}

		[SecurityCritical]
		private static void OnPreviewMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseLeftButtonDown(args);
		}

		[SecurityCritical]
		private static void OnMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseLeftButtonDown(args);
		}

		[SecurityCritical]
		private static void OnPreviewMouseLeftButtonUpThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseLeftButtonUp(args);
		}

		[SecurityCritical]
		private static void OnMouseLeftButtonUpThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseLeftButtonUp(args);
		}

		[SecurityCritical]
		private static void OnPreviewMouseRightButtonDownThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseRightButtonDown(args);
		}

		[SecurityCritical]
		private static void OnMouseRightButtonDownThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseRightButtonDown(args);
		}

		[SecurityCritical]
		private static void OnPreviewMouseRightButtonUpThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseRightButtonUp(args);
		}

		[SecurityCritical]
		private static void OnMouseRightButtonUpThunk(object sender, MouseButtonEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseRightButtonUp(args);
		}

		[SecurityCritical]
		private static void OnPreviewMouseMoveThunk(object sender, MouseEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseMove(args);
		}


		[SecurityCritical]
		private static void OnMouseMoveThunk(object sender, MouseEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseMove(args);
		}

		[SecurityCritical]
		private static void OnPreviewMouseWheelThunk(object sender, MouseWheelEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnPreviewMouseWheel(args);
		}

		[SecurityCritical]
		private static void OnMouseWheelThunk(object sender, MouseWheelEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseWheel(args);
		}

		[SecurityCritical]
		private static void OnMouseEnterThunk(object sender, MouseEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseEnter(args);
		}

		[SecurityCritical]
		private static void OnMouseLeaveThunk(object sender, MouseEventArgs args)
		{
			if (!(sender is ZipCodeGeometryPresenter presenter))
				throw new InvalidCastException();

			presenter.OnMouseLeave(args);
		}

		static ZipCodeGeometryPresenter()
		{
			ControlExtensions.OverrideStyleKey<ZipCodeGeometryPresenter>();


			//EventManager.RegisterClassHandler(_type, Mouse.PreviewMouseDownEvent, (Delegate)new MouseButtonEventHandler(UIElement.OnPreviewMouseDownThunk), true);
			//EventManager.RegisterClassHandler(_type, Mouse.MouseDownEvent, (Delegate)new MouseButtonEventHandler(UIElement.OnMouseDownThunk), true);
			//EventManager.RegisterClassHandler(_type, Mouse.PreviewMouseUpEvent, (Delegate)new MouseButtonEventHandler(UIElement.OnPreviewMouseUpThunk), true);
			//EventManager.RegisterClassHandler(_type, Mouse.MouseUpEvent, (Delegate)new MouseButtonEventHandler(UIElement.OnMouseUpThunk), true);
			//EventManager.RegisterClassHandler(_type, UIElement.PreviewMouseLeftButtonDownEvent, (Delegate)new MouseButtonEventHandler(UIElement.OnPreviewMouseLeftButtonDownThunk), false);


			EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseEventHandler>(
				MouseEnterEvent, OnMouseEnterThunk);

			EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseEventHandler>(
				MouseLeaveEvent, OnMouseLeaveThunk);

			EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseButtonEventHandler>(
				MouseDownEvent, OnMouseDownThunk);

			EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseButtonEventHandler>(
				MouseUpEvent, OnMouseUpThunk);
			

			//EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseEventHandler>(
			//	ZipCodeMouseEnterEvent, OnMouseEnterThunk);

			//EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseEventHandler>(
			//	ZipCodeMouseLeaveEvent, OnMouseLeaveThunk);

			//EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseButtonEventHandler>(
			//	ZipCodeMouseDownEvent, OnMouseDownThunk);

			//EM.RegisterClassHandler<ZipCodeGeometryPresenter, MouseButtonEventHandler>(
			//	ZipCodeMouseUpEvent, OnMouseUpThunk);

		}

		public ZipCodeGeometryPresenter()
		{
			DeliverySchedule = new ZipCodeDeliverySchedule(
				false,
				false,
				false,
				false,
				false,
				false,
				false);

			//MouseUp += OnMouseUpThunk;
			//MouseDown += OnMouseDownThunk;

			//MouseEnter += OnMouseEnterThunk;
			//MouseLeave += OnMouseLeaveThunk;
		}


		protected override void OnMouseDown(
			MouseButtonEventArgs args)
		{
			base.OnMouseDown(args);
			args.Handled = true;

			var forwardedEventArgs = new MouseButtonEventArgs(
				args.MouseDevice,
				args.Timestamp,
				args.ChangedButton,
				args.StylusDevice)
			{
				RoutedEvent = ZipCodeMouseDownEvent
			};

			forwardedEventArgs.Source = this;
			RaiseEvent(forwardedEventArgs);
		}

		protected override void OnMouseUp(
			MouseButtonEventArgs args)
		{
			base.OnMouseUp(args);
			args.Handled = true;
			
			var forwardedEventArgs = new MouseButtonEventArgs(
				args.MouseDevice,
				args.Timestamp,
				args.ChangedButton,
				args.StylusDevice)
			{
				RoutedEvent = ZipCodeMouseUpEvent
			};

			forwardedEventArgs.Source = this;
			RaiseEvent(forwardedEventArgs);
		}

		/// <summary>Provides class handling for the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.ClickMode" /> routed event that occurs when the mouse enters this control. </summary>
		/// <param name="args">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> event.</param>
		protected override void OnMouseEnter(
			MouseEventArgs args)
		{
			base.OnMouseEnter(args);
			args.Handled = true;

			if (!HandleIsMouseOverChanged())
				return;

			var forwardedEventArgs = new MouseEventArgs(
				args.MouseDevice,
				args.Timestamp,
				args.StylusDevice)
			{
				RoutedEvent = ZipCodeMouseEnterEvent,
			};

			forwardedEventArgs.Source = this;
			RaiseEvent(forwardedEventArgs);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeave" /> routed event that occurs when the mouse leaves an element. </summary>
		/// <param name="args">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> event.</param>
		protected override void OnMouseLeave(
			MouseEventArgs args)
		{
			base.OnMouseLeave(args);
			args.Handled = true;

			if (!HandleIsMouseOverChanged())
				return;

			var forwardedEventArgs = new MouseEventArgs(
				args.MouseDevice,
				args.Timestamp,
				args.StylusDevice)
			{
				RoutedEvent = ZipCodeMouseLeaveEvent,
			};

			forwardedEventArgs.Source = this;
			RaiseEvent(forwardedEventArgs);
		}

		protected override void OnMouseDoubleClick(
			MouseButtonEventArgs args)
		{
			base.OnMouseDoubleClick(args);
			args.Handled = true;

			var forwardedEventArgs = new MouseButtonEventArgs(
				args.MouseDevice,
				args.Timestamp,
				args.ChangedButton,
				args.StylusDevice)
			{
				RoutedEvent = ZipCodeClickEvent
			};

			forwardedEventArgs.Source = this;
			RaiseEvent(forwardedEventArgs);
		}


		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			PART_ZipCodeBoundaryPath = GetTemplateChild(nameof(PART_ZipCodeBoundaryPath)).As<Path>();
		}
	}
}
/*

				
			//Enumerable.Repeat(false, 7)
			//	.ToArray());
			MouseDown += OnMouseDown;
			MouseUp += OnMouseUp;
			MouseEnter += OnMouseEnter;
			MouseLeave += OnMouseLeave;
			//Loaded += (s, Args) =>
			//{
			//	DeliveryConfigurationCurrentlyPaintingId = this.FindVisualAncestor<MainWindow>()
			//		.DataContext
			//		.As<MainViewModel>()
			//		.BitwiseId;
			//};


 private void OnMouseDown(object sender, MouseButtonEventArgs E)
		{
			//CaptureMouse();
		}

		private void OnMouseUp(object sender, MouseButtonEventArgs E)
		{
			RaiseEvent(new RoutedEventArgs(ZipCodeGeometryPresenterClickedEvent, this));
			//ReleaseMouseCapture();
		}

		private void OnMouseLeave(object sender, MouseEventArgs E)
		{
			RaiseEvent(new RoutedEventArgs(ZipCodeGeometryPresenterMouseLeaveEvent, this));
		}

		private void OnMouseEnter(object sender, MouseEventArgs E)
		{
			RaiseEvent(new RoutedEventArgs(ZipCodeGeometryPresenterMouseOverEvent, this));
		}

	
		private static int brushIndex = 0;

		private static readonly List<Brush> _brushes = new List<Brush>()
		{
			Brushes.DarkRed,
			Brushes.Red,
			Brushes.OrangeRed,
			Brushes.DarkOrange,
			Brushes.Orange,
			Brushes.Yellow,
			Brushes.Chartreuse,
			Brushes.Lime,
			Brushes.Green,
			Brushes.CadetBlue,
			Brushes.Blue,
			Brushes.BlueViolet,
			Brushes.MediumPurple,
			Brushes.Purple
		};


	private static void HandleDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount != 2)
				return;
			Control control = (Control)sender;
			MouseButtonEventArgs e1 = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice);
			if (e.RoutedEvent == UIElement.PreviewMouseLeftButtonDownEvent || e.RoutedEvent == UIElement.PreviewMouseRightButtonDownEvent)
			{
				e1.RoutedEvent = Control.PreviewMouseDoubleClickEvent;
				e1.Source = e.OriginalSource;
				e1.OverrideSource(e.Source);
				control.OnPreviewMouseDoubleClick(e1);
			}
			else
			{
				e1.RoutedEvent = Control.MouseDoubleClickEvent;
				e1.Source = e.OriginalSource;
				e1.OverrideSource(e.Source);
				control.OnMouseDoubleClick(e1);
			}
			if (!e1.Handled)
				return;
			e.Handled = true;
		}*/
