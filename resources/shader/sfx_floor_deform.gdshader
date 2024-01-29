// Meat Shader by Mono
shader_type canvas_item;

const float HALF_PI = 1.570796;
const vec3 luminance_factor = vec3(0.2126, 0.7152, 0.0722);

uniform vec2 flow_speed = vec2(3.0, 0.0);
uniform float fps_limit = 2.0;
uniform float max_luminance = 0.8;
uniform sampler2D noise;

varying vec2 vert;

void vertex() {
	vert = VERTEX;
}

vec2 rotate(vec2 v, float a) {
	float s = sin(a);
	float c = cos(a);
	return vec2((c * v.x) - (s * v.y),(s * v.x) + (c * v.y));
}

float get_luminance_of(vec4 color) {
	vec3 factored = color.rgb * luminance_factor;
	return factored.r + factored.g + factored.b;
}

void fragment() {
	float frame = floor(TIME * fps_limit);
	
	float noise_value = texture(noise, floor(vert + flow_speed * frame) / vec2(144.0)).r;
	
	
	float rotation_angle = frame * HALF_PI;
	rotation_angle = noise_value > 0.5 ? rotation_angle : -rotation_angle;
	vec2 tex_size = vec2(textureSize(TEXTURE, 0));
	
	vec2 pos0 = vert + rotate(vec2(1.0, 0.0), rotation_angle);
	vec2 pos1 = vert + rotate(vec2(0.0, 1.0), rotation_angle);
	vec2 pos2 = vert + rotate(vec2(-1.0, 0.0), rotation_angle);
	vec2 pos3 = vert + rotate(vec2(0.0, -1.0), rotation_angle);
	
	vec4 c0 = texture(TEXTURE, pos0 / tex_size);
	vec4 c1 = texture(TEXTURE, pos1 / tex_size);
	vec4 c2 = texture(TEXTURE, pos2 / tex_size);
	vec4 c3 = texture(TEXTURE, pos3 / tex_size);
	
	c0 = get_luminance_of(c0) < max_luminance * 0.75 ? vec4(0.0) : c0;
	c1 = get_luminance_of(c1) < max_luminance * 0.50 ? vec4(0.0) : c1;
	c2 = get_luminance_of(c2) < max_luminance * 0.25 ? vec4(0.0) : c2;
	c3 = get_luminance_of(c3) < max_luminance * 0.05 ? vec4(0.0) : c3;
	
	COLOR = max(max(max(max(c0, c1), c2), c3), vec4(vec3(0.0), 1.0));
}
