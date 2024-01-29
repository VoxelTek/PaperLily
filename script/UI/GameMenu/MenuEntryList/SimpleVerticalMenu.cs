using System;
using System.Collections.Generic;
using Godot;

namespace LacieEngine.UI
{
	public abstract class SimpleVerticalMenu : IMenuEntryList
	{
		protected List<ColorRect> _selectBgs;

		public IMenuEntryContainer Container { get; set; }

		public IMenuEntryList Parent { get; set; }

		public List<IMenuEntry> Entries { get; protected set; }

		public int Selection { get; set; }

		public Action OnBack { get; set; }

		public virtual Control DrawContent()
		{
			return UIUtil.CreateVerticalEntryList(Entries, out _selectBgs);
		}

		public virtual void HandleInput(InputEvent @event)
		{
			UIUtil.HandleVerticalNavigationInput(this, @event);
		}

		public virtual void HighlightSelection()
		{
			foreach (ColorRect selectBg in _selectBgs)
			{
				selectBg.Color = Colors.Transparent;
			}
			if (Selection > -1)
			{
				_selectBgs[Selection].Color = UIUtil.SelectionColor;
			}
		}

		public void Root()
		{
			Container.Clear();
			Container.AddToFrame(DrawContent());
			Container.Menu = this;
			ResetSelection();
		}

		public virtual void Back()
		{
			if (OnBack != null)
			{
				OnBack();
			}
			else
			{
				Parent?.Root();
			}
		}

		public void ResetSelection()
		{
			Selection = 0;
			HighlightSelection();
		}
	}
}
