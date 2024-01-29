using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestRed : GameRoom
	{
		[Inject]
		private IItemManager Items;

		[Export(PropertyHint.None, "")]
		private PackedScene scnLacieFall;

		[GetNode("Main/chr_rune")]
		private NpcStaticIdleTurnable nRune;

		[GetNode("TilesPath1")]
		private Node2D nPath1Tiles;

		[GetNode("BgPath1")]
		private Node2D nPath1Bg;

		[GetNode("TilesUnderwaterPath1")]
		private Node2D nTilesPath1Underwater;

		[GetNode("TilesPath2")]
		private Node2D nPath2Tiles;

		[GetNode("BgPath2")]
		private Node2D nPath2Bg;

		[GetNode("TilesUnderwaterPath2")]
		private Node2D nTilesPath2Underwater;

		[GetNode("TilesPath3")]
		private Node2D nPath3Tiles;

		[GetNode("BgPath3")]
		private Node2D nPath3Bg;

		[GetNode("TilesUnderwaterPath3")]
		private Node2D nTilesPath3Underwater;

		[GetNode("Main/Lamp3")]
		private Node2D nLamp3;

		[GetNode("Main/Lamp4")]
		private Node2D nLamp4;

		[GetNode("Main/Lamp5")]
		private Node2D nLamp5;

		[GetNode("Main/Lamp6")]
		private Node2D nLamp6;

		[GetNode("Main/Lamp7")]
		private Node2D nLamp7;

		[GetNode("Main/Lamp8")]
		private Node2D nLamp8;

		[GetNode("Main/FurnitureBirdbox")]
		private Sprite nBirdbox;

		[GetNode("Main/FurnitureBirdbox/Crows1")]
		private Particles2D nBirdsEscaping1;

		[GetNode("Main/FurnitureBirdbox/Crows2")]
		private Particles2D nBirdsEscaping2;

		[GetNode("Main/FurnitureBirdbox/Crow")]
		private Node2D nSaveCrow;

		[GetNode("Lighting/LightPath1a")]
		private Node2D nLight1a;

		[GetNode("Lighting/LightPath1b")]
		private Node2D nLight1b;

		[GetNode("Lighting/LightPath2")]
		private Node2D nLight2;

		[GetNode("Lighting/LightPath3")]
		private Node2D nLight3;

		private PVar varBirdboxOpen = "ch1.forest_red_birdbox_open";

		private PVar varHasRunes = "ch1.temp_player_has_runes";

		private PVar varDeathReaction = "ch1.temp_rune_death_reaction";

		private PVar varRuneAllTalks = "ch1.forest_rune_alltalks";

		public override void _UpdateRoom()
		{
			switch (Game.Variables.GetIntVariable("ch1.forest_red_entrance_stage"))
			{
			case 0:
				ShowPath1();
				break;
			case 1:
				ShowPath2();
				break;
			case 2:
				ShowPath3();
				break;
			case 3:
				ShowPath3();
				break;
			case 4:
				ShowNoPaths();
				break;
			}
			nSaveCrow.Visible = varBirdboxOpen.Value;
			nBirdbox.Frame = (((bool)varBirdboxOpen.Value) ? 1 : 0);
			if ((bool)varRuneAllTalks.Value)
			{
				nRune.TurningEnabled = true;
				nRune.TurnToDefault();
				nRune.TurningEnabled = false;
			}
		}

		public void ShowPath1()
		{
			nPath1Tiles.Visible = true;
			nPath1Bg.Visible = true;
			nTilesPath1Underwater.Visible = true;
			nPath2Tiles.Visible = false;
			nPath2Bg.Visible = false;
			nTilesPath2Underwater.Visible = false;
			nLamp5.Visible = false;
			nLamp6.Visible = false;
			nPath3Tiles.Visible = false;
			nPath3Bg.Visible = false;
			nTilesPath3Underwater.Visible = false;
			nLamp7.Visible = false;
			nLamp8.Visible = true;
			nLight1a.Visible = true;
			nLight1b.Visible = true;
			nLight2.Visible = false;
			nLight3.Visible = false;
		}

		public void ShowPath2()
		{
			nPath1Tiles.Visible = true;
			nPath1Bg.Visible = true;
			nTilesPath1Underwater.Visible = true;
			nPath2Tiles.Visible = true;
			nPath2Bg.Visible = true;
			nTilesPath2Underwater.Visible = true;
			nPath3Tiles.Visible = false;
			nPath3Bg.Visible = false;
			nTilesPath3Underwater.Visible = false;
			nLamp7.Visible = false;
			nLamp8.Visible = true;
			nLight1a.Visible = true;
			nLight1b.Visible = true;
			nLight2.Visible = true;
			nLight3.Visible = false;
		}

		public void ShowPath3()
		{
			nPath1Tiles.Visible = true;
			nPath1Bg.Visible = true;
			nTilesPath1Underwater.Visible = true;
			nPath2Tiles.Visible = true;
			nPath2Bg.Visible = true;
			nTilesPath2Underwater.Visible = true;
			nPath3Tiles.Visible = true;
			nPath3Bg.Visible = true;
			nTilesPath3Underwater.Visible = true;
			nLamp7.Visible = true;
			nLamp8.Visible = true;
			nLight1a.Visible = true;
			nLight1b.Visible = true;
			nLight2.Visible = true;
			nLight3.Visible = true;
		}

		public void ShowNoPaths()
		{
			nPath1Tiles.Visible = false;
			nPath1Bg.Visible = false;
			nTilesPath1Underwater.Visible = false;
			nLamp3.Visible = false;
			nLamp4.Visible = false;
			nPath2Tiles.Visible = false;
			nPath2Bg.Visible = false;
			nTilesPath2Underwater.Visible = false;
			nLamp5.Visible = false;
			nLamp6.Visible = false;
			nLamp7.Visible = false;
			nLamp8.Visible = false;
			nPath3Tiles.Visible = false;
			nPath3Bg.Visible = false;
			nTilesPath3Underwater.Visible = false;
			nLight1a.Visible = false;
			nLight1b.Visible = false;
			nLight2.Visible = false;
			nLight3.Visible = false;
		}

		public async void OpenBirdBox()
		{
			nBirdbox.Frame = 1;
			await this.DelaySeconds(0.2);
			nBirdsEscaping1.Emitting = true;
			nBirdsEscaping2.Emitting = true;
			Node2D lacieFall = scnLacieFall.Instance<Node2D>();
			lacieFall.Position = Game.Player.Node.Position;
			GetMainLayer().AddChild(lacieFall);
			Game.Player.Node.Visible = false;
		}

		public void UpdateRunesVar()
		{
			varHasRunes.NewValue = false;
			foreach (IItem ownedItem in Items.GetOwnedItems())
			{
				if (ownedItem.Tags.Contains("rune"))
				{
					varHasRunes.NewValue = true;
				}
			}
		}

		public void RandomizeDeathReaction()
		{
			Random rnd = new Random();
			varDeathReaction.NewValue = rnd.Next(0, 5);
		}
	}
}
