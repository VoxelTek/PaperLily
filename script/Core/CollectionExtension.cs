// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.CollectionExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LacieEngine.Core
{
  public static class CollectionExtension
  {
    public static bool IsEmpty<T>(this ICollection<T> collection) => collection.Count == 0;

    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
      return collection == null || collection.IsEmpty<T>();
    }

    public static string ToUsefulString<T>(this ICollection<T> collection)
    {
      return string.Join<T>(",", (IEnumerable<T>) collection);
    }

    public static bool IsEmpty<T>(this Queue<T> collection) => collection.Count == 0;

    public static bool IsNullOrEmpty<T>(this Queue<T> collection)
    {
      return collection == null || collection.IsEmpty<T>();
    }

    public static bool Contains<T>(this IList<T> array, T o)
    {
      foreach (T obj in (IEnumerable<T>) array)
      {
        if (obj.Equals((object) o))
          return true;
      }
      return false;
    }

    public static void RemoveAll<K, V>(
      this IDictionary<K, V> dictionary,
      Func<K, V, bool> predicate)
    {
      foreach (K key in ((IEnumerable<K>) dictionary.Keys.ToArray<K>()).Where<K>((Func<K, bool>) (key => predicate(key, dictionary[key]))))
        dictionary.Remove(key);
    }

    public static void AddRange<T>(this LinkedList<T> linkedList, IEnumerable<T> collection)
    {
      foreach (T obj in collection)
        linkedList.AddLast(obj);
    }

    public static void RemoveAll<T>(this LinkedList<T> linkedList, Func<T, bool> predicate)
    {
      LinkedListNode<T> next;
      for (LinkedListNode<T> node = linkedList.First; node != null; node = next)
      {
        next = node.Next;
        if (predicate(node.Value))
          linkedList.Remove(node);
      }
    }
  }
}
