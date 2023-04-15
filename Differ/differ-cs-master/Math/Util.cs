using System;

namespace Differ.Math
{
	public class Util
	{
		public static float vec_lengthsq(float x, float y) {
	        return x * x + y * y;
	    }

	    public static float vec_length(float x, float y) {
	    	return (float)System.Math.Sqrt(vec_lengthsq(x, y));
	    }

		public static float vec_normalize(float length, float component) {
			if (length == 0) return 0;
	        return component /= length;
	    }

	    public static float vec_dot(float x, float y, float otherx, float othery) {
	    	return x * otherx + y * othery;
	    }
	}
}

