using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI.ColorPicker;
using Myra.MML;
using System;

namespace MyraPad.UI
{
	public partial class ImageEditorDialog
	{
		private IBrush _image;

		public IBrush Image
		{
			get => _image;

			set
			{
				_image = value;
				OnImageSet();
			}
		}

		private Color Color
		{
			get
			{
				var color = Color.Transparent;
				var hasColor = _image as IHasColor;
				if (hasColor != null)
				{
					color = hasColor.Color;
				}

				return color;
			}
		}


		public ImageEditorDialog()
		{
			BuildUI();

			_buttonChangeColor.Click += _buttonChangeColor_Click;
			_buttomSetFromStylesheet.Click += _buttomSetFromStylesheet_Click;
		}

		private void _buttomSetFromStylesheet_Click(object sender, MyraEventArgs e)
		{
			var dlg = new ChooseTextureRegionDialog();

			dlg.Closed += (s, a) =>
			{
				if (!dlg.Result)
				{
					return;
				}
			};

			dlg.ShowModal(Desktop);
		}

		private void _buttonChangeColor_Click(object sender, MyraEventArgs e)
		{
			var dlg = new ColorPickerDialog()
			{
				Color = Color
			};

			dlg.Closed += (s, a) =>
			{
				if (!dlg.Result)
				{
					return;
				}

				do
				{
					if (_image == null)
					{
						Image = new SolidBrush(dlg.Color);
						break;
					}

					var asSolidBrush = _image as SolidBrush;
					if (asSolidBrush != null)
					{
						Image = new SolidBrush(dlg.Color);
						break;
					}

					var asTextureRegion = _image as TextureRegion;
					if (asTextureRegion != null)
					{
						Image = new TintedImage(asTextureRegion, dlg.Color);
						break;
					}

					var asTinted = _image as TintedImage;
					if (asTinted != null)
					{
						Image = new TintedImage(asTinted.Region, dlg.Color);
					}

					throw new Exception($"Could not set Color for type {_image.GetType().Name}");
				} while (false);
			};

			dlg.ShowModal(Desktop);
		}

		private void OnImageSet()
		{
			if (_image != null)
			{
				_textValue.Text = _image.ToString();
			}
			else
			{
				_textValue.Text = string.Empty;
			}

			_panelColor.Background = new SolidBrush(Color);
		}
	}
}