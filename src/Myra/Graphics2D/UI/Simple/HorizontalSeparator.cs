using Myra.Graphics2D.UI.Styles;
using System.ComponentModel;

namespace Myra.Graphics2D.UI
{
	/// <summary>
	/// A horizontal separator widget that visually divides UI sections.
	/// </summary>
	public class HorizontalSeparator : SeparatorWidget
	{
		/// <summary>
		/// Gets or sets the horizontal alignment of the separator.
		/// </summary>
		[DefaultValue(HorizontalAlignment.Stretch)]
		public override HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return base.HorizontalAlignment;
			}
			set
			{
				base.HorizontalAlignment = value;
			}
		}

		/// <summary>
		/// Gets or sets the vertical alignment of the separator.
		/// </summary>
		[DefaultValue(VerticalAlignment.Center)]
		public override VerticalAlignment VerticalAlignment
		{
			get
			{
				return base.VerticalAlignment;
			}
			set
			{
				base.VerticalAlignment = value;
			}
		}

		/// <summary>
		/// Gets the orientation of the separator, which is always horizontal.
		/// </summary>
		public override Orientation Orientation => Orientation.Horizontal;

		/// <summary>
		/// Initializes a new instance of the <see cref="HorizontalSeparator"/> class with the specified style.
		/// </summary>
		/// <param name="styleName">The name of the style to apply. Defaults to the default stylesheet style.</param>
		public HorizontalSeparator(string styleName = Stylesheet.DefaultStyleName) : base(styleName)
		{
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Center;
		}


		/// <summary>
		/// Applies a named horizontal separator style from the stylesheet to the horizontal separator.
		/// </summary>
		/// <param name="stylesheet">The stylesheet containing the style.</param>
		/// <param name="name">The name of the horizontal separator style to apply.</param>
		protected override void InternalSetStyle(Stylesheet stylesheet, string name)
		{
			ApplySeparatorStyle(stylesheet.HorizontalSeparatorStyles.SafelyGetStyle(name));
		}
	}
}