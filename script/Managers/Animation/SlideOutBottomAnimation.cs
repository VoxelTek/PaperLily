using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x02000148 RID: 328
    public class SlideOutBottomAnimation : SlideOutAnimation
    {
        // Token: 0x06000B66 RID: 2918 RVA: 0x0002D737 File Offset: 0x0002B937
        public SlideOutBottomAnimation(Control node, float? displacement = null, Vector2? from = null, float duration = 0.15f)
            : base(node, displacement, from, duration)
        {
        }

        // Token: 0x06000B67 RID: 2919 RVA: 0x0002D744 File Offset: 0x0002B944
        public override void InitialCalculations()
        {
            this._from = this.pFrom ?? this._node.RectPosition;
            this._to = this._from + new Vector2(0f, this._pDisplacement ?? this._node.RectSize.y);
            this._track = this._to - this._from;
        }
    }
}
