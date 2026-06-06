using FontStashSharp;

namespace MyraPad.UI
{
	public partial class FontEditorDialog
	{
		private SpriteFontBase _font;

		public SpriteFontBase Font
		{
			get => _font;

			set
			{
				_font = value;

				if (value == null)
				{
					_textValue.Text = string.Empty;
				}
				else
				{
					_textValue.Text = value.ToString();
				}
			}
		}

		public FontEditorDialog()
		{
			BuildUI();
		}
	}
}