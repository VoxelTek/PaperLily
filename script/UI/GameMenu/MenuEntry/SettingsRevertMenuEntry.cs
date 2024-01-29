using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SettingsRevertMenuEntry : IMenuEntry
	{
		public IMenuEntryList Parent { get; set; }

		public SettingsRevertMenuEntry(IMenuEntryList menu)
		{
			Parent = menu;
		}

		public void Accept()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			if (GDUtil.FileExists(Game.Settings.SettingsFilename))
			{
				new Directory().Remove(Game.Settings.SettingsFilename);
			}
			Game.Settings.RevertSettings();
			Parent.Back();
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			Label label = new Label();
			label.Name = "Label";
			label.Text = "system.settings.revertall";
			label.Align = Label.AlignEnum.Center;
			label.SetAnchorsPreset(Control.LayoutPreset.Wide);
			label.SetDefaultFontAndColor();
			marginContainer.AddChild(label);
			return marginContainer;
		}
	}
}
