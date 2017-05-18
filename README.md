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
* Fill tanks collection UI from config
* Show selected tank model
* Initial API commentaries
* Additional scenes are loaded from asset bundles

# To do
* Make data loading/unloading less naive
* Select a tank UI by default
* Protect tank model creation code
* Fill UI from configs: tank info, user data
* Store changes made by user
* Sketch hangar scene
* Script fly around camera
