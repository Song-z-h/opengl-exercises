#version 330 core

out vec4 FragColor;

in vec3 Normal;
in vec3 Position;
in vec2 TexCoords;

uniform vec3 cameraPos;
uniform samplerCube skybox;
uniform bool isRefracted;
uniform sampler2D texture_specular1;
uniform sampler2D texture_height1;

void main(){
	
	vec3 specular = vec3(texture(texture_specular1, TexCoords));
	vec3 environment = vec3(texture(texture_height1, TexCoords));
	
	float ratio = 1.00 / 1.89;
	vec3 I = normalize(Position - cameraPos);
	vec3 R = vec3(1.0);
	if(!isRefracted)
	 R = reflect(I, normalize(Normal));
	else
	 R = refract(I, normalize(Normal), ratio);


	vec3 k = texture(skybox, R).rgb * environment;
	
	if(!isRefracted)
	FragColor = vec4(specular + environment * R , 1.0f);
	//FragColor = vec4(texture(skybox, R).rgb, 1.0);
	else
	FragColor = vec4(k, 1.0f);
}