using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Injectable]
	public class ProductionLogger : ILogger
	{
		private const string INFO = "INFO: ";

		private const string ERROR = "ERROR: ";

		public void Error(params object[] message)
		{
			GD.PrintErr("ERROR: ", string.Concat(message));
		}

		public void Warn(params object[] _)
		{
		}

		public void Info(params object[] message)
		{
			GD.Print("INFO: ", string.Concat(message));
		}

		public void Debug(params object[] _)
		{
		}

		public void Trace(params object[] _)
		{
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
