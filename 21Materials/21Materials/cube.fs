#version 330 core

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

out vec4 FragColor;

struct Material{
	sampler2D diffuse;
	sampler2D specular;
	sampler2D emission;
	float shininess;
};

struct Light{
	vec3 position;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform Light light;
uniform Material material;
uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 objectColor;
uniform float time;
//uniform vec3 lightColor;

void main(){
//ambient
	vec3 ambient = light.ambient * texture(material.specular, TexCoords).rgb;
	
//diffuse
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);
	float diff = max(dot(norm, lightDir), 0.0f);
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.specular, TexCoords));

//specular
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0f), material.shininess);
	vec3 specular = light.specular * spec *  vec3(texture(material.specular, TexCoords));

//emision
	vec3 emission = vec3(0.0f);
	if(texture(material.specular, TexCoords).r == 0.0f)
	{
		emission = vec3(texture(material.emission, TexCoords)); /*only adding emission to its center*/
		
		emission = vec3(texture(material.emission, TexCoords + vec2(0.0f, time / 2.0f))); /*to make it move*/
		emission *= vec3(sin(time)*10.0f, 1.0f, cos(time)*10.0f); 
	}
	
	
	vec3 result = ambient + diffuse + specular + emission;
	FragColor = vec4(result, 1.0f);

}