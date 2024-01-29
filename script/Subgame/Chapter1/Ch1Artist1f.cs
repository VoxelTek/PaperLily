using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Artist1f : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private PackedScene scnLacieFall;

		[Export(PropertyHint.None, "")]
		private PackedScene scnFlowerRed;

		[Export(PropertyHint.None, "")]
		private PackedScene scnFlowerGreen;

		[Export(PropertyHint.None, "")]
		private PackedScene scnFlowerYellow;

		[Export(PropertyHint.None, "")]
		private Texture texKettMad;

		[GetNode("Background")]
		private Node2D nBackground;

		[GetNode("Main/KettMad")]
		private SimpleAnimatedSprite nKettMad;

		[GetNode("Main/KettSleep")]
		private Node2D nKettSleep;

		[GetNode("Main/KettAwake")]
		private Node2D nKettAwake;

		[GetNode("Other/Events/chr_kett_sleep")]
		private EventTrigger nKettSleepEvent;

		[GetNode("Other/Events/chr_kett_awake")]
		private EventTrigger nKettAwakeEvent;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varKettCalm = "ch1.forest_kett_calm_down";

		private PVar varKettChase = "ch1.forest_kett_chase";

		private PVar varKettAwake = "ch1.forest_kett_awake";

		private PVar varSpawnedRedFlower = "ch1.forest_kett_spawned_flower_red";

		private PVar varSpawnedGreenFlower = "ch1.forest_kett_spawned_flower_green";

		private PVar varSpawnedYellowFlower = "ch1.forest_kett_spawned_flower_yellow";

		private PVar varPositionRedFlower = "ch1.forest_kett_pos_flower_red";

		private PVar varPositionGreenFlower = "ch1.forest_kett_pos_flower_green";

		private PVar varPositionYellowFlower = "ch1.forest_kett_pos_flower_yellow";

		private NPCChaser nKettChasing;

		private Dictionary<string, Node2D> flowers = new Dictionary<string, Node2D>();

		public override void _BeforeFadeIn()
		{
			if ((bool)varKettChase.Value)
			{
				nKettChasing = new NPCChaser("kett");
				nKettChasing.Position = GetPoint("entrance");
				nKettChasing.MoveSpeedMultiplier = 1.3f;
				GetMainLayer().AddChild(nKettChasing);
				nKettChasing.OverrideTextureForState("stand", texKettMad);
				nKettChasing.OverrideTextureForState("walk", texKettMad);
				nKettChasing.SetNavigation(nNavigation);
				nKettChasing.Caught += PlayerDeath;
			}
			if ((bool)varSpawnedRedFlower.Value)
			{
				SpawnFlower("red", scnFlowerRed);
			}
			if ((bool)varSpawnedGreenFlower.Value)
			{
				SpawnFlower("green", scnFlowerGreen);
			}
			if ((bool)varSpawnedYellowFlower.Value)
			{
				SpawnFlower("yellow", scnFlowerYellow);
			}
		}

		public override void _AfterFadeIn()
		{
			if (nKettChasing.IsValid())
			{
				nKettChasing.Chasing = true;
			}
		}

		public override void _UpdateRoom()
		{
			nKettAwake.Visible = false;
			nKettSleep.Visible = false;
			nKettMad.Visible = false;
			nKettSleepEvent.Enabled = false;
			nKettAwakeEvent.Enabled = false;
			if (!varKettChase.Value)
			{
				if ((bool)varKettAwake.Value)
				{
					nKettAwake.Visible = true;
					nKettAwakeEvent.Enabled = true;
				}
				else
				{
					nKettMad.Visible = !varKettCalm.Value;
					nKettSleep.Visible = varKettCalm.Value;
					nKettSleepEvent.Enabled = true;
				}
			}
		}

		public async Task KettMadTurn()
		{
			nKettMad.Playing = false;
			nKettMad.AnimationFrames = "0,1,2,3,4,5";
			nKettMad.FPS = 10f;
			nKettMad.Loop = false;
			nKettMad.Playing = true;
			await this.DelaySeconds(0.05);
			Node2D lacieFall = scnLacieFall.Instance<Node2D>();
			lacieFall.Position = Game.Player.Node.Position;
			GetMainLayer().AddChild(lacieFall);
			Game.Player.Node.Visible = false;
		}

		private void PauseChaser()
		{
			if (nKettChasing.IsValid())
			{
				nKettChasing.Chasing = false;
			}
		}

		public void KettChomp()
		{
			nKettMad.Playing = false;
			nKettMad.Frame = 6;
		}

		public void KettBloomRed()
		{
			KettBloom("red", scnFlowerRed);
		}

		public void KettBloomGreen()
		{
			KettBloom("green", scnFlowerGreen);
		}

		public void KettBloomYellow()
		{
			KettBloom("yellow", scnFlowerYellow);
		}

		public void RemoveFlowerRed()
		{
			DeleteFlower("red");
		}

		public void RemoveFlowerGreen()
		{
			DeleteFlower("green");
		}

		public void RemoveFlowerYellow()
		{
			DeleteFlower("yellow");
		}

		public async void KettSniff()
		{
			NpcStaticIdleTurnable kett = (NpcStaticIdleTurnable)Game.Room.RegisteredNPCs["kett"];
			Direction dirBeforeAnimation = kett.Direction;
			kett.SpriteState = "sniff";
			await this.DelaySeconds(1.5);
			kett.SpriteState = "stand";
			kett.Turn(dirBeforeAnimation);
		}

		public async void KettSnack()
		{
			NpcStaticIdleTurnable kett = (NpcStaticIdleTurnable)Game.Room.RegisteredNPCs["kett"];
			Direction dirBeforeAnimation = kett.Direction;
			kett.SpriteState = "snack";
			await this.DelaySeconds(2.0);
			kett.SpriteState = "stand";
			kett.Turn(dirBeforeAnimation);
		}

		public void PlayerDeath()
		{
			Game.Events.PlayEvent("ch1_death_impact");
		}

		private async void KettBloom(string color, PackedScene flowerScn)
		{
			NpcStaticIdleTurnable kett = (NpcStaticIdleTurnable)Game.Room.RegisteredNPCs["kett"];
			Direction dirBeforeAnimation = kett.Direction;
			kett.SpriteState = "bloom_" + color;
			await this.DelaySeconds(2.5);
			kett.SpriteState = "cut_" + color;
			await this.DelaySeconds(2.5);
			((PVar)("ch1.forest_kett_spawned_flower_" + color)).NewValue = true;
			((PVar)("ch1.forest_kett_pos_flower_" + color)).NewValue = string.Join(",", kett.Position.x, kett.Position.y);
			SpawnFlower(color, flowerScn);
			kett.SpriteState = "stand";
			kett.Turn(dirBeforeAnimation);
		}

		private void SpawnFlower(string color, PackedScene flowerScn)
		{
			Node2D flower = flowerScn.Instance<Node2D>();
			PVar varPosFlower = "ch1.forest_kett_pos_flower_" + color;
			flower.Position = GDUtil.StringToVector2(varPosFlower.Value);
			nBackground.AddChild(flower);
			flowers[color] = flower;
		}

		private void DeleteFlower(string color)
		{
			if (flowers.ContainsKey(color))
			{
				flowers[color].DeleteIfValid();
			}
		}
	}
}
