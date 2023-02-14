#load "./common.config.cake"

const string MONOGAME_ASEPRITE_SLN_DIR = "../";
const string MONOGAME_ASEPRITE_SLN_PATH = $"{MONOGAME_ASEPRITE_SLN_DIR}MonoGame.Aseprite.sln";

const string MONOGAME_ASEPRITE_PROJECT_DIR = "../source/MonoGame.Aseprite/";
const string MONOGAME_ASEPRITE_COMMON_PROJECT_DIR = "../source/MonoGame.Aseprite.Common/";
const string MONOGAME_ASEPRITE_CONTENT_PIPELINE_PROJECT_DIR = "../source/MonoGame.Aseprite.Content.Pipeline/";
const string MONOGAME_ASEPRITE_COMMON_TESTS_PROJECT_DIR = "../tests/MonoGame.Aseprite.Common.Tests/";

const string MONOGAME_ASEPRITE_CSPROJ = $"{MONOGAME_ASEPRITE_PROJECT_DIR}MonoGame.Aseprite.csproj";
const string MONOGAME_ASEPRITE_COMMON_CSPROJ = $"{MONOGAME_ASEPRITE_COMMON_PROJECT_DIR}MonoGame.Aseprite.Common.csproj";
const string MONOGAME_ASEPRITE_CONTENT_PIPELINE_CSPROJ = $"{MONOGAME_ASEPRITE_CONTENT_PIPELINE_PROJECT_DIR}MonoGame.Aseprite.Content.Pipeline.csproj";
const string MONOGAME_ASEPRITE_COMMON_TESTS_CSPROJ = $"{MONOGAME_ASEPRITE_COMMON_TESTS_PROJECT_DIR}MonoGame.Aseprite.Common.Tests.csproj";

const string DEFAULT_DOC_PLUGIN_DIR = $"./DefaultDocumentation.Plugin/";
const string DEFAULT_DOC_PLUGIN_SLN_PATH = $"{DEFAULT_DOC_PLUGIN_DIR}DefaultDocumentation.Plugin.sln";

void ToggleGenerateDocuments(ICakeContext context, string projPath, bool value)
{
    context.XmlPoke(projPath, "//GenerateDocumentationFile", $"{value}");
}