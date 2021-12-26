#ifndef _light_caster_
#define _light_caster_
#include <glm\glm.hpp>
#include <glm\gtc\matrix_transform.hpp>
#include <glm\gtc\type_ptr.hpp>
#include<learnopengl\shader_s.h>
#include <string>
#include <sstream>
using namespace std;
class LightCasters
{
public:
	 glm::vec3 Ambient = glm::vec3(0.05f, 0.05f, 0.05f);
	 glm::vec3 Diffuse = glm::vec3(0.3f, 0.3f, 0.3f);
	 glm::vec3 Specular = glm::vec3(0.4f, 0.4f, 0.4f);

public:
	LightCasters() {};
	~LightCasters() {};

	// Directional light
	static void setDirLight(Shader &cubeShader, glm::vec3 direction, glm::vec3 ambient = glm::vec3(0.05f, 0.05f, 0.05f),
		glm::vec3 diffuse = glm::vec3(0.3f, 0.3f, 0.3f), glm::vec3 specular = glm::vec3(0.4f, 0.4f, 0.4f))
	{
		cubeShader.setVec3("dirLight.direction", direction);
		cubeShader.setVec3("dirLight.ambient", ambient);
		cubeShader.setVec3("dirLight.diffuse", diffuse);
		cubeShader.setVec3("dirLight.specular", specular);
	}

	//Point light
	static void setPointLight(Shader &cubeShader, glm::vec3 position, glm::vec3 ambient,
		glm::vec3 diffuse, glm::vec3 specular, int i, float constant = 1.0f, float linear = 0.09f, float quadratic = 0.032f)
	{
		stringstream ss;
		ss << i;
		string str;
		ss >> str;

		cubeShader.setVec3("pointLights[" + str + "].position", position);
		cubeShader.setVec3("pointLights[" + str + "].ambient", ambient);
		cubeShader.setVec3("pointLights[" + str + "].diffuse", diffuse);  //0.8f, 0.8f, 0.8f
		cubeShader.setVec3("pointLights[" + str + "].specular", specular);
		cubeShader.setFloat("pointLights[" + str + "].constant", constant);
		cubeShader.setFloat("pointLights[" + str + "].linear", linear);
		cubeShader.setFloat("pointLights[" + str + "].quadratic", quadratic);
	}

	//Spot light
	static void setSpotLight(Shader &cubeShader, glm::vec3 direction, glm::vec3 position, glm::vec3 ambient,
		glm::vec3 diffuse, glm::vec3 specular, float constant = 1.0f, float linear = 0.09f, float quadratic = 0.032f, float cutOff = 12.5f, float outerCutOff = 15.0f) 
	{
		cubeShader.setVec3("spotLight.position", position);
		cubeShader.setVec3("spotLight.direction", direction);
		cubeShader.setVec3("spotLight.ambient", ambient);
		cubeShader.setVec3("spotLight.diffuse", diffuse);
		cubeShader.setVec3("spotLight.specular", specular);
		cubeShader.setFloat("spotLight.constant", constant);
		cubeShader.setFloat("spotLight.linear", linear);
		cubeShader.setFloat("spotLight.quadratic", quadratic);
		cubeShader.setFloat("spotLight.cutOff", glm::cos(glm::radians(cutOff)));
		cubeShader.setFloat("spotLight.outerCutOff", glm::cos(glm::radians(outerCutOff)));

	}
};

#endif _light_caster_
