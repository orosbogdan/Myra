using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System.Collections.Generic;

namespace MyraPad.UI
{
	public partial class ChooseTextureRegionDialog
	{
		private readonly Dictionary<int, string> _keys = new Dictionary<int, string>();

		private Stylesheet Stylesheet => Studio.Instance.Project.Stylesheet;

		public TextureRegion Image => Stylesheet.Atlas.Regions[_keys[_gridData.SelectedRowIndex.Value]];


		public ChooseTextureRegionDialog()
		{
			BuildUI();

			RebuildList();

			_textFilter.TextChanged += (s, a) => RebuildList();
			_gridData.SelectedIndexChanged += (s, a) => UpdateEnabled();

			UpdateEnabled();
		}

		private void RebuildList()
		{
			_gridData.Widgets.Clear();
			_keys.Clear();

			var row = 0;
			foreach (var pair in Stylesheet.Atlas.Regions)
			{
				var region = pair.Value;

				if (!string.IsNullOrEmpty(_textFilter.Text) && !region.Name.Contains(_textFilter.Text))
				{
					continue;
				}

				var image = new Image
				{
					Width = 32,
					Height = 32,
					Renderable = region
				};

				Grid.SetRow(image, row);
				_gridData.Widgets.Add(image);

				var labelSize = new Label
				{
					Text = $"{region.Size.X}x{region.Size.Y}",
					VerticalAlignment = VerticalAlignment.Center
				};

				Grid.SetColumn(labelSize, 1);
				Grid.SetRow(labelSize, row);
				_gridData.Widgets.Add(labelSize);

				var checkNinePatch = new CheckButton
				{
					IsChecked = region is NinePatchRegion,
					Enabled = false,
					VerticalAlignment = VerticalAlignment.Center
				};

				Grid.SetColumn(checkNinePatch, 2);
				Grid.SetRow(checkNinePatch, row);
				_gridData.Widgets.Add(checkNinePatch);

				var labelName = new Label
				{
					Text = region.Name,
					VerticalAlignment = VerticalAlignment.Center
				};

				Grid.SetColumn(labelName, 3);
				Grid.SetRow(labelName, row);
				_gridData.Widgets.Add(labelName);

				_keys[row] = pair.Key;

				++row;
			}
		}

		private void UpdateEnabled()
		{
			ButtonOk.Enabled = _gridData.SelectedRowIndex != null;
		}
	}
}