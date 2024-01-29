using Godot;

namespace LacieEngine.UI
{
	public interface IMenuEntry
	{
		IMenuEntryList Parent { get; set; }

		void Accept()
		{
		}

		void Left()
		{
		}

		void Right()
		{
		}

		void Cancel()
		{
			Parent.Back();
		}

		void Update()
		{
		}

		Control DrawEntry();
	}
}
