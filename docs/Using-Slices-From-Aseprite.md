## Overview  
Slices in Aseprite provide a nifty little tool to define things for your animation in the game, such as per-frame defined areas that you can use, for example, per-frame collision boxes in your game. There doesn't appear to be any documentation on the Aseprite website on using the Slice tool, so I'll do my best here to describe how to use it in relation to MonoGame.Aseprite, and provide a few gotchas and issues along the way.  

## Creating A Slice
In Aseprite, to create a slice, first you'll need to select the **Slice Tool**.  You can select this in the toolbox, or by using the  keyboard shortcut "C".

![aseprite-slice-tool](uploads/cebb882a71f541d22b954efa5ef419b3/aseprite-slice-tool.png)

With the tool selected, just drag some where on the image to create the slice rectangle.  

![aseprite-create-slice](uploads/145685ab64515b3569f03906f492378b/aseprite-create-slice.gif)

### Gotcha #1
When you create a slice, like above, you'll notice that every frame of animation now contains this slice.  However, if at this point, you were to export the spritesheet, you will notice in the .json file, that the only key added to the slice is for the frame you had selected, and not every frame.  

### Gotcha #2  
As stated above, only the frame you had selected will be included in the export, even though you'll see the slice appear on every frame of animation in Aseprite.  If you want the slice to export for any other frames, you must go to those frames individually and just adjust the slice.  Even if you want the slice to be the exact same on a different frame, go to that frame, and make a small adjustment, then move the adjustment back to what you want (this does not mean, make adjustment, then ctrl-z, that doesn't work).

## The Name Of The Slice Is Super Important
Every slice you create in Aseprite is given the default generic name of "slice".  When using the slice information in MonoGame.Aseprite, it is important that every slice have a different unique name.  

To change the name of a slice, with the slice tool selected, right click the slice, select Properties, and give it a name.  Remember, the name for a slice must be unique if you are creating multiple slice definitions in the animation.  

![aseprite-slice-name](uploads/f504809ace97628411025bc702ebdde5/aseprite-slice-name.gif)  

### Gotcha #3  
This is going to be hard to explain, but I'll do my best.  Properties for a slice are as a whole. When you change the name of a slice, or the color (explained below), you only have to change it once. You don't have to do this for every single frame that you want slice information in.  I'm putting this as a gotcha, because i stated twice above about the slice having to have a unique name if you use multiple slice definitions.  I didn't want you going frame by frame and changing the name for a slice each frame and getting confused.  

Even though i just said it twice in the section above, i'm stating it here as well. **Every slice if using multiple slice definitions in your animation must have a unique name for it to work in MonoGame.Aseprite**  

## Changing The Color Of A Slice  
You can change the color of a slice in Aseprite. The color information is also imported in with MonoGame.Aseprite if you need to use it for whatever reason.  For example, I use the information to during by debug render to visually see the area defined by the slice for the collision boxes.  

To change the color of a slice, with the slice tool selected, right click the slice, select Properties, click the little box to the right of the name field, click the color box, and change the color to whatever you want.  

![aseprite-slice-color](uploads/4ab32ee36a0cbe0616190519643c4d06/aseprite-slice-color.gif)  

## Adding Multiple Slice Definitions
You may have a need to add multiple slices to your animation.  For instance, in the project I'm working on that initially started me making this importer, I needed a way to defined multiple seperate collision box areas for a character, per frame of animation.  To add multiple slice definitions, just create a new slice the same way we did in the section up above.  All the information from above applies when working with this slice as well.

![aseprite-slice-multiple](uploads/4824461fd2addd72e1cbd89b92c9db82/aseprite-slice-multiple.gif)  

### Gotcha #4  
**Remember to edit the properties of the new slice so it has a different UNIQUE name.***  

### Gotcha #5  
Working with multiple slice definitions, it can be a little difficult if you want to define a smaller slice within the area that another slice area defines. It's possible, but a little cumbersome to work with.  For example, You can't control which slice the mouse is working with if one is inside another.  See the gif below for a demonstration of this. I create a smaller slice, and move it inside the slice on the hat.  Then I try to move the smaller slice, but instead, it's only moving the bigger one.  

![aseprite-slice-multiple-issue](uploads/6652b32e96e2ecc9bbb8d9d9ec4e2910/aseprite-slice-multiple-issue.gif)  



That's all I can think to add for now.  I'll keep this updated as I discover other relevant information for creating slices in Aseprite to be used in MonoGame.Aseprite.  

[Return to Home](home)