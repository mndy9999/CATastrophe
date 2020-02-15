

	To get the AI patrol working:


	Each level set prefab has a child land_platform_# (the ground)
	This object must have a Box Collider attached to it - make sure the collider's center is (0, 0, 0).
	It must also contain the Custom Grid component.
	Any other big object on the map (tents) must have a mesh collider to stop the AI from walking through it.

	Create empty gameobject (Environment).
	Add the NavMeshSurface component to the Environment GO.
	Spawn all the level sets as children of the Environment GO.
	When done spawning, call NavMeshSurface.BuildNavMesh();

	Spawn enemies at position y = 1.0f. (they use a raycast towards Vector3.down to check for the ground collider so they know their tile)