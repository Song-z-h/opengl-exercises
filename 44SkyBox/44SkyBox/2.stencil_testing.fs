#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D texture1;
uniform float time;

void main()
{    
    vec4 texColor = texture(texture1, TexCoords + vec2(0.0, time/2.0));
	
/*	
   if(texColor.a < 0.1)
	discard;
*/

  FragColor = texColor;
}