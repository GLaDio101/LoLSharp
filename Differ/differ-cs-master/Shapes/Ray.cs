using System;
using Differ.Math;

namespace Differ.Shapes
{
	public class Ray
	{
		public Vector start;
		public Vector end;

		private Vector _dir;
		public Vector dir { 
			get {
				_dir.x = end.x - start.x;
       	 		_dir.y = end.y - start.y;
       	 		return _dir;
			}
		}

		public bool infinite;

		public Ray (Vector start, Vector end, bool infinite = true)
		{
			this.start = start;
			this.end = end;
			this.infinite = infinite;

			_dir = new Vector(end.x - start.x, end.y - start.y);
		}
	}
}