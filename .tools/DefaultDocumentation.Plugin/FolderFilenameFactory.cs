using System.Text.RegularExpressions;
using DefaultDocumentation.Api;
using DefaultDocumentation.Models;
using ICSharpCode.Decompiler.TypeSystem;

namespace DefaultDocumentation.Plugin
{
    public sealed class FolderFileNameFactory : IFileNameFactory
    {
        private const string INVALID_CHAR_REPLACEMENT_KEY = "Markdown.InvalidCharReplacement";
        public static string? GetInvalidCharReplacement(IGeneralContext context) => context.GetSetting<string>(INVALID_CHAR_REPLACEMENT_KEY);
        public string Name => "Folder";


        /// <summary>
        ///     Cleans the output directory of previously generated documentation file.
        /// </summary>
        /// <remarks>
        ///     Implements IFileNameFactor.Clean interface member.  Perform
        /// </remarks>
        /// <param name="context">
        ///     The context of the current documentation generation process.
        /// </param>
        public void Clean(IGeneralContext context)
        {
            //  Sample project goes file-by-file and deletes them. It seems to do that so it can skip deleting some
            //  files like a "readme.md" file.
            //
            //  However, I have all documentation generated using a cake build script, which sets the 
            //  context.Settings.OutputDirectory to a general purpose output. So I'm opting to delete the entire
            //  directory instead since I know it only contains the generated output and not other files.
            context.Settings.Logger.Debug($"Deleting output folder '{context.Settings.OutputDirectory}'");
            if (Directory.Exists(context.Settings.OutputDirectory.FullName))
            {
                Directory.Delete(context.Settings.OutputDirectory.FullName, recursive: true);
            }
        }


        public string GetFileName(IGeneralContext context, DocItem item)
        {
            
            //  File name is dependent on the DocItem type
            //  Below handles the AssemblyDocItem and EntityDocItem types
            //
            //  My project does not have any ExternDocItem or NameSpaceDocItem types.  For now, I'll just throw an
            //  exception that they are not implemented.  I'll revisit should I decide I need them.
            string value = item switch
            {
                AssemblyDocItem assemblyDocItem when item is AssemblyDocItem => assemblyDocItem.FullName,
                EntityDocItem entityDocItem when item is EntityDocItem => GetFilenameFromEntityDoc(entityDocItem),
                ExternDocItem externDocItem when item is ExternDocItem => throw new NotImplementedException($"Extern Doc Item: " + item.FullName),
                NamespaceDocItem namespaceDocItem when item is NamespaceDocItem => throw new NotImplementedException($"NameSpace Doc Item: " + item.FullName),
                _ => throw new InvalidOperationException($"Unknown doc item type: {item.GetType()}")
            };

            //  Clean up the file path by removing invalid characters and appending the ".md" extension.
            value = PathUtils.Sanitize(value, GetInvalidCharReplacement(context)) + ".md";

            return value;
        }

        //  ----------------------------------------------------------------
        //  Gets the path for EntityDocItem types
        //  The should generate paths similar to the following
        //
        //  For this example, assume the following is true
        //      - Assembly Name = "MonoGame.Aseprite"
        //      - Namespace of class = "MonoGame.Aseprite.Sprites"
        //      - Class Name = "Sprite"
        //          - Has properties: Color and Name
        //          - Has 2 constructors
        //          - Has 2 methods
        //
        //  Assuming the above, the paths generated for this type
        //  should be like the following
        //
        //  MonoGame.Aseprite/
        //      ↳ MonoGame.Aseprite.Sprites/
        //          ↳ Sprite/
        //              ↳ Sprite.md
        //              ↳ Properties/
        //                  - Color.md
        //                  - Name.md
        //              ↳ Constructors/
        //                  - Sprite(string,Texture2D).md
        //                  - Sprite(string,TextureRegion).md
        //              ↳ Methods/
        //                  - Draw(SpriteBatch,Vector2).md
        //                  - FromRaw(GraphicsDevice,RawSprite).md
        //  ----------------------------------------------------------------
        private string GetFilenameFromEntityDoc(EntityDocItem item)
        {
            string path = GetPathForEntityDocItem(item);
            return $"{path}/{item.Name}";
        }


        private string GetPathForEntityDocItem(EntityDocItem item)
        {
            IEnumerable<DocItem> parents = item.GetParents().Skip(1);
            string[] pathParts = new string[parents.Count()];

            int i = 0;
            foreach (DocItem parent in parents)
            {
                pathParts[i] = parent.Name;
                i++;
            }

            string path = string.Join('/', pathParts);

            path += item.Entity.SymbolKind switch
            {
                SymbolKind.TypeDefinition => $"/{item.Name}",
                SymbolKind.Property => "/Properties",
                _ => $"/{item.Entity.SymbolKind}s"
            };

            return path;
        }
    }
}
