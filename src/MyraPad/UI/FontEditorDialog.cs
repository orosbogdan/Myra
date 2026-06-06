using FontStashSharp;

namespace MyraPad.UI
{
	public partial class FontEditorDialog
	{
		public SpriteFontBase Font { get; set; }

		public FontEditorDialog()
		{
			BuildUI();
		}
	}
}