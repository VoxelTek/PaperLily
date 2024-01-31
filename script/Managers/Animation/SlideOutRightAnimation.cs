using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x0200014A RID: 330
    public class SlideOutRightAnimation : SlideOutAnimation
    {
        // Token: 0x06000B6A RID: 2922 RVA: 0x0002D876 File Offset: 0x0002BA76
        public SlideOutRightAnimation(Control node, float? displacement = null, Vector2? from = null, float duration = 0.15f)
            : base(node, displacement, from, duration)
        {
        }

        // Token: 0x06000B6B RID: 2923 RVA: 0x0002D884 File Offset: 0x0002BA84
        public override void InitialCalculations()
        {
            this._from = this.pFrom ?? this._node.RectPosition;
            this._to = this._from + new Vector2(this._pDisplacement ?? this._node.RectSize.x, 0f);
            this._track = this._to - this._from;
        }
    }
}
