// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.Injector
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace LacieEngine.Core
{
  public class Injector
  {
    private static Dictionary<System.Type, List<System.Type>> _allImpls;
    private static Dictionary<System.Type, System.Type> _implementations;
    private static Dictionary<System.Type, object> _instances;

    public static void Init()
    {
      Injector._allImpls = new Dictionary<System.Type, List<System.Type>>();
      Injector._implementations = new Dictionary<System.Type, System.Type>();
      Injector._instances = new Dictionary<System.Type, object>();
      List<System.Type> typeList1 = new List<System.Type>();
      foreach (System.Type type in DrkieUtil.GetTypesWithAttribute(typeof (Injectable)))
      {
        foreach (System.Type key in type.GetInterfaces())
        {
          if (key.GetCustomAttributes(typeof (InjectableInterface), true).Length != 0)
          {
            if (!Injector._allImpls.ContainsKey(key))
              Injector._allImpls[key] = new List<System.Type>();
            Injector._allImpls[key].Add(type);
          }
        }
      }
      foreach (System.Type key in Injector._allImpls.Keys)
      {
        int num = -1;
        System.Type type1 = (System.Type) null;
        foreach (System.Type element in Injector._allImpls[key])
        {
          int priority = element.GetCustomAttribute<Injectable>().priority;
          string condition = element.GetCustomAttribute<Injectable>().condition;
          if (priority > num && (condition.IsNullOrEmpty() || OS.HasFeature(condition)))
          {
            num = priority;
            type1 = element;
          }
        }
        if (type1 != (System.Type) null)
          Injector._implementations[key] = type1;
        else
          Injector._allImpls.Remove(key);
        if (key.GetCustomAttribute<InjectableInterface>().unique)
        {
          foreach (System.Type type2 in Injector._allImpls[key])
          {
            if (type2 != type1)
              typeList1.Add(type2);
          }
        }
      }
      foreach (System.Type type in typeList1)
      {
        System.Type bannedImpl = type;
        foreach (List<System.Type> typeList2 in Injector._allImpls.Values)
          typeList2.RemoveAll((Predicate<System.Type>) (j => j == bannedImpl));
      }
      foreach (System.Type key in DrkieUtil.GetTypesWithAttribute(typeof (InjectableInterface)))
      {
        if (!Injector._implementations.ContainsKey(key))
          Log.Warn((object) "Interface ", (object) key.Name, (object) " has no implementations!");
      }
    }

    public static T Get<T>() => (T) Injector.Get(typeof (T));

    public static List<T> GetAll<T>()
    {
      System.Type key = typeof (T);
      List<T> all = new List<T>();
      if (!Injector._allImpls.ContainsKey(key))
      {
        Log.Error((object) "No implementations found for ", (object) key.Name);
        return all;
      }
      foreach (System.Type @class in Injector._allImpls[key])
        all.Add((T) Injector.GetInstance(@class));
      return all;
    }

    public static void Resolve(object obj)
    {
      foreach (FieldInfo field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
      {
        if (Attribute.IsDefined((MemberInfo) field, typeof (Inject)))
        {
          object obj1 = Injector.Get(field.FieldType);
          if (obj1 != null)
            field.SetValue(obj, obj1);
        }
      }
    }

    private static object Get(System.Type @interface)
    {
      if (@interface.GetCustomAttributes(typeof (InjectableInterface), true).Length == 0)
      {
        Log.Error((object) "Interface ", (object) @interface.Name, (object) " is not an injectable target!");
        return (object) null;
      }
      if (Injector._implementations.ContainsKey(@interface))
        return Injector.GetInstance(Injector._implementations[@interface]);
      Log.Error((object) "No implementations found for ", (object) @interface.Name);
      return (object) null;
    }

    private static object GetInstance(System.Type @class)
    {
      if (Injector._instances.ContainsKey(@class))
        return Injector._instances[@class];
      object instance = Activator.CreateInstance(@class);
      Injector.Resolve(instance);
      Injector._instances[@class] = instance;
      return instance;
    }
  }
}
