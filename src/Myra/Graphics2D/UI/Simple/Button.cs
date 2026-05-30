using Myra.Graphics2D.UI.Styles;
using Myra.Attributes;
using System;
using System.ComponentModel;
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
	/// A clickable button widget that can contain any widget as its content.
	/// </summary>
	[StyleTypeName("Button")]
	public class Button : ButtonBase
	{
		private readonly SingleItemLayout<Widget> _layout;
		internal bool ReleaseOnTouchLeft;

		/// <summary>
		/// Gets or sets the desktop that contains this button.
		/// </summary>
		public override Desktop Desktop
		{
			get
			{
				return base.Desktop;
			}

			internal set
			{
				// If we're not releasing the button on touch left,
				// we have to do it on touch up
				if (!ReleaseOnTouchLeft && Desktop != null)
				{
					Desktop.TouchUp -= DesktopTouchUp;
				}

				base.Desktop = value;

				if (!ReleaseOnTouchLeft && Desktop != null)
				{
					Desktop.TouchUp += DesktopTouchUp;
				}
			}
		}

		/// <summary>
		/// Gets or sets the content widget displayed inside the button.
		/// </summary>
		[Browsable(false)]
		[Content]
		public override Widget Content
		{
			get => _layout.Child;
			set => _layout.Child = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Button"/> class with the specified style.
		/// </summary>
		/// <param name="styleName">The name of the style to apply. Defaults to the default stylesheet style.</param>
		public Button(string styleName = Stylesheet.DefaultStyleName)
		{
			_layout = new SingleItemLayout<Widget>(this);
			ChildrenLayout = _layout;
			ReleaseOnTouchLeft = true;

			SetStyle(styleName);
		}

		/// <summary>
		/// Handles the event when the cursor/touch leaves the button.
		/// </summary>
		public override void OnTouchLeft()
		{
			base.OnTouchLeft();

			if (ReleaseOnTouchLeft)
			{
				SetValueByUser(false);
			}
		}

		/// <summary>
		/// Called when a touch point is released on the button.
		/// </summary>
		protected override void InternalOnTouchUp()
		{
			SetValueByUser(false);
		}

		/// <summary>
		/// Called when a touch point is pressed on the button.
		/// </summary>
		protected override void InternalOnTouchDown()
		{
			SetValueByUser(true);
		}

		/// <summary>
		/// Handles keyboard input for the button, simulating a click when Space is pressed.
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
				// Emulate click
				DoClick();
			}
		}

		private void DesktopTouchUp(object sender, MyraEventArgs args)
		{
			IsPressed = false;
		}

		/// <summary>
		/// Applies a named button style from the stylesheet to the button.
		/// </summary>
		/// <param name="stylesheet">The stylesheet containing the style.</param>
		/// <param name="name">The name of the button style to apply.</param>
		protected override void InternalSetStyle(Stylesheet stylesheet, string name)
		{
			ApplyButtonStyle(stylesheet.ButtonStyles.SafelyGetStyle(name));
		}

		/// <summary>
		/// Creates a button with a text label as its content.
		/// </summary>
		/// <param name="text">The text to display on the button.</param>
		/// <returns>A new Button with a text label.</returns>
		public static Button CreateTextButton(string text)
		{
			return new Button
			{
				Content = new Label
				{
					Text = text
				}
			};
		}
	}
}