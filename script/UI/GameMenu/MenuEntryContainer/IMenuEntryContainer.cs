// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.IMenuEntryContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public interface IMenuEntryContainer
  {
    Frame Control { get; }

    IMenuEntryList Menu { get; set; }

    Action OnClose { get; set; }

    void Clear() => this.Control.ClearFrame();

    void AddToFrame(Godot.Control child) => this.Control.AddToFrame((Node) child);
  }
}
