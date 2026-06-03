using Myra.Utility;
using System;
using Myra.MML;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using System.Drawing;
using Color = FontStashSharp.FSColor;
#endif

namespace Myra.Graphics2D.TextureAtlases
{
	public class TintedImage : IImage, IHasColor
	{
		internal const char Separator = '/';

		public TextureRegion Region { get; }
		public Color Color { get; }

		public Point Size => Region.Size;

		public TintedImage(TextureRegion image, Color color)
		{
			Region = image ?? throw new ArgumentNullException(nameof(image));
			Color = color;
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

			Region.Draw(context, dest, color);
		}

		public override bool Equals(object obj)
		{
			var asTinted = obj as TintedImage;
			if (asTinted == null)
			{
				return false;
			}

			return Region.Equals(asTinted.Region) && Color == asTinted.Color;
		}

		public override string ToString()
		{
			if (Color == Color.White)
			{
				return Region.ToString();
			}

			return Region.ToString() + Separator + Color.ToColorString();
		}
	}
}
