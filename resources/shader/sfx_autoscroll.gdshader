// Based on https://godotshaders.com/shader/scrolling-background/
shader_type canvas_item;

uniform float speed_x = 1.0;
uniform float speed_y = 1.0;

void fragment () {
	COLOR = texture(TEXTURE, vec2(UV.x + TIME * speed_x, UV.y + TIME * speed_y));
}
