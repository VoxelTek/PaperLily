using System;
using Godot;
using LacieEngine.API;

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
			GD.PrintErr("ERROR: ", string.Concat(message));
		}

		public void Warn(params object[] message)
		{
			GD.Print("WARN: ", string.Concat(message));
		}

		public void Info(params object[] message)
		{
			GD.Print("INFO: ", string.Concat(message));
		}

		public void Debug(params object[] message)
		{
			GD.Print("DEBUG: ", string.Concat(message));
		}

		public void Trace(params object[] message)
		{
			GD.Print("TRACE: ", string.Concat(message));
		}

		public void Exception(Exception exception, params object[] message)
		{
			GD.PrintErr("ERROR: ", string.Concat(message), "\n", exception.Message);
		}

		public void RotateLogLevel()
		{
		}
	}
}
