#version 330
vec4 projectedPosition(vec3 originalPosition, mat4 projectionMatrix, mat4 translationMatrix, mat4 rotationMatrix, mat4 scaleMatrix);

layout (location=0) in vec3 vPos;
layout (location=1) in vec3 vNorm;
layout (location=2) in vec2 vUv;

uniform vec2 matTiling;

uniform mat4 translation;
uniform mat4 rotation;
uniform mat4 scale;
uniform mat4 projection;

out vec3 vertexPosition;
out vec3 vertexNormal;
out vec2 vertexUv;

void main()
{
    gl_Position = projectedPosition(vPos, projection,translation,rotation,scale);

    vertexPosition = vPos;
    vertexNormal = vNorm;
    vertexUv = vUv;
}