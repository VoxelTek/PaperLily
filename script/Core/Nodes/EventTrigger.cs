// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.EventTrigger
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "bolt")]
  public abstract class EventTrigger : Node2D, IToggleable, IEventTriggerNode
  {
    public static readonly Color SolidEventColor = new Color(0.0f, 0.0f, 1f, 0.42f);
    public static readonly Color NonSolidEventColor = new Color(1f, 1f, 0.0f, 0.42f);
    public static readonly Color ContactEventColor = new Color(1f, 0.0f, 0.0f, 0.4f);
    private bool _enabled = true;
    private bool _solid = true;
    private EventTrigger.ETrigger _trigger;

    [Export(PropertyHint.None, "")]
    public string Event { get; set; } = "";

    [Export(PropertyHint.None, "")]
    public bool Enabled
    {
      get => this._enabled;
      set
      {
        this._enabled = value;
        this.UpdateValues();
      }
    }

    [Export(PropertyHint.None, "")]
    public bool Solid
    {
      get => this._solid;
      set
      {
        this._solid = value;
        this.UpdateValues();
      }
    }

    [Export(PropertyHint.None, "")]
    public EventTrigger.ETrigger Trigger
    {
      get => this._trigger;
      set
      {
        this._trigger = value;
        this.UpdateValues();
      }
    }

    [Export(PropertyHint.Flags, "Left,Up,Right,Down")]
    public byte Directions { get; set; }

    [Export(PropertyHint.None, "")]
    public NodePath RelatedNode { get; private set; } = new NodePath();

    public string EffectiveEventName => !this.Event.IsNullOrEmpty() ? this.Event : this.Name;

    public int Priority { get; set; }

    public abstract void UpdateValues();

    public enum ETrigger
    {
      Action,
      Touch,
      Item,
    }
  }
}
