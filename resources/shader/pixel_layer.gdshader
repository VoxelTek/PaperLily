shader_type canvas_item;

uniform float brightness = 1.0;
uniform float contrast = 1.0;
uniform float saturation = 1.0;
uniform float gamma = 0.0;

vec4 texturePointSmooth(sampler2D tex, vec2 uv) {
	vec2 size = vec2(textureSize(tex, 0));
	vec2 pixel = vec2(1.0) / size;
	uv *= size - vec2(0.5);
	vec2 delta_pixel = fract(uv) - vec2(0.5);
	vec2 ddxy = fwidth(uv);
	vec2 mip = log2(ddxy) - 0.5;
	return textureLod(tex, (uv + clamp(delta_pixel / ddxy, -0.5, 0.5) + vec2(0.5) - delta_pixel) * pixel, min(mip.x, mip.y));
}

vec4 imageCorrection(vec4 c) {
	//vec4 c = textureLod(tex, uv, 0.0);
	
	if (gamma != 0.0) {
		float gammaCorrection = 10.0 / (gamma + 10.0);
	
		c.r = pow (c.r / 255.0, gammaCorrection);
	    c.g = pow (c.g / 255.0, gammaCorrection);
	    c.b = pow (c.b / 255.0, gammaCorrection);
		
		c.r = 255.0 * c.r;
	    c.g = 255.0 * c.g;
	    c.b = 255.0 * c.b;
	}
	
	if (brightness != 1.0)
		c.rgb = mix(vec3(0.0), c.rgb, brightness);
	if (contrast != 1.0)
		c.rgb = mix(vec3(0.5), c.rgb, contrast);
	if (saturation != 1.0)
		c.rgb = mix(vec3(dot(vec3(1.0), c.rgb) * 0.33333), c.rgb, saturation);
	
	return c;
}

void fragment() {
	vec4 output = texturePointSmooth(TEXTURE, UV);
    output = imageCorrection(output);
	COLOR = output.rgba;
}
