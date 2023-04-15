using System;

namespace Differ.Math
{
	//NOTE : Only implements the basics required for the shape transformations
	//NOTE : Not a full implementation, some code copied from openfl/flash/geom/Matrix.hx
	public class Matrix
	{
		public float a;
		public float b;
		public float c;
		public float d;
		public float tx;
		public float ty;

		public Matrix(float a = 1, float b = 0, float c = 0, float d = 1, float tx = 0, float ty = 0) 
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.d = d;
			this.tx = tx;
			this.ty = ty;
		}

		public void identity() {
			a = 1;
			b = 0;
			c = 0;
			d = 1;
			tx = 0;
			ty = 0;
		}

		public void translate(float x, float y) {
			tx += x;
			ty += y;
		}

		public Matrix makeTranslation(float x, float y) {
			tx = x;
			ty = y;

			return this;
		}

		public void rotate (float angle) {
			var cos = (float)System.Math.Cos(angle);
	        var sin = (float)System.Math.Sin(angle);

	        var a1 = a * cos - b * sin;
	            b = a * sin + b * cos;
	            a = a1;

	        var c1 = c * cos - d * sin;
	            d = c * sin + d * cos;
	            c = c1;

	        var tx1 = tx * cos - ty * sin;
	            ty = tx * sin + ty * cos;
	            tx = tx1;
		}

		public void scale(float x, float y) {
			a *= x;
	        b *= y;

	        c *= x;
	        d *= y;

	        tx *= x;
	        ty *= y;
		}

		public void compose(Vector position, float rotation, Vector scale) {
			identity();

	        this.scale(scale.x, scale.y);
	        rotate(rotation);
			makeTranslation(position.x, position.y);
		}

		public override string ToString ()
		{
			return string.Format ("[Matrix a={0} b={1] c={2} d={3} tx={4} ty={5}]", a, b, c, d, tx, ty);
		}
	}
}