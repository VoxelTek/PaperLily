// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1MkWisteria
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1MkWisteria : GameRoom
  {
    [LacieEngine.API.GetNode("Background/Canvas")]
    private Sprite nCanvas;
    [LacieEngine.API.GetNode("Background/Canvas/MkDarkness")]
    private Node2D nDarkness;
    [LacieEngine.API.GetNode("Background/Canvas/Painting")]
    private Sprite nPainting;
    [LacieEngine.API.GetNode("Background/WallPainting")]
    private Sprite nItemHolderPainting;
    private PVar varRevealedCanvas = (PVar) "ch1.mk_revealed_canvas";
    private PVar varTookCanvas = (PVar) "ch1.mk_took_canvas";
    private PVar varPainting = (PVar) "ch1.mk_canvas_color";
    private PVar varItemHolderItem = (PVar) "ch1.mk_wisteria_item";

    public override void _UpdateRoom()
    {
      this.nCanvas.Frame = (bool) this.varTookCanvas.Value ? 2 : ((bool) this.varRevealedCanvas.Value ? 1 : 0);
      this.nDarkness.Visible = this.nCanvas.Frame == 1;
      this.nPainting.Visible = !(bool) this.varTookCanvas.Value && (bool) this.varPainting.Value;
      if (this.nPainting.Visible)
      {
        Sprite nPainting = this.nPainting;
        string str = (string) this.varPainting.Value;
        int num;
        if (str != null)
        {
          switch (str.Length)
          {
            case 3:
              switch (str[0])
              {
                case 'a':
                  if (str == "all")
                  {
                    num = 6;
                    goto label_23;
                  }
                  else
                    break;
                case 'r':
                  if (str == "red")
                  {
                    num = 0;
                    goto label_23;
                  }
                  else
                    break;
              }
              break;
            case 4:
              if (str == "blue")
              {
                num = 1;
                goto label_23;
              }
              else
                break;
            case 5:
              switch (str[0])
              {
                case 'b':
                  if (str == "brown")
                  {
                    num = 7;
                    goto label_23;
                  }
                  else
                    break;
                case 'g':
                  if (str == "green")
                  {
                    num = 5;
                    goto label_23;
                  }
                  else
                    break;
              }
              break;
            case 6:
              switch (str[0])
              {
                case 'o':
                  if (str == "orange")
                  {
                    num = 4;
                    goto label_23;
                  }
                  else
                    break;
                case 'p':
                  if (str == "purple")
                  {
                    num = 3;
                    goto label_23;
                  }
                  else
                    break;
                case 'y':
                  if (str == "yellow")
                  {
                    num = 2;
                    goto label_23;
                  }
                  else
                    break;
              }
              break;
          }
        }
        num = 0;
label_23:
        nPainting.Frame = num;
      }
      this.nItemHolderPainting.Frame = (int) this.varItemHolderItem.Value;
    }
  }
}
