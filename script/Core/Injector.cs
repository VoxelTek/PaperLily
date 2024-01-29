using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	public class Injector
	{
		private static Dictionary<Type, List<Type>> _allImpls;

		private static Dictionary<Type, Type> _implementations;

		private static Dictionary<Type, object> _instances;

		public static void Init()
		{
			_allImpls = new Dictionary<Type, List<Type>>();
			_implementations = new Dictionary<Type, Type>();
			_instances = new Dictionary<Type, object>();
			List<Type> bannedImplementations = new List<Type>();
			foreach (Type type in DrkieUtil.GetTypesWithAttribute(typeof(Injectable)))
			{
				Type[] interfaces = type.GetInterfaces();
				foreach (Type interface3 in interfaces)
				{
					if (interface3.GetCustomAttributes(typeof(InjectableInterface), inherit: true).Length != 0)
					{
						if (!_allImpls.ContainsKey(interface3))
						{
							_allImpls[interface3] = new List<Type>();
						}
						_allImpls[interface3].Add(type);
					}
				}
			}
			foreach (Type interface2 in _allImpls.Keys)
			{
				int curPriority = -1;
				Type selectedImpl = null;
				foreach (Type implementation2 in _allImpls[interface2])
				{
					int priority = implementation2.GetCustomAttribute<Injectable>().priority;
					string condition = implementation2.GetCustomAttribute<Injectable>().condition;
					if (priority > curPriority && (condition.IsNullOrEmpty() || OS.HasFeature(condition)))
					{
						curPriority = priority;
						selectedImpl = implementation2;
					}
				}
				if (selectedImpl != null)
				{
					_implementations[interface2] = selectedImpl;
				}
				else
				{
					_allImpls.Remove(interface2);
				}
				if (!interface2.GetCustomAttribute<InjectableInterface>().unique)
				{
					continue;
				}
				foreach (Type implementation in _allImpls[interface2])
				{
					if (implementation != selectedImpl)
					{
						bannedImplementations.Add(implementation);
					}
				}
			}
			foreach (Type bannedImpl in bannedImplementations)
			{
				foreach (List<Type> value in _allImpls.Values)
				{
					value.RemoveAll((Type j) => j == bannedImpl);
				}
			}
			foreach (Type @interface in DrkieUtil.GetTypesWithAttribute(typeof(InjectableInterface)))
			{
				if (!_implementations.ContainsKey(@interface))
				{
					Log.Warn("Interface ", @interface.Name, " has no implementations!");
				}
			}
		}

		public static T Get<T>()
		{
			return (T)Get(typeof(T));
		}

		public static List<T> GetAll<T>()
		{
			Type @interface = typeof(T);
			List<T> implementations = new List<T>();
			if (!_allImpls.ContainsKey(@interface))
			{
				Log.Error("No implementations found for ", @interface.Name);
				return implementations;
			}
			foreach (Type implType in _allImpls[@interface])
			{
				implementations.Add((T)GetInstance(implType));
			}
			return implementations;
		}

		public static void Resolve(object obj)
		{
			FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (FieldInfo field in fields)
			{
				if (Attribute.IsDefined(field, typeof(Inject)))
				{
					object impl = Get(field.FieldType);
					if (impl != null)
					{
						field.SetValue(obj, impl);
					}
				}
			}
		}

		private static object Get(Type @interface)
		{
			if (@interface.GetCustomAttributes(typeof(InjectableInterface), inherit: true).Length == 0)
			{
				Log.Error("Interface ", @interface.Name, " is not an injectable target!");
				return null;
			}
			if (!_implementations.ContainsKey(@interface))
			{
				Log.Error("No implementations found for ", @interface.Name);
				return null;
			}
			return GetInstance(_implementations[@interface]);
		}

		private static object GetInstance(Type @class)
		{
			if (_instances.ContainsKey(@class))
			{
				return _instances[@class];
			}
			object instance = Activator.CreateInstance(@class);
			Resolve(instance);
			_instances[@class] = instance;
			return instance;
		}
	}
}
