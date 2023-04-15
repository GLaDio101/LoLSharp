using System.Collections.Generic;
using System.Numerics;
using LoLSharp.Extensions;

namespace LoLSharp.Modules
{
  public static class Sat
  {

    public class Circle
    {
      public Vector2 Pos;
      public float R;
      public Circle(Vector2 pos, float r)
      {
        R = r;
        Pos = pos;
      }
    }

    public class Box
    {
      public Vector2 Pos;
      public int Width;
      public int Height;

      public Box(Vector2 pos, int width, int height)
      {
        Pos = pos;
        Width = width;
        Height = height;
      }

      public Polygon ToPolygon()
      {
        return new Polygon(Pos, new List<Vector2>() { new Vector2(0, 0), new Vector2(Width, 0), new Vector2(Width, Height), new Vector2(0, Height) });
      }
    }

    public class Polygon
    {
      public Vector2 Pos;
      public List<Vector2> Points;
      public float Angle;
      public Vector2 Offset;
      public List<Vector2> CalcPoints;
      public List<Vector2> Edges;
      public List<Vector2> Normals;

      public Polygon(Vector2 pos, List<Vector2> points)
      {
        Points = points;
        Pos = pos;
        CalcPoints = new List<Vector2>();
        Edges = new List<Vector2>();
        Normals = new List<Vector2>();
      }

      public void ReCalc()
      {
        for (var i = 0; i < Points.Count; i++)
        {
          var calcPoint = CalcPoints[i] = Points[i];
          calcPoint.X += Offset.X;
          calcPoint.Y += Offset.Y;

          if (Angle != 0)
          {
            calcPoint.Rotate(Angle);
          }
        }

        for (var i = 0; i < Points.Count; i++)
        {
          var p1 = CalcPoints[i];
          var p2 = i < Points.Count - 1 ? CalcPoints[i + 1] : CalcPoints[0];
          p2.Sub(p1);
          Edges[i] = p2;
          p2.Perp();
          p2.Normalize();
          Normals[i] = p2;
        }
      }

      public void SetAngle(float angle)
      {
        Angle = angle;
        ReCalc();
      }
      public void SetOffset(Vector2 offset)
      {
        Offset = offset;
        ReCalc();
      }

      public void SetPoints(List<Vector2> points)
      {
        CalcPoints = new List<Vector2>();
        Edges = new List<Vector2>();
        Normals = new List<Vector2>();

        for (var i = 0; i < points.Count; i++)
        {
          var p1 = points[i];
          var p2 = i < points.Count - 1 ? points[i + 1] : points[0];

          if (p1 != p2 && (int)p1.X == (int)p2.X && (int)p1.Y == (int)p2.Y)
          {
            points.RemoveAt(i);
            i -= 1;
            continue;
          }

          CalcPoints.Add(new Vector2());
          Edges.Add(new Vector2());
          Normals.Add(new Vector2());
        }

        Points = points;
        ReCalc();
      }
    }
  }
}
