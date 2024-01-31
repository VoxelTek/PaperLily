// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.MessageMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public class MessageMenu : SimpleVerticalMenu
  {
    public MessageMenu(IMenuEntryContainer container)
    {
      this.Container = container;
      this.Entries = new List<IMenuEntry>();
      this.Entries.Add((IMenuEntry) new BackMenuEntry("system.common.ok", (IMenuEntryList) this));
    }
  }
}
