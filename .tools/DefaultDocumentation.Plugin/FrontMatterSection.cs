
using DefaultDocumentation.Api;
using DefaultDocumentation.Models;
using DefaultDocumentation.Models.Members;
using DefaultDocumentation.Models.Parameters;
using DefaultDocumentation.Models.Types;
using ICSharpCode.Decompiler.TypeSystem;

namespace DefaultDocumentation.Plugin;

public sealed class FrontMatterSection : ISection
{
    /// <summary>
    /// The name of this implementation used at the configuration level.
    /// </summary>
    public const string ConfigName = "FrontMatter";

    /// <inheritdoc/>
    public string Name => ConfigName;

    /// <inheritdoc/>
    public void Write(IWriter writer)
    {
        DocItem currentItem = writer.GetCurrentItem();

        string id = currentItem.Name.ToLower().Replace(' ', '-');
        string title = currentItem switch
        {
            AssemblyDocItem => $"{currentItem.Name} Assembly",
            NamespaceDocItem => $"{currentItem.Name} Namespace",
            TypeDocItem typeItem => $"{currentItem.GetLongName()} {typeItem.Type.Kind}",
            ConstructorDocItem => $"{currentItem.Name} Constructor",
            EventDocItem => $"{currentItem.GetLongName()} Event",
            FieldDocItem => $"{currentItem.GetLongName()} Field",
            MethodDocItem => $"{currentItem.GetLongName()} Method",
            OperatorDocItem => $"{currentItem.GetLongName()} Operator",
            PropertyDocItem => $"{currentItem.GetLongName()} Property",
            ExplicitInterfaceImplementationDocItem explicitItem when explicitItem.Member is IEvent => $"{currentItem.GetLongName()} Event",
            ExplicitInterfaceImplementationDocItem explicitItem when explicitItem.Member is IMethod => $"{currentItem.GetLongName()} Method",
            ExplicitInterfaceImplementationDocItem explicitItem when explicitItem.Member is IProperty => $"{currentItem.GetLongName()} Property",
            EnumFieldDocItem enumFieldItem => $"`{currentItem.Name}`{(enumFieldItem.Field.IsConst ? $" {enumFieldItem.Field.GetConstantValue()}" : string.Empty)}",
            ParameterDocItem parameterItem => $"`{currentItem.Name}`",
            TypeParameterDocItem typeParameterItem => $"`{typeParameterItem.TypeParameter.Name}`",
            _ => currentItem.GetLongName()
        };

        string sidebarLabel = currentItem.Name;

        writer.AppendLine("---");
        writer.AppendLine($"id: {id}");
        writer.AppendLine($"title: {title}");
        writer.AppendLine($"sidebar_label: {sidebarLabel}");
        writer.AppendLine("---");
    }
}
