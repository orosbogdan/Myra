using Microsoft.Xna.Framework;
using Myra.Utility;
using System;

namespace Myra.Graphics2D.Brushes
{
	public class TintedImage : IImage
	{
		internal const char Separator = '/';

		public IImage Image { get; }
		public Color Color { get; }

		public Point Size => Image.Size;

		public TintedImage(IImage image, Color color)
		{
			Image = image ?? throw new ArgumentNullException(nameof(image));
			Color = color;
		}

		public TintedImage(IImage image) : this(image, Color.White)
		{
		}

		public void Draw(RenderContext context, Rectangle dest, Color color)
		{
			if (Color != Color.White)
			{
				color = new Color((int)(Color.R * color.R / 255.0f),
					(int)(Color.G * color.G / 255.0f),
					(int)(Color.B * color.B / 255.0f),
					(int)(Color.A * color.A / 255.0f));
			}

			Image.Draw(context, dest, color);
		}

		public override bool Equals(object obj)
		{
			var asTinted = obj as TintedImage;
			if (asTinted == null)
			{
				return false;
			}

			return Image.Equals(asTinted.Image) && Color == asTinted.Color;
		}

		public override string ToString()
		{
			if (Color == Color.White)
			{
				return Image.ToString();
			}

			return Image.ToString() + Separator + Color.ToColorString();
		}
	}
}
