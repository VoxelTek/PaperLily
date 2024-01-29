using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SubMenuEntry : SimpleVerticalMenu, IMenuEntry
	{
		private string _caption;

		public SubMenuEntry(string caption, IMenuEntryList parentMenu, IMenuEntryContainer container)
		{
			_caption = caption;
			base.Parent = parentMenu;
			base.Container = container;
			base.Entries = new List<IMenuEntry>();
		}

		public void Accept()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
			((IMenuEntryList)this).Root();
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			Label label = new Label();
			label.Name = "Label";
			label.Text = _caption;
			label.Align = Label.AlignEnum.Center;
			label.SetAnchorsPreset(Control.LayoutPreset.Wide);
			label.SetDefaultFontAndColor();
			marginContainer.AddChild(label);
			return marginContainer;
		}

		public void AddEntry(IMenuEntry entry)
		{
			entry.Parent = this;
			base.Entries.Add(entry);
		}
	}
}
