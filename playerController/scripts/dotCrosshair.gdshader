shader_type canvas_item;
uniform vec4 crosshair_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);
uniform float radius : hint_range(0.0, 0.15, 0.001) = 0.06;
uniform float opacity : hint_range(0.0, 1.0, 0.01) = 0.8;

void fragment() {

    vec2 uv = UV - vec2(0.5);
    float dist = length(uv);

    if (dist < radius) {
        COLOR = vec4(crosshair_color.rgb, crosshair_color.a * opacity);
    } else {
        discard;
    }
}
