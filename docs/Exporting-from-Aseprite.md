## Overview

Once you have finished creating your sprite animation in Aseprite, you'll need to export it out creating the image file and the .json file for the animation.  There are a couple of configurations before and during the export that must be specified in order to use with Monogame.Aseprite, as described below.

## Steps in order
1. [Create Frame Tags](#step-1-create-frame-tags)
2. [Export Sprite Sheet](#step-2-export-sprite-sheet)

## Step 1: Create Frame Tags
If you haven't already, you need to create frame tags for your animation.  Frame tags are a way of giving a logical name to a group of frames in the animation.  For example, in the image below, Frames 2 and 3 in the animation define the "walking down" animation. 

Example of animation with no frame tags  
![no-frame-tags](uploads/b00dde36c9d3c0c4d4caa0b7c278965c/no-frame-tags.gif)

To create a frame tag, just select the frames that are to be grouped together, right-click the selection, and choose **New Tag** from the context menu.  In the dialog that appears, enter the name you would like to give the frame tag, and choose **Ok** to create it. **The name you give the frame tag is important, as this is what you'll reference it by in code later**

Example of adding frame tags to animation  
![add-frame-tag](uploads/702fcd0fef2a34a6d2aaa373c3f29f6c/add-frame-tag.gif)

Once you've finished, all of your frames of animation should be tagged.  **Any frames of animation that are not tagged will not be animated in your game.  The tags are how they are referenced**

Example of all frames of animation tagged  
![frame-tags](uploads/ccb3bf4ec6802afa702ff5c8bd014dfb/frame-tags.png)

## Step 2: Export Sprite Sheet
Once you've added all of your frame tags, the next step is to export the sprite sheet.  This will produce the image file and the .json file we'll need later for MonoGame.

First select, **Export Sprite Sheet** from the **File** menu.  
![file-export-sprite-sheet](uploads/fac1f61f210c98417b067635acf5feb7/file-export-sprite-sheet.png)

This will open the Export Sprite Sheet dialog.  
![export-sprite-sheet-dialog](uploads/00b2c9162067cc5042e5929115624e82/export-sprite-sheet-dialog.png)

The following are the requirements for the settings in this dialog.  Anything in bold is something you need to pay attention to.
* Sheet Type: You can select whatever you'd like here. 
* Padding: **Leave this unchecked.  Currently padding is not supported**
* Trim: **Leave this unchecked.  Currently trim is not supported**
* Layers: Choose the layers you would like. Generally, using the Visible layers option is best
* Output File: **Check this.  Click the Select File box and choose where to save the file and the file name.  Ensure that the name of the file is different than the name of the file for the JSON Data below.**
* JSON Data: **Check this. Click the Select File box and choose where to save the file and the file name.  Ensure that the name of the file is different than the name of the file for the Output File above.**
* Meta: **Choose Array from the dropdown menu.  Hash is not supported**
* Meta Layers: Can leave checked or unchecked
* Meta Frame Tags: **Check this box**
* Meta Slices: **Check this box** (even if you aren't using slices)

**Quick note: Be sure that the OutputFIle and JSON Data files have different names.  For instance, don't name them both "character.png" and "character.json".  This is due to how MonoGame Pipeline Tool will convert the files later, they cannot both have the same name.  Instead do something like "character.png" and "characterAnimation.json"**.  


Once you have ensured your settings above are correct, click the **Export** button, and your done.  

[Return to home page](home)