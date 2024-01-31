// Decompiled with JetBrains decompiler
// Type: LacieEngine.Characters.Character
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Characters
{
  public class Character : ICharacter
  {
    public Dictionary<string, Character.CharacterStateSprites> StateSprites = new Dictionary<string, Character.CharacterStateSprites>();
    public static readonly Character.CharacterStateSprites DefaultStand = new Character.CharacterStateSprites()
    {
      Hframes = 9,
      Vframes = 4,
      LeftFrame = 9,
      UpFrame = 27,
      RightFrame = 18,
      DownFrame = 0
    };
    public static readonly Character.CharacterStateSprites DefaultWalk = new Character.CharacterStateSprites()
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
    public static readonly Character.CharacterStateSprites DefaultRun = new Character.CharacterStateSprites()
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
      this.Moods = new List<string>();
      this.IdleAnimation = false;
    }

    public void UpdateSpriteState(Node node, string state)
    {
      if (!(node is CharacterSprite characterSprite))
        throw new NotSupportedException("Only CharacterSprite is currently supported.");
      Character.CharacterStateSprites stateSprite = this.StateSprites[state];
      characterSprite.Playing = false;
      characterSprite.Frame = 0;
      characterSprite.Texture = stateSprite.Texture;
      characterSprite.Hframes = stateSprite.Hframes;
      characterSprite.Vframes = stateSprite.Vframes;
      characterSprite.Offset = stateSprite.OffsetOverride ?? Vector2.Up * characterSprite.Texture.GetSize().y / (float) characterSprite.Vframes / 2f;
      characterSprite.Loop = stateSprite.Loop;
      characterSprite.FPS = stateSprite.AnimationFps;
      characterSprite.AnimationFrames = (string) null;
      if (stateSprite.HasDirection((Direction) characterSprite.Direction))
        this.UpdateSpriteDirection(node, (Direction) characterSprite.Direction, state);
      else
        characterSprite.Direction = (Direction.Enum) Direction.None;
      characterSprite.Playing = stateSprite.Animated;
    }

    public void UpdateSpriteDirection(Node node, Direction direction, string state)
    {
      if (!(node is CharacterSprite characterSprite1))
        throw new NotSupportedException("Only CharacterSprite is currently supported.");
      characterSprite1.Direction = (Direction.Enum) direction;
      Character.CharacterStateSprites stateSprite = this.StateSprites[state];
      if (!stateSprite.Animated)
      {
        CharacterSprite characterSprite2 = characterSprite1;
        int num;
        switch (direction.ToEnum())
        {
          case Direction.Enum.None:
            num = stateSprite.DownFrame;
            break;
          case Direction.Enum.Left:
            num = stateSprite.LeftFrame;
            break;
          case Direction.Enum.Up:
            num = stateSprite.UpFrame;
            break;
          case Direction.Enum.Right:
            num = stateSprite.RightFrame;
            break;
          case Direction.Enum.Down:
            num = stateSprite.DownFrame;
            break;
          default:
            throw new NotSupportedException("Character sprite must be 4-directional.");
        }
        characterSprite2.Frame = num;
      }
      else
      {
        characterSprite1.Playing = false;
        CharacterSprite characterSprite3 = characterSprite1;
        string str;
        switch (direction.ToEnum())
        {
          case Direction.Enum.None:
            str = stateSprite.AnimationDownFrames;
            break;
          case Direction.Enum.Left:
            str = stateSprite.AnimationLeftFrames;
            break;
          case Direction.Enum.Up:
            str = stateSprite.AnimationUpFrames;
            break;
          case Direction.Enum.Right:
            str = stateSprite.AnimationRightFrames;
            break;
          case Direction.Enum.Down:
            str = stateSprite.AnimationDownFrames;
            break;
          default:
            throw new NotSupportedException("Character sprite must be 4-directional.");
        }
        characterSprite3.AnimationFrames = str;
        characterSprite1.Playing = true;
      }
    }

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

      public CharacterStateSprites(Character.CharacterStateSprites source)
      {
        this.Texture = source.Texture;
        this.NormalMap = source.NormalMap;
        this.TextureVariation = source.TextureVariation;
        this.Hframes = source.Hframes;
        this.Vframes = source.Vframes;
        this.OffsetOverride = source.OffsetOverride;
        this.Animated = source.Animated;
        this.LeftFrame = source.LeftFrame;
        this.UpFrame = source.UpFrame;
        this.RightFrame = source.RightFrame;
        this.DownFrame = source.DownFrame;
        this.AnimationFps = source.AnimationFps;
        this.AnimationLeftFrames = source.AnimationLeftFrames;
        this.AnimationUpFrames = source.AnimationUpFrames;
        this.AnimationRightFrames = source.AnimationRightFrames;
        this.AnimationDownFrames = source.AnimationDownFrames;
        this.Loop = source.Loop;
      }

      public bool HasDirection(Direction direction)
      {
        if (!this.Animated)
          return true;
        bool flag;
        switch (direction.ToEnum())
        {
          case Direction.Enum.Left:
            flag = !this.AnimationLeftFrames.IsNullOrEmpty();
            break;
          case Direction.Enum.Up:
            flag = !this.AnimationUpFrames.IsNullOrEmpty();
            break;
          case Direction.Enum.Right:
            flag = !this.AnimationRightFrames.IsNullOrEmpty();
            break;
          case Direction.Enum.Down:
            flag = !this.AnimationDownFrames.IsNullOrEmpty();
            break;
          default:
            flag = false;
            break;
        }
        return flag;
      }
    }
  }
}
