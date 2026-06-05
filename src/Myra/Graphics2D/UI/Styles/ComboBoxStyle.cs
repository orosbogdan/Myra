namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Style class that defines the visual appearance of combo box widgets.
	/// </summary>
	public class ComboBoxStyle : ButtonStyle
	{
		/// <summary>
		/// Gets or sets the style applied to the combo box's dropdown list.
		/// </summary>
		public ListBoxStyle ListBoxStyle { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ComboBoxStyle"/> class.
		/// </summary>
		public ComboBoxStyle()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ComboBoxStyle"/> class by copying properties from another style.
		/// </summary>
		/// <param name="style">The source combo box style to copy from.</param>
		public ComboBoxStyle(ComboBoxStyle style) : base(style)
		{
			ListBoxStyle = style.ListBoxStyle != null ? new ListBoxStyle(style.ListBoxStyle) : null;
		}

		/// <summary>
		/// Creates a deep copy of this combo box style.
		/// </summary>
		/// <returns>A new ComboBoxStyle instance with the same properties.</returns>
		public override WidgetStyle Clone()
		{
			return new ComboBoxStyle(this);
		}
	}
}