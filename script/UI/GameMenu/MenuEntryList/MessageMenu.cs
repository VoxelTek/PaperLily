using System.Collections.Generic;

namespace LacieEngine.UI
{
	public class MessageMenu : SimpleVerticalMenu
	{
		public MessageMenu(IMenuEntryContainer container)
		{
			base.Container = container;
			base.Entries = new List<IMenuEntry>();
			base.Entries.Add(new BackMenuEntry("system.common.ok", this));
		}
	}
}
