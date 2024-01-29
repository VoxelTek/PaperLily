using System;
using Godot;

namespace LacieEngine.UI
{
	public interface IMenuEntryContainer
	{
		Frame Control { get; }

		IMenuEntryList Menu { get; set; }

		Action OnClose { get; set; }

		void Clear()
		{
			Control.ClearFrame();
		}

		void AddToFrame(Control child)
		{
			Control.AddToFrame(child);
		}
	}
}
