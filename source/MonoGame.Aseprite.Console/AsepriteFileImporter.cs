using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.IO;

public static class AsepriteFileImporter
{
    public static AsepriteFile Import(string filename)
    {
        return AsepriteFile.Load(filename);
    }
}
