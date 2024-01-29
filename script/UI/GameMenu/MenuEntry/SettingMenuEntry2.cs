using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

namespace LacieEngine.UI
{
	public class SettingMenuEntry2<T, U> : IMenuEntry where T : Setting<U>
	{
		protected Setting<U> setting;

		protected Label nCaptionLabel;

		protected Label nValueLabel;

		public IMenuEntryList Parent { get; set; }

		public SettingMenuEntry2(IMenuEntryList menu)
		{
			setting = Game.Settings.GetSetting<U, T>();
			Parent = menu;
		}

		public virtual void Left()
		{
			if (!setting.OwnSound)
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
			}
			setting.Decrement();
			setting.Apply();
			Parent.Update();
		}

		public virtual void Right()
		{
			if (!setting.OwnSound)
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
			}
			setting.Increment();
			setting.Apply();
			Parent.Update();
		}

		public virtual Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			marginContainer.SetContainerMarginLeft(10);
			marginContainer.SetContainerMarginRight(10);
			nCaptionLabel = GDUtil.MakeNode<Label>("Label");
			nCaptionLabel.Text = setting.CaptionLabel();
			nCaptionLabel.Align = Label.AlignEnum.Left;
			nCaptionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
			nCaptionLabel.SetDefaultFontAndColor();
			nValueLabel = GDUtil.MakeNode<Label>("Value");
			nValueLabel.Text = setting.ValueLabel();
			nValueLabel.Align = Label.AlignEnum.Right;
			nValueLabel.SetDefaultFontAndColor();
			marginContainer.AddChild(nCaptionLabel);
			marginContainer.AddChild(nValueLabel);
			return marginContainer;
		}

		public void Update()
		{
			nCaptionLabel.Text = setting.CaptionLabel();
			nValueLabel.Text = setting.ValueLabel();
		}
	}
}
