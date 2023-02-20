using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Content.Writers;
using MonoGame.Aseprite.RawTypes;

string fileToProcess = "./character_robot.aseprite";

//  Load the Aseprite file like usual
AsepriteFile aseFile = AsepriteFile.Load(fileToProcess);

//  Process using a process, only this time, use the ProcessRaw method to get the
//  raw values of the type
RawSpriteSheet rawSpriteSheet = SpriteSheetProcessor.ProcessRaw(aseFile);

//  Then use the built-in writers to write it to disk. In this example, I'm going to 
//  write it to the "Content" directory of the game project this is for
RawSpriteSheetWriter.Write("../CustomProcessingExample/Content/character_robot.rawSpriteSheet", rawSpriteSheet);

//  That's it really.  Whatever custom content processing workflow you might have,
//  if you can add the Monogame.Aseprite NuGet or reference it somehow, then you 
//  just do the above in your workflow and write the raw value out.  
//  There is a Raw* type and a Raw* writer for each of the types that can be processed.

//  Next, head over to the CustomProcessingExample game project to see who it is 
//  read back in at runtime.