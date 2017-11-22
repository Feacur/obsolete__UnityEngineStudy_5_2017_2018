# Summary
Exploring Entity Component System pattern.  

# How to work with this stuff
Use scene "Scenes/Main.unity"  

# Current state
## Generic
Entity is an interface. Probably I should provide a generic C# implementation.  

## Inspired by Svelto.ECS
Components are interfaces:  
* Single class or struct can implement multiple components
* Unity MonoBehaviour can be utilized for this purpose

# Entity Component System
Basic elements:  
* Entity - a set of components
* Component - some data, state, trait
* System - specific logic for engine or game

# Data Oriented Design
Cache coherency  
Components can be laid out as a Structure of Arrays  
Surely, better implementing them as value-objects then  

# See also
Unity (GameObject, MonoBehaviour, but no systems)  
Unreal Engine 4 (Actor or its descendants, Component, but no systems)  
[Svelto.ECS](https://github.com/sebas77/Svelto.ECS)  
[Entitas](https://github.com/sschmid/Entitas-CSharp)  
[Ash](https://github.com/richardlord/Ash)  
[Artemis](https://github.com/junkdog/artemis-odb)  