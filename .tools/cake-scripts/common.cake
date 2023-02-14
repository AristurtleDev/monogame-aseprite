public const string ROOT_DIR = "../";

private string _target = Argument("target", "All");
public bool _doGenerateDocs = HasArgument("docs");
public bool _doIncrementBuildNumber = HasArgument("increment-build");
public string _outputDirectory = Argument("output-directory", $"{ROOT_DIR}Artifacts");
public string _configuration = Argument("configuration", "Debug");
private bool _noRestore = HasArgument("no-restore");
private bool _noClean = HasArgument("no-clean");
