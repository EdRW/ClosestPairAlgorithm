using System;
using System.Collections.Generic;
using System.Text;

namespace PA2ClosestPair
{
    public class Point
    {

        public int x;
        public int y;

        public double polarAngleProxy;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public double Distance(Point point)
        {
            return Math.Sqrt(Math.Pow(this.x - point.x, 2) + Math.Pow(this.y - point.y, 2));
        }

        public override string ToString()
        {
            return String.Format("( Point x: {0}, y: {1})", x, y);
        }
    }

    public class CompareX : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            return point1.x.CompareTo(point2.x);
        }
    }

    public class CompareY : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            return point1.y.CompareTo(point2.y);
        }
    }

    public class Pair
    {
        public Point pointA;
        public Point pointB;
        public double distance;

        public Pair(Point pointA, Point pointB, double distance)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.distance = distance;
        }

        public Pair(Pair pair)
        {
            this.pointA = pair.pointA;
            this.pointB = pair.pointB;
            this.distance = pair.distance;
        }

        public void Set(Point pointA, Point pointB, double distance)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.distance = distance;
        }

        public override string ToString()
        {
            return String.Format("{0}: ({1}, {2})<--->({3}, {4})", distance, pointA.x, pointA.y, pointB.x, pointB.y);
        }
    }
}
