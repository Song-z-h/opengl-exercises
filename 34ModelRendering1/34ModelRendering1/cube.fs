#version 330 core

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;



out vec4 FragColor;
#define NUM_POINT_LIGHTS 4

struct Material{
	 sampler2D diffuse;
	 sampler2D specular;
	float shininess;
};

struct DirLight{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight{
	vec3 position;

	float constant;
	float linear;
	float quadratic;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct SpotLight {
	vec3 position;
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;

	float cutOff;
	float outerCutOff;
};

//uniform Material material;
uniform DirLight dirLight;
uniform PointLight pointLights[NUM_POINT_LIGHTS];
uniform SpotLight spotLight;
uniform vec3 viewPos;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform int index;
float shininess = 32.0f;


vec3 CalcDirLight(DirLight light, vec3 norm, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 fragPos, vec3 norm, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 fragPos, vec3 norm, vec3 viewDir);

void main(){


	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 normal = normalize(Normal);

	vec3 result = vec3(0.0);
	 result += CalcDirLight(dirLight, normal, viewDir);

	for(int i = 0; i < index; i++)
		result += CalcPointLight(pointLights[i], FragPos, normal, viewDir);

		//if(switcher == 1)
			//result += CalcSpotLight(spotLight, FragPos, normal, viewDir);


	FragColor = vec4(result, 1.0f);

}


vec3 CalcDirLight(DirLight light, vec3 norm, vec3 viewDir){
	vec3 lightDir = normalize(-light.direction);

	//diffuse shading
	float diff = max(dot(norm, lightDir), 0.0);

	//specular shading
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);

	//combining result
	vec3 ambient = light.ambient * vec3(texture(texture_diffuse1, TexCoords));
	vec3 diffuse = light.diffuse * diff *  vec3(texture(texture_diffuse1, TexCoords));
	vec3 specular = light.specular * spec * vec3(texture(texture_specular1, TexCoords));

	return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 fragPos, vec3 norm, vec3 viewDir){
	vec3 lightDir = normalize(light.position - fragPos);

	//diffuse shading
	float diff = max(dot(lightDir, norm), 0.0);

	//specular shading
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);

	//attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic* (distance * distance));

	//combining result
	vec3 ambient = light.ambient * texture(texture_diffuse1, TexCoords).rgb;
	vec3 diffuse = light.diffuse * diff *  texture(texture_diffuse1, TexCoords).rgb;
	vec3 specular = light.specular * spec * texture(texture_specular1, TexCoords).rgb;

	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;

	return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light, vec3 fragPos, vec3 norm, vec3 viewDir){
	vec3 lightDir = normalize(light.position - fragPos);

	//diffuse shading
	float diff = max(dot(lightDir, norm), 0.0);

	//specular shading
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);

	//attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1 / (light.constant + light.linear * distance + light.quadratic* (distance * distance));

	//spotlight(soft edges)
	float theta = dot(-light.direction, lightDir);
	float epsilon = light.cutOff - light.outerCutOff;
	float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

	//combining result
	vec3 ambient = light.ambient * texture(texture_diffuse1, TexCoords).rgb;
	vec3 diffuse = light.diffuse * diff *  texture(texture_diffuse1, TexCoords).rgb;
	vec3 specular = light.specular * spec * texture(texture_specular1, TexCoords).rgb;
	ambient *= attenuation * intensity;
	diffuse*= attenuation * intensity;
	specular*= attenuation * intensity;

	return (ambient + diffuse + specular);
}
