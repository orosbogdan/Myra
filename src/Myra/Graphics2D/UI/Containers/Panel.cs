using Myra.Utility;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using System.Drawing;
#endif

namespace Myra.Graphics2D.UI
{
	/// <summary>
	/// A container that arranges all of its children at the same location, allowing them to overlap.
	/// </summary>
	public class Panel : Container
	{
		/// <summary>
		/// Arranges child controls at the same location in the panel bounds.
		/// </summary>
		protected override void InternalArrange()
		{
			foreach (var control in ChildrenCopy)
			{
				if (!control.Visible)
				{
					continue;
				}

				LayoutControl(control);
			}
		}

		private void LayoutControl(Widget control)
		{
			control.Arrange(ActualBounds);
		}

		/// <summary>
		/// Measures the size required for all child controls in the panel.
		/// </summary>
		/// <param name="availableSize">The available size for measurement.</param>
		/// <returns>The largest size required by any child control.</returns>
		protected override Point InternalMeasure(Point availableSize)
		{
			Point result = Mathematics.PointZero;

			foreach (var control in ChildrenCopy)
			{
				if (!control.Visible)
				{
					continue;
				}

				Point measure = control.Measure(availableSize);

				if (measure.X > result.X)
				{
					result.X = measure.X;
				}

				if (measure.Y > result.Y)
				{
					result.Y = measure.Y;
				}
			}

			return result;
		}
	}
}