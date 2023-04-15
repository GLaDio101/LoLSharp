using System;
using Differ.Shapes;

namespace Differ.Data
{
	public class RayCollision
	{
		/** Shape the intersection was with. */
		public Shape shape {get;set;}
		/** The ray involved in the intersection. */
		public Ray ray {get;set;}

		/** Distance along ray that the intersection start at. */
		public float start {get;set;}
		/** Distance along ray that the intersection ended at. */
		public float end {get;set;}

		public RayCollision ()
		{
		}

		public void reset() {
			ray = null;
			shape = null;
			start = 0;
			end = 0;
		}

		public void copy_from(RayCollision other) {
			ray = other.ray;
			shape = other.shape;
			start = other.start;
			end = other.end;
		}

		public RayCollision clone() {
			var clone = new RayCollision();
			clone.copy_from(this);

			return clone;
		}
	}
}

// Need helper extensions later