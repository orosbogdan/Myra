#if MONOGAME || FNA
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using Color = FontStashSharp.FSColor;
#endif

namespace Myra.Graphics2D.UI.Styles
{
	public class GridStyle : WidgetStyle
	{
		/// <summary>
		/// Gets or sets a value indicating whether grid lines are displayed for debugging purposes.
		/// </summary>
		public bool ShowGridLines { get; set; }

		/// <summary>
		/// Gets or sets the color of the grid lines when displayed.
		/// </summary>
		public Color GridLinesColor { get; set; }

		/// <summary>
		/// Gets or sets the spacing in pixels between grid columns.
		/// </summary>
		public int ColumnSpacing { get; set; }

		/// <summary>
		/// Gets or sets the spacing in pixels between grid rows.
		/// </summary>
		public int RowSpacing { get; set; }

		/// <summary>
		/// Gets or sets the brush used to draw the background of selected rows, columns, or cells.
		/// </summary>
		public IBrush SelectionBackground { get; set; }

		/// <summary>
		/// Gets or sets the brush used to draw the background of rows, columns, or cells being hovered over.
		/// </summary>
		public IBrush SelectionHoverBackground { get; set; }

		/// <summary>
		/// Gets or sets the selection mode for the grid (rows, columns, cells, or none).
		/// </summary>
		public GridSelectionMode GridSelectionMode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the hover index can be null when the mouse is outside the grid.
		/// </summary>
		public bool HoverIndexCanBeNull { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether nothing can be selected by clicking an already-selected item.
		/// </summary>
		public bool CanSelectNothing { get; set; }
	}
}
