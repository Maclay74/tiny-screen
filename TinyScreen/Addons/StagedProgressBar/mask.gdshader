shader_type canvas_item;
render_mode blend_disabled, unshaded;

uniform sampler2D mask;
uniform bool isMasked = true;

uniform vec2 size = vec2(40.0, 40.0);

varying vec2 vert;

void vertex(){
    vert = VERTEX;
}

void fragment() {	
    vec2 full_uv = vert/size;
	
	vec4 texture_color = texture(mask, full_uv);
	if (texture_color.a > 0.5f && isMasked) {
		discard;
	}
}
