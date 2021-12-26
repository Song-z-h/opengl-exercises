#version 330 core

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

out vec4 FragColor;

struct Material{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
};

struct Light{
	vec3 position;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

//directional light and spotlight
	vec3 direction;


//point light
	float constant;
	float linear;
	float quadratic;

//spotlight
	float cutOff;
	float outerCutOff;
};

uniform Material material;
uniform Light light;
uniform vec3 viewPos;

void main(){

	//vec3 lightDir = normalize(light.position - FragPos);

		//ambient
	vec3 ambient = light.ambient * texture(material.diffuse, TexCoords).rgb;

//diffuse
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(light.position - FragPos);
	//vec3 lightDir = normalize(-light.direction);
	float diff = max(dot(norm, lightDir), 0.0f);
	vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;

//specular
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0f), material.shininess);
	vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;

//spotlight(soft edges)
	float theta = dot(lightDir, -light.direction);
	float epsilon = light.cutOff - light.outerCutOff;
	float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0f, 1.0f);
	diffuse*= intensity;
	specular*= intensity;

//calculate pointlight's distance attenuation
	float distance = length(light.position - FragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
	//ambient *= attenuation;
	diffuse*= attenuation ;
	specular*= attenuation;
	vec3 result = ambient + diffuse + specular;


	FragColor = vec4(result, 1.0f);







}
