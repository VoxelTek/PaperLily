using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestLakeside2 : GameRoom
	{
		[GetNode("Ground/Paddle")]
		private Node2D nPaddle;

		[GetNode("Background/FaucetOn")]
		private Node2D nFaucetOn;

		[GetNode("Background/FaucetOff")]
		private Node2D nFaucetOff;

		private PVar varTookPaddle = "ch1.forest_lakeside_took_paddle_temple";

		private PVar varFaucetOn = "ch1.forest_lakeside_faucet_on";

		private PVar varEnding8 = "ch1.temp_ending_1_8";

		private PVar varCorrectPetals = "ch1.forest_lakeside_correct_petals";

		private PVar varCorrectAlgae = "ch1.forest_lakeside_correct_algae";

		private PVar varSeenLakeside3 = "persistent.ch1_seen_lakeside_3";

		public override void _BeforeFadeIn()
		{
			if (!varCorrectPetals.Value)
			{
				int r = new Random().Next(varSeenLakeside3.Value ? 3 : 2);
				PVar pVar = varCorrectPetals;
				pVar.NewValue = r switch
				{
					0 => "pink", 
					1 => "purple", 
					2 => "white", 
					_ => "pink", 
				};
			}
			if (!varCorrectAlgae.Value)
			{
				int r2 = new Random().Next(3);
				PVar pVar = varCorrectAlgae;
				pVar.NewValue = r2 switch
				{
					0 => "green", 
					1 => "blue", 
					2 => "black", 
					_ => "green", 
				};
			}
		}

		public override void _UpdateRoom()
		{
			nPaddle.Visible = !varEnding8.Value && !varTookPaddle.Value;
			nFaucetOn.Visible = !varEnding8.Value && (bool)varFaucetOn.Value;
			nFaucetOff.Visible = !nFaucetOn.Visible;
		}
	}
}
