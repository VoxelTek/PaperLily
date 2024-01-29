using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Characters
{
	public class Character : ICharacter
	{
		public class CharacterStateSprites
		{
			public Texture Texture;

			public Texture NormalMap;

			public string TextureVariation;

			public int Hframes;

			public int Vframes;

			public Vector2? OffsetOverride;

			public bool Animated;

			public int LeftFrame;

			public int UpFrame;

			public int RightFrame;

			public int DownFrame;

			public float AnimationFps;

			public string AnimationLeftFrames;

			public string AnimationUpFrames;

			public string AnimationRightFrames;

			public string AnimationDownFrames;

			public bool Loop;

			public CharacterStateSprites()
			{
			}

			public CharacterStateSprites(CharacterStateSprites source)
			{
				Texture = source.Texture;
				NormalMap = source.NormalMap;
				TextureVariation = source.TextureVariation;
				Hframes = source.Hframes;
				Vframes = source.Vframes;
				OffsetOverride = source.OffsetOverride;
				Animated = source.Animated;
				LeftFrame = source.LeftFrame;
				UpFrame = source.UpFrame;
				RightFrame = source.RightFrame;
				DownFrame = source.DownFrame;
				AnimationFps = source.AnimationFps;
				AnimationLeftFrames = source.AnimationLeftFrames;
				AnimationUpFrames = source.AnimationUpFrames;
				AnimationRightFrames = source.AnimationRightFrames;
				AnimationDownFrames = source.AnimationDownFrames;
				Loop = source.Loop;
			}

			public bool HasDirection(Direction direction)
			{
				if (!Animated)
				{
					return true;
				}
				return direction.ToEnum() switch
				{
					Direction.Enum.Left => !AnimationLeftFrames.IsNullOrEmpty(), 
					Direction.Enum.Up => !AnimationUpFrames.IsNullOrEmpty(), 
					Direction.Enum.Right => !AnimationRightFrames.IsNullOrEmpty(), 
					Direction.Enum.Down => !AnimationDownFrames.IsNullOrEmpty(), 
					_ => false, 
				};
			}
		}

		public Dictionary<string, CharacterStateSprites> StateSprites = new Dictionary<string, CharacterStateSprites>();

		public static readonly CharacterStateSprites DefaultStand = new CharacterStateSprites
		{
			Hframes = 9,
			Vframes = 4,
			LeftFrame = 9,
			UpFrame = 27,
			RightFrame = 18,
			DownFrame = 0
		};

		public static readonly CharacterStateSprites DefaultWalk = new CharacterStateSprites
		{
			Hframes = 9,
			Vframes = 4,
			Animated = true,
			AnimationFps = 6f,
			AnimationLeftFrames = "10,9,11,9",
			AnimationUpFrames = "28,27,29,27",
			AnimationRightFrames = "19,18,20,18",
			AnimationDownFrames = "1,0,2,0",
			Loop = true
		};

		public static readonly CharacterStateSprites DefaultRun = new CharacterStateSprites
		{
			Hframes = 9,
			Vframes = 4,
			Animated = true,
			AnimationFps = 10f,
			AnimationLeftFrames = "12,13,14,15,16,17",
			AnimationUpFrames = "30,31,32,33,34,35",
			AnimationRightFrames = "21,22,23,24,25,26",
			AnimationDownFrames = "3,4,5,6,7,8",
			Loop = true
		};

		public string Id { get; set; }

		public string Name { get; set; }

		public string OriginalName { get; set; }

		public bool IdleAnimation { get; set; }

		public List<string> Costumes { get; set; } = new List<string>();


		public List<string> Moods { get; set; } = new List<string>();


		public Character()
		{
			Moods = new List<string>();
			IdleAnimation = false;
		}

		public void UpdateSpriteState(Node node, string state)
		{
			if (!(node is CharacterSprite sprite))
			{
				throw new NotSupportedException("Only CharacterSprite is currently supported.");
			}
			CharacterStateSprites stateSprites = StateSprites[state];
			sprite.Playing = false;
			sprite.Frame = 0;
			sprite.Texture = stateSprites.Texture;
			sprite.Hframes = stateSprites.Hframes;
			sprite.Vframes = stateSprites.Vframes;
			sprite.Offset = stateSprites.OffsetOverride ?? (Vector2.Up * sprite.Texture.GetSize().y / sprite.Vframes / 2f);
			sprite.Loop = stateSprites.Loop;
			sprite.FPS = stateSprites.AnimationFps;
			sprite.AnimationFrames = null;
			if (stateSprites.HasDirection(sprite.Direction))
			{
				UpdateSpriteDirection(node, sprite.Direction, state);
			}
			else
			{
				sprite.Direction = Direction.None;
			}
			sprite.Playing = stateSprites.Animated;
		}

		public void UpdateSpriteDirection(Node node, Direction direction, string state)
		{
			if (!(node is CharacterSprite sprite))
			{
				throw new NotSupportedException("Only CharacterSprite is currently supported.");
			}
			sprite.Direction = direction;
			CharacterStateSprites stateSprites = StateSprites[state];
			if (!stateSprites.Animated)
			{
				CharacterSprite characterSprite = sprite;
				characterSprite.Frame = direction.ToEnum() switch
				{
					Direction.Enum.Left => stateSprites.LeftFrame, 
					Direction.Enum.Up => stateSprites.UpFrame, 
					Direction.Enum.Right => stateSprites.RightFrame, 
					Direction.Enum.Down => stateSprites.DownFrame, 
					Direction.Enum.None => stateSprites.DownFrame, 
					_ => throw new NotSupportedException("Character sprite must be 4-directional."), 
				};
			}
			else
			{
				sprite.Playing = false;
				CharacterSprite characterSprite = sprite;
				characterSprite.AnimationFrames = direction.ToEnum() switch
				{
					Direction.Enum.Left => stateSprites.AnimationLeftFrames, 
					Direction.Enum.Up => stateSprites.AnimationUpFrames, 
					Direction.Enum.Right => stateSprites.AnimationRightFrames, 
					Direction.Enum.Down => stateSprites.AnimationDownFrames, 
					Direction.Enum.None => stateSprites.AnimationDownFrames, 
					_ => throw new NotSupportedException("Character sprite must be 4-directional."), 
				};
				sprite.Playing = true;
			}
		}
	}
}
