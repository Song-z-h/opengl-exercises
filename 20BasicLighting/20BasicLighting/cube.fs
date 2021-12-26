#version 330 core

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoord;
out vec4 FragColor;


uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 viewPos;


uniform sampler2D texture1;

void main(){
	float ambientLight = 0.1f;
	float specularStrength = 0.9f;

	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);
	float theta = max(dot(norm, lightDir), 0.0f);
	vec3 diffuse = theta * lightColor;

	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f), 2048);
	vec3 specular = specularStrength * spec * lightColor;

	vec3 result = (ambientLight + diffuse + specular) * lightColor;
	FragColor = vec4(result, 1.0f) * texture(texture1, TexCoord);
}

