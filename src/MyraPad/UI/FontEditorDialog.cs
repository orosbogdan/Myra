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

			_buttomSetFromStylesheet.Click += _buttomSetFromStylesheet_Click;
		}

		private void _buttomSetFromStylesheet_Click(object sender, MyraEventArgs e)
		{
			var dlg = new ChooseFontDialog();

			dlg.Closed += (s, a) =>
			{
				if (!dlg.Result)
				{
					return;
				}

				Font = dlg.Font.Font;
			};

			dlg.ShowModal(Desktop);
		}
	}
}