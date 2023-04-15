using System;
using Differ.Shapes;

namespace Differ.Data
{
	public class ShapeCollision
	{
		/** The overlap amount */
		public float overlap = 0;
        /** X component of the separation vector, when subtracted from shape 1 will separate it from shape 2 */
        public float separationX = 0;
        /** Y component of the separation vector, when subtracted from shape 1 will separate it from shape */
    	public float separationY = 0;
        /** X component of the unit vector, on the axis of the collision (i.e the normal of the face that was collided with) */
    	public float unitVectorX = 0;
        /** Y component of the unit vector, on the axis of the collision (i.e the normal of the face that was collided with) */
        public float unitVectorY = 0;

        public float otherOverlap = 0;
        public float otherSeparationX = 0;
        public float otherSeparationY = 0;
        public float otherUnitVectorX = 0;
        public float otherUnitVectorY = 0;

        /** The shape that was tested */
    	public Shape shape1;
        /** The shape that shape1 was tested against */
    	public Shape shape2;

		public ShapeCollision ()
		{
		}

		public void reset() {
			shape1 = shape2 = null;
	        overlap = separationX = separationY = unitVectorX = unitVectorY = 0.0f;
	        otherOverlap = otherSeparationX = otherSeparationY = otherUnitVectorX = otherUnitVectorY = 0.0f;
		}

		public ShapeCollision clone() {
			var clone = new ShapeCollision();
			clone.copy_from(this);

			return clone;
		}

		public void copy_from(ShapeCollision other) {
			overlap = other.overlap;
	        separationX = other.separationX;
	        separationY = other.separationY;
	        unitVectorX = other.unitVectorX;
	        unitVectorY = other.unitVectorY;
	        otherOverlap = other.otherOverlap;
	        otherSeparationX = other.otherSeparationX;
	        otherSeparationY = other.otherSeparationY;
	        otherUnitVectorX = other.otherUnitVectorX;
	        otherUnitVectorY = other.otherUnitVectorY;
	        shape1 = other.shape1;
	        shape2 = other.shape2;
		}
	}
}