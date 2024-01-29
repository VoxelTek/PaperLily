using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestPath1 : GameRoom
	{
		[GetNode("Main/StructSignpostBlue")]
		private Sprite nSignpost;

		[GetNode("TilesBlock1")]
		private TileMap nTilesNorthBlock1;

		[GetNode("TilesBlock2")]
		private TileMap nTilesNorthBlock2;

		public override void _BeforeFadeIn()
		{
			Game.Variables.SetInitialValue("ch1.forest_path_1_sign_direction", "left");
		}

		public override void _UpdateRoom()
		{
			UpdateSignDirection(nSignpost, "ch1.forest_path_1_sign_direction");
			if (Game.Variables.GetFlag("ch1.forest_path_1_opened_exit"))
			{
				nTilesNorthBlock1.Visible = false;
				nTilesNorthBlock1.CollisionLayer = 0u;
				nTilesNorthBlock2.Visible = false;
				nTilesNorthBlock2.CollisionLayer = 0u;
			}
			else
			{
				nTilesNorthBlock1.Visible = true;
				nTilesNorthBlock1.CollisionLayer = 2u;
				nTilesNorthBlock2.Visible = true;
				nTilesNorthBlock2.CollisionLayer = 2u;
			}
		}

		public void RotateSign()
		{
			RotateSignVariable("ch1.forest_path_1_sign_direction");
		}

		private void RotateSignVariable(string variable)
		{
			string newDirection = Game.Variables.GetVariable(variable) switch
			{
				"left" => "up", 
				"right" => "down", 
				"up" => "right", 
				"down" => "left", 
				_ => throw new InvalidOperationException(), 
			};
			Game.Variables.SetVariable(variable, newDirection);
			UpdateSignSolutions();
		}

		private void UpdateSignDirection(Sprite sign, string variable)
		{
			sign.Frame = Game.Variables.GetVariable(variable) switch
			{
				"left" => 0, 
				"up" => 1, 
				"right" => 2, 
				"down" => 3, 
				_ => throw new InvalidOperationException(), 
			};
		}

		private void UpdateSignSolutions()
		{
			Game.Variables.SetFlag("ch1.forest_path_1_opened_exit", Game.Variables.GetVariable("ch1.forest_path_1_sign_direction") == "up");
		}
	}
}
