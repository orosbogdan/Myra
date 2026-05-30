using Myra.Graphics2D.UI.Styles;

namespace Myra.Graphics2D.UI
{
	/// <summary>
	/// A split pane container that divides space vertically between two child widgets with a resizable separator.
	/// </summary>
	public class VerticalSplitPane : SplitPane
	{
		/// <summary>
		/// Gets the orientation of the split pane, which is always vertical.
		/// </summary>
		public override Orientation Orientation
		{
			get
			{
				return Orientation.Vertical;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VerticalSplitPane"/> class with the specified style.
		/// </summary>
		/// <param name="styleName">The name of the style to apply. Defaults to the default stylesheet style.</param>
		public VerticalSplitPane(string styleName = Stylesheet.DefaultStyleName) : base(styleName)
		{
		}

		/// <summary>
		/// Applies internal styling to the vertical split pane based on the stylesheet.
		/// </summary>
		/// <param name="stylesheet">The stylesheet to apply.</param>
		/// <param name="name">The name of the style.</param>
		protected override void InternalSetStyle(Stylesheet stylesheet, string name)
		{
			ApplySplitPaneStyle(stylesheet.VerticalSplitPaneStyles.SafelyGetStyle(name));
		}
	}
}