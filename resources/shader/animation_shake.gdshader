shader_type canvas_item;

uniform float progress = 0.0;
uniform int shakes = 1;
const float PI = 3.141592;

void vertex() {
    VERTEX.x += 3.0 * sin(progress * 2.0 * PI * float(shakes));
}
