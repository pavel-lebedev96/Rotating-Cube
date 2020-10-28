using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace OpenGL
{
    class RotatingCube
    {
        [STAThread]
        public static void Main()
        {
            using (var window = new GameWindow())
            {
                double xrot = 0, yrot = 0, zrot = 0;
                int VertexShader = 0;
                int FragmentShader = 0;
                int ShaderProgram = 0;
                const string VertexShaderSource = @"
                #version 330
                out vec3 normal;
                void main(void)
                {
                    normal = gl_Normal;
                    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
                }
                ";
                const string FragmentShaderSource = @"
                #version 330
                in vec3 normal;
                vec3 front = vec3(0.0, 0.0, 1.0);
                vec3 back = vec3(0.0, 0.0, -1.0);
                vec3 top = vec3(0.0, 1.0, 0.0);
                vec3 bottom = vec3(0.0, -1.0, 0.0);
                vec3 right = vec3(1.0, 0.0, 0.0);
                vec3 left = vec3(-1.0, 0.0, 0.0);
                void main(void)
                {
                    if (normal == front)
                        gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
                    if (normal == back)
                        gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
                
                    if (normal == top)
                        gl_FragColor = vec4(0.0, 1.0, 0.0, 1.0);
                    if (normal == bottom)
                        gl_FragColor = vec4(0.0, 1.0, 0.0, 1.0);
                
                    if (normal == left)
                        gl_FragColor = vec4(0.0, 0.0, 1.0, 1.0);
                    if (normal == right)
                        gl_FragColor = vec4(0.0, 0.0, 1.0, 1.0);
                }
                ";
                window.Load += (sender, e) =>
                {
                    // Load the source of the vertex shader and compile it.
                    VertexShader = GL.CreateShader(ShaderType.VertexShader);
                    GL.ShaderSource(VertexShader, VertexShaderSource);
                    GL.CompileShader(VertexShader);

                    // Load the source of the fragment shader and compile it.
                    FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                    GL.ShaderSource(FragmentShader, FragmentShaderSource);
                    GL.CompileShader(FragmentShader);

                    /* Create the shader program, attach the vertex 
                     * and fragment shaders and link the program.*/
                    ShaderProgram = GL.CreateProgram();
                    GL.AttachShader(ShaderProgram, VertexShader);
                    GL.AttachShader(ShaderProgram, FragmentShader);
                    GL.LinkProgram(ShaderProgram);
                    GL.UseProgram(ShaderProgram);
                    GL.Enable(EnableCap.DepthTest);
                };
                window.Unload += (Sender, e) =>
                {
                    GL.UseProgram(0);

                    //Delete all the resources.
                    GL.DeleteProgram(ShaderProgram);
                    GL.DeleteShader(FragmentShader);
                    GL.DeleteShader(VertexShader);
                };
                window.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, window.Width, window.Height);
                    Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(45.0f),
                        (float)window.Width / (float)window.Height, 0.1f, 100.0f);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref perspective);

                    GL.MatrixMode(MatrixMode.Modelview);
                };
                window.RenderFrame += (sender, e) =>
                {
                    GL.Clear(ClearBufferMask.ColorBufferBit
                        | ClearBufferMask.DepthBufferBit);
                    GL.LoadIdentity();
                    GL.Translate(0, 0, -5.0f);
                    GL.Rotate(xrot, 1, 0, 0);
                    GL.Rotate(yrot, 0, 1, 0);
                    GL.Rotate(zrot, 0, 0, 1);

                    GL.Begin(PrimitiveType.Quads);
                    // Front Face
                    GL.Normal3(0.0f, 0.0f, 1.0f);
                    GL.Vertex3(-1.0f, -1.0f, 1.0f);
                    GL.Vertex3(1.0f, -1.0f, 1.0f);
                    GL.Vertex3(1.0f, 1.0f, 1.0f);
                    GL.Vertex3(-1.0f, 1.0f, 1.0f);
                    // Back Face
                    GL.Normal3(0.0f, 0.0f, -1.0f);
                    GL.Vertex3(-1.0f, -1.0f, -1.0f);
                    GL.Vertex3(-1.0f, 1.0f, -1.0f);
                    GL.Vertex3(1.0f, 1.0f, -1.0f);
                    GL.Vertex3(1.0f, -1.0f, -1.0f);
                    // Top Face
                    GL.Normal3(0.0f, 1.0f, 0.0f);
                    GL.Vertex3(-1.0f, 1.0f, -1.0f);
                    GL.Vertex3(-1.0f, 1.0f, 1.0f);
                    GL.Vertex3(1.0f, 1.0f, 1.0f);
                    GL.Vertex3(1.0f, 1.0f, -1.0f);
                    // Bottom Face
                    GL.Normal3(0.0f, -1.0f, 0.0f);
                    GL.Vertex3(-1.0f, -1.0f, -1.0f);
                    GL.Vertex3(1.0f, -1.0f, -1.0f);
                    GL.Vertex3(1.0f, -1.0f, 1.0f);
                    GL.Vertex3(-1.0f, -1.0f, 1.0f);
                    // Right face
                    GL.Normal3(1.0f, 0.0f, 0.0f);
                    GL.Vertex3(1.0f, -1.0f, -1.0f);
                    GL.Vertex3(1.0f, 1.0f, -1.0f);
                    GL.Vertex3(1.0f, 1.0f, 1.0f);
                    GL.Vertex3(1.0f, -1.0f, 1.0f);
                    // Left Face
                    GL.Normal3(-1.0f, 0.0f, 0.0f);
                    GL.Vertex3(-1.0f, -1.0f, -1.0f);
                    GL.Vertex3(-1.0f, -1.0f, 1.0f);
                    GL.Vertex3(-1.0f, 1.0f, 1.0f);
                    GL.Vertex3(-1.0f, 1.0f, -1.0f);
                    GL.End();
                    window.SwapBuffers();
                    xrot += 5;
                    yrot += 4;
                    zrot += 3;
                };
                // Run the application at 60 updates per second
                window.Run(60.0);
            }
        }
    }
}