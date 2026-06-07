using AssetManagementBase;
using FontStashSharp;
using Myra.Graphics2D.UI.Styles;
using System;

namespace MyraPad.UI
{
	public partial class FontEditorDialog
	{
		private SpriteFontBase _font;

		public SpriteFontBase Font
		{
			get
			{
				var dynamicFont = _font as DynamicSpriteFont;
				if (dynamicFont == null)
				{
					return _font;
				}

				if (FontSize == DefaultFontSize)
				{
					return _font;
				}

				var assetName = _font.Name;
				string parameter;
				MyraAssetManagerExtensions.TryGetParameter(ref assetName, out parameter);

				var project = Studio.MainForm.Project;
				return Studio.AssetManager.LoadFont($"{assetName}{StylesheetFont.Separator}{FontSize}", project.Stylesheet);
			}

			set
			{
				_font = value;

				UpdateFont();
			}
		}

		private int FontSize => (int)Math.Round(_spinSize.Value.Value);

		private int DefaultFontSize => (int)Math.Round(_font.FontSize);

		public FontEditorDialog()
		{
			BuildUI();

			_buttomSetFromStylesheet.Click += _buttomSetFromStylesheet_Click;
			_spinSize.ValueChangedByUser += _spinSize_ValueChanged;

			UpdateFont();
		}

		private void UpdateFont()
		{
			if (_font == null)
			{
				_textValue.Text = string.Empty;
				_spinSize.TextBox.Text = string.Empty;
				_spinSize.Enabled = false;
			}
			else
			{
				_textValue.Text = _font.ToString();
				_spinSize.Value = DefaultFontSize;
				_spinSize.Enabled = _font is DynamicSpriteFont;
			}
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

		private void _spinSize_ValueChanged(object sender, ValueChangedEventArgs<float?> e)
		{
			_textValue.Text = Font.ToString();
		}
	}
}