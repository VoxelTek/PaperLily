using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x02000149 RID: 329
    public class SlideOutLeftAnimation : SlideOutAnimation
    {
        // Token: 0x06000B68 RID: 2920 RVA: 0x0002D7D6 File Offset: 0x0002B9D6
        public SlideOutLeftAnimation(Control node, float? displacement = null, Vector2? from = null, float duration = 0.15f)
            : base(node, displacement, from, duration)
        {
        }

        // Token: 0x06000B69 RID: 2921 RVA: 0x0002D7E4 File Offset: 0x0002B9E4
        public override void InitialCalculations()
        {
            this._from = this.pFrom ?? this._node.RectPosition;
            this._to = this._from - new Vector2(this._pDisplacement ?? this._node.RectSize.x, 0f);
            this._track = this._to - this._from;
        }
    }
}
