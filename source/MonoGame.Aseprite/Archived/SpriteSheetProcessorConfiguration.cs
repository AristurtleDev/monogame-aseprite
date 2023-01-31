// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */

// using MonoGame.Aseprite.AsepriteTypes;

// namespace MonoGame.Aseprite.Processors;

// /// <summary>
// ///     Defines the configuration for the <see cref="SpriteSheetProcessor"/>.
// /// </summary>
// public sealed class SpriteSheetProcessorConfiguration
// {
//     /// <summary>
//     ///     Gets or Sets a value that indicates whether only <see cref="AsepriteCel"/> elements that are on visible
//     ///     <see cref="AsepriteLayer"/> elements should be processed.
//     /// </summary>
//     public bool OnlyVisibleLayers { get; set; } = true;

//     /// <summary>
//     ///     Gets or Sets a value that indicates whether <see cref="AsepriteCel"/> elements that are on the
//     ///     <see cref="AsepriteLayer"/> marked as the background should be processed.
//     /// </summary>
//     public bool IncludeBackgroundLayer { get; set; } = false;

//     /// <summary>
//     ///     Gets or Sets a value that indicates whether <see cref="AsepriteTilemapCel"/> elements that are on
//     ///     <see cref="AsepriteTilemapLayer"/> elements should be processed.
//     /// </summary>
//     public bool IncludeTilemapLayers { get; set; } = true;

//     /// <summary>
//     ///     Gets or Sets a value that indicates whether <see cref="AsepriteFrame"/> elements that are detected as
//     ///     duplicates should be merged.
//     /// </summary>
//     public bool MergeDuplicateFrames { get; set; } = true;

//     /// <summary>
//     ///     Gets or Sets the amount of transparent pixels to add between between the edge of the generated image for the
//     ///     <see cref="SpriteSheet"/> and each of the <see cref="TextureRegion"/> elements within it.
//     /// </summary>
//     public int BorderPadding { get; set; } = 0;

//     /// <summary>
//     ///     Gets or Sets the amount of transparent pixels to add between each of the <see cref="TextureRegion"/>
//     ///     elements the <see cref="SpriteSheet"/>.
//     /// </summary>
//     public int Spacing { get; set; } = 0;

//     /// <summary>
//     ///     Gets or Sets the amount of transparent pixels to add around the edge of each <see cref="TextureRegion"/>
//     ///     for the <see cref="SpriteSheet"/>.
//     /// </summary>
//     public int InnerPadding { get; set; } = 0;

//     /// <summary>
//     ///     Initializes a new instance of the <see cref="SpriteSheetProcessorConfiguration"/> class with the default
//     ///     values.
//     /// </summary>
//     /// <remarks>
//     ///     The default configuration is to only process <see cref="AsepriteCel"/> elements on visible
//     ///     <see cref="AsepriteLayer"/> elements, do not include the <see cref="AsepriteLayer"/> that is set as the
//     ///     background, include all <see cref="AsepriteTilemapLayer"/> elements, merge duplicate
//     ///     <see cref="AsepriteFrame"/> elements, zero border padding, zero spacing, and zero inner padding.
//     /// </remarks>
//     public SpriteSheetProcessorConfiguration() { }

//     /// <summary>
//     ///     Initializes a new instance of the <see cref="SpriteSheetProcessorConfiguration"/> class.
//     /// </summary>
//     /// <param name="onlyVisibleLayers">
//     ///     Indicates whether only <see cref="AsepriteCel"/> elements that are on visible <see cref="AsepriteLayer"/>
//     ///     elements should be processed.
//     /// </param>
//     /// <param name="includeBackgroundLayer">
//     ///     Indicates whether <see cref="AsepriteCel"/> elements that are on the <see cref="AsepriteLayer"/> marked as
//     ///     the background should be processed.
//     /// </param>
//     /// <param name="includeTilemapLayers">
//     ///     Indicates whether <see cref="AsepriteTilemapCel"/> elements that are on  <see cref="AsepriteTilemapLayer"/>
//     ///     elements should be processed.
//     /// </param>
//     /// <param name="mergeDuplicates">
//     ///     Indicates whether <see cref="AsepriteFrame"/> elements that are detected as duplicates should be merged.
//     /// </param>
//     /// <param name="borderPadding">
//     ///     The amount of transparent pixels to add between between the edge of the generated image for the
//     ///     <see cref="SpriteSheet"/> and each of the <see cref="TextureRegion"/> elements within it.
//     /// </param>
//     /// <param name="spacing">
//     ///     The amount of transparent pixels to add between each of the <see cref="TextureRegion"/> elements the
//     ///     <see cref="SpriteSheet"/>.
//     /// </param>
//     /// <param name="innerPadding">
//     ///     The amount of transparent pixels to add around the edge of each <see cref="TextureRegion"/> for the
//     ///     <see cref="SpriteSheet"/>.
//     /// </param>
//     public SpriteSheetProcessorConfiguration(bool onlyVisibleLayers, bool includeBackgroundLayer, bool includeTilemapLayers, bool mergeDuplicates, int borderPadding, int spacing, int innerPadding)
//     {
//         OnlyVisibleLayers = onlyVisibleLayers;
//         IncludeBackgroundLayer = includeBackgroundLayer;
//         IncludeTilemapLayers = includeTilemapLayers;
//         MergeDuplicateFrames = mergeDuplicates;
//         BorderPadding = borderPadding;
//         Spacing = spacing;
//         InnerPadding = innerPadding;
//     }
// }