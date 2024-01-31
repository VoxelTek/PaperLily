// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.UiLookAndFeelProvider
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.UI;

#nullable disable
namespace LacieEngine.Core
{
  public class UiLookAndFeelProvider : Resource
  {
    [Export(PropertyHint.None, "")]
    public PackedScene DefaultFrame { private get; set; }

    [Export(PropertyHint.None, "")]
    public PackedScene ChoicesFrame { private get; set; }

    [Export(PropertyHint.None, "")]
    public PackedScene DialogueBox { private get; set; }

    public Frame MakeDefaultFrame() => this.DefaultFrame.Instance<Frame>();

    public Frame MakeChoicesFrame() => this.ChoicesFrame.Instance<Frame>();

    public DialogueFrameNode MakeDialogueBox() => this.DialogueBox.Instance<DialogueFrameNode>();
  }
}
