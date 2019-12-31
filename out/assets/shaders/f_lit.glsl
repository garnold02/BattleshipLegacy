#version 330 core

out vec4 FragColor;

in vec3 vertexNormal;
in vec2 vertexUv;

uniform sampler2D tex0;

uniform mat4 rotation;
uniform bool useTexture;
uniform vec4 matColor;
uniform vec2 matTiling;

uniform float lightAmbient;
uniform vec4 lightColor;
uniform vec3 lightDirection;

void main()
{
    vec3 rotatedNormal = (rotation * vec4(vertexNormal,0)).xyz;

    float diffuse = clamp(dot(-lightDirection, rotatedNormal),0,1);
    float lightValue = clamp(diffuse+lightAmbient,0,1);

    if(useTexture)
    {
        vec4 textureColor = texture(tex0, vertexUv*matTiling); 
        vec4 unlitColor = textureColor * matColor * vec4(lightColor.xyz,1);
        FragColor = vec4(unlitColor.xyz*lightValue*lightColor.w,unlitColor.w);
    }else
    {
        vec4 unlitColor = matColor * vec4(lightColor.xyz,1);
        FragColor = vec4(unlitColor.xyz* lightValue * lightColor.w, unlitColor.w);
    }
} 