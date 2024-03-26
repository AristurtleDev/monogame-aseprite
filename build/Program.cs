using Cake.Frosting;
using MonoGame.Aseprite.Build;

return new CakeHost()
            .UseWorkingDirectory("../")
            .UseContext<BuildContext>()
            .Run(args);
