using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class BackMenuEntry : IMenuEntry
	{
		private string caption;

		public IMenuEntryList Parent { get; set; }

		public BackMenuEntry(string caption, IMenuEntryList parentMenu)
		{
			this.caption = caption;
			Parent = parentMenu;
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			Label label = new Label();
			label.Name = "Label";
			label.Text = caption;
			label.Align = Label.AlignEnum.Center;
			label.SetAnchorsPreset(Control.LayoutPreset.Wide);
			label.SetDefaultFontAndColor();
			marginContainer.AddChild(label);
			return marginContainer;
		}

		public void Accept()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
			Parent.Back();
		}
	}
}
