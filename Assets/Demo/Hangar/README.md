# Summary
Very simple hangar of a tank game.  

# How to work with this stuff
Use scene "Scenes/Main.unity"  
See "Editor/HangarBuilder.cs"  

### Prepare assets before playing
Use menu "Custom/Hangar/Prepare streaming assets"  
Use menu "Custom/Hangar/Mark asset bundles"  

### Clean up assets before leaving
Probably delete "Assets/Streaming Assets"  
Use menu "Custom/Hangar/Unmark asset bundles"  

### Your changes won't automatically get into asset bundles in editor
Be sure you've built asset bundles before play mode  
Probably you want to rebuild asset bundles if you've updated Unity version  
Probably you want to rebuild asset bundles if you've changed some code assemblies  

### Also you can define EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
See "Assets/Plugins/Custom/Data/StreamingData.cs"  

# Current state
* Assets: Hangar UI, Hangar, tanks, prefabs, yaml configs
* User data is changable and savable
* Sketched tanks, hangar
* Simple hangar camera

# To do
* Add inertia to the hangar camera
