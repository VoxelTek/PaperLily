// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.Passthrough
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Nodes
{
  [Tool]
  [ExportType(icon = "ghost")]
  public class Passthrough : Node2D, IEditorArea, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public Vector2 Area { get; set; } = new Vector2(32f, 32f);

    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    public override void _EnterTree()
    {
      if (Engine.EditorHint)
        return;
      Passthrough.PassthroughArea passthroughArea = new Passthrough.PassthroughArea(this);
      passthroughArea.CollisionLayer = 9U;
      passthroughArea.CollisionMask = 0U;
      this.AddChild((Node) passthroughArea);
      CollisionShape2D collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = (Shape2D) new RectangleShape2D()
      {
        Extents = (this.Area / 2f)
      };
      collisionShape2D.Position = this.GetPixelPerfectOffset();
      passthroughArea.AddChild((Node) collisionShape2D);
    }

    void IEditorArea.Update() => this.Update();

    public class PassthroughArea : Area2D
    {
      private Passthrough _parent;

      private PassthroughArea()
      {
      }

      public PassthroughArea(Passthrough parent)
      {
        this.Name = nameof (PassthroughArea);
        this._parent = parent;
      }

      public override void _Ready()
      {
        int num1 = (int) this.Connect("body_entered", (Object) this, "StartCollisionBypass");
        int num2 = (int) this.Connect("body_exited", (Object) this, "EndCollisionBypass");
      }

      public void StartCollisionBypass(object body)
      {
        if (!this._parent.Enabled || !(body is WalkingCharacter walkingCharacter))
          return;
        walkingCharacter.CollisionMask |= 8U;
        walkingCharacter.CollisionMask &= 4294967292U;
      }

      public void EndCollisionBypass(object body)
      {
        if (!this._parent.Enabled || !(body is WalkingCharacter walkingCharacter))
          return;
        walkingCharacter.CollisionMask |= 3U;
        walkingCharacter.CollisionMask &= 4294967287U;
      }
    }
  }
}
