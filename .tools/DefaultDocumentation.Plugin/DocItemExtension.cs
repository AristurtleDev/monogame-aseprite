using DefaultDocumentation.Models;

namespace DefaultDocumentation.Plugin;

public static class DocItemExtension
{
    /// <summary>
    ///     Gets the long name of a <see cref="DocItem"/>, being its full name without its namespace.
    ///     This method should not be called on <see cref="AssemblyDocItem"/> or <see cref="NamespaceDocItem"/> types.
    /// </summary>
    /// <param name="item">
    ///     The <see cref="DocItem"/> for which to get its long name.
    /// </param>
    /// <returns>
    ///     The long name of the <see cref="DocItem"/>.
    /// </returns>
    public static string GetLongName(this DocItem item) => string.Join(".", item.GetParents().Skip(2).Select(p => p.Name).Concat(Enumerable.Repeat(item.Name, 1)));
}