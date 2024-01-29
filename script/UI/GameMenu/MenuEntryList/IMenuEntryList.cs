using System;
using System.Collections.Generic;
using Godot;

namespace LacieEngine.UI
{
	public interface IMenuEntryList
	{
		IMenuEntryContainer Container { get; set; }

		IMenuEntryList Parent { get; set; }

		int Selection { get; set; }

		List<IMenuEntry> Entries { get; }

		Action OnBack { get; set; }

		Control DrawContent();

		void Update()
		{
			foreach (IMenuEntry entry in Entries)
			{
				entry.Update();
			}
		}

		void Back();

		void Root()
		{
			Container.Clear();
			Container.AddToFrame(DrawContent());
			Container.Menu = this;
			ResetSelection();
		}

		void HandleInput(InputEvent @event);

		void HighlightSelection();

		void ResetSelection()
		{
			Selection = 0;
			HighlightSelection();
		}
	}
}
