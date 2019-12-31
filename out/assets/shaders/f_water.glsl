#version 330 core

out vec4 FragColor;

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec2 vertexUv;

uniform mat4 rotation;
uniform vec4 matColor;

uniform float time;
uniform float lightAmbient;
uniform vec4 lightColor;
uniform vec3 lightDirection;

float delta = 1;
float waterFlowTime = 0.4;
float SimplexPerlin3D(vec3 P);
float wnoise(vec3 pos, float s, float t)
{
    float n =
    SimplexPerlin3D(vec3(pos.xz*900,t))*0.5+
    SimplexPerlin3D(vec3(pos.xz*1000+vec2(-1000,0),t))*0.25;
    n = clamp(abs(n)*2*0.15,0,0.15)+0.85;
    return n;
}

void main()
{
    vec3 normal = vec3(0,1,0);
    float diffuse = clamp(dot(-lightDirection, normal),0,1);
    float lightValue = clamp(diffuse+lightAmbient,0,1);

    vec4 unlitColor = matColor * vec4(lightColor.xyz,1) * wnoise(vertexPosition, 1000, time*waterFlowTime);
    FragColor = vec4(unlitColor.xyz* lightValue * lightColor.w, unlitColor.w);
} 