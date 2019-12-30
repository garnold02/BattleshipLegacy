#version 330 core

in vec2 vertexUv;

out vec4 FragColor;
uniform vec4 color;
uniform sampler2D tex0;

void main()
{
    FragColor = texture(tex0, vertexUv)*color;
} 