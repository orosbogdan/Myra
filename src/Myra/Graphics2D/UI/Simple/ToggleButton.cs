using Myra.Attributes;
using Myra.Graphics2D.UI.Styles;
using System.ComponentModel;
using System;
using Myra.Events;


#if MONOGAME || FNA
using Microsoft.Xna.Framework.Input;
#elif STRIDE
using Stride.Input;
#else
using Myra.Platform;
#endif

namespace Myra.Graphics2D.UI
{
	/// <summary>
	/// A toggle button widget that can be toggled between two states.
	/// </summary>
	[StyleTypeName("Button")]
	public class ToggleButton : ButtonBase
	{
		private readonly SingleItemLayout<Widget> _layout;

		/// <summary>
		/// Gets or sets a value indicating whether the toggle button is in the toggled (pressed) state.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool IsToggled
		{
			get => IsPressed;
			set => IsPressed = value;
		}

		/// <summary>
		/// Gets or sets the content widget displayed inside the toggle button.
		/// </summary>
		[Browsable(false)]
		[Content]
		public override Widget Content
		{
			get => _layout.Child;
			set => _layout.Child = value;
		}

		/// <summary>
		/// Occurs when the toggled state of the toggle button changes.
		/// </summary>
		public event MyraEventHandler IsToggledChanged
		{
			add
			{
				PressedChanged += value;
			}

			remove
			{
				PressedChanged -= value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToggleButton"/> class with the specified style.
		/// </summary>
		/// <param name="styleName">The name of the style to apply. Defaults to the default stylesheet style.</param>
		public ToggleButton(string styleName = Stylesheet.DefaultStyleName)
		{
			_layout = new SingleItemLayout<Widget>(this);
			ChildrenLayout = _layout;
			SetStyle(styleName);
		}

		/// <summary>
		/// Called when a touch point is released on the toggle button.
		/// </summary>
		protected override void InternalOnTouchUp()
		{
		}

		/// <summary>
		/// Called when a touch point is pressed on the toggle button, toggling its state.
		/// </summary>
		protected override void InternalOnTouchDown()
		{
			SetValueByUser(!IsPressed);
		}

		/// <summary>
		/// Handles keyboard input for the toggle button, toggling state when Space is pressed.
		/// </summary>
		/// <param name="k">The key being pressed.</param>
		public override void OnKeyDown(Keys k)
		{
			base.OnKeyDown(k);

			if (!Enabled)
			{
				return;
			}

			if (k == Keys.Space)
			{
				SetValueByUser(!IsPressed);
			}
		}

		/// <summary>
		/// Applies a named button style from the stylesheet to the toggle button.
		/// </summary>
		/// <param name="stylesheet">The stylesheet containing the style.</param>
		/// <param name="name">The name of the button style to apply.</param>
		protected override void InternalSetStyle(Stylesheet stylesheet, string name)
		{
			ApplyButtonStyle(stylesheet.ButtonStyles.SafelyGetStyle(name));
		}

		/// <summary>
		/// Creates a toggle button with a text label as its content.
		/// </summary>
		/// <param name="text">The text to display on the button.</param>
		/// <returns>A new ToggleButton with a text label.</returns>
		public static ToggleButton CreateTextButton(string text)
		{
			return new ToggleButton
			{
				Content = new Label
				{
					Text = text
				}
			};
		}

	}
}
