shader_type canvas_item;

void fragment() {
    float time = TIME * 24.0 * 3.14;
    if (sin(time) > 0.0)
        COLOR = texture(TEXTURE, UV);
    else
        COLOR = vec4(0,0,0,0);
}
