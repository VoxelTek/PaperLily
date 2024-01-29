using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

namespace LacieEngine.UI
{
	public class FullScreenMenuEntry : IMenuEntry
	{
		public class FullScreenLabel : Label
		{
			private FullScreenSetting setting;

			public FullScreenLabel(FullScreenSetting setting)
			{
				this.setting = setting;
			}

			public override void _Process(float delta)
			{
				base.Text = setting.ValueLabel();
			}
		}

		private FullScreenSetting setting;

		private Label captionLabel;

		private Label valueLabel;

		public IMenuEntryList Parent { get; set; }

		public FullScreenMenuEntry(IMenuEntryList menu)
		{
			setting = new FullScreenSetting();
			Parent = menu;
		}

		public void Left()
		{
			if (!setting.OwnSound)
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
			}
			setting.Decrement();
			setting.Apply();
			Parent.Update();
		}

		public void Right()
		{
			if (!setting.OwnSound)
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
			}
			setting.Increment();
			setting.Apply();
			Parent.Update();
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			marginContainer.SetContainerMarginLeft(10);
			marginContainer.SetContainerMarginRight(10);
			captionLabel = new Label();
			captionLabel.Name = "Label";
			captionLabel.Text = setting.CaptionLabel();
			captionLabel.Align = Label.AlignEnum.Left;
			captionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
			captionLabel.SetDefaultFontAndColor();
			valueLabel = new FullScreenLabel(setting);
			valueLabel.Name = "Value";
			valueLabel.Text = setting.ValueLabel();
			valueLabel.Align = Label.AlignEnum.Right;
			valueLabel.SetDefaultFontAndColor();
			marginContainer.AddChild(captionLabel);
			marginContainer.AddChild(valueLabel);
			return marginContainer;
		}
	}
}
