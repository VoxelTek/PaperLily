// Blink transition shader version 1.0 by Mono
shader_type canvas_item;
render_mode blend_mix;

uniform float progress : hint_range(0.0, 1.0) = 0.0;
uniform float blur : hint_range(0.0, 1.0) = 0.45;

void fragment() {
    float alpha = texture(TEXTURE, UV).r;
    alpha = (alpha - progress + (1.0 - progress) * blur) / blur;
    alpha = clamp(alpha, 0.0, 1.0);
    COLOR = vec4(0.0, 0.0, 0.0, alpha);
}
