// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerErrorCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.StoryPlayer
{
  public class StoryPlayerErrorCommand : StoryPlayerNoopCommand
  {
    public string Message { get; set; }

    public StoryPlayerErrorCommand(string message) => this.Message = message;

    public StoryPlayerErrorCommand(StoryPlayerCommand block, string message)
    {
      this.IndentLv = block.IndentLv;
      this.RawCommand = block.RawCommand;
      this.Index = block.Index;
      this.Event = block.Event;
      this.Message = message;
    }
  }
}
