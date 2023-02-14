#load "cake-scripts/common/common.cake"
#load "cake-scripts/setup.cake"
#load "cake-scripts/teardown.cake"
#load "cake-scripts/tasks/tasks.cake"


CommonConfiguration.ParseArguments(Context);
CommonConfiguration.ParseVersion(Context);
CommonConfiguration.ParseRepositoryUrl(Context);
CommonConfiguration.InitializeBuildSettings();

RunTarget(CommonConfiguration.Target);