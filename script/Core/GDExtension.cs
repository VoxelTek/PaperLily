using Godot;
using Godot.Collections;

namespace LacieEngine.Core
{
	public static class GDExtension
	{
		public static bool IsNullOrEmpty(this NodePath nodePath)
		{
			return nodePath?.IsEmpty() ?? true;
		}

		public static T[] ToArray<T>(this Array godotArray)
		{
			T[] array = new T[godotArray.Count];
			godotArray.CopyTo(array, 0);
			return array;
		}

		public static T[] ToArray<T>(this Array<T> godotArray)
		{
			T[] array = new T[godotArray.Count];
			godotArray.CopyTo(array, 0);
			return array;
		}
	}
}
