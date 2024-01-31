// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.MinigameManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Minigames
{
  [Injectable]
  public class MinigameManager : IMinigameManager
  {
    private const float SlideAnimationTime = 0.3f;
    private IMinigame minigame;
    private Control nMinigameContainer;
    private Control nMinigameInfo;

    public bool Running => this.nMinigameContainer.IsValid();

    public Node Node => this.minigame?.Node;

    public async void Start(string file)
    {
      Log.Debug((object) "Starting minigame: ", (object) file);
      Game.InputProcessor = Inputs.Processor.None;
      MinigameResource minigameResource = GD.Load<MinigameResource>("res://resources/minigame/" + file + ".tres");
      this.nMinigameContainer = (Control) GDUtil.MakeNode<CenterContainer>("MinigameContainer");
      this.nMinigameContainer.RectPosition = new Vector2(0.0f, Game.Settings.BaseResolution.y);
      this.nMinigameContainer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      this.minigame = minigameResource.MakeMinigame();
      this.nMinigameContainer.AddChild(this.minigame.Node);
      if (this.minigame.CustomTransition)
      {
        await this.minigame.TransitionIn();
        Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, (Node) this.nMinigameContainer);
      }
      else
      {
        Game.Screen.DarkenScreen();
        Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, (Node) this.nMinigameContainer);
        LacieAnimation animation = (LacieAnimation) new SlideInBottomAnimation(this.nMinigameContainer, new float?(Game.Settings.BaseResolution.y), new Vector2?(Vector2.Zero), 0.3f);
        Game.Animations.Play(animation);
        await animation.WaitUntilFinished();
      }
      this.nMinigameInfo = GDUtil.MakeNode<Control>("MinigameInfo");
      this.nMinigameInfo.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, (Node) this.nMinigameInfo);
      this.minigame.AddUiElements(this.nMinigameInfo);
      Game.InputProcessor = Inputs.Processor.Minigame;
      this.minigame.Start();
    }

    public async void End(string @event)
    {
      if (this.nMinigameContainer == null)
        return;
      this.nMinigameInfo.DeleteIfValid();
      Game.Screen.UndarkenScreen();
      if (this.minigame.CustomTransition)
      {
        await this.minigame.TransitionOut();
      }
      else
      {
        LacieAnimation animation = (LacieAnimation) new SlideOutBottomAnimation(this.nMinigameContainer, new float?(Game.Settings.BaseResolution.y), new Vector2?(Vector2.Zero), 0.3f);
        Game.Animations.Play(animation);
        await animation.WaitUntilFinished();
      }
      this.nMinigameContainer.Delete();
      this.nMinigameContainer = (Control) null;
      this.nMinigameInfo = (Control) null;
      this.minigame = (IMinigame) null;
      if (!@event.IsNullOrEmpty())
        Game.Events.PlayEvent(@event);
      Game.Core.AssignInputProcessor();
    }

    public void End() => this.End((string) null);

    public bool Function(string function)
    {
      if (!this.Running)
        return false;
      if (this.minigame.Node.HasMethod(function))
      {
        this.minigame.Node.Call(function);
        return true;
      }
      Log.Warn((object) "Method ", (object) function, (object) "() not found in minigame: ", (object) this.minigame.Node.Name);
      return false;
    }
  }
}
