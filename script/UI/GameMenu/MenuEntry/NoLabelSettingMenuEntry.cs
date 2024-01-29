using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

namespace LacieEngine.UI
{
	public class NoLabelSettingMenuEntry<T, U> : IMenuEntry where T : Setting<U>, new()
	{
		private Setting<U> setting;

		private Label valueLabel;

		public IMenuEntryList Parent { get; set; }

		public NoLabelSettingMenuEntry(IMenuEntryList menu)
		{
			setting = new T();
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
			valueLabel = GDUtil.MakeNode<Label>("Value");
			valueLabel.Text = setting.ValueLabel();
			valueLabel.Align = Label.AlignEnum.Center;
			valueLabel.SetDefaultFontAndColor();
			marginContainer.AddChild(valueLabel);
			return marginContainer;
		}

		public void Update()
		{
			valueLabel.Text = setting.ValueLabel();
		}
	}
}
