# WGTestAssignement
Test assignment on Unity UI and stuff

# How to work with this stuff
### Be sure you've built asset bundles before play mode
* See "Assets/Plugins/Editor/AssetBundlesBuilder.cs"
* See "Assets/Plugins/Editor/ProjectBuilder.cs"

### Build project using these custom scripts
### Probably you want to build asset bundles if you've updated Unity version

# Current state
* Plugins: Rider, VSCode, YamlDotNet
* Initial assets: UI, hangar, yaml configs, build time and not-so-fancy asset bundles
* Custom wannabe generic build scripts: asset bundles, project
* Generic async data loading by url and from asset bundles
* Convenient scene loading: somewhat emulates one-level-deep nested prefabs
* Initial configuration classes generated and filled using yaml config files
* Initial usage of configs, from which I get tank to display
* Initial API commentaries

# To do
* Fill UI from configs
* Store changes made by user
* Move generic scripts into plugins folder
* Sketch hangar scene
* Script fly around camera
