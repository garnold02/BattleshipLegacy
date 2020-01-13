#version 330 core

out vec4 FragColor;
in vec2 vertexUv;

uniform sampler2D tex0;
uniform bool useTexture;
uniform vec4 matColor;
uniform vec2 matTiling;

void main()
{
    if(useTexture)
    {
        FragColor = texture(tex0, vertexUv) * matColor;
    }else{
        FragColor = matColor;
    }
} 