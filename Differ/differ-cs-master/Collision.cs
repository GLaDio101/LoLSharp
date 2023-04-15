using System;
using Differ.Data;
using Differ.Shapes;
using System.Collections.Generic;
using Differ.Sat;

namespace Differ
{
	public class Collision
	{
		public Collision ()
		{
		}

		/** Test a single shape against another shape.
            When no collision is found between them, this function returns null.
            Returns a `ShapeCollision` if a collision is found. */
		public static ShapeCollision shapeWithShape(Shape shape1, Shape shape2) {
			return shape1.test(shape2);
		}

		/** Test a single shape against multiple other shapes.
            When no collision is found, this function returns an empty array, this function will never return null.
            Returns a list of `ShapeCollision` information for each collision found. */
		public static IList<ShapeCollision> shapeWithShapes(Shape shape, IList<Shape> shapes) {
			var results = new List<ShapeCollision>();

			foreach (var otherShape in shapes) {
				var result = shapeWithShape(shape, otherShape);
				if (result != null) {
					results.Add(result);
				}
			}

			return results;
		}

		/** Test a line between two points against a list of shapes.
            When no collision is found, this function returns null.
            Returns a `RayCollision` if a collision is found. */
		public static RayCollision rayWithShape(Ray ray, Shape shape) {
			return shape.testRay(ray);
		}

		/** Test a ray between two points against a list of shapes.
            When no collision is found, this function returns an empty array, this function will never return null.
            Returns a list of `RayCollision` information for each collision found. */
		public static IList<RayCollision> rayWithShapes(Ray ray, IList<Shape> shapes) {
			var results = new List<RayCollision>();

	        foreach (var shape in shapes) {
	            var result = rayWithShape(ray, shape);
	            if (result != null) {
	                results.Add(result);
	            }
	        }

	        return results;
		}

		/** Test a ray against another ray.
            When no collision is found, this function returns null.
            Returns a `RayIntersection` if a collision is found. */
		public static RayIntersection rayWithRay(Ray ray1, Ray ray2) {
			return Sat2D.testRayVsRay(ray1, ray2);
		}

		/** Test a ray against a list of other rays.
            When no collision is found, this function returns an empty array, this function will never return null.
            Returns a list of `RayIntersection` information for each collision found. */
	    public static IList<RayIntersection> rayWithRays(Ray ray, IList<Ray> rays) {

	        var results = new List<RayIntersection>();

	        foreach (var other in rays) {
	            var result = rayWithRay(ray, other);
	            if (result != null) {
	                results.Add(result);
	            }
	        }

	        return results;

	    }

		/** Test if a given point lands inside the given polygon.
            Returns true if it does, false otherwise. */
	    public bool pointInPoly(float x, float y, Polygon poly) {
	    	throw new NotImplementedException();
	    }
	}
}