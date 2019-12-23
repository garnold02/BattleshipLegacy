#version 330

layout (location=0) in vec3 vPos;
layout (location=1) in vec3 vNorm;
layout (location=2) in vec2 vUv;

layout (location=3) in vec4 iTransformation;
layout (location=4) in vec4 iColor;

uniform mat4 projection;

out vec2 vertexUv;
out vec4 vertexColor;

void main() {
    vec3 pos = vPos*iTransformation.w + iTransformation.xyz;
    gl_Position=vec4(projection*vec4(pos,1));
	vertexUv=vUv;
    vertexColor = iColor;
}