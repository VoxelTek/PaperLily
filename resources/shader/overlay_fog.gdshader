// Fog Shader by Mono
shader_type canvas_item;

uniform vec4 color : hint_color;
uniform bool manual_time_enabled = false;
uniform float manual_time; 
uniform vec2 fog_flow_speed = vec2(3.0, 0.0);
uniform float fog_chaos_speed = 2.0;
uniform float fps_limit = 12.0; // I recommend either 12 or 8

uniform sampler2D noise;
uniform sampler2D dithering;

varying vec2 vert;

void vertex() {
	vert = VERTEX;
}

float get_noise_value(vec2 at, float time) {
	vec2 ft = fog_flow_speed * time;
	vec2 aft = at + ft;
	
	// Noise channels
	float noise_1 = texture(noise, aft / 412.0).r;

	float noise_2 = texture(
		noise, (aft + ft * 1.3 * fog_chaos_speed) / 462.0
		).g;

	float noise_3 = texture(
		noise, (aft + ft * 1.94 * fog_chaos_speed) / 512.0
		).b;

	return (noise_1 + noise_2 + noise_3) / 3.0;
}

void fragment() {
	float u_time = manual_time_enabled ? manual_time : TIME;
	float ff_time = floor(u_time * fps_limit) / fps_limit; // Fps limited time.
	float noise_value = get_noise_value(floor(vert), ff_time);
	
	float transparency;
	
	if (noise_value < 0.65) {
		transparency = 0.0;
	} else if (noise_value < 0.75) {
		transparency = texture(dithering, vert * vec2(0.5)).r;
	} else {
		transparency = 1.0;
	}
	
	COLOR = color * vec4(vec3(1.0), transparency);
}
