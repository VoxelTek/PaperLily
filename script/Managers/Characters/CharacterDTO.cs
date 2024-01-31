// Decompiled with JetBrains decompiler
// Type: LacieEngine.Characters.CharacterDTO
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Characters
{
  internal class CharacterDTO
  {
    public string Name;
    public List<string> Costumes = new List<string>();
    public List<CharacterDTO.CharacterSpriteDTO> Sprites = new List<CharacterDTO.CharacterSpriteDTO>();
    public bool NoDefaultSprites;

    public class CharacterSpriteDTO
    {
      public string State;
      public string TextureSuffix;
      public int Hframes;
      public int Vframes;
      public string LeftFrames;
      public string UpFrames;
      public string RightFrames;
      public string DownFrames;
      public string Offset;
      public float Fps;
      public bool Loop;
    }
  }
}
