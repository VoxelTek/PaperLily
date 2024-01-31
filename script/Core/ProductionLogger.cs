// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ProductionLogger
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Injectable]
  public class ProductionLogger : ILogger
  {
    private const string INFO = "INFO: ";
    private const string ERROR = "ERROR: ";

    public void Error(params object[] message)
    {
      GD.PrintErr((object) "ERROR: ", (object) string.Concat(message));
    }

    public void Warn(params object[] _)
    {
    }

    public void Info(params object[] message)
    {
      GD.Print((object) "INFO: ", (object) string.Concat(message));
    }

    public void Debug(params object[] _)
    {
    }

    public void Trace(params object[] _)
    {
    }

    public void Exception(System.Exception exception, params object[] message)
    {
      GD.PrintErr((object) "ERROR: ", (object) string.Concat(message), (object) "\n", (object) exception.Message);
    }

    public void RotateLogLevel()
    {
    }
  }
}
