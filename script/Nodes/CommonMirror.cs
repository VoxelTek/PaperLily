using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

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
		public Vector2 ReflectionOffset = new Vector2(0f, 0f);

		[Export(PropertyHint.None, "")]
		public bool OverrideColorOn;

		[Export(PropertyHint.None, "")]
		public Color OverrideColor = Colors.White;

		private Dictionary<string, Sprite> _mirroredSprites = new Dictionary<string, Sprite>();

		private Vector2 _floorPoint = Vector2.Zero;

		public override void _Ready()
		{
			base._Ready();
			base.Color = (OverrideColorOn ? OverrideColor : Game.Room.Modulate);
			base.Mode = ModeEnum.Mix;
			base.RangeItemCullMask = 4;
			_floorPoint = new Vector2(base.GlobalPosition.x, base.GlobalPosition.y + (float)DistanceToFloor + base.Texture.GetSize().y * base.TextureScale * base.Transform.Scale.y / 2f);
		}

		public override void _Process(float delta)
		{
			if (!OverrideColorOn)
			{
				base.Color = Game.Room.Modulate;
			}
			foreach (IReflectable mirrorReflection in Game.Room.MirrorReflections)
			{
				SpriteState sprite = mirrorReflection.GetReflectedSprite();
				if (_mirroredSprites.ContainsKey(sprite.Name))
				{
					UpdateReflection(sprite);
				}
				else
				{
					CreateReflection(sprite);
				}
			}
		}

		private void CreateReflection(SpriteState sprite)
		{
			if (IsInReflectableArea(sprite.Pos))
			{
				Sprite mirroredSprite = new Sprite();
				mirroredSprite.Scale = new Vector2(-1f / base.Scale.x, 1f / base.Scale.y);
				mirroredSprite.Material = GD.Load<Material>("res://resources/material/mirror_reflection.tres");
				mirroredSprite.LightMask = 4;
				AddChild(mirroredSprite);
				_mirroredSprites[sprite.Name] = mirroredSprite;
				UpdateReflection(sprite);
			}
		}

		private void UpdateReflection(SpriteState sprite)
		{
			if (IsInReflectableArea(sprite.Pos))
			{
				Sprite mirroredSprite = _mirroredSprites[sprite.Name];
				mirroredSprite.Visible = true;
				mirroredSprite.Texture = sprite.Texture;
				mirroredSprite.Hframes = sprite.HFrames;
				mirroredSprite.Vframes = sprite.VFrames;
				mirroredSprite.Frame = sprite.Frame;
				mirroredSprite.GlobalPosition = CalculateReflectionPosition(sprite.Pos, mirroredSprite);
			}
			else
			{
				_mirroredSprites[sprite.Name].Visible = false;
			}
		}

		private bool IsInReflectableArea(Vector2 position)
		{
			if (position.y > _floorPoint.y && Math.Abs(position.x - _floorPoint.x) <= (float)XExtents)
			{
				return Math.Abs(position.y - _floorPoint.y) <= (float)YExtents;
			}
			return false;
		}

		private Vector2 CalculateReflectionPosition(Vector2 position, Sprite mirroredSprite)
		{
			float floorPoint = _floorPoint.y;
			float distanceToMirror = position.y - floorPoint;
			return new Vector2(position.x, floorPoint - distanceToMirror - mirroredSprite.GetRect().Size.y / 2f) + ReflectionOffset;
		}
	}
}
