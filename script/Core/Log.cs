// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.Log
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  public class Log
  {
    private static ILogger _logger = (ILogger) new BasicLogger();

    public static void Init() => Log._logger = Injector.Get<ILogger>();

    public static void Error(params object[] message) => Log._logger.Error(message);

    public static void Warn(params object[] message) => Log._logger.Warn(message);

    public static void Info(params object[] message) => Log._logger.Info(message);

    public static void Debug(params object[] message) => Log._logger.Debug(message);

    public static void Trace(params object[] message) => Log._logger.Trace(message);

    public static void Exception(System.Exception exception, params object[] message)
    {
      Log._logger.Exception(exception, message);
    }
  }
}
