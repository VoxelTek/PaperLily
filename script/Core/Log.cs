using System;
using LacieEngine.API;

namespace LacieEngine.Core
{
	public class Log
	{
		private static ILogger _logger = new BasicLogger();

		public static void Init()
		{
			_logger = Injector.Get<ILogger>();
		}

		public static void Error(params object[] message)
		{
			_logger.Error(message);
		}

		public static void Warn(params object[] message)
		{
			_logger.Warn(message);
		}

		public static void Info(params object[] message)
		{
			_logger.Info(message);
		}

		public static void Debug(params object[] message)
		{
			_logger.Debug(message);
		}

		public static void Trace(params object[] message)
		{
			_logger.Trace(message);
		}

		public static void Exception(Exception exception, params object[] message)
		{
			_logger.Exception(exception, message);
		}
	}
}
