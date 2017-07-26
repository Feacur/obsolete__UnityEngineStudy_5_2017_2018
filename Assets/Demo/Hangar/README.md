# Summary
Study on Unity engine. Very simple hangar of a tank game.

# How to work with this stuff
### Mark asset bundles
* "environment": "Materials/Hangar", "Prefabs/Hangar"
* "tanks": "Materials/Tanks", "Prefabs/Tanks"
* "scenes": "Base.unity", "Hangar UI.unity", "Hangar.unity"

### I might have forgotten something here... See previous revisions on GitHub!
* Make "Loader.unity" a default scene

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
* Assets: Hangar UI, Hangar, tanks, prefabs, yaml configs
* User data is changable and savable
* Sketched tanks, hangar
* Simple hangar camera

# To do
* Add inertia to the hangar camera
