// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IInputManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IInputManager : IModule
  {
    event Action InputTypeChanged;
  }
}
