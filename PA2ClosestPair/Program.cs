using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PA2ClosestPair
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            StreamReader file = new StreamReader("10points.txt");

            List<Point> points = new List<Point>();

            string line = null;
            while ((line = file.ReadLine()) != null)
            {
                String[] words = line.Split(" ");
                Point newPoint = new Point(int.Parse(words[0]), int.Parse(words[1]));
                points.Add(newPoint);
                //Debug.WriteLine(line);
            }

            List<Point> xSorted = points;
            List<Point> ySorted = new List<Point>(points);
            ySorted.Sort(new CompareY());
            xSorted.Sort(new CompareX());

            Debug.WriteLine("Sorted by Y:");
            foreach (Point point in ySorted)
            {
                Debug.WriteLine(point);
            }

            Debug.WriteLine("Sorted by X:");
            foreach (Point point in xSorted)
            {
                Debug.WriteLine(point);
            }

            ClosestPair(xSorted, ySorted);

            // Keep the console window open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();
        }

        public static Pair ClosestPair(List<Point> xSorted, List<Point> ySorted)
        {
            Pair closestPair = null;

            // Base case. This step takes O(1) time.
            if (xSorted.Count <= 3)
            {
                double d1 = xSorted[0].Distance(xSorted[1]);
                double d2 = xSorted[0].Distance(xSorted[2]);
                double d3 = xSorted[1].Distance(xSorted[2]);

                if(d1 >= d2 && d1 >= d3)
                {
                    closestPair = new Pair(xSorted[0], xSorted[1], d1);
                }
                else if (d2 >= d1 && d2 >= d3)
                {
                    closestPair = new Pair(xSorted[0], xSorted[2], d2);
                }
                else if (d3 >= d1 && d3 >= d2)
                {
                    closestPair = new Pair(xSorted[1], xSorted[2], d3);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            else
            {
                int verticalLineIndex = xSorted.Count / 2;
                int verticalLineValue = xSorted[verticalLineIndex].x;

                List<Point> xSortedL = new List<Point>(xSorted.GetRange(0, verticalLineIndex));
                List<Point> xSortedR = new List<Point>(xSorted.GetRange(verticalLineIndex, xSorted.Count - verticalLineIndex));

                List<Point> ySortedL = new List<Point>();
                List<Point> ySortedR = new List<Point>();

                for (int i = 0; i < ySorted.Count; i++)
                {
                    if (ySorted[i].x < verticalLineValue)
                    {
                        ySortedL.Add(ySorted[i]);
                    }
                    else
                    {
                        ySortedR.Add(ySorted[i]);
                    }
                }

                Debug.Assert(xSortedL.Count == ySortedL.Count);
                Debug.Assert(xSortedR.Count == ySortedR.Count);
                //Pair leftClosestPair = ClosestPair(xSortedL, ySortedL);
                //Pair rightClosestPair = ClosestPair(xSortedR, ySortedR);
            }

            return closestPair;
        }
    }
}
