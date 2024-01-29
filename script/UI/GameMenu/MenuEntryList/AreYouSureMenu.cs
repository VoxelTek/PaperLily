using System;
using System.Collections.Generic;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class AreYouSureMenu : SimpleVerticalMenu
	{
		private Action _yesAction;

		public AreYouSureMenu(Action yesAction, IMenuEntryContainer container)
		{
			base.Container = container;
			_yesAction = yesAction;
			base.Entries = new List<IMenuEntry>();
			base.Entries.Add(new SimpleMenuEntry("system.common.yes", delegate
			{
				Yes();
			}, this));
			base.Entries.Add(new SimpleMenuEntry("system.common.no", delegate
			{
				No();
			}, this));
		}

		private void Yes()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			_yesAction();
			base.Back();
		}

		private void No()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
			base.Back();
		}
	}
}
