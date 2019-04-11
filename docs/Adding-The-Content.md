## Overview
To use the export for Aseprite in your MonoGame project, you'll need to added the exported sprite sheet and the .json file to in the content pipeline tool.  If you have not read the guides on exporting the sprite sheet from Aseprite and the guide for adding the reference to the MonoGame Pipeline Tool, please do so now and ensure that those steps have been completed.

## Prerequisites
The following are required in order to complete these the steps in this guide.  
* Export a sprite sheet and the .json file form Aseprite ([Click For Reference Guide](exporting-from-aseprite))
* Add the **MonoGame.Aseprite.ContentPipeline.dll** reference to the MonoGame Pipeline Tool for your project ([Click For Reference Guide](add-reference-to-monogame-pipeline-tool))  

## Instructions
The following steps will show you how to add the sprite sheet and .json file to your project.
1. Open the MonoGame Pipeline Tool for your project
2. Choose to add an Existing Item.  This can be done by clicking Edit, then Add, the Existing Item, or by right-clicking in the project box and selecting Add, then Existing Item.  
![add-existing-item](uploads/849b4a6befff1347dc2456e390b57d8b/add-existing-item.png)  
3. Select the sprite sheet image you exported from Aseprite and add it.
4. Repeat step 2 and 3, but this time select the .json file that you exported from Aseprite.
5. The sprite sheet image can be imported using the default importer and processor
6. Click the .json file inside the Project box in the top left.
7. Ensure that the Improter and the Processor in the properties box for the .json file say **Aseprite Animation Importer** and **Aseprite Animation Processor**.  If it does not show this, click there to select it from the list of importers and processors.  If you do not see these importers and processors as options to choose, this means you do not have the correct reference in the Pipeline Tool. Go back to the [Add Reference To MonoGame Pipeline Tool](add-reference-to-monogame-pipeline-tool) guide and complete all the steps there.  

![importer-processor](uploads/f30ed2aa59f986e279beeca57549038f/importer-processor.png)

8. Save the project and then select Rebuild.  If all is successful, then you are good to go.

If building fails, ensure that when you exported the .json file in Aseprite, that you choose the correct configurations as outlined in the [Exporting From Asperite](exporting-from-aseprite) guide.

[Return To Home](home)


