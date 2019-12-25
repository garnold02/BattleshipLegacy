#version 330 core

out vec4 FragColor;
  
in vec4 vertexColor;
in vec2 vertexUv;

uniform sampler2D tex0;

void main()
{
    FragColor = texture(tex0, vertexUv)*vertexColor;
} 