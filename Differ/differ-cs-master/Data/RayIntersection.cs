using System;
using Differ.Shapes;

namespace Differ.Data
{
	public class RayIntersection
	{
		/** The first ray in the test */
		public Ray ray1 {get;set;}
		/** The second ray in the test */
		public Ray ray2 {get;set;}

		/** u value for ray1. */
		public float u1 = 0;
		/** u value for ray2. */
		public float u2 = 0;

		public RayIntersection ()
		{
		}

		public void reset() {
			ray1 = null;
			ray2 = null;
			u1 = 0;
			u2 = 0;
		}

		public void copy_from(RayIntersection other) {
			ray1 = other.ray1;
			ray2 = other.ray2;
			u1 = other.u1;
			u2 = other.u2;
		}

		public RayIntersection clone() {
			var clone = new RayIntersection();
			clone.copy_from(this);

			return clone;
		}
	}
}