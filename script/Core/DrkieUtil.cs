// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.DrkieUtil
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Core
{
  public static class DrkieUtil
  {
    private static Random Random = new Random();

    public static float EaseOutQuad(float x)
    {
      return (float) (1.0 - (1.0 - (double) x) * (1.0 - (double) x));
    }

    public static float EaseInCubic(float x) => x * x * x;

    public static Task DelaySeconds(double seconds) => Task.Delay(TimeSpan.FromSeconds(seconds));

    public static string FormatString(string message, params string[] args)
    {
      if (args != null)
      {
        if (args.Length != 0)
        {
          try
          {
            return string.Format(message, (object[]) args);
          }
          catch (FormatException ex)
          {
            Log.Error((object) "Expected string with composite formatting: ", (object) message);
            return message;
          }
        }
      }
      return message;
    }

    public static IEnumerable<Type> GetTypesWithAttribute(Type attribute, bool inherit = false)
    {
      Type[] typeArray = Assembly.GetExecutingAssembly().GetTypes();
      for (int index = 0; index < typeArray.Length; ++index)
      {
        Type element = typeArray[index];
        if (Attribute.IsDefined((MemberInfo) element, attribute, inherit))
          yield return element;
      }
      typeArray = (Type[]) null;
    }

    public static bool TossCoin() => DrkieUtil.Random.NextDouble() < 0.5;

    public static bool RollPercent(double successChance)
    {
      return DrkieUtil.Random.NextDouble() < successChance / 100.0;
    }
  }
}
