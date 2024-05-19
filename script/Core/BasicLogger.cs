// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.BasicLogger
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Injectable(priority = -1)]
  public class BasicLogger : ILogger
  {
	private const string TRACE = "TRACE: ";
	private const string DEBUG = "DEBUG: ";
	private const string INFO = "INFO: ";
	private const string WARN = "WARN: ";
	private const string ERROR = "ERROR: ";

	public void Error(params object[] message)
	{
	  GD.PrintErr((object) "ERROR: ", (object) string.Concat(message));
	}

	public void Warn(params object[] message)
	{
	  GD.Print((object) "WARN: ", (object) string.Concat(message));
	}

	public void Info(params object[] message)
	{
	  GD.Print((object) "INFO: ", (object) string.Concat(message));
	}

	public void Debug(params object[] message)
	{
	  GD.Print((object) "DEBUG: ", (object) string.Concat(message));
	}

	public void Trace(params object[] message)
	{
	  GD.Print((object) "TRACE: ", (object) string.Concat(message));
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
