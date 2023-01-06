#!/usr/bin/env bash
VERBOSITY="q"
RESTORE="--no-restore"

while getopts v:r: flag
do
    case "${flag}" in
        v) VERBOSITY=${OPTARG};;
        r) RESTORE="";;
    esac
done

# -----------------------------------------------------------------------------
# Do a restore on all projects
# -----------------------------------------------------------------------------
echo "Restoring dependencies"
dotnet restore --verbosity $VERBOSITY

# -----------------------------------------------------------------------------
# Build the MonoGame.Aseprite project
# -----------------------------------------------------------------------------
pushd ./source/MonoGame.Aseprite/   # Navigate to the MonoGame.Aseprite directory
dotnet build $NORESTORE             # Build
popd                                # Back out to the root directory

# -----------------------------------------------------------------------------
# Build the MonoGame.Aseprite.Content.Pipeline project
# -----------------------------------------------------------------------------
pushd ./source/MonoGame.Aseprite.Content.Pipeline/  # navigate to the MonoGame.Aseprite.Content.Pipeline directory
dotnet build $NORESTORE             # Build
popd                                # back out to the root directory

# -----------------------------------------------------------------------------
# clean the DemoGame's content files
# -----------------------------------------------------------------------------
pushd ./source/DemoGame/            # navigate to the DemoGame project directory
rm -rf ./bin/                       # remove the bin directory
pushd ./Content/                    # navigate to the demo game content directory
rm -rf ./bin/                       # remove the bin directory there
popd                                # back out to the DemoGame project directory
dotnet build $NORESTORE             # Build
popd                                # back out to the root directory

