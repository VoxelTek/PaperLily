using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestCenter : GameRoom
	{
		[Inject]
		private IExtraFunctions Functions;

		[Export(PropertyHint.None, "")]
		private Texture texKozmoCrack1;

		[Export(PropertyHint.None, "")]
		private Texture texKozmoCrack2;

		[Export(PropertyHint.None, "")]
		private Texture texKozmoHeadPot;

		[Export(PropertyHint.None, "")]
		private Texture texKozmoHeadThing;

		[Export(PropertyHint.None, "")]
		private Texture texKozmoHeadHead;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmQuiet;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmDynamic;

		[GetNode("Main/chr_alba")]
		private Node2D nChrAlba;

		[GetNode("Main/chr_alba_pissed")]
		private SimpleAnimatedSprite nChrAlbaHiss;

		[GetNode("Main/chr_aoi")]
		private Node2D nChrAoi;

		[GetNode("Main/MiscLog2/chr_milion")]
		private NPCStaticTurnable nChrMilion;

		[GetNode("Main/MiscLog/chr_kozmo")]
		private Sprite nChrKozmo;

		[GetNode("Main/MiscLog/chr_kozmo_liedown")]
		private Node2D nChrKozmoLieDown;

		[GetNode("Main/MiscLog/chr_kozmo_explode")]
		private AnimatedSprite nChrKozmoExplode;

		[GetNode("Main/Campfire")]
		private Node2D nCampfire;

		[GetNode("Main/StructShop")]
		private Sprite nShop;

		[GetNode("Other/Events/chr_alba")]
		private EventTrigger nChrAlbaEvent;

		[GetNode("Other/Events/chr_aoi")]
		private EventTrigger nChrAoiEvent;

		[GetNode("Other/Events/chr_milion")]
		private EventTrigger nChrMilionEvent;

		[GetNode("Other/Events/chr_kozmo")]
		private EventTrigger nChrKozmoEvent;

		[GetNode("Other/Events/item_lamp_get")]
		private EventTrigger nBrokenLampEvt;

		[GetNode("Main/StructHeroTreeBottom/LacieSit")]
		private Sprite nCh1CutsceneLacieSit;

		[GetNode("Main/StructHeroTreeBottom/LacieSitReflection")]
		private Sprite nCh1CutsceneLacieReflection;

		[GetNode("Background/MiscLamppostHead")]
		private Node2D nFallenLamp;

		[GetNode("Main/FairyringDeath")]
		private SimpleAnimatedSprite nFairyringDeath;

		[GetNode("Other/Actions/show_crater")]
		private IAction nShowCraterAction;

		private PVar varCampfireOn = "ch1.forest_center_campfire_on";

		private PVar varKozmoUpset = "ch1.forest_kozmo_upset_stutter";

		private PVar varKozmoKnocks = "ch1.forest_kozmo_knocks";

		private PVar varKozmoLeaking = "ch1.forest_kozmo_leaking";

		private PVar varKozmoGone = "ch1.forest_kozmo_gone";

		private PVar varKozmoHead = "ch1.forest_kozmo_head";

		private PVar varTookLamp = "ch1.forest_center_took_lamp";

		private PVar varSeenTree = "ch1.forest_center_tree_seen";

		private PVar varTreeDestroyed = "ch1.forest_center_tree_destroyed";

		private PVar varAoiLeft = "ch1.forest_center_aoi_left";

		private PVar varBoughtAllFish = "ch1.forest_center_alba_bought_all";

		private const float TIME_TO_FIND_HEAD = 300f;

		public override void _BeforeFadeIn()
		{
			if (!varSeenTree.Value)
			{
				Game.Audio.PlayBgm(bgmQuiet, 1f, crossFade: true);
			}
			else
			{
				Game.Audio.PlayBgm(bgmDynamic, 1f, crossFade: true);
			}
		}

		public override void _UpdateRoom()
		{
			bool campfireOn = varCampfireOn.Value;
			nChrAlba.Visible = campfireOn;
			nChrMilion.Visible = campfireOn;
			nChrKozmo.Visible = !varKozmoGone.Value && campfireOn;
			nCampfire.Visible = campfireOn;
			nChrAlbaEvent.Enabled = campfireOn;
			nChrMilionEvent.Enabled = campfireOn;
			nChrKozmoEvent.Enabled = !varKozmoGone.Value && campfireOn;
			nShop.Frame = ((campfireOn && !varBoughtAllFish.Value) ? 2 : 0);
			nBrokenLampEvt.Enabled = !varTookLamp.Value;
			nFallenLamp.Visible = !varTookLamp.Value;
			if ((bool)varTreeDestroyed.Value)
			{
				nShowCraterAction.Execute();
				nChrAlba.Visible = false;
				nChrMilion.Visible = false;
				nChrKozmo.Visible = false;
				nChrAlbaEvent.Enabled = false;
				nChrMilionEvent.Enabled = false;
				nChrKozmoEvent.Enabled = false;
				nChrAoi.Visible = !varAoiLeft.Value;
				nChrAoiEvent.Enabled = !varAoiLeft.Value;
				return;
			}
			nChrKozmoLieDown.Visible = false;
			nChrKozmoExplode.Visible = false;
			if ((bool)varKozmoGone.Value)
			{
				return;
			}
			if (campfireOn && (bool)varKozmoUpset.Value)
			{
				nChrKozmo.Visible = false;
				nChrKozmoLieDown.Visible = true;
			}
			if (varKozmoKnocks.Value == 1)
			{
				nChrKozmo.Texture = texKozmoCrack1;
			}
			else if (varKozmoKnocks.Value == 2)
			{
				nChrKozmo.Texture = texKozmoCrack2;
			}
			else if ((bool)varKozmoLeaking.Value)
			{
				nChrKozmo.Visible = false;
				nChrKozmoExplode.Visible = true;
				nChrKozmoExplode.Play("leaking");
			}
			if ((bool)varKozmoHead.Value)
			{
				if (varKozmoHead.Value == "pot")
				{
					nChrKozmo.Texture = texKozmoHeadPot;
				}
				if (varKozmoHead.Value == "thing")
				{
					nChrKozmo.Texture = texKozmoHeadThing;
				}
				if (varKozmoHead.Value == "head")
				{
					nChrKozmo.Texture = texKozmoHeadHead;
				}
			}
		}

		public void PeekBehindCurtain()
		{
			nShop.Frame = 1;
		}

		public async void KozmoExplode()
		{
			nChrKozmo.Visible = false;
			nChrKozmoExplode.Visible = true;
			nChrKozmoExplode.Play("explode");
			await ToSignal(nChrKozmoExplode, "animation_finished");
			nChrKozmoExplode.Play("leaking");
		}

		public void AlbaHiss()
		{
			nChrAlba.Visible = false;
			nChrAlbaHiss.Visible = true;
			nChrAlbaHiss.Playing = true;
		}

		public void AlbaReset()
		{
			nChrAlba.Visible = true;
			nChrAlbaHiss.Visible = false;
		}

		public void LacieSitTree()
		{
			nCh1CutsceneLacieSit.Visible = true;
			nCh1CutsceneLacieSit.Frame = 0;
			nCh1CutsceneLacieReflection.Visible = true;
			nCh1CutsceneLacieReflection.Frame = 0;
		}

		public void LacieSitTreeSmile()
		{
			nCh1CutsceneLacieSit.Visible = true;
			nCh1CutsceneLacieSit.Frame = 1;
			nCh1CutsceneLacieReflection.Visible = true;
			nCh1CutsceneLacieReflection.Frame = 1;
		}

		public async void FairyringDeath()
		{
			nFairyringDeath.Visible = true;
			nFairyringDeath.AnimationFrames = "0,1,2,3,4,5,6,7";
			nFairyringDeath.FPS = 50f;
			nFairyringDeath.Playing = true;
			await this.DelaySeconds(0.2);
			Game.Player.Node.Visible = false;
			nFairyringDeath.AnimationFrames = "8,9,10,11,12,13,14";
			nFairyringDeath.FPS = 6.66f;
			nFairyringDeath.Playing = true;
			await this.DelaySeconds(1.0);
			nFairyringDeath.Visible = false;
		}

		public void StartKozmoTimer()
		{
			Functions.StartTimer(300f, delegate
			{
				Game.Events.PlayEvent("ch1_event_kozmotimeout");
			}, destroyTimerOnTimeout: true);
		}

		public void StopTimer()
		{
			Functions.StopTimer();
		}
	}
}
