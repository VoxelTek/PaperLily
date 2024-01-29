using Godot;

namespace LacieEngine.UI
{
	public class ObjectivesMenuEmptyEntry : IMenuEntry
	{
		public IMenuEntryList Parent { get; set; }

		public ObjectivesMenuEmptyEntry(IMenuEntryList parentMenu)
		{
			Parent = parentMenu;
		}

		public Control DrawEntry()
		{
			return UIUtil.CreateSimpleEntry("system.menu.objectives.empty");
		}
	}
}
