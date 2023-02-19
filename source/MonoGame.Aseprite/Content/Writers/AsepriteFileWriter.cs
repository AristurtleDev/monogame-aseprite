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

// namespace MonoGame.Aseprite.Content.Writers;

// /// <summary>
// ///     Defines a writer that serializes and writes the contents of an <see cref="AsepriteFile"/> class to a file.
// /// </summary>
// public static class AsepriteFileWriter
// {
//     /// <summary>
//     /// Writes the contents of the <see cref="AsepriteFile"/> class given to a file.
//     /// </summary>
//     /// <param name="path">
//     /// The path and name of the file to write to.  If no file exists at this path, one will be created.  If a file 
//     /// already exists, it will be overwritten.
//     /// </param>
//     /// <param name="aseFile">The instance of the <see cref="AsepriteFile"/> class to write.</param>
//     public static void Write(string path, AsepriteFile aseFile)
//     {
//         Stream stream = File.Create(path);
//         BinaryWriter writer = new(stream);
//         Write(writer, aseFile);
//     }

//     internal static void Write(BinaryWriter writer, AsepriteFile aseFile)
//     {
//         writer.WriteMagic();
//         writer.Write(aseFile.Name);
//         writer.Write(aseFile.CanvasWidth);
//         writer.Write(aseFile.CanvasHeight);

//         writer.Write(aseFile.PaletteCount);
//         for (int i = 0; i < aseFile.PaletteCount; i++)
//         {
//             writer.Write(aseFile.Palette[i]);
//         }

//         writer.Write(aseFile.TagCount);
//         for (int i = 0; i < aseFile.TagCount; i++)
//         {
//             WriteTag(writer, aseFile.Tags[i]);
//         }

//         writer.Write(aseFile.SliceCount);
//         for (int i = 0; i < aseFile.SliceCount; i++)
//         {
//             WriteSlice(writer, aseFile.Slices[i]);
//         }

//         writer.Write(aseFile.TilesetCount);
//         for (int i = 0; i < aseFile.TilesetCount; i++)
//         {
//             WriteTileset(writer, aseFile.Tilesets[i]);
//         }

//         writer.Write(aseFile.LayerCount);
//         for (int i = 0; i < aseFile.LayerCount; i++)
//         {
//             WriteLayer(writer, aseFile.Layers[i]);
//         }

//         writer.Write(aseFile.FrameCount);
//         for (int i = 0; i < aseFile.FrameCount; i++)
//         {
//             WriteFrame(writer, aseFile.Frames[i]);
//         }

//         writer.Write(aseFile.Name);
//         WriteUserData(writer, aseFile.UserData);
//     }

//     private static void WriteTag(BinaryWriter writer, AsepriteTag tag)
//     {
//         writer.Write(tag.From);
//         writer.Write(tag.To);
//         writer.Write((byte)tag.Direction);
//         writer.Write(tag.Name);
//         writer.Write(tag._tagColor);
//         WriteUserData(writer, tag.UserData);
//     }

//     private static void WriteSlice(BinaryWriter writer, AsepriteSlice slice)
//     {
//         writer.Write(slice.Name);
//         writer.Write(slice.IsNinePatch);
//         writer.Write(slice.HasPivot);
//         writer.Write(slice.KeyCount);

//         for (int i = 0; i < slice.KeyCount; i++)
//         {
//             WriteSliceKey(writer, slice.Keys[i]);
//         }

//         WriteUserData(writer, slice.UserData);
//     }

//     private static void WriteTileset(BinaryWriter writer, AsepriteTileset tileset)
//     {
//         writer.Write(tileset.ID);
//         writer.Write(tileset.TileCount);
//         writer.Write(tileset.TileWidth);
//         writer.Write(tileset.TileHeight);
//         writer.Write(tileset.Name);
//         writer.Write(tileset.Width);
//         writer.Write(tileset.Height);
//         writer.Write(tileset.Pixels.Length);

//         for (int i = 0; i < tileset.Pixels.Length; i++)
//         {
//             writer.Write(tileset.Pixels[i]);
//         }
//     }

