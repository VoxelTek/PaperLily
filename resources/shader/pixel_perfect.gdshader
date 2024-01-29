shader_type canvas_item;

vec4 texturePointSmooth(sampler2D tex, vec2 uv) {
	vec2 size = vec2(textureSize(tex, 0));
	vec2 pixel = vec2(1.0) / size;
	uv *= size - vec2(0.5);
	vec2 delta_pixel = fract(uv) - vec2(0.5);
	vec2 ddxy = fwidth(uv);
	vec2 mip = log2(ddxy) - 0.5;
	return textureLod(tex, (uv + clamp(delta_pixel / ddxy, -0.5, 0.5) + vec2(0.5) - delta_pixel) * pixel, min(mip.x, mip.y));
}

void fragment() {
	vec4 Texture = texturePointSmooth(TEXTURE, UV);
	COLOR = Texture.rgba;
}
