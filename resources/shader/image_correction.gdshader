shader_type canvas_item;

uniform float brightness = 1.0;
uniform float contrast = 1.0;
uniform float saturation = 1.0;
uniform float gamma = 0.0;

void fragment() {
	vec3 c = textureLod(SCREEN_TEXTURE, SCREEN_UV, 0.0).rgb;
	
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
	
	COLOR.rgb = c;
}
