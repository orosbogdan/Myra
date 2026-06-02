namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Style class for images that can display different images when pressed or hovered.
	/// </summary>
	public class PressableImageStyle : ImageStyle
	{
		/// <summary>
		/// Gets or sets the image displayed when the widget is pressed.
		/// </summary>
		public IImage PressedImage { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PressableImageStyle"/> class.
		/// </summary>
		public PressableImageStyle()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PressableImageStyle"/> class by copying properties from another style.
		/// </summary>
		/// <param name="style">The source pressable image style to copy from.</param>
		public PressableImageStyle(PressableImageStyle style) : base(style)
		{
			PressedImage = style.PressedImage;
		}

		/// <summary>
		/// Creates a deep copy of this pressable image style.
		/// </summary>
		/// <returns>A new PressableImageStyle instance with the same properties.</returns>
		public override WidgetStyle Clone()
		{
			return new PressableImageStyle(this);
		}
	}
}
