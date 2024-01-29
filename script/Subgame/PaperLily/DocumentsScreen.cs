using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Rooms
{
	public class DocumentsScreen : Control
	{
		[GetNode("Center")]
		private Control nContainer;

		public override void _Ready()
		{
			Game.InputProcessor = Inputs.Processor.None;
			List<IMenuEntry> options = new List<IMenuEntry>();
			foreach (IItem item in Game.Items.GetOwnedItems())
			{
				if (item.Tags.Contains("document"))
				{
					options.Add(new SimpleMenuEntry(item.Name, delegate
					{
						ExecuteEvent(item.Event);
					}, null));
				}
			}
			SimpleChoicesMenuContainer documentsMenu = new SimpleChoicesMenuContainer(options);
			documentsMenu.OnClose = delegate
			{
				Close();
			};
			nContainer.AddChild(documentsMenu);
			Game.InputProcessor = Inputs.Processor.Menu;
		}

		private void ExecuteEvent(string @event)
		{
			this.Delete();
			Game.Events.PlayEvent(@event);
		}

		private void Close()
		{
			this.Delete();
			Game.Core.AssignInputProcessor();
		}
	}
}
