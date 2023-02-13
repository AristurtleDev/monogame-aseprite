using System.Text.RegularExpressions;
using DefaultDocumentation.Api;
using DefaultDocumentation.Models;
using ICSharpCode.Decompiler.TypeSystem;

namespace DefaultDocumentation.Plugin
{
    public sealed class FolderFileNameFactory : IFileNameFactory
    {
        #region Markdown members not yet available in nuget

        private const string InvalidCharReplacementKey = "Markdown.InvalidCharReplacement";

        public static string? GetInvalidCharReplacement(IGeneralContext context) => context.GetSetting<string>(InvalidCharReplacementKey);


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

        #endregion

        public string Name => "Folder";

        public void Clean(IGeneralContext context)
        {
            context.Settings.Logger.Debug($"Cleaning output folder \"{context.Settings.OutputDirectory}\"");

            if (context.Settings.OutputDirectory.Exists)
            {
                IEnumerable<FileInfo> files = context.Settings.OutputDirectory.EnumerateFiles("*.md", SearchOption.AllDirectories).Where(f => !string.Equals(f.Name, "readme.md", StringComparison.OrdinalIgnoreCase));

                int i;

                foreach (FileInfo file in files)
                {
                    i = 3;
                start:
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        if (--i > 0)
                        {
                            Thread.Sleep(100);
                            goto start;
                        }

                        throw;
                    }
                }

                i = 3;
                while (files.Any() && i-- > 0)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public string GetFileName(IGeneralContext context, DocItem item)
        {
            string value = item switch
            {
                AssemblyDocItem assemblyDocItem when item is AssemblyDocItem => GetAssemblyDocItemFileName(assemblyDocItem),
                EntityDocItem entityDocItem when item is EntityDocItem => GetEntityDocItemFileName(entityDocItem),
                ExternDocItem externDocItem when item is ExternDocItem => throw new NotImplementedException($"Extern Doc Item: " + item.FullName),
                NamespaceDocItem namespaceDocItem when item is NamespaceDocItem => throw new NotImplementedException($"NameSpace Doc Item: " + item.FullName),
                _ => throw new InvalidOperationException($"Unknown doc item type: {item.GetType()}")
            };

            value = PathCleaner.Clean(value, GetInvalidCharReplacement(context)) + ".md";
            return value;

            // return PathCleaner.Clean(item is AssemblyDocItem ? item.FullName : string.Join("/", item.GetParents().Skip(1).Select(p => p.Name).Concat(Enumerable.Repeat(item.Name, 1))), GetInvalidCharReplacement(context)) + ".md";
        }

        private string GetAssemblyDocItemFileName(AssemblyDocItem item)
        {
            string value = item.FullName;
            return value;
            // return item.FullName;
        }

        private string GetEntityDocItemFileName(EntityDocItem item)
        {
            string assumedValue = string.Join("/", item.GetParents().Skip(1).Select(p => p.Name).Concat(Enumerable.Repeat(item.Name, 1)));
            string value = Attempt(item);
            return value;
        }

        private string Attempt(EntityDocItem item)
        {
            IEnumerable<DocItem> parents = item.GetParents().Skip(1);
            string[] pathNames = new string[parents.Count()];

            int i = 0;
            foreach(DocItem parent in parents)
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

            if(item.Entity.SymbolKind == SymbolKind.TypeDefinition)
            {
                // string name = PathCleaner.RemoveCamelCase(item.Name);
                path += $"/{item.Name}";
            }

            else if(item.Entity.SymbolKind == SymbolKind.Property)
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

            path += $"/{item.Name}";

            return path;


        }
    }
}
