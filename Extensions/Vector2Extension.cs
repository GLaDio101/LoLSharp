using System;
using System.Numerics;

namespace LoLSharp.Extensions
{
  public static class Vector2Extension
  {
    public static void Rotate(this Vector2 vector2, float angle)
    {
      var x = vector2.X;
      var y = vector2.Y;
      vector2.X = (float)(x * Math.Cos(angle) - y * Math.Sin(angle));
      vector2.Y = (float)(x * Math.Sin(angle) + y * Math.Cos(angle));
    }

    public static void Sub(this Vector2 vector2, Vector2 other)
    {
      vector2.X -= other.X;
      vector2.Y -= other.Y;
    }

    public static void Perp(this Vector2 vector2)
    {
      (vector2.X, vector2.Y) = (vector2.Y, vector2.X);
    }

    public static void Normalize(this Vector2 vector2)
    {
      var d = vector2.Len();
      if (!(d > 0))
        return;
      vector2.X /= d;
      vector2.Y /= d;
    }

    public static float Len(this Vector2 vector2)
    {
      return (float)Math.Sqrt(vector2.Len2());
    }

    public static float Len2(this Vector2 vector2)
    {
      return vector2.Dot(vector2);
    }

    public static float Dot(this Vector2 vector2, Vector2 other)
    {
      return vector2.X * other.X + vector2.X * other.Y;
    }
  }
}
