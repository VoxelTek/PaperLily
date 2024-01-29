using System;

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

		void Exception(Exception exception, params object[] message);

		void RotateLogLevel();
	}
}
