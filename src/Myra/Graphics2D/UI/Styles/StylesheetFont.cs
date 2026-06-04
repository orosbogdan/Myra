using FontStashSharp;
using Myra.Attributes;
using Myra.MML;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Myra.Graphics2D.UI.Styles
{
	[XmlName("Font")]
	public class StylesheetFont : IItemWithId
	{
		private SpriteFontBase _font;

		public const char Separator = ':';

		public string Id { get; set; }

		public string File { get; set; }

		[DefaultValue(0)]
		public int Size { get; set; }

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

		public StylesheetFont Clone() => new StylesheetFont
		{
			Id = Id,
			File = File,
			Size = Size,
			Font = Font
		};

		public string BuildFontFileKey()
		{
			if (Size == 0)
			{
				return File;
			}

			return File + Separator + Size;
		}

		public override string ToString() => Id;
	}
}
