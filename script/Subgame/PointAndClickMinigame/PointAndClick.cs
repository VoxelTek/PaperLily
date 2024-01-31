// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.PointAndClick
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LacieEngine.Minigames
{
  public class PointAndClick : Control, IMinigame
  {
    private const int CursorSpeed = 300;
    private const int SlowCursorSpeed = 100;
    private static readonly string[] HandledInputs = new string[2]
    {
      "input_action",
      "input_cancel"
    };
    protected TextureRect nCursor;
    private List<Control> _targets = new List<Control>();
    private bool _ready;
    private Direction _cursorDirection = Direction.None;

    [Export(PropertyHint.None, "")]
    public string OnCloseEvent { get; private set; }

    [Export(PropertyHint.None, "")]
    public bool SnapEnabled { get; private set; }

    [Export(PropertyHint.None, "")]
    public bool HideCursor { get; private set; }

    [Export(PropertyHint.None, "")]
    public Dictionary<string, string> EventTargets { get; private set; }

    [Export(PropertyHint.None, "")]
    public Dictionary<string, string> CloseEventTargets { get; private set; }

    [Export(PropertyHint.None, "")]
    public Dictionary<string, string> FunctionTargets { get; private set; }

    [Export(PropertyHint.None, "")]
    public ShaderMaterial HighlightShader { get; private set; }

    public override void _Ready()
    {
      foreach (Control child in this.GetNode((NodePath) "Targets").GetChildren())
        this._targets.Add(child);
      this._targets.Reverse();
      if (this.SnapEnabled)
        this.NavigateToFirstTarget();
      this._ready = true;
    }

    public virtual void Init()
    {
      this.nCursor = new TextureRect();
      this.nCursor.Expand = true;
      this.nCursor.Texture = GD.Load<Texture>("res://assets/img/ui/cursor.png");
      this.nCursor.RectSize = new Vector2(32f, 32f);
      this.nCursor.RectPosition = new Vector2(this.RectMinSize.x / 2f, this.RectMinSize.y / 2f);
      if (this.HideCursor)
        this.nCursor.Visible = false;
      this.AddChild((Node) this.nCursor);
      Dictionary<string, string> dictionary;
      if (this.EventTargets == null)
        this.EventTargets = dictionary = new Dictionary<string, string>();
      if (this.CloseEventTargets == null)
        this.CloseEventTargets = dictionary = new Dictionary<string, string>();
      if (this.FunctionTargets != null)
        return;
      this.FunctionTargets = dictionary = new Dictionary<string, string>();
    }

    public void AddUiElements(Control parent)
    {
      if (this.SnapEnabled)
        return;
      Control boxWithInputIcons = (Control) UIUtil.CreateInfoBoxWithInputIcons("system.minigames.pointandclick.info", "input_run");
      boxWithInputIcons.Name = "MinigameInfo";
      boxWithInputIcons.SetAnchorsAndMarginsPreset(Control.LayoutPreset.TopLeft);
      parent.AddChild((Node) boxWithInputIcons);
      Game.Animations.Play((LacieAnimation) new SlideInTopAnimation(boxWithInputIcons));
    }

    public override void _Input(InputEvent @event)
    {
      switch (Inputs.Handle(@event, Inputs.Processor.Minigame, PointAndClick.HandledInputs))
      {
        case "input_action":
          Control targetUnderCursor1 = this.GetTargetUnderCursor();
          if (targetUnderCursor1 != null)
          {
            this.CallTarget(targetUnderCursor1.Name);
            break;
          }
          break;
        case "input_cancel":
          Game.InputProcessor = Inputs.Processor.None;
          Game.Minigames.End(this.OnCloseEvent);
          break;
      }
      if (!this.SnapEnabled || !(this.GetTargetUnderCursor() is PointAndClickSnapTarget targetUnderCursor2))
        return;
      switch (Inputs.Handle(@event, Inputs.Processor.Minigame, Inputs.AllMovement))
      {
        case "input_left":
          if (targetUnderCursor2.Left == null)
            break;
          this.NavigateToSnapTarget(targetUnderCursor2.Left);
          break;
        case "input_up":
          if (targetUnderCursor2.Up == null)
            break;
          this.NavigateToSnapTarget(targetUnderCursor2.Up);
          break;
        case "input_right":
          if (targetUnderCursor2.Right == null)
            break;
          this.NavigateToSnapTarget(targetUnderCursor2.Right);
          break;
        case "input_down":
          if (targetUnderCursor2.Down == null)
            break;
          this.NavigateToSnapTarget(targetUnderCursor2.Down);
          break;
      }
    }

    public override void _Process(float delta)
    {
      if (!this._ready || this.SnapEnabled)
        return;
      this._cursorDirection = Inputs.GetDirectionFromInput(this._cursorDirection, Inputs.Processor.Minigame);
      if (Game.InputProcessor == Inputs.Processor.Minigame && Inputs.IsActionPressed("input_run"))
      {
        TextureRect nCursor = this.nCursor;
        nCursor.RectPosition = nCursor.RectPosition + this._cursorDirection.ToVector() * 100f * delta;
      }
      else
      {
        TextureRect nCursor = this.nCursor;
        nCursor.RectPosition = nCursor.RectPosition + this._cursorDirection.ToVector() * 300f * delta;
      }
      if ((double) this.nCursor.RectPosition.x < 0.0)
        this.nCursor.RectPosition = new Vector2(0.0f, this.nCursor.RectPosition.y);
      else if ((double) this.nCursor.RectPosition.x > (double) this.RectMinSize.x)
        this.nCursor.RectPosition = new Vector2(this.RectMinSize.x, this.nCursor.RectPosition.y);
      if ((double) this.nCursor.RectPosition.y < 0.0)
      {
        this.nCursor.RectPosition = new Vector2(this.nCursor.RectPosition.x, 0.0f);
      }
      else
      {
        if ((double) this.nCursor.RectPosition.y <= (double) this.RectMinSize.y)
          return;
        this.nCursor.RectPosition = new Vector2(this.nCursor.RectPosition.x, this.RectMinSize.y);
      }
    }

    public void CallFunction(string functionName)
    {
      Log.Debug((object) "Call minigame function: ", (object) functionName);
      if (!this.HasMethod(functionName))
        Log.Error((object) "Minigame function not found: ", (object) functionName);
      else
        this.Call(functionName);
    }

    private void NavigateToFirstTarget()
    {
      foreach (PointAndClickSnapTarget andClickSnapTarget in this._targets.Cast<PointAndClickSnapTarget>())
      {
        if (andClickSnapTarget.FirstTarget)
        {
          this.nCursor.RectPosition = andClickSnapTarget.RectPosition + andClickSnapTarget.SnapOffset;
          if (this.HighlightShader == null)
            break;
          andClickSnapTarget.Material = (Material) this.HighlightShader;
          break;
        }
      }
    }

    private void NavigateToSnapTarget(string targetName)
    {
      foreach (Control target in this._targets)
      {
        target.Material = (Material) null;
        if (target.Name == targetName)
        {
          PointAndClickSnapTarget andClickSnapTarget = target as PointAndClickSnapTarget;
          this.nCursor.RectPosition = andClickSnapTarget.RectPosition + andClickSnapTarget.SnapOffset;
          if (this.HighlightShader != null)
            andClickSnapTarget.Material = (Material) this.HighlightShader;
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
        }
      }
    }

    private Control GetTargetUnderCursor()
    {
      foreach (Control target in this._targets)
      {
        if (target.Visible && target.GetRect().HasPoint(this.nCursor.RectPosition))
        {
          if (!(target is TextureRect texture))
            return target;
          Vector2 coords = this.nCursor.RectPosition - target.RectPosition;
          if ((double) texture.GetPixelAt(coords).a != 0.0)
            return target;
        }
      }
      return (Control) null;
    }

    private void CallTarget(string target)
    {
      Log.Debug((object) "Selected PnC target: ", (object) target);
      if (this.EventTargets.ContainsKey(target))
      {
        Game.InputProcessor = Inputs.Processor.None;
        Game.Events.PlayEvent(this.EventTargets[target]);
      }
      else if (this.CloseEventTargets.ContainsKey(target))
      {
        Game.InputProcessor = Inputs.Processor.None;
        Game.Minigames.End(this.CloseEventTargets[target]);
      }
      else
      {
        if (!this.FunctionTargets.ContainsKey(target))
          return;
        this.CallFunction(this.FunctionTargets[target]);
      }
    }
  }
}
