## Overview
In order to use MonoGame.Aseprite in your project, you'll need to have a reference to **MonoGame.Aseprite.dll** in your project and a refrence to **MonoGame.Aseprite.ContentPipeline.dll** in your Content Pipeline Tool.  

The following instructions show the steps to take when adding MonoGame.Aseprite to your project using Nuget in Visual Studio.  

## Files needed
You can obtain the files needed from Nuget.  If using Visual Studio, you can use the Nuget Package Manager for the project, or using the commands below

**Package Manager**  
```PM> Install-Package MonoGame.Aseprite -Version 1.0.0 ```

**.Net CLI**  
```> dotnet add package MonoGame.Aseprite --version 1.0.0 ```  

[MonoGame.Aseprite Nuget Page](https://www.nuget.org/packages/MonoGame.Aseprite/)  

When the package has finished installing, you should now have a reference to **MonoGame.Aseprite.dll** in your project references.  Next you'll need to add the reference for your Content Pipeline tool.

## Add Content Pipeline Tool Reference  
After adding the Nuget package, the **MonoGame.Aseprite.ContentPipelin.dll** file is copied to the "\packages\MonoGame.Aseprite.1.0.0\content\" folder.  **The "packages" folder can be found in the root of your project folder.**  

In Visual Studio, open the MonoGame Pipeline Tool for your project by double clicking the **Content.mgbc** located in the content folder in the Solution Explorer.  

![open-pipeline-tool](uploads/a8a167245c75e59956ba53d5c03c56f7/open-pipeline-tool.png)  

This will open the MonoGame Pipeline Tool for your project. 

![pipeline-tool](uploads/fd682da2beaf9dda9fefe22b16775d91/pipeline-tool.png)  

Now, you need to add the reference to the **MonoGame.Aseprite.ContentPipeline.dll** to the pipeline tool.  To do so, complete the following steps.

* Click **Content** in the Project box on the top left
* Find **References** in the Properties box located in the bottom left of the window.  You may have to scroll down to find it if your window isn't maximized.
* Click the box to the right of **References**.  If you have no references already added, it should say "None".  This should open the Reference Editor Dialog
* Click the **Add** button at the top right of the Reference Editor Window
* Navigate to the "pacakges" folder in the root directory of your project.  Inside this folder, open the "MonoGame.Aseprite.1.0.0" folder, and then the "content" folder.
* Select the **MonoGame.Aseprite.ContentPipeline.dll** and click the **Open** button. This will add it to the list of references in the Reference Editor Window.
* Click the **Ok** button to close the Reference Editor Window
* Close the MonoGame Pipeline Tool window.  If you are asked to save first, choose **Yes** to save.  


That's it, if you completed the above steps, you're not good to go.

[Return to home](home)