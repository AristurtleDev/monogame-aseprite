#load "./common.config.cake"

const string MONOGAME_ASEPRITE_SLN_DIR = "../";
const string MONOGAME_ASEPRITE_SLN_PATH = $"{MONOGAME_ASEPRITE_SLN_DIR}MonoGame.Aseprite.sln";

const string MONOGAME_ASEPRITE_PROJECT_DIR = "../source/MonoGame.Aseprite/";
const string MONOGAME_ASEPRITE_CONTENT_PIPELINE_PROJECT_DIR = "../source/MonoGame.Aseprite.Content.Pipeline/";
const string MONOGAME_ASEPRITE_TESTS_PROJECT_DIR = "../tests/MonoGame.Aseprite.Tests/";

const string MONOGAME_ASEPRITE_CSPROJ = $"{MONOGAME_ASEPRITE_PROJECT_DIR}MonoGame.Aseprite.csproj";
const string MONOGAME_ASEPRITE_CONTENT_PIPELINE_CSPROJ = $"{MONOGAME_ASEPRITE_CONTENT_PIPELINE_PROJECT_DIR}MonoGame.Aseprite.Content.Pipeline.csproj";
const string MONOGAME_ASEPRITE_TESTS_CSPROJ = $"{MONOGAME_ASEPRITE_TESTS_PROJECT_DIR}MonoGame.Aseprite.Tests.csproj";

void ToggleGenerateDocuments(ICakeContext context, string projPath, bool value)
{
    context.XmlPoke(projPath, "//GenerateDocumentationFile", $"{value}");
}