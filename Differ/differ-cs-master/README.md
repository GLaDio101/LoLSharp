## What is differ-cs?

A port of the original differ for Haxe (https://github.com/underscorediscovery/differ). The port is (where possible) a one-to-one mapping, with a few changes to the API (no more out parameters). This is a [Separating Axis Theorom](http://en.wikipedia.org/wiki/Hyperplane_separation_theorem) collision library for C# games, intended for use in Unity.

----

## Facts

- Supports polygons, circles, and rays currently.
- 2D only (for now).
- Includes a simple drawing interface for debugging shapes (Not in my port, in the original)
- **COLLISION ONLY.** No physics here. By design :)
- Contributions welcome

##Quick look

**A simple collision example**
This is taken from the original, but this syntax is 100% compatible.

    var circle = new Circle( 300, 200, 50 );
    var box = Polygon.rectangle( 0, 0, 50, 150 );

    box.rotation = 45;

    var collideInfo = Collision.shapeWithShape( circle, box );

    if(collideInfo != null) {
        //use collideInfo.separationX
        //    collideInfo.separationY
        //    collideInfo.normalAxisX
        //    collideInfo.normalAxisY
        //    collideInfo.overlap
    }

### Roadmap

- Unit tests, the original had none
- Clean up code style
- Implement ShapeDrawer for Unity
- Create demos
