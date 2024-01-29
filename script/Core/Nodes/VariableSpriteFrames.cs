using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class VariableSpriteFrames : Node, IAction, IToggleable
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public string Variable { get; set; }

		[Export(PropertyHint.None, "")]
		public NodePath Node { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public int FalseFrame { get; set; }

		[Export(PropertyHint.None, "")]
		public int TrueFrame { get; set; } = 1;


		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredRoomUpdates.Add(this);
			}
		}

		public void Execute()
		{
			if (Enabled && !Node.IsNullOrEmpty())
			{
				Node node = GetNode(Node);
				bool @true = Game.Variables.GetFlag(Variable);
				if (node is Sprite sprite)
				{
					sprite.Frame = (@true ? TrueFrame : FalseFrame);
				}
			}
		}
	}
}
