//连接glfw的库
#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <iostream>


using namespace std;
//注册窗口的回调函数，来处理每次窗口大小变动
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}



void processInput(GLFWwindow* window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(window, true);
	}
}
int main()
{
	//创建储存顶点的数组
	float vertices[] = {
		-0.5f, -0.5f, 0.0f,
		0.5f, -0.5f,  0.0f,
		0.0f, 0.5f, 0.0f
	};

	//创建缓冲ID
	unsigned int VBO, VAO;
	glGenVertexArrays(1, &VAO);
	glGenBuffers(1, &VBO);

	glBindVertexArray(VAO);


	//绑定缓冲对象
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	//保存顶点 到缓存的内存中
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	
	//着色器源代码
	const GLchar* vertexShaderSource = "#version 330\n"
		"layout(location = 0) in vec3 aPos;\n"
		"void main()\n"
		"{\n gl_Position = vec4(aPos.x,aPos.y, aPos.z, 1.0);\n}";

	//创建着色器对象
	unsigned int vertexShader;
	//把需要创建的着色器类型以参数形式提供给glCreateShader
	vertexShader = glCreateShader(GL_VERTEX_SHADER);

	//着色器源码附加到着色器对象上
	glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
	glCompileShader(vertexShader);

	//判断着色器源码是否编译成功
	int  success;
	char infoLog[512];
	glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
	if (!success)
	{
		glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
	}


	//创建片段着色器
	const GLchar* fragmentShaderSource = "version 330 core\n"
		"out vec4 FragColor\n"
		"void main() \n"
		"{FragColor = vec(1.0f, 0.5f, 0.2f, 0.5f);}";

	unsigned int fragmentShader;
	fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
	glCompileShader(fragmentShader);
	glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
	if (!success)
	{
		glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
		std::cout << "ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n" << infoLog << std::endl;
	}



	//创建着色器程序
	unsigned int shaderProgram;
	shaderProgram = glCreateProgram();

	//连接所有着色器
	glAttachShader(shaderProgram, vertexShader);
	glAttachShader(shaderProgram, fragmentShader);
	glLinkProgram(shaderProgram);

	glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
	if (!success) {
		glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
		std::cout << "ERROR::SHADER::PROGRAM::LINKING_FAILED\n" << infoLog << std::endl;
	}
	//激活这个程序对象  当我们渲染一个物体时要使用着色器程序
	glUseProgram(shaderProgram);

	//删除着色器对象
	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);

	//解析顶点数据 设置顶点属性指针
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);

	glBindBuffer(GL_ARRAY_BUFFER, 0);



	glBindVertexArray(0);



	glfwInit(); //glfw的初始化
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3); //设置glfw的版本号
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);//告诉glfw使用core模式
	//glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);

	//创建窗口对象
	GLFWwindow * window = glfwCreateWindow(800, 600, "LearnOpenGL", NULL, NULL);
	
	//如果创建失败，返回
	if (window == NULL)
	{
		cout << "Failed to create GLFW window" << endl;
		glfwTerminate();
		return -1;
	}
	//将创建的对象绑定到当前主线程的上下文
	glfwMakeContextCurrent(window);
	
	//再调用任何函数之前都要初始化glad
	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		cout << "Failed to initialize GLAD" << endl;
	}

	//创建显示的 视口
	glViewport(0, 0, 800, 600);

	//告诉glfw每次我们要变动窗口的时候，要调用这个函数来调整
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

	

	//创建渲染循环
	while (!glfwWindowShouldClose(window))//查看是否有退出窗口的消息
	{
		//输入消息
		processInput(window);

		glClearColor(0.2f, 0.5f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT);

		glUseProgram(shaderProgram);
		glBindVertexArray(VAO);
		glDrawArrays(GL_TRIANGLES, 0, 3);





		//函数交替颜色缓冲，储存像素值缓冲
		glfwSwapBuffers(window);
		//处理触发事件，并且调用相对函数
		glfwPollEvents();
	}

	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);

	//正常关闭资源
	glfwTerminate();

	return 0;
}