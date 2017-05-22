# Summary
Test assignment on Unity, UI and programming stuff

Reference: WoT hangar scene

Up to the point it has been like 32 hours of pure busy time. Not bad, I believe. Besides this project turned out to be extremely instrumental for my current job. For reals, you need to sit back sometimes, put down your busy work down for a while and do some R&D.

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
* Simple fly around camera

# To do
* Loader splash screen to hide async loading from user
* Rective UI changes, probably? This is more of a clean code feature.
* PersistentData script should be able to create folders
* DragInput script should support gamepads
* Fix lighting issue when using EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
