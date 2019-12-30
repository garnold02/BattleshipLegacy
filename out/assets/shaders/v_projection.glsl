#version 330

vec4 projectedPosition(vec3 originalPosition, mat4 projectionMatrix, mat4 translationMatrix, mat4 rotationMatrix, mat4 scaleMatrix)
{
    return vec4(projectionMatrix*translationMatrix*rotationMatrix*scaleMatrix*vec4(originalPosition,1));
}