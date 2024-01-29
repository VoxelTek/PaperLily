using Godot;
using LacieEngine.UI;

namespace LacieEngine.Core
{
	public class UiLookAndFeelProvider : Resource
	{
		[Export(PropertyHint.None, "")]
		public PackedScene DefaultFrame { private get; set; }

		[Export(PropertyHint.None, "")]
		public PackedScene ChoicesFrame { private get; set; }

		[Export(PropertyHint.None, "")]
		public PackedScene DialogueBox { private get; set; }

		public Frame MakeDefaultFrame()
		{
			return DefaultFrame.Instance<Frame>();
		}

		public Frame MakeChoicesFrame()
		{
			return ChoicesFrame.Instance<Frame>();
		}

		public DialogueFrameNode MakeDialogueBox()
		{
			return DialogueBox.Instance<DialogueFrameNode>();
		}
	}
}
