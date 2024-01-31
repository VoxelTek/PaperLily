// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.GameRoom
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  [ExportType(icon = "room")]
  public class GameRoom : Node2D, INodeWithInjections, IInspectorCustomizer
  {
    [Export(PropertyHint.None, "")]
    public bool Cutscene;
    [Export(PropertyHint.None, "")]
    public Lighting Lighting;
    [Export(PropertyHint.Range, "0.1,3.0")]
    public float CameraZoom = 1f;
    [Export(PropertyHint.None, "")]
    public int CameraLimitLeft = -10000000;
    [Export(PropertyHint.None, "")]
    public int CameraLimitTop = -10000000;
    [Export(PropertyHint.None, "")]
    public int CameraLimitRight = 10000000;
    [Export(PropertyHint.None, "")]
    public int CameraLimitBottom = 10000000;
    [Export(PropertyHint.None, "")]
    public AudioStream Bgm;
    [Export(PropertyHint.Range, "0,1")]
    public float BgmVolume = 1f;
    [Export(PropertyHint.None, "")]
    public bool BgmCrossfade;
    [Export(PropertyHint.Enum, "default,sidescroller")]
    public string Type = "default";
    [Export(PropertyHint.None, "")]
    public bool DisableRunning;
    [Export(PropertyHint.None, "")]
    public bool EnableSneaking;
    [Export(PropertyHint.None, "")]
    public bool HideFollowers;
    [Export(PropertyHint.None, "")]
    public string SaveLocation = "";
    [Export(PropertyHint.None, "")]
    public string SaveImage = "";

    public bool Ready { get; set; }

    public override sealed void _Ready()
    {
    }

    public override sealed void _EnterTree()
    {
      if (!Engine.EditorHint)
        return;
      this.SetProcess(false);
    }

    public override sealed void _Process(float delta)
    {
      if (!this.Ready)
        return;
      this._RoomProcess(delta);
    }

    public override sealed void _PhysicsProcess(float delta)
    {
    }

    public virtual void _Initialize()
    {
    }

    public virtual void _BeforeFadeIn()
    {
    }

    public virtual void _BeforeFadeOut()
    {
    }

    public virtual void _AfterFadeIn()
    {
    }

    public virtual void _AfterFadeOut()
    {
    }

    public virtual void _UpdateRoom()
    {
    }

    public virtual void _RoomProcess(float delta)
    {
    }

    public virtual Node2D GetMainLayer() => this.GetNode<Node2D>((NodePath) "Main");

    public virtual Node FindNodeInRoom(string nodeName)
    {
      if (Game.Room.RegisteredNPCs.ContainsKey(nodeName.ToLower()))
        return (Node) Game.Room.RegisteredNPCs[nodeName.ToLower()];
      if (Game.Room.RegisteredPoints.ContainsKey(nodeName.ToLower()))
        return (Node) Game.Room.RegisteredPoints[nodeName.ToLower()];
      if (this.GetMainLayer().HasNode((NodePath) nodeName))
        return this.GetMainLayer().GetNode((NodePath) nodeName);
      if (this.HasNode((NodePath) ("Background/" + nodeName)))
        return this.GetNode((NodePath) ("Background/" + nodeName));
      if (this.HasNode((NodePath) ("Foreground/" + nodeName)))
        return this.GetNode((NodePath) ("Foreground/" + nodeName));
      return this.HasNode((NodePath) ("Events/" + nodeName)) ? this.GetNode((NodePath) ("Events/" + nodeName)) : (Node) null;
    }

    public virtual SpawnPoint GetSpawnPoint(string pointName)
    {
      if (Game.Room.RegisteredPoints.ContainsKey(pointName))
        return Game.Room.RegisteredPoints[pointName];
      Log.Warn((object) "Registered point not found: ", (object) pointName);
      return (SpawnPoint) null;
    }

    public virtual Vector2 GetPoint(string pointName)
    {
      SpawnPoint spawnPoint = this.GetSpawnPoint(pointName);
      return spawnPoint == null ? Vector2.Zero : spawnPoint.Position;
    }

    public virtual void ChangeLayer(int newLayer)
    {
    }
  }
}
