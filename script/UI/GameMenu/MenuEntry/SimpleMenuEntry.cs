using System;
using Godot;

namespace LacieEngine.UI
{
	public class SimpleMenuEntry : IMenuEntry
	{
		private string _caption;

		private Action _action;

		public IMenuEntryList Parent { get; set; }

		public SimpleMenuEntry(string caption, Action action, IMenuEntryList parent)
		{
			Parent = parent;
			_caption = caption;
			_action = action;
		}

		public Control DrawEntry()
		{
			return UIUtil.CreateSimpleEntry(_caption);
		}

		public void Accept()
		{
			_action();
		}
	}
}
