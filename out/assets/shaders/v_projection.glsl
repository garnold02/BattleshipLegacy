#version 330

layout (location=0) in vec3 vPos;
layout (location=1) in vec3 vNorm;
layout (location=2) in vec2 vUv;

uniform mat4 translation;
uniform mat4 rotation;
uniform mat4 scale;
uniform mat4 projection;

out vec4 vertexColor;
out vec3 vertexPosition;
out vec3 vertexNormal;
out vec2 vertexUv;

out mat4 translationMatrix;
out mat4 rotationMatrix;
out mat4 scaleMatrix;

void main() {
    gl_Position=vec4(projection*translation*rotation*scale*vec4(vPos,1));
    vertexColor = vec4(1,1,1,1);
    vertexPosition = vPos;
    vertexNormal = vNorm;
	vertexUv=vUv;

    translationMatrix=translation;
    rotationMatrix=rotation;
    scaleMatrix = scale;
}