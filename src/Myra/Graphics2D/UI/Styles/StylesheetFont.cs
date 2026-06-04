using FontStashSharp;
using Myra.Attributes;
using Myra.MML;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Myra.Graphics2D.UI.Styles
{
	/// <summary>
	/// Represents a font definition within a stylesheet, including file path and optional size.
	/// </summary>
	[XmlName("Font")]
	public class StylesheetFont : IItemWithId
	{
		private SpriteFontBase _font;

		/// <summary>
		/// The character used to separate font file names from size values in font keys.
		/// </summary>
		public const char Separator = ':';

		/// <summary>
		/// Gets or sets the identifier for this font.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the file path to the font file.
		/// </summary>
		public string File { get; set; }

		/// <summary>
		/// Gets or sets the font size (only used for TTF and OTF fonts).
		/// </summary>
		[DefaultValue(0)]
		public int Size { get; set; }

		/// <summary>
		/// Gets or sets the loaded sprite font instance.
		/// </summary>
		[XmlIgnore]
		public SpriteFontBase Font
		{
			get => _font;

			internal set
			{
				_font = value ?? throw new ArgumentNullException(nameof(value));
				_font.Name = Id;
			}
		}

		/// <summary>
		/// Validates that all required properties are set and valid for the font type.
		/// </summary>
		/// <exception cref="Exception">Thrown when required properties are missing or invalid.</exception>
		public void Validate()
		{
			if (string.IsNullOrEmpty(Id))
			{
				throw new Exception("Font Id is required");
			}
			if (string.IsNullOrEmpty(File))
			{
				throw new Exception($"Font '{Id}' File is required");
			}

			if (File.EndsWith(".ttf", StringComparison.InvariantCultureIgnoreCase) || File.EndsWith(".otf", StringComparison.InvariantCultureIgnoreCase))
			{
				if (Size == 0)
				{
					throw new Exception($"Font '{Id}' Size is required for TTF/OTF fonts");
				}
			}
		}

		/// <summary>
		/// Creates a deep copy of this stylesheet font.
		/// </summary>
		/// <returns>A new StylesheetFont instance with the same properties.</returns>
		public StylesheetFont Clone() => new StylesheetFont
		{
			Id = Id,
			File = File,
			Size = Size,
			Font = Font
		};

		/// <summary>
		/// Builds a font file key combining the file name and size.
		/// </summary>
		/// <returns>The font file key for caching and lookup.</returns>
		public string BuildFontFileKey()
		{
			if (Size == 0)
			{
				return File;
			}

			return File + Separator + Size;
		}

		/// <summary>
		/// Returns the font identifier.
		/// </summary>
		/// <returns>The font ID.</returns>
		public override string ToString() => Id;
	}
}
