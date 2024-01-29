using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestArtist : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Texture texKettMad;

		[GetNode("Main/MiscPot")]
		private Node2D nPot;

		[GetNode("Other/Events/misc_pot")]
		private EventTrigger nPotEvent;

		private PVar varTookPot = "ch1.forest_took_pot";

		public override void _UpdateRoom()
		{
			nPotEvent.Enabled = !varTookPot.Value;
			nPot.Visible = !varTookPot.Value;
		}

		public void KettMad()
		{
			WalkingCharacter obj = Game.Room.RegisteredNPCs["kett"] as WalkingCharacter;
			obj.OverrideTextureForState("stand", texKettMad);
			obj.OverrideTextureForState("walk", texKettMad);
		}
	}
}
