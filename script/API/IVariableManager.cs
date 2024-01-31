// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IVariableManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IVariableManager : IModule
  {
    string GetVariable(string variableName);

    bool GetFlag(string flagName);

    int GetIntVariable(string variableName);

    void SetVariable(string variableName, string value);

    void SetFlag(string variableName, bool value);

    void ToggleFlag(string variableName);

    void SetInitialValue(string variableName, string initialValue);

    bool IsValid(string variableId);

    void Validate(string variableId);
  }
}
