#version 330 core

out vec4 FragColor;
  
in vec4 vertexColor;
in vec3 vertexPosition;
in vec3 vertexNormal;
in vec2 vertexUv;

in mat4 translationMatrix;
in mat4 rotationMatrix;
in mat4 scaleMatrix;

uniform sampler2D tex0;
uniform bool useTexture;
uniform vec4 matColor;
uniform vec2 matTiling;

void main()
{
    if(useTexture)
    {
        vec4 texColor = texture(tex0, vertexUv*matTiling);
        FragColor = texColor * matColor;
    }else
    {
        FragColor = matColor;
    }
} 