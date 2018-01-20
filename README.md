# Summary
Study on Unity engine.  

# How to work with this stuff
See sub-projects inside "Assets/Demo" folder  
Choose one, follow instructions of the embedded README.md

### Notably
* ClientServerUi - emulating client-server communication
* Hangar - yaml configs, asset bundles, async loading
* Portals - simple portals with look-through and teleportation

# Current state
* Plugins: YamlDotNet, also custom utilities
* Asset bundles: non-automatic; there is an option to emulate them in editor
* Build scripts: for asset bundles and for the project
* Data IO: load by url, load from asset bundles, persistent data
* Input: drag, pinch, tap
* One level deep nested prefabs emulation using scene files and actual prefabs
* API comments

# To do
* Loader splash screen to hide async loading from user
* DragInput script should support gamepads (use virtual position)
* Fix lighting issue when using EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
* Create a build system for multiple sub-projects

# Additional Unity notes
* Explicitly include your data to the build
* Standard shaders can be linked through GraphicSettings
