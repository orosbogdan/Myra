namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Style class that defines the visual appearance of image-text button widgets (buttons with both icon and text).
	/// </summary>
	public class ImageTextButtonStyle : ButtonStyle
	{
		/// <summary>
		/// Gets or sets the style applied to the button's image display.
		/// </summary>
		public PressableImageStyle ImageStyle
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the spacing in pixels between the button's image and text.
		/// </summary>
		public int ImageTextSpacing
		{
			get; set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageTextButtonStyle"/> class.
		/// </summary>
		public ImageTextButtonStyle()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageTextButtonStyle"/> class by copying properties from another style.
		/// </summary>
		/// <param name="style">The source image-text button style to copy from.</param>
		public ImageTextButtonStyle(ImageTextButtonStyle style) : base(style)
		{
			ImageStyle = style.ImageStyle != null ? new PressableImageStyle(style.ImageStyle) : null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageTextButtonStyle"/> class by copying properties from a button style.
		/// </summary>
		/// <param name="buttonStyle">The base button style to copy from.</param>
		public ImageTextButtonStyle(ButtonStyle buttonStyle) : base(buttonStyle)
		{
		}

		/// <summary>
		/// Creates a deep copy of this image-text button style.
		/// </summary>
		/// <returns>A new ImageTextButtonStyle instance with the same properties.</returns>
		public override WidgetStyle Clone()
		{
			return new ImageTextButtonStyle(this);
		}
	}
}