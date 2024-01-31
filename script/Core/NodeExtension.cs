// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.NodeExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Core
{
  public static class NodeExtension
  {
    public static Node[] GetSiblings(this Node node)
    {
      Godot.Collections.Array children = node.GetParent().GetChildren();
      children.Remove((object) node);
      return children.ToArray<Node>();
    }

    public static void AddFirst(this Node node, Node childNode)
    {
      node.AddChild(childNode);
      node.MoveChild(childNode, 0);
    }

    public static void AddChildDeferred(this Node node, Node nodeToAdd)
    {
      node.CallDeferred("add_child", (object) nodeToAdd);
    }

    public static void Delete(this Node node)
    {
      node.GetParent().RemoveChild(node);
      node.QueueFree();
    }

    public static void DeleteIfValid(this Node node)
    {
      if (!node.IsValid())
        return;
      node.Delete();
    }

    public static void Clear(this Node node)
    {
      foreach (Node child in node.GetChildren())
      {
        node.RemoveChild(child);
        child.QueueFree();
      }
    }

    public static void Reparent(this Node node, Node newParent)
    {
      node.GetParent().RemoveChild(node);
      newParent.AddChild(node);
    }

    public static void Pause(this Node node)
    {
      node.SetProcess(false);
      node.SetPhysicsProcess(false);
      node.SetProcessInput(false);
      node.SetProcessInternal(false);
      node.SetProcessUnhandledInput(false);
      node.SetProcessUnhandledKeyInput(false);
      foreach (Node child in node.GetChildren())
        child.Pause();
    }

    public static bool IsValid(this Node node)
    {
      return node != null && Godot.Object.IsInstanceValid((Godot.Object) node) && node.IsInsideTree();
    }

    public static bool IsEmpty(this Node node) => node.GetChildCount() < 1;

    public static async Task DelaySeconds(this Node baseNode, double seconds)
    {
      object[] signal = await baseNode.ToSignal((Godot.Object) baseNode.GetTree().CreateTimer((float) seconds, false), "timeout");
    }

    public static T EnsureNode<T>(this Node node, string name) where T : Node, new()
    {
      T obj = node.GetNodeOrNull<T>((NodePath) name);
      if ((object) obj == null)
      {
        obj = GDUtil.MakeNode<T>(name);
        node.AddChild((Node) obj);
      }
      return obj;
    }

    public static List<T> FindChildren<T>(this Node node) where T : Node
    {
      List<T> collectedNodes = new List<T>();
      CollectTypedChildrenRecursive(node, collectedNodes);
      return collectedNodes;

      static void CollectTypedChildrenRecursive(Node rootNode, List<T> collectedNodes)
      {
        foreach (Node child in rootNode.GetChildren())
        {
          if (child is T obj)
            collectedNodes.Add(obj);
          if (child.GetChildCount() > 0)
            CollectTypedChildrenRecursive(child, collectedNodes);
        }
      }
    }

    public static void InjectNodes(this Node node)
    {
      foreach (FieldInfo field in node.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
      {
        if (Attribute.IsDefined((MemberInfo) field, typeof (GetNode)))
        {
          Node node1 = node.GetNode((NodePath) field.GetCustomAttribute<GetNode>().Path);
          field.SetValue((object) node, (object) node1);
          if (node1 is INodeWithInjections)
          {
            Injector.Resolve((object) node1);
            node1.InjectNodes();
          }
        }
      }
    }
  }
}
