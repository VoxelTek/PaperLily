shader_type canvas_item;

uniform float amplitude = 3.0;
uniform float frequency = 2.0;
const float PI = 3.141592;

void vertex() {
    VERTEX.y += amplitude * sin(frequency * PI * TIME) ;
}
