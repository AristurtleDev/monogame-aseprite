## Overview
In order to load the .json file produced by Aseprite and make use of it using the AnimatedSprite class in MonoGame.Aseprite, you'll need to have the **MonoGame.Aseprite.ContentPipeline.dll** reference added to the MonoGame Pipeline Tool for your project.  

## Files Needed
You can obtain the files needed by downloading the current release zip file located at https://gitlab.com/manbeardgames/monogame-aseprite/tags/version-1.0  

* MonoGame.Aseprite.ContentPipeline.dll  

## Instructions
In Visual Studio, open the MonoGame Pipeline Tool for your project by double clicking the **Content.mgbc** located in the content folder in the Solution Explorer.  

![open-pipeline-tool](uploads/a8a167245c75e59956ba53d5c03c56f7/open-pipeline-tool.png)  

This will open the MonoGame Pipeline Tool for your project. 

![pipeline-tool](uploads/fd682da2beaf9dda9fefe22b16775d91/pipeline-tool.png)  

Now, you need to add the reference to the **MonoGame.Aseprite.ContentPipeline.dll** to the pipeline tool.  To do so, complete the following steps.

* Click **Content** in the Project box on the top left
* Find **References** in the Properties box located in the bottom left of the window.  You may have to scroll down to find it if your window isn't maximized.
* Click the box to the right of **References**.  If you have no references already added, it should say "None".  This should open the Reference Editor Dialog
* Click the **Add** button at the top right of the Reference Editor Window
* Navigate to the location where you put the **MonoGame.Aseprite.ContentPipeline.dll** when you downloaded and extracted the files needed.
* Select the **MonoGame.Aseprite.ContentPipeline.dll** and click the **Open** button. This will add it to the list of refrences in the Reference Editor Window.
* Click the **Ok** button to close the Reference Editor Window
* Close the MonoGame Pipeline Tool window.  If you are asked to save first, choose **Yes** to save.  

The following gif goes through the steps above for reference.  
![add-reference-to-pipeline](uploads/9142bb61bc45bc376373558ce6936533/add-reference-to-pipeline.gif)  

That's it for adding the reference to your MonoGame Pipeline Tool.

[Return To Home](home)