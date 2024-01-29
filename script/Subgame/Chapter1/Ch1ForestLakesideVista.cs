using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestLakesideVista : GameRoom
	{
		[GetNode("Foreground/Paddle")]
		private Node2D nPaddle;

		private PVar varTookPaddle = "ch1.forest_lakeside_took_paddle_vista";

		public override void _UpdateRoom()
		{
			nPaddle.Visible = !varTookPaddle.Value;
		}
	}
}
