# Summary
Study on Unity engine.

# How to work with this stuff
### Build the project and asset bundles using these
* See "Assets/Plugins/Custom/Editor/AssetBundlesBuilder.cs"
* See "Assets/Plugins/Custom/Editor/ProjectBuilder.cs"

### Your changes won't automatically get into asset bundles in editor
### Be sure you've built asset bundles before play mode
### Probably you want to rebuild asset bundles if you've updated Unity version
### Probably you want to rebuild asset bundles if you've changed some code assemblies

### Also you can define EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
* See "Assets/Plugins/Custom/Data/StreamingData.cs"
* But there is an issue with lighting, sorry

# Current state
* Plugins: Rider, VSCode, YamlDotNet, also custom utilities
* Assets: Hangar UI, Hangar, tanks, prefabs, yaml configs
* Asset bundles: non-automatic; there is an option to emulate them in editor
* Build scripts: for asset bundles and for the project
* Data IO: load by url, load from asset bundles, persistent data
* Input: abstract drag
* One level deep nested prefabs emulation using scene files and actual prefabs
* API comments
* User data is changable and savable
* Sketched tanks, hangar
* Simple hangar camera

# To do
* Loader splash screen to hide async loading from user
* DragInput script should support gamepads (use virtual position)
* Fix lighting issue when using EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
* Add inertia to the hangar camera
* Create a build system for multiple sub-projects

# Additional Unity notes
* Explicitly include your data to the build
* Standard shaders can be linked through GraphicSettings