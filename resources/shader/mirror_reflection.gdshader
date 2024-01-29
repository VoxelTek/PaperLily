shader_type canvas_item;
render_mode light_only;

void fragment() {
    vec4 x = texture(TEXTURE, UV);
	
    x.r = x.r * 0.80;
    //x.g = x.g * 1.0;
    //x.b = x.b * 1.0;
    x.rgb = mix(vec3(0.5), x.rgb, 0.85);
    x.rgb = mix(vec3(0.0), x.rgb, 1.2);
	
	COLOR = x;
}
