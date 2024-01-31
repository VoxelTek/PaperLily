// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1MkB1fPaintings
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1MkB1fPaintings : GameRoom
  {
    [LacieEngine.API.GetNode("Background/Painting1")]
    private Sprite nPainting1;
    [LacieEngine.API.GetNode("Background/Painting2")]
    private Sprite nPainting2;
    [LacieEngine.API.GetNode("Background/Painting3")]
    private Sprite nPainting3;
    [LacieEngine.API.GetNode("Background/Painting4")]
    private Sprite nPainting4;
    [LacieEngine.API.GetNode("Background/Painting5")]
    private Sprite nPainting5;
    [LacieEngine.API.GetNode("Background/Painting6")]
    private Sprite nPainting6;
    private PVar varPainting1 = (PVar) "ch1.mk_painting_1";
    private PVar varPainting2 = (PVar) "ch1.mk_painting_2";
    private PVar varPainting3 = (PVar) "ch1.mk_painting_3";
    private PVar varPainting4 = (PVar) "ch1.mk_painting_4";
    private PVar varPainting5 = (PVar) "ch1.mk_painting_5";
    private PVar varPainting6 = (PVar) "ch1.mk_painting_6";
    private PEvent evtSuccess = (PEvent) "event_clear";
    private const int PAINTINGS = 6;
    private const int BLESSINGS = 5;
    private PVar[] _vars;
    private int[,] _values = new int[6, 5]
    {
      {
        1,
        3,
        5,
        4,
        2
      },
      {
        4,
        2,
        3,
        1,
        5
      },
      {
        3,
        2,
        1,
        4,
        5
      },
      {
        5,
        2,
        4,
        1,
        3
      },
      {
        3,
        1,
        5,
        4,
        2
      },
      {
        1,
        2,
        5,
        4,
        3
      }
    };

    public override void _BeforeFadeIn()
    {
      this._vars = new PVar[6]
      {
        this.varPainting1,
        this.varPainting2,
        this.varPainting3,
        this.varPainting4,
        this.varPainting5,
        this.varPainting6
      };
    }

    public override void _UpdateRoom()
    {
      if ((bool) this.varPainting1.Value)
        this.nPainting1.Frame = this._values[0, (int) this.varPainting1.Value];
      if ((bool) this.varPainting2.Value)
        this.nPainting2.Frame = this._values[1, (int) this.varPainting2.Value];
      if ((bool) this.varPainting3.Value)
        this.nPainting3.Frame = this._values[2, (int) this.varPainting3.Value];
      if ((bool) this.varPainting4.Value)
        this.nPainting4.Frame = this._values[3, (int) this.varPainting4.Value];
      if ((bool) this.varPainting5.Value)
        this.nPainting5.Frame = this._values[4, (int) this.varPainting5.Value];
      if (!(bool) this.varPainting6.Value)
        return;
      this.nPainting6.Frame = this._values[5, (int) this.varPainting6.Value];
    }

    public void RotatePainting1() => this.RotatePainting(0);

    public void RotatePainting2() => this.RotatePainting(1);

    public void RotatePainting3() => this.RotatePainting(2);

    public void RotatePainting4() => this.RotatePainting(3);

    public void RotatePainting5() => this.RotatePainting(4);

    public void RotatePainting6() => this.RotateFinalPainting();

    private void RotatePainting(int idx)
    {
      int num = (int) this._vars[idx].Value + 1;
      if (num >= 5)
        num = 0;
      this._vars[idx].NewValue = (PVar.PVarSetProxy) num;
    }

    private void RotateFinalPainting()
    {
      if (this._values[0, (int) this.varPainting1.Value] == 1 && this._values[1, (int) this.varPainting2.Value] == 2 && this._values[2, (int) this.varPainting3.Value] == 3 && this._values[3, (int) this.varPainting4.Value] == 4 && this._values[4, (int) this.varPainting5.Value] == 5)
        this.evtSuccess.Play();
      else
        this.RotatePainting(5);
    }
  }
}
