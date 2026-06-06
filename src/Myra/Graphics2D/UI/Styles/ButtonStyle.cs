namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Style class that defines the visual appearance of button widgets.
	/// </summary>
	public class ButtonStyle : WidgetStyle
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonStyle"/> class.
		/// </summary>
		public ButtonStyle()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonStyle"/> class by copying properties from another style.
		/// </summary>
		/// <param name="style">The source button style to copy from.</param>
		public ButtonStyle(ButtonStyle style) : base(style)
		{
			PressedBackground = style.PressedBackground;
		}

		/// <summary>
		/// Creates a deep copy of this button style.
		/// </summary>
		/// <returns>A new ButtonStyle instance with the same properties.</returns>
		public override WidgetStyle Clone()
		{
			return new ButtonStyle(this);
		}
	}
}
