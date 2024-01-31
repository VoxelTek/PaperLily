// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.PVar
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Core
{
  public class PVar
  {
    public string Id { get; private set; }

    public PVar.PVarGetProxy Value => new PVar.PVarGetProxy(this.Id);

    public PVar.PVarSetProxy NewValue
    {
      set => value.Apply(this.Id);
    }

    private PVar(string variableId) => this.Id = variableId;

    public static implicit operator PVar(string variableId) => new PVar(variableId);

    public struct PVarGetProxy
    {
      private readonly string id;

      internal PVarGetProxy(string variableId) => this.id = variableId;

      public static implicit operator string(PVar.PVarGetProxy val)
      {
        return Game.Variables.GetVariable(val.id);
      }

      public static implicit operator bool(PVar.PVarGetProxy val) => Game.Variables.GetFlag(val.id);

      public static implicit operator int(PVar.PVarGetProxy val)
      {
        return Game.Variables.GetIntVariable(val.id);
      }

      public static bool operator ==(PVar.PVarGetProxy val1, string val2)
      {
        return Game.Variables.GetVariable(val1.id) == val2;
      }

      public static bool operator !=(PVar.PVarGetProxy val1, string val2)
      {
        return Game.Variables.GetVariable(val1.id) != val2;
      }

      public static bool operator ==(PVar.PVarGetProxy val1, bool val2)
      {
        return Game.Variables.GetFlag(val1.id) == val2;
      }

      public static bool operator !=(PVar.PVarGetProxy val1, bool val2)
      {
        return Game.Variables.GetFlag(val1.id) != val2;
      }

      public static bool operator ==(PVar.PVarGetProxy val1, int val2)
      {
        return Game.Variables.GetIntVariable(val1.id) == val2;
      }

      public static bool operator !=(PVar.PVarGetProxy val1, int val2)
      {
        return Game.Variables.GetIntVariable(val1.id) != val2;
      }

      public override bool Equals(object obj)
      {
        switch (obj)
        {
          case string str:
            return this == str;
          case int num:
            return this == num;
          case bool flag:
            return this == flag;
          default:
            return false;
        }
      }

      public override int GetHashCode() => ((string) this).GetHashCode();

      public override string ToString() => ((string) this).ToString();
    }

    public struct PVarSetProxy
    {
      private readonly string value;

      private PVarSetProxy(string variableValue) => this.value = variableValue;

      public static implicit operator PVar.PVarSetProxy(string val) => new PVar.PVarSetProxy(val);

      public static implicit operator PVar.PVarSetProxy(bool val)
      {
        return new PVar.PVarSetProxy(val ? "true" : "false");
      }

      public static implicit operator PVar.PVarSetProxy(int val)
      {
        return new PVar.PVarSetProxy(val.ToString());
      }

      public static implicit operator PVar.PVarSetProxy(Vector2 val)
      {
        return new PVar.PVarSetProxy(val.x.ToString() + "," + val.y.ToString());
      }

      public static implicit operator PVar.PVarSetProxy(PVar.PVarGetProxy val)
      {
        return new PVar.PVarSetProxy((string) val);
      }

      public void Apply(string variableId) => Game.Variables.SetVariable(variableId, this.value);
    }
  }
}
