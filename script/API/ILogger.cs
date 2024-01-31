// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.ILogger
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface ILogger
  {
    void Error(params object[] message);

    void Warn(params object[] message);

    void Info(params object[] message);

    void Debug(params object[] message);

    void Trace(params object[] message);

    void Exception(System.Exception exception, params object[] message);

    void RotateLogLevel();
  }
}
