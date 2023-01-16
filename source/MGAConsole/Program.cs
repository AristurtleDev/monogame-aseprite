using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.IO;
using MonoGame.Aseprite.Content.Pipeline.Processors;
using MonoGame.Aseprite.Content.Pipeline.Writers;
using MonoGame.Framework.Content.Pipeline.Builder;


if (Assembly.GetAssembly(typeof(ContentReader)) is not Assembly assembly)
{
    throw new InvalidOperationException($"Unable to load Microsoft.Xna.Framework assembly");
}

if (assembly.GetType("Microsoft.Xna.Framework.Content.Texture2DReader") is not Type texture2DReaderType)
{
    throw new InvalidOperationException($"Unable to load Texture2DReader type from assembly");
}

if (Activator.CreateInstance(texture2DReaderType) is not object texture2DReaderInstance)
{
    throw new InvalidOperationException($"Unable to create instance of Texture2DReader");
}

BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
MethodInfo[] methods = texture2DReaderType.GetMethods(flags);

// for (int i = 0; i < methods.Length; i++)
// {
//     Console.WriteLine(methods[i].Name);
// }

if (texture2DReaderType.GetMethod("Read", flags, new [] {typeof(ContentReader), typeof(Texture2D)}) is not MethodInfo processMethod)
{
    throw new InvalidOperationException($"Unable to get Process method form Texture2DReader type");
}

ParameterInfo[] parameters = processMethod.GetParameters();

for (int i = 0; i < parameters.Length; i++)
{
    Console.WriteLine(parameters[i].Name);
}

// if (processMethod.Invoke(texture2DReaderInstance, new object?[] { reader, existingInstance }) is not Texture2D texture)
// {
//     throw new InvalidOperationException("$Unable to create texture from Texture2DReader.Process method");
// }

// return texture;

// string path = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");
// PipelineManager manager = new(Environment.CurrentDirectory, Environment.CurrentDirectory, Environment.CurrentDirectory);
// PipelineBuildEvent pipelineEvent = new();
// ContentProcessorContext context = new PipelineProcessorContext(manager, pipelineEvent);

// AsepriteFile file = AsepriteFileReader.ReadFile(path);

// var processor = new SingleFrameProcessor();
// var result = processor.Process(file, context);



// Console.WriteLine(file.Name);
