using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace LacieEngine.Core
{
	public static class DrkieUtil
	{
		private static Random Random = new Random();

		public static float EaseOutQuad(float x)
		{
			return 1f - (1f - x) * (1f - x);
		}

		public static float EaseInCubic(float x)
		{
			return x * x * x;
		}

		public static Task DelaySeconds(double seconds)
		{
			return Task.Delay(TimeSpan.FromSeconds(seconds));
		}

		public static string FormatString(string message, params string[] args)
		{
			if (args == null || args.Length == 0)
			{
				return message;
			}
			try
			{
				return string.Format(message, args);
			}
			catch (FormatException)
			{
				Log.Error("Expected string with composite formatting: ", message);
				return message;
			}
		}

		public static IEnumerable<Type> GetTypesWithAttribute(Type attribute, bool inherit = false)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (Attribute.IsDefined(type, attribute, inherit))
				{
					yield return type;
				}
			}
		}

		public static bool TossCoin()
		{
			return Random.NextDouble() < 0.5;
		}

		public static bool RollPercent(double successChance)
		{
			return Random.NextDouble() < successChance / 100.0;
		}
	}
}
