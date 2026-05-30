namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Style class that defines the visual appearance of image widgets.
	/// </summary>
	public class ImageStyle: WidgetStyle
	{
		/// <summary>
		/// Gets or sets the image displayed by the image widget.
		/// </summary>
		public IImage Image { get; set; }

		/// <summary>
		/// Gets or sets the image displayed when the mouse is over the widget, or null to use the default image.
		/// </summary>
		public IImage OverImage { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageStyle"/> class.
		/// </summary>
		public ImageStyle()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageStyle"/> class by copying properties from another style.
		/// </summary>
		/// <param name="style">The source image style to copy from.</param>
		public ImageStyle(ImageStyle style): base(style)
		{
			Image = style.Image;
			OverImage = style.OverImage;
		}

		/// <summary>
		/// Creates a deep copy of this image style.
		/// </summary>
		/// <returns>A new ImageStyle instance with the same properties.</returns>
		public override WidgetStyle Clone()
		{
			return new ImageStyle(this);
		}
	}
}
