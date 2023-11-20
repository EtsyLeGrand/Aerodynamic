# Aerodynamic
**Comment utiliser**

Le personnage est fait de plusieurs parties du corps:
- Avant-bras x2  
- Bras x2  
- Cuisses x2  
- Mollets x2  
- Torse  
- Tête  
- Cou*  

*Le cou n'est pas compté dans les opérations avec la physique  

Chaque partie du corps sauf le cou contiennent des Hinge Joints et des Colliders, en plus du Collider sur le GameObject parent.
Le Collider général sert de collider d'animation, et les autres servent au mode Physique.

Les scripts du mode physique sont sur le prefab du Stickman. Les valeurs de spring de Hinge Joint (qui gèrent les mouvements quand le personnage est en l'air) sont dans des "Target" (ScriptableObjects).

Pour tout ce qui est hors-personnage, les prefabs sont bâtis, donc c'est du plug-and-play. Vous pouvez placer un trapèze dans une map pour tester.

Note de mapbuilding, assurez-vous que le layer du sol est "Floor", et que les murs soient sur un autre layer. Si ce n'est pas le cas, le personnage pourrait se relever dans un mur.