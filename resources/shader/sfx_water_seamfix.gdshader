// Water shader version 1.0 by Mono
shader_type canvas_item;

// Overall appearance
uniform vec4 water_color : hint_color = vec4(0.14, 0.18, 0.27, 1.0);
uniform vec4 water_shine_color : hint_color = vec4(0.41, 0.52, 0.52, 1.0); // Color of both blobs and sparkles
uniform float water_transparency = 0.8;
uniform bool manual_time_enabled = false;
uniform float manual_time = 0.0;
// Blob uniforms
uniform float water_flow_direction_rad = 0.0;
uniform float water_flow_speed = 1.0;
uniform float water_chaos_speed = 2.0;
uniform bool water_still = false;
// Sparkle uniforms
uniform float sparkle_amount = 2.8;
uniform float sparkle_speed = 0.1;
uniform float fps_limit = 12.0; // I recommend either 12 or 8

// Textures for technical purposes
uniform sampler2D blob_noise;
uniform sampler2D sparkle_texture;
uniform sampler2D random_noise;
uniform sampler2D cel_scale;

uniform mat4 camera_matrix;

varying vec2 vert;


void vertex() {
    vert=(camera_matrix * WORLD_MATRIX * vec4(VERTEX, 1.0, 1.0)).xy; // Necessary to work with world coordinates.
}


// This does loop after seed 65535, but it's only used for the row offset of
// the sparkles, so I don't think it matters.
float randf(float seed){
	vec2 read_point = vec2(seed);
	read_point.x += floor(seed) / 256.0;
	return texture(random_noise, read_point / 256.0).r;
}


vec4 randv4(vec2 quadrant, float seed){
	vec4 noise_0 = texture(random_noise, quadrant + quadrant * seed);
	vec4 noise_1 = texture(random_noise, quadrant * 1.766021);
	
	return fract(noise_0 + noise_1);
}


vec2 rotate(vec2 v, float a) {
	float s = sin(a);
	float c = cos(a);
	float tx = v.x;
	float ty = v.y;
	return vec2((c * tx) - (s * ty),(s * tx) + (c * ty));
}


float get_blob_noise_value(vec2 at, vec2 flow_dir, float time) {
	vec2 water_flow_factor = water_still ? vec2(0.0) : vec2(5.0);
	vec2 ft = flow_dir * time;
	vec2 aftss = at + ft * water_flow_speed * water_flow_factor;
	
	// Noise channels
	float noise_1 = texture(blob_noise, aftss / 412.0).r;

	float noise_2 = texture(
		blob_noise, (aftss + ft * water_chaos_speed * 0.8) / 462.0
		).g;

	float noise_3 = texture(
		blob_noise, (aftss + ft * water_chaos_speed * 1.44) / 512.0
		).b;

	return (noise_1 + noise_2 + noise_3);
}


void fragment() {
    vec4 input = texture(TEXTURE, UV);
    if (input.a == 0.0) {
        // Follow the shape of the mask - don't do any work if the mask's pixel is transparent
        COLOR = vec4(0.0,0.0,0.0,0.0);
    } else {
        // Use manual_time if enabled.
        float u_time = manual_time_enabled ? manual_time : TIME;

        float ff_time = floor(u_time * fps_limit) / fps_limit; // Fps limited time. (For sparkles)

        // Calculate stuff for flowing water.
        vec2 water_flow_factor = water_still ? vec2(0.0) : vec2(5.0);
        vec2 flow_dir = rotate(vec2(1.0), water_flow_direction_rad);
        vec2 flow_vector = flow_dir * water_flow_speed * water_flow_factor;
        vec2 pos = vert + u_time * flow_vector;
        vec2 ff_pos = vert + ff_time * flow_vector;

        // Waves/rippling
        vec2 tiled_uvs = vert * 0.09765625;
        vec2 displacement = vec2(
            cos(u_time * 5.0 + tiled_uvs.x + tiled_uvs.y) * 0.001,
            cos(u_time * 0.1 + tiled_uvs.x + tiled_uvs.y) * 0.001
        );

        // Below the surface.
        vec4 refraction = texture(SCREEN_TEXTURE, SCREEN_UV + displacement); // Different zoom factors may affect this effect. Needs further research.
        // The actual water color is a mix of the surface and below.
        COLOR = mix(refraction, water_color, water_transparency);

        // Make blobs.
        float noise_here = get_blob_noise_value(vert, flow_dir, u_time);
        float blob_light = noise_here * 0.1923;

        // Making sparkles from here on.
        vec2 sparkle_size = vec2(textureSize(sparkle_texture, 0)); // Get texture size.

        // Get tile/stepped position. (assuming all sparkles are positioned in a grid)
        vec2 grid_pos;
        grid_pos.y = floor(pos.y / sparkle_size.y);
        float x_offset = randf(grid_pos.y); // Offset each row by a random value.
        pos.x += x_offset * sparkle_size.x;
        grid_pos.x = floor(pos.x / sparkle_size.x);

        vec2 uv = (pos / sparkle_size) - grid_pos;
        vec4 sparkle_indicator = texture(sparkle_texture, uv);

        // Get center of current tile.
        vec2 center = (grid_pos + vec2(0.5 - x_offset, 0.5)) * sparkle_size;

        float noise_at_center = clamp(pow(
            get_blob_noise_value(center - ff_time * flow_vector, flow_dir, ff_time),
            5.6) * 0.00265 * (sparkle_amount + 1.0),
            0.0, 1.0);

        float slowed_time = ff_time * sparkle_speed; // At least it probably is slowed.
        // The following time offset is applied to half the sparkles in order to avoid gabs without any
        // sparkles at all. This seems to be the most efficient solution I found.
        float s_time = slowed_time + fract((grid_pos.x + grid_pos.y) * 0.5); 
        float iteration = floor(s_time);

        vec4 random_color = randv4(grid_pos, iteration); // To vary lifetime, appearance_time, size and visibility of each sparkle.

        float sparkle_lifetime = 0.15 + random_color.r * 0.5; // How long a sparkle will survive.
        float appearance_time = sparkle_indicator.r + random_color.g * 0.5; // When a sparkle will appear.

        // 'squished_uv' is for different sparkle sizes.
        float stepped_uv_x = floor(uv.x * sparkle_size.x) / sparkle_size.x;
        float rand_squish = 1.0 / (0.2 + random_color.b * 0.8);
        vec2 squished_uv = uv;
        squished_uv.x = stepped_uv_x * rand_squish + (1.0 - rand_squish) * 0.5;

        // 'visibility' lets some sparkles appear weaker or not at all.
        // Therefore it affects the amount of sparkles shown on screen.
        float visibility = 0.85 + random_color.a * 0.15;
        visibility *= texture(sparkle_texture, squished_uv).g * sparkle_indicator.a;

        float inv_time = 1.0 - fract(s_time);
        float closeness = sparkle_lifetime - abs(appearance_time - inv_time);
        float lifetime_value = clamp(closeness / sparkle_lifetime, 0.0, 1.0);
        float sparkle_light = lifetime_value * noise_at_center * visibility;
        // Done with making sparkles.

        // apply cel "shading".
        blob_light = texture(cel_scale, vec2(blob_light, 0.0)).r;
        sparkle_light = texture(cel_scale, vec2(sparkle_light, 1.0)).r;
        
        float total_light = max(blob_light, sparkle_light);

        COLOR.rgb += water_shine_color.rgb * total_light;
    }
}
