#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D screenTexture;

const float offset = 1.0/500.0;
void main()
{

	vec2 offsets[9] = vec2[](
	vec2(-offset,  offset), // top-left
        vec2( 0.0f,    offset), // top-center
        vec2( offset,  offset), // top-right
        vec2(-offset,  0.0f),   // center-left
        vec2( 0.0f,    0.0f),   // center-center
        vec2( offset,  0.0f),   // center-right
        vec2(-offset, -offset), // bottom-left
        vec2( 0.0f,   -offset), // bottom-center
        vec2( offset, -offset)  // bottom-right
);
	//outline

	 float kernel[9] = float[](
			 1, 1, 1,
			 1,  -9, 1,
			 1, 1, 1
	 );
	 
	//blur
	/*float kernel[9] = float[](
    1.0 / 16, 2.0 / 16, 1.0 / 16,
    2.0 / 16, 4.0 / 16, 2.0 / 16,
    1.0 / 16, 2.0 / 16, 1.0 / 16
);*/
	//left sobel
/*float kernel[9] = float[](
	1, 0, -1,
	2, 0, -2,
	1, 0, -1
	);*/

	vec3 sampleTex[9];
	for(int i = 0; i < 9; i++)
	sampleTex[i] = vec3(texture(screenTexture, TexCoords.st + offsets[i]));

	if(gl_FragCoord.x < 600)
	{
		vec3 col = vec3(0.0);
		for(int i = 0; i < 9; i++)
		col += sampleTex[i] * kernel[i];
		FragColor = vec4(col, 1.0);
	}
	else{
		vec3 col = texture(screenTexture, TexCoords).rgb;
		//reverse of colour
		FragColor = vec4(1.0-col, 1.0);
	}

	//vec3 col = texture(screenTexture, TexCoords).rgb;
	//reverse of colour
	//FragColor = vec4(1.0-col, 1.0);

	//grayscale
	//float average = (col.r+col.g+col.b)/3.0;
	//FragColor = vec4(average, average, average, 1.0);


}
