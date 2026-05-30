namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Style class that defines the visual appearance of image button widgets.
	/// </summary>
	public class ImageButtonStyle: ButtonStyle
	{
		/// <summary>
		/// Gets or sets the style applied to the button's image display.
		/// </summary>
		public PressableImageStyle ImageStyle { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageButtonStyle"/> class.
		/// </summary>
		public ImageButtonStyle()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageButtonStyle"/> class by copying properties from another style.
		/// </summary>
		/// <param name="style">The source image button style to copy from.</param>
		public ImageButtonStyle(ImageButtonStyle style): base(style)
		{
			ImageStyle = style.ImageStyle != null ? new PressableImageStyle(style.ImageStyle) : null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageButtonStyle"/> class by copying properties from a button style.
		/// </summary>
		/// <param name="buttonStyle">The base button style to copy from.</param>
		public ImageButtonStyle(ButtonStyle buttonStyle) : base(buttonStyle)
		{
		}

		/// <summary>
		/// Creates a deep copy of this image button style.
		/// </summary>
		/// <returns>A new ImageButtonStyle instance with the same properties.</returns>
		public override WidgetStyle Clone()
		{
			return new ImageButtonStyle(this);
		}
	}
}
