using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.Importers;
using MonoGame.Aseprite.Content.Pipeline.Processors;
using MonoGame.Aseprite.Content.Pipeline.Writers;
using MonoGame.Aseprite.IO;
using MonoGame.Framework.Content.Pipeline.Builder;


string path = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");
PipelineManager manager = new(Environment.CurrentDirectory, Environment.CurrentDirectory, Environment.CurrentDirectory);
ContentImporterContext importerContext = new PipelineImporterContext(manager);
PipelineBuildEvent pipelineEvent = new();
ContentProcessorContext context = new PipelineProcessorContext(manager, pipelineEvent);

AsepriteFile file = AsepriteFile.Load(path);

var importer = new AsepriteFileImporter();
var importResult = importer.Import(path, importerContext);

var processor = new SingleFrameContentProcessor();
var result = processor.Process(importResult, context);





Console.WriteLine(file.Name);
