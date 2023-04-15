using System;
using System.Collections.Generic;
using Differ.Math;
using Differ.Data;

namespace Differ.Shapes
{
	public class Shape
	{
		/** The state of this shape, if inactive can be ignored in results */
		public bool active = true;
        /** The name of this shape, to help in debugging */
	    public string name = "shape";
        /** A generic data object where you can store anything you want, for later use */
	    public object data;
        /** A list of tags to use for marking shapes with data for later use, by key/value */
	    public IDictionary<String, String> tags {get; private set;}

        /** The position of this shape */
	    public Vector position {
	    	get {
	    		return _position;
	    	}
	    	set {
				_position = value;
		        refresh_transform();
	    	}
	    }
        /** The x position of this shape */
	    public float x {
		    get {
		    	return _position.x;
		    }
		    set {
		    	_position.x = value;
		    	refresh_transform();
		    }
	    }

        /** The y position of this shape */
		public float y {
		    get {
		    	return _position.y;
		    }
		    set {
		    	_position.y = value;
		    	refresh_transform();
		    }
	    }

        /** The rotation of this shape, in degrees */
	    public float rotation {
	    	get {
	    		return _rotation;
	    	}
	    	set {
		        _rotation = value;
		        refresh_transform();
	    	}
	    }

        /** The scale in the x direction of this shape */
		public float scaleX {
			get {
				return _scaleX;
			}

			set {
				_scaleX = value;
		        refresh_transform();
			}
		}
        /** The scale in the y direction of this shape */
		public float scaleY {
			get {
				return _scaleY;
			}

			set {
				_scaleY = value;
		        refresh_transform();
			}
		}

		private Vector _position;
		private float _rotation = 0;

		private float _scaleX = 1;
		private float _scaleY = 1;

		protected bool _transformed = false;
		protected Matrix _transformMatrix;


		public Shape (float x, float y)
		{
			tags = new Dictionary<string, string>();

	        _position = new Vector(x, y);
	        _rotation = 0;

	        _scaleX = 1;
	        _scaleY = 1;

			_transformMatrix = new Matrix();
			_transformMatrix.makeTranslation(x, y);
		}

		/** Test this shape against another shape. */
		public virtual ShapeCollision test(Shape shape) {
			return null;
		}

        /** Test this shape against a circle. */
	    public virtual ShapeCollision testCircle( Circle circle, bool flip = false ) {
			return null;
	    }

        /** Test this shape against a polygon. */
	    public virtual ShapeCollision testPolygon( Polygon polygon, bool flip = false ) {
			return null;
	    }

        /** Test this shape against a ray. */
	    public virtual RayCollision testRay( Ray ray ) {
			return null;
	    }

	    private void refresh_transform() {

	    	_transformMatrix.compose(position, rotation, new Vector(scaleX, scaleY));
	        _transformed = false;

	    }

	        /** clean up and destroy this shape */
	    public void destroy() {

	        _position = null;
	        _transformMatrix = null;

	    }
	}
}