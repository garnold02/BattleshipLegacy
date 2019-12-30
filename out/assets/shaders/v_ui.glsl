#version 330

layout (location=0) in vec3 vPos;
layout (location=2) in vec2 vUv;

uniform mat4 projection;
uniform vec4 transformation;
uniform vec4 uvTransformation;

out vec3 vertexPosition;
out vec2 vertexUv;

void main() {
    vec2 scaledUv = uvTransformation.xy + vec2(1-vUv.x,1-vUv.y) * uvTransformation.zw;
    gl_Position= projection * vec4((vPos.xy*transformation.zw+transformation.xy),0,1);

    vertexPosition = vPos;
    vertexUv = scaledUv;
}