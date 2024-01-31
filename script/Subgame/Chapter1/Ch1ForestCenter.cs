// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestCenter
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using System;

#nullable disable
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
    [LacieEngine.API.GetNode("Main/chr_alba")]
    private Node2D nChrAlba;
    [LacieEngine.API.GetNode("Main/chr_alba_pissed")]
    private SimpleAnimatedSprite nChrAlbaHiss;
    [LacieEngine.API.GetNode("Main/chr_aoi")]
    private Node2D nChrAoi;
    [LacieEngine.API.GetNode("Main/MiscLog2/chr_milion")]
    private NPCStaticTurnable nChrMilion;
    [LacieEngine.API.GetNode("Main/MiscLog/chr_kozmo")]
    private Sprite nChrKozmo;
    [LacieEngine.API.GetNode("Main/MiscLog/chr_kozmo_liedown")]
    private Node2D nChrKozmoLieDown;
    [LacieEngine.API.GetNode("Main/MiscLog/chr_kozmo_explode")]
    private AnimatedSprite nChrKozmoExplode;
    [LacieEngine.API.GetNode("Main/Campfire")]
    private Node2D nCampfire;
    [LacieEngine.API.GetNode("Main/StructShop")]
    private Sprite nShop;
    [LacieEngine.API.GetNode("Other/Events/chr_alba")]
    private EventTrigger nChrAlbaEvent;
    [LacieEngine.API.GetNode("Other/Events/chr_aoi")]
    private EventTrigger nChrAoiEvent;
    [LacieEngine.API.GetNode("Other/Events/chr_milion")]
    private EventTrigger nChrMilionEvent;
    [LacieEngine.API.GetNode("Other/Events/chr_kozmo")]
    private EventTrigger nChrKozmoEvent;
    [LacieEngine.API.GetNode("Other/Events/item_lamp_get")]
    private EventTrigger nBrokenLampEvt;
    [LacieEngine.API.GetNode("Main/StructHeroTreeBottom/LacieSit")]
    private Sprite nCh1CutsceneLacieSit;
    [LacieEngine.API.GetNode("Main/StructHeroTreeBottom/LacieSitReflection")]
    private Sprite nCh1CutsceneLacieReflection;
    [LacieEngine.API.GetNode("Background/MiscLamppostHead")]
    private Node2D nFallenLamp;
    [LacieEngine.API.GetNode("Main/FairyringDeath")]
    private SimpleAnimatedSprite nFairyringDeath;
    [LacieEngine.API.GetNode("Other/Actions/show_crater")]
    private IAction nShowCraterAction;
    [LacieEngine.API.GetNode("Other/Events/misc_log_nosit_1")]
    private IToggleable nEvtNoSit1;
    [LacieEngine.API.GetNode("Other/Events/misc_log_nosit_2")]
    private IToggleable nEvtNoSit2;
    [LacieEngine.API.GetNode("Main/misc_stump/Chair")]
    private IToggleable nLogChair1;
    [LacieEngine.API.GetNode("Main/misc_stump3/Chair")]
    private IToggleable nLogChair2;
    private PVar varCampfireOn = (PVar) "ch1.forest_center_campfire_on";
    private PVar varKozmoUpset = (PVar) "ch1.forest_kozmo_upset_stutter";
    private PVar varKozmoKnocks = (PVar) "ch1.forest_kozmo_knocks";
    private PVar varKozmoLeaking = (PVar) "ch1.forest_kozmo_leaking";
    private PVar varKozmoGone = (PVar) "ch1.forest_kozmo_gone";
    private PVar varKozmoHead = (PVar) "ch1.forest_kozmo_head";
    private PVar varTookLamp = (PVar) "ch1.forest_center_took_lamp";
    private PVar varSeenTree = (PVar) "ch1.forest_center_tree_seen";
    private PVar varTreeDestroyed = (PVar) "ch1.forest_center_tree_destroyed";
    private PVar varAoiLeft = (PVar) "ch1.forest_center_aoi_left";
    private PVar varBoughtAllFish = (PVar) "ch1.forest_center_alba_bought_all";
    private const float TIME_TO_FIND_HEAD = 300f;

    public override void _BeforeFadeIn()
    {
      if (!(bool) this.varSeenTree.Value)
        Game.Audio.PlayBgm(this.bgmQuiet, 1f, true);
      else
        Game.Audio.PlayBgm(this.bgmDynamic, 1f, true);
    }

    public override void _UpdateRoom()
    {
      bool flag = (bool) this.varCampfireOn.Value;
      this.nChrAlba.Visible = flag;
      this.nChrMilion.Visible = flag;
      this.nChrKozmo.Visible = !(bool) this.varKozmoGone.Value & flag;
      this.nCampfire.Visible = flag;
      this.nChrAlbaEvent.Enabled = flag;
      this.nChrMilionEvent.Enabled = flag;
      this.nChrKozmoEvent.Enabled = !(bool) this.varKozmoGone.Value & flag;
      this.nShop.Frame = !flag || (bool) this.varBoughtAllFish.Value ? 0 : 2;
      this.nBrokenLampEvt.Enabled = !(bool) this.varTookLamp.Value;
      this.nFallenLamp.Visible = !(bool) this.varTookLamp.Value;
      if ((bool) this.varTreeDestroyed.Value)
      {
        this.nShowCraterAction.Execute();
        this.nChrAlba.Visible = false;
        this.nChrMilion.Visible = false;
        this.nChrKozmo.Visible = false;
        this.nChrAlbaEvent.Enabled = false;
        this.nChrMilionEvent.Enabled = false;
        this.nChrKozmoEvent.Enabled = false;
        this.nChrAoi.Visible = !(bool) this.varAoiLeft.Value;
        this.nChrAoiEvent.Enabled = !(bool) this.varAoiLeft.Value;
      }
      else
      {
        this.nChrKozmoLieDown.Visible = false;
        this.nChrKozmoExplode.Visible = false;
        if ((bool) this.varKozmoGone.Value)
          return;
        if (flag && (bool) this.varKozmoUpset.Value)
        {
          this.nChrKozmo.Visible = false;
          this.nChrKozmoLieDown.Visible = true;
        }
        if (this.varKozmoKnocks.Value == 1)
          this.nChrKozmo.Texture = this.texKozmoCrack1;
        else if (this.varKozmoKnocks.Value == 2)
          this.nChrKozmo.Texture = this.texKozmoCrack2;
        else if ((bool) this.varKozmoLeaking.Value)
        {
          this.nChrKozmo.Visible = false;
          this.nChrKozmoExplode.Visible = true;
          this.nChrKozmoExplode.Play("leaking");
          this.nEvtNoSit1.Enabled = true;
          this.nEvtNoSit2.Enabled = true;
          this.nLogChair1.Enabled = false;
          this.nLogChair2.Enabled = false;
        }
        else
        {
          this.nEvtNoSit1.Enabled = false;
          this.nEvtNoSit2.Enabled = false;
          this.nLogChair1.Enabled = true;
          this.nLogChair2.Enabled = true;
        }
        if (!(bool) this.varKozmoHead.Value)
          return;
        if (this.varKozmoHead.Value == "pot")
          this.nChrKozmo.Texture = this.texKozmoHeadPot;
        if (this.varKozmoHead.Value == "thing")
          this.nChrKozmo.Texture = this.texKozmoHeadThing;
        if (!(this.varKozmoHead.Value == "head"))
          return;
        this.nChrKozmo.Texture = this.texKozmoHeadHead;
      }
    }

    public void PeekBehindCurtain() => this.nShop.Frame = 1;

    public async void KozmoExplode()
    {
      Ch1ForestCenter ch1ForestCenter = this;
      ch1ForestCenter.nChrKozmo.Visible = false;
      ch1ForestCenter.nChrKozmoExplode.Visible = true;
      ch1ForestCenter.nChrKozmoExplode.Play("explode");
      object[] signal = await ch1ForestCenter.ToSignal((Godot.Object) ch1ForestCenter.nChrKozmoExplode, "animation_finished");
      ch1ForestCenter.nChrKozmoExplode.Play("leaking");
    }

    public void AlbaHiss()
    {
      this.nChrAlba.Visible = false;
      this.nChrAlbaHiss.Visible = true;
      this.nChrAlbaHiss.Playing = true;
    }

    public void AlbaReset()
    {
      this.nChrAlba.Visible = true;
      this.nChrAlbaHiss.Visible = false;
    }

    public void LacieSitTree()
    {
      this.nCh1CutsceneLacieSit.Visible = true;
      this.nCh1CutsceneLacieSit.Frame = 0;
      this.nCh1CutsceneLacieReflection.Visible = true;
      this.nCh1CutsceneLacieReflection.Frame = 0;
    }

    public void LacieSitTreeSmile()
    {
      this.nCh1CutsceneLacieSit.Visible = true;
      this.nCh1CutsceneLacieSit.Frame = 1;
      this.nCh1CutsceneLacieReflection.Visible = true;
      this.nCh1CutsceneLacieReflection.Frame = 1;
    }

    public async void FairyringDeath()
    {
      Ch1ForestCenter baseNode = this;
      baseNode.nFairyringDeath.Visible = true;
      baseNode.nFairyringDeath.AnimationFrames = "0,1,2,3,4,5,6,7";
      baseNode.nFairyringDeath.FPS = 50f;
      baseNode.nFairyringDeath.Playing = true;
      await baseNode.DelaySeconds(0.2);
      Game.Player.Node.Visible = false;
      baseNode.nFairyringDeath.AnimationFrames = "8,9,10,11,12,13,14";
      baseNode.nFairyringDeath.FPS = 6.66f;
      baseNode.nFairyringDeath.Playing = true;
      await baseNode.DelaySeconds(1.0);
      baseNode.nFairyringDeath.Visible = false;
    }

    public void StartKozmoTimer()
    {
      this.Functions.StartTimer(300f, (Action) (() => Game.Events.PlayEvent("ch1_event_kozmotimeout")), true);
    }

    public void StopTimer() => this.Functions.StopTimer();
  }
}
