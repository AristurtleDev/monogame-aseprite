using AsepriteDotNet.IO.Image;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.AsepriteTypes;

public static class Program
{
    private static void Main(string[] args)
    {
        string filename = Path.Combine(Environment.CurrentDirectory, "Files", "adventurer.aseprite");
        AsepriteFile aseFile = AsepriteFileImporter.Import(filename);

        for (int i = 0; i < aseFile.Frames.Count; i++)
        {
            AsepriteFrame frame = aseFile.Frames[i];

            string outPath = Path.Combine(Environment.CurrentDirectory, "output", $"frame_{i}.png");

            Color[] pixels = frame.FlattenFrame();
            PngWriter.SaveTo(outPath, frame.Size, pixels);
        }
    }
}


