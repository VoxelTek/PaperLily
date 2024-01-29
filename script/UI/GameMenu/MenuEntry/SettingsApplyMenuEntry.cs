using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SettingsApplyMenuEntry : IMenuEntry
	{
		public IMenuEntryList Parent { get; set; }

		public SettingsApplyMenuEntry(IMenuEntryList menu)
		{
			Parent = menu;
		}

		public void Accept()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			Game.Settings.SaveSettings();
			Parent.Back();
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			Label label = new Label();
			label.Name = "Label";
			label.Text = "system.common.done";
			label.Align = Label.AlignEnum.Center;
			label.SetAnchorsPreset(Control.LayoutPreset.Wide);
			label.SetDefaultFontAndColor();
			marginContainer.AddChild(label);
			return marginContainer;
		}
	}
}
