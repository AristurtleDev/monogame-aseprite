## Overview
The **AnimatedSprite** class provided in **MonoGame.Aseprite** is a simple sprite class that supports using animations that are created with Aseprite.  To use this class you'll need to be able to load an **AnimationDefinition** using the content manager in your game. 

## Prerequisites  
If you have completed the following, then you are good to go.  If not, please reference these and ensure you have completed them all successfully before continuing.  
1. [Downloaded and added reference](downloading-and-adding-references)
2. [Add reference to **MonoGame.Aseprite.ContentPipeline.dll** in Pipeline Tool](downloading-and-adding-references#add-content-pipeline-tool-reference)
3. [Export Sprite Sheet and .JSON From Aseprite](exporting-from-aseprite)
4. [Add Sprite Sheet and .JSON to Content](adding-the-content)  

## Example Code
### Creating an **AnimatedSprite**
The following code demonstrates creating and **AnimatedSprite** using the sprite sheet and animation definition.

1. Add the using statement
```csharp
using MonoGame.Aseprite;
```
2. Load an animation definition from the content manager
```csharp
// This is the .json file that you exported from Aseprite and  added in the Pipeline Tool
AnimationDefinition animationDefinition = content.Load<AnimationDefinition>("playerAnimation");
```

3. Load the sprite sheet from the content manager
```csharp
// This is the image file that you exported from Aseprite and added in the Pipeline Tool
Texture2D texture = content.Load<Texture2D>("player");
```
4. Create a new AnimatedSprite using the texture and animation definition
```csharp
AnimatedSprite sprite = new AnimatedSprite(texture, animationDefinition);
```

## Updating the AnimatedSprite
Whenever you perform your updates, just call the update method, passing in the GameTime reference

```csharp
sprite.Update(gameTime);
```

## Rendering the AnimatedSprite
To render the AnimatedSprite, just call the render method, passing it the SpriteBatch reference.  **SpriteBatch.Begin() and SpriteBatch.End() are not called inside the AnimatedSprite. It is assumed that you called them before and after calling the .Render method**

```csharp
sprite.Render(spriteBatch);
```

## Choosing Which Animation To Play
To choose which animation is currently playing, call the .Play method and give it the name of the animation to play.  **The name of the animation to play is the name of the Frame Tag that you created inside Aseprite.  Please reference the [Exporting From Aseprite](exporting-from-aseprite) for reference to frame tags.

```csharp
sprite.Play("walk down");
```


## Getting Slice Information  
If you created slices in Aseprite for the animation, you can retrieve the slice information from the AnimatedSprite.  

To get the color that the slice was assigned in Asprite  
```csharp
sprite.GetSliceColor("the-name-of-the-slice");
```  

To get the slice rectangle for the current frame of animation  
```csharp
// The return type from this is Rectangle? ( a nullable Rectangle)
var rect = sprite.GetCurrentFrameSlice("the-name-of-the-slice);  
 
// Always, always, always, check that the return has a value 
if(rect.HasValue)  
{  
    // Do something with the rect like set a collision box data
    collisionBox = rect.Value;
}
```

**Note: When calling ```GetCurrentFrameSlice```, the return value is of type ```Rectangle?```.  That's a nulllable rectangle. If no slice has been defined for that current frame of animation, a null Rectangle will be returned. Be sure to always call ```rect.HasValue``` on the return value to ensure it's not null and ```rect.Value``` to get the rectangle if it is not null.**  


That's it.  For more examples on usage, please reference the **MonoGame.Aseprite.Demo** project files.  In there, a Player class is created that utilizes the AnimatedSprite for rendering the player.  All necessary project files are included in the demo.

[Return To Home](home)

