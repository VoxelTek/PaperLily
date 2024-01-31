// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.CommonMirror
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Nodes
{
  public class CommonMirror : Light2D
  {
    [Export(PropertyHint.None, "")]
    public int XExtents = 100;
    [Export(PropertyHint.None, "")]
    public int YExtents = 100;
    [Export(PropertyHint.None, "")]
    public int DistanceToFloor;
    [Export(PropertyHint.None, "")]
    public Vector2 ReflectionOffset = new Vector2(0.0f, 0.0f);
    [Export(PropertyHint.None, "")]
    public bool OverrideColorOn;
    [Export(PropertyHint.None, "")]
    public Color OverrideColor = Godot.Colors.White;
    private Dictionary<string, Sprite> _mirroredSprites = new Dictionary<string, Sprite>();
    private Vector2 _floorPoint = Vector2.Zero;

    public override void _Ready()
    {
      base._Ready();
      this.Color = this.OverrideColorOn ? this.OverrideColor : Game.Room.Modulate;
      this.Mode = Light2D.ModeEnum.Mix;
      this.RangeItemCullMask = 4;
      this._floorPoint = new Vector2(this.GlobalPosition.x, (float) ((double) this.GlobalPosition.y + (double) this.DistanceToFloor + (double) this.Texture.GetSize().y * (double) this.TextureScale * (double) this.Transform.Scale.y / 2.0));
    }

    public override void _Process(float delta)
    {
      if (!this.OverrideColorOn)
        this.Color = Game.Room.Modulate;
      foreach (IReflectable mirrorReflection in Game.Room.MirrorReflections)
      {
        SpriteState reflectedSprite = mirrorReflection.GetReflectedSprite();
        if (this._mirroredSprites.ContainsKey(reflectedSprite.Name))
          this.UpdateReflection(reflectedSprite);
        else
          this.CreateReflection(reflectedSprite);
      }
    }

    private void CreateReflection(SpriteState sprite)
    {
      if (!this.IsInReflectableArea(sprite.Pos))
        return;
      Sprite sprite1 = new Sprite();
      sprite1.Scale = new Vector2(-1f / this.Scale.x, 1f / this.Scale.y);
      sprite1.Material = GD.Load<Material>("res://resources/material/mirror_reflection.tres");
      sprite1.LightMask = 4;
      this.AddChild((Node) sprite1);
      this._mirroredSprites[sprite.Name] = sprite1;
      this.UpdateReflection(sprite);
    }

    private void UpdateReflection(SpriteState sprite)
    {
      if (this.IsInReflectableArea(sprite.Pos))
      {
        Sprite mirroredSprite = this._mirroredSprites[sprite.Name];
        mirroredSprite.Visible = true;
        mirroredSprite.Texture = sprite.Texture;
        mirroredSprite.Hframes = sprite.HFrames;
        mirroredSprite.Vframes = sprite.VFrames;
        mirroredSprite.Frame = sprite.Frame;
        mirroredSprite.GlobalPosition = this.CalculateReflectionPosition(sprite.Pos, mirroredSprite);
      }
      else
        this._mirroredSprites[sprite.Name].Visible = false;
    }

    private bool IsInReflectableArea(Vector2 position)
    {
      return (double) position.y > (double) this._floorPoint.y && (double) Math.Abs(position.x - this._floorPoint.x) <= (double) this.XExtents && (double) Math.Abs(position.y - this._floorPoint.y) <= (double) this.YExtents;
    }

    private Vector2 CalculateReflectionPosition(Vector2 position, Sprite mirroredSprite)
    {
      float y = this._floorPoint.y;
      float num = position.y - y;
      return new Vector2(position.x, (float) ((double) y - (double) num - (double) mirroredSprite.GetRect().Size.y / 2.0)) + this.ReflectionOffset;
    }
  }
}
