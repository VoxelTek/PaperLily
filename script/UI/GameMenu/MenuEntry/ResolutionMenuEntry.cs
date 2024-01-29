using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

namespace LacieEngine.UI
{
	public class ResolutionMenuEntry : SettingMenuEntry<ResolutionSetting, Vector2>
	{
		public class ResolutionLabel : Label
		{
			private ResolutionSetting setting;

			public ResolutionLabel(ResolutionSetting setting)
			{
				this.setting = setting;
			}

			public override void _Ready()
			{
				ApplyCurrentResolution();
				GetTree().Root.Connect("size_changed", this, "ApplyCurrentResolution");
			}

			public override void _Process(float delta)
			{
				base.Text = setting.ValueLabel();
			}

			public void ApplyCurrentResolution()
			{
				setting.ApplyCurrentResolution();
			}
		}

		public ResolutionMenuEntry(IMenuEntryList menu)
			: base(menu)
		{
		}

		public override Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			marginContainer.SetContainerMarginLeft(10);
			marginContainer.SetContainerMarginRight(10);
			nCaptionLabel = GDUtil.MakeNode<Label>("Label");
			nCaptionLabel.Text = setting.CaptionLabel();
			nCaptionLabel.Align = Label.AlignEnum.Left;
			nCaptionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
			nCaptionLabel.SetDefaultFontAndColor();
			nValueLabel = new ResolutionLabel((ResolutionSetting)setting);
			nValueLabel.Name = "Value";
			nValueLabel.Text = setting.ValueLabel();
			nValueLabel.Align = Label.AlignEnum.Right;
			nValueLabel.SetDefaultFontAndColor();
			marginContainer.AddChild(nCaptionLabel);
			marginContainer.AddChild(nValueLabel);
			return marginContainer;
		}
	}
}
