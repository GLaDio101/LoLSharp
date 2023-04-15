using System;
using Differ.Data;
using Differ.Sat;

namespace Differ.Shapes
{
	public class Circle : Shape
	{
		private float _radius;
		public float radius { get { return _radius; } set { _radius = value; name = "circle " + radius; } }
		public float transformedRadius { get { return radius * scaleX; } }

		public Circle (float x, float y, float radius) : base(x, y)
		{
			this.radius = radius;
		}

		public override ShapeCollision test (Shape shape)
		{
			return shape.testCircle(this, true);
		}

		public override ShapeCollision testCircle (Circle circle, bool flip = false)
		{
			return Sat2D.testCircleVsCircle(this, circle, flip);
		}

		public override ShapeCollision testPolygon (Polygon polygon, bool flip = false)
		{
			return Sat2D.testCircleVsPolygon( this, polygon, flip );
		}

		public override RayCollision testRay (Ray ray)
		{
			return Sat2D.testRayVsCircle(ray, this);
		}
	}
}