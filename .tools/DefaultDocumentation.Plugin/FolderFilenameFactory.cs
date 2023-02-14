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


        private static class PathCleaner
        {
            private static readonly string[] toTrimChars = new[] { '=', ' ' }.Select(c => $"{c}").ToArray();
            private static readonly string[] invalidChars = new[] { '\"', '<', '>', ':', '*', '?' }.Concat(Path.GetInvalidPathChars()).Select(c => $"{c}").ToArray();

            public static string RemoveCamelCase(string value)
            {
                //  Split on camel case
                value = Regex.Replace(value, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();

                //  Split on space
                string[] split = value.Split(" ");

                //  Join back with '-' between
                value = string.Join('-', split);

                return value;
            }

            public static string Clean(string value, string invalidCharReplacement)
            {
                foreach (string toTrimChar in toTrimChars)
                {
                    value = value.Replace(toTrimChar, string.Empty);
                }

                invalidCharReplacement = string.IsNullOrEmpty(invalidCharReplacement) ? "_" : invalidCharReplacement;

                foreach (string invalidChar in invalidChars)
                {
                    value = value.Replace(invalidChar, invalidCharReplacement);
                }

                return value.Trim('/');
            }
        }



        //  Opting to delete the entire output directory.  
        public void Clean(IGeneralContext context)
        {
            context.Settings.Logger.Debug($"Deleting output folder '{context.Settings.OutputDirectory}'");
            if (Directory.Exists(context.Settings.OutputDirectory.FullName))
            {
                Directory.Delete(context.Settings.OutputDirectory.FullName, recursive: true);
            }
        }


        public string GetFileName(IGeneralContext context, DocItem item)
        {
            string value = item switch
            {
                AssemblyDocItem assemblyDocItem when item is AssemblyDocItem => assemblyDocItem.FullName,
                EntityDocItem entityDocItem when item is EntityDocItem => GetFilenameFromEntityDoc(entityDocItem),
                ExternDocItem externDocItem when item is ExternDocItem => throw new NotImplementedException($"Extern Doc Item: " + item.FullName),
                NamespaceDocItem namespaceDocItem when item is NamespaceDocItem => throw new NotImplementedException($"NameSpace Doc Item: " + item.FullName),
                _ => throw new InvalidOperationException($"Unknown doc item type: {item.GetType()}")
            };

            value = PathUtils.Sanitize(value, GetInvalidCharReplacement(context)) + ".md";
            return value;
        }

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

        private string Attempt(EntityDocItem item)
        {
            IEnumerable<DocItem> parents = item.GetParents().Skip(1);
            string[] pathNames = new string[parents.Count()];

            int i = 0;
            foreach (DocItem parent in parents)
            {
                string parentName = parent.Name;
                // if(parentName.Contains('.'))
                // {
                //     parentName = parentName.Replace('.', '-');
                // }
                // else
                // {
                //     parentName = PathCleaner.RemoveCamelCase(parentName);
                // }
                pathNames[i] = parentName;
                i++;
            }


            string path = string.Join('/', pathNames);

            if (item.Entity.SymbolKind == SymbolKind.TypeDefinition)
            {
                // string name = PathCleaner.RemoveCamelCase(item.Name);
                path += $"/{item.Name}";
            }

            else if (item.Entity.SymbolKind == SymbolKind.Property)
            {
                path += $"/Properties";
            }
            else
            {
                path += $"/{item.Entity.SymbolKind}s";
            }

            // string itemName = PathCleaner.RemoveCamelCase(item.Name);
            // path += item.Entity.SymbolKind switch
            // {
            //     SymbolKind.TypeDefinition => $"/{itemName}",
            //     SymbolKind.Property => $"/properties",
            //     _ => $"/{item.Entity.SymbolKind}s"
            // };

            string longName = string.Join(".", item.GetParents().Skip(2).Select(p => p.Name).Concat(Enumerable.Repeat(item.Name, 1)));
            path += $"/{longName}";
            Console.WriteLine(longName);

            return path;


        }
    }
}
