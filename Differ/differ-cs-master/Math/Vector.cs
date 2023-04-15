using System;

namespace Differ.Math
{
	//NOTE : Only implements the basics required for the collision code.
	//The goal is to make this library as simple and unencumbered as possible, making it easier to integrate
	//into an existing codebase. This means that using abstracts or similar you can add a function like "toMyEngineVectorFormat()"
	//or simple an adapter pattern to convert to your preferred format. It simplifies usage and handles internals, nothing else.
	//This also means that ALL of these functions are used and are needed.
	[Serializable]
	public class Vector
	{
		public float x = 0;
		public float y = 0;

		public float length {
			get {
				return (float)System.Math.Sqrt(lengthsq);
			}

			set {
				float ep = float.Epsilon;
				float angle = (float)System.Math.Atan2(y, x);

				x = (float)System.Math.Cos(angle) * value;
				y = (float)System.Math.Sin(angle) * value;

				if (System.Math.Abs(x) < ep) x = 0;
				if (System.Math.Abs(y) < ep) y = 0;
			}
		}

		public float lengthsq { get { return x * x + y * y; } }

		public Vector (float x = 0, float y = 0)
		{	
			this.x = x;
			this.y = y;
		}

		/** Copy, returns a new vector instance from this vector. */
		public Vector clone() {
			return new Vector(x, y);
		}

		/** Transforms Vector based on the given Matrix. Returns this vector, cloned and modified. */
		public Vector transform(Matrix matrix) {
			var v = clone();

			v.x = x*matrix.a + y*matrix.c + matrix.tx;
            v.y = x*matrix.b + y*matrix.d + matrix.ty;

        	return v;
		}

		/** Sets the vector's length to 1. Returns this vector, modified. */
		public Vector normalize() {

			// Weird hack from original?
			if (length == 0) {
	            x = 1;
	            return this;
	        }

	        var len = length;

            x /= len;
            y /= len;

	        return this;
		}

		/** Sets the length to fit under the given maximum value.
            Nothing is done if the vector is already shorter.
            Returns this vector, modified. */
		public Vector truncate(float max) {
			length = (float)System.Math.Min(max, length);
			return this;
		}

		/** Invert this vector. Returns this vector, modified. */
		public Vector invert() {
			x = -x;
            y = -y;

        	return this;
		}

		/** Return the dot product of this vector and another vector. */
		public float dot(Vector other) {
			return x * other.x + y * other.y;
		}

		/** Return the cross product of this vector and another vector. */
		public float cross(Vector other) {
			return x * other.x - y * other.y;
		}

		/** Add a vector to this vector. Returns this vector, modified. */
		public Vector add(Vector other) {

			x += other.x;
            y += other.y;

        	return this;
		}

		/** Subtract a vector from this one. Returns this vector, modified. */
	    public Vector subtract(Vector other) {

            x -= other.x;
            y -= other.y;

	        return this;
	    }

	    public override string ToString ()
		{
			return string.Format ("[Vector: x={0} y={1} length={2}]", x, y, length);
		}
	}
}