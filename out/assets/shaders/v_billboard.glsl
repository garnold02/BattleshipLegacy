#version 330

layout (location=0) in vec3 vPos;
layout (location=1) in vec3 vNorm;
layout (location=2) in vec2 vUv;

layout (location=3) in vec4 iTransformation;
layout (location=4) in vec4 iColor;

uniform mat4 translation;
uniform mat4 rotation;
uniform mat4 projection;

out vec2 vertexUv;
out vec4 vertexColor;

void main() {
    vec4 camSpaceBillboardPos = rotation * inverse(translation) * vec4(iTransformation.xyz,1);
    vec4 pPos = vec4(vPos*iTransformation.w,1);
    gl_Position=projection * (camSpaceBillboardPos + pPos);
	vertexUv=vUv;
    vertexColor = iColor;
}