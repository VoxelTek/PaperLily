// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.SpawnPoint
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Nodes
{
  [Tool]
  [ExportType(icon = "target")]
  public class SpawnPoint : Node2D, IInspectorCustomizer
  {
	private int _layer = 1;

	public override void _Ready()
	{
	  if (Engine.EditorHint)
		return;
	  Game.Room.RegisteredPoints[this.Name] = this;
	}

	[Export(PropertyHint.None, "")]
	public LacieEngine.Core.Direction.Enum Direction { get; set; } = LacieEngine.Core.Direction.Enum.Down;

	[Export(PropertyHint.Range, "1,10240")]
	public int Layer
	{
	  get => this._layer;
	  set => this._layer = value;
	}
  }
}