//     private static void WriteSliceKey(BinaryWriter writer, AsepriteSliceKey key)
//     {
//         writer.Write(key.FrameIndex);
//         writer.Write(key.Bounds);
//         writer.Write(key.IsNinePatch);

//         if (key.IsNinePatch)
//         {
//             writer.Write(key.CenterBounds.Value);
//         }

//         writer.Write(key.HasPivot);

//         if (key.HasPivot)
//         {
//             writer.Write(key.Pivot.Value);
//         }
//     }

//     private static void WriteLayer(BinaryWriter writer, AsepriteLayer layer)
//     {
//         byte typeFlag = 0;

//         if (layer.GetType() == typeof(AsepriteTilemapCel))
//         {
//             typeFlag = 1;
//         }

//         writer.Write(typeFlag);
//         writer.Write(layer.SerializationID);
//         writer.Write((ushort)layer.Flags);
//         writer.Write((int)layer.BlendMode);
//         writer.Write(layer.Opacity);
//         writer.Write(layer.Name);

//         if (layer is AsepriteTilemapLayer tilemapLayer)
//         {
//             writer.Write(tilemapLayer.Tileset.ID);
//         }

//         WriteUserData(writer, layer.UserData);

//     }

//     private static void WriteFrame(BinaryWriter writer, AsepriteFrame frame)
//     {
//         writer.Write(frame.Name);
//         writer.Write(frame.Width);
//         writer.Write(frame.Height);
//         writer.Write(frame.DurationInMilliseconds);
//         writer.Write(frame.Cels.Length);

//         for (int i = 0; i < frame.Cels.Length; i++)
//         {
//             WriteCel(writer, frame.Cels[i]);
//         }
//     }

//     private static void WriteCel(BinaryWriter writer, AsepriteCel cel)
//     {
//         byte typeFlag = 0;

//         if (cel.GetType() == typeof(AsepriteTilemapCel))
//         {
//             typeFlag = 1;
//         }

//         writer.Write(typeFlag);
//         writer.Write(cel.Layer.SerializationID);
//         writer.Write(cel.Position);
//         writer.Write(cel.Opacity);


//         if (cel is AsepriteImageCel imageCel)
//         {
//             WriteImageCel(writer, imageCel);
//         }
//         else if (cel is AsepriteTilemapCel tilemapCel)
//         {
//             WriteTilemapCel(writer, tilemapCel);
//         }

//         WriteUserData(writer, cel.UserData);
//     }

//     private static void WriteImageCel(BinaryWriter writer, AsepriteImageCel imageCel)
//     {
//         writer.Write(imageCel.Width);
//         writer.Write(imageCel.Height);
//         writer.Write(imageCel.Pixels.Length);

//         for (int i = 0; i < imageCel.Pixels.Length; i++)
//         {
//             writer.Write(imageCel.Pixels[i]);
//         }
//     }

//     private static void WriteTilemapCel(BinaryWriter writer, AsepriteTilemapCel tilemapCel)
//     {
//         writer.Write(tilemapCel.Tileset.ID);
//         writer.Write(tilemapCel.Columns);
//         writer.Write(tilemapCel.Rows);
//         writer.Write(tilemapCel.TileCount);

//         for (int i = 0; i < tilemapCel.TileCount; i++)
//         {
//             WriteTile(writer, tilemapCel.Tiles[i]);
//         }
//     }

//     private static void WriteTile(BinaryWriter writer, AsepriteTile tile)
//     {
//         writer.Write(tile.TilesetTileID);
//         writer.Write(tile.XFlip);
//         writer.Write(tile.YFlip);
//         writer.Write(tile.Rotation);
//     }


//     private static void WriteUserData(BinaryWriter writer, AsepriteUserData userData)
//     {
//         writer.Write(userData.HasText);

//         if (userData.HasText)
//         {
//             writer.Write(userData.Text);
//         }

//         writer.Write(userData.HasColor);

//         if (userData.HasColor)
//         {
//             writer.Write(userData.Color.Value);
//         }
//     }
// }
