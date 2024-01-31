// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.PauseScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class PauseScreen : CenterContainer
  {
    private static readonly string[] ProcessedInputs = new string[1]
    {
      "input_menu"
    };

    public PauseScreen() => this.PauseMode = Node.PauseModeEnum.Process;

    public override void _EnterTree()
    {
      ColorRect darkenerOverlay = UIUtil.CreateDarkenerOverlay();
      this.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      Control infoBox = (Control) UIUtil.CreateInfoBox("system.menu.paused");
      this.AddChild((Node) darkenerOverlay);
      this.AddChild((Node) infoBox);
    }

    public override void _Process(float delta)
    {
      if (this.GetTree().Paused)
        return;
      this.Delete();
    }

    public override void _Input(InputEvent @event)
    {
      switch (Inputs.Handle(@event, Inputs.Processor.Any, PauseScreen.ProcessedInputs))
      {
        case "input_menu":
          this.GetTree().Paused = false;
          break;
      }
    }
  }
}
