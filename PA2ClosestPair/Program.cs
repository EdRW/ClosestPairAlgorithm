using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PA2ClosestPair
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] fileNames = { "10points.txt", "100points.txt", "1000points.txt" };

            foreach (String fileName in fileNames)
            {
                // Read in file and parse out the points data
                StreamReader file = new StreamReader(fileName);
                List<Point> points = new List<Point>();

                string line = null;
                while ((line = file.ReadLine()) != null)
                {
                    String[] words = line.Split(" ");
                    Point newPoint = new Point(int.Parse(words[0]), int.Parse(words[1]));
                    points.Add(newPoint);
                }

                file.Close();

                Debug.WriteLine("{0} points test file:\n", points.Count);
                Console.WriteLine("{0} points test file:\n", points.Count);

                /* Presort the points into 2 arrays. In one the points are sorted by their
                 * X-value, in the other the points are sorted by their Y-value. I use C#'s
                 * default sort which is implmented as a hybrid algo that makes use of heapsort 
                 * for most values of n and then intersertion sort when the n becomes very 
                 * small in order to minimize operations.
                 */ 
                List<Point> xSorted = points;
                List<Point> ySorted = new List<Point>(points);
                ySorted.Sort(new CompareY()); // CompareY is a "comparer" that sorts points according to Y-value
                xSorted.Sort(new CompareX()); // CompareX is a "comparer" that sorts points according to X-value

                /* Call the recursive closest pair of points algo which takes the 2 sorted arrays and 
                 * returns a Pair object containing the closest pair of points and their distance.
                 */
                Pair closestPair = ClosestPair(xSorted, ySorted);

                Debug.WriteLine("The minimum distance is:\n{0}\n\n", closestPair);
                Console.WriteLine("The minimum distance is:\n{0}\n\n", closestPair);
            }

            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static Pair ClosestPair(List<Point> xSorted, List<Point> ySorted)
        {
            Pair closestPair = null;

            // Base case is when n <= 3. This step takes O(1) time to brute force finding the closest pair
            if (xSorted.Count == 2)
            {
                double d1 = xSorted[0].Distance(xSorted[1]);

                closestPair = new Pair(xSorted[0], xSorted[1], d1);
            }
            else if (xSorted.Count == 3)
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
            // General case for when n > 3. This step takes O(nlgn) + O(n) time.
            else
            {
                // define the vertical line, by index and value, that will be used to split the points
                int verticalLineIndex = xSorted.Count / 2;
                int verticalLineValue = xSorted[verticalLineIndex].x;
                
                // use the vertical line index to split the array(sorted by X-values) into points to the left and right of the vertical line
                List<Point> xSortedL = new List<Point>(xSorted.GetRange(0, verticalLineIndex));
                List<Point> xSortedR = new List<Point>(xSorted.GetRange(verticalLineIndex, xSorted.Count - verticalLineIndex));

                // use the vertical line value to split the array(sorted by Y-values) into points to the left and right of the vertical line
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

                /*
                 * Make 2 recusive calls to ClosestPair, one for the left side and one for the right side points
                 */
                Pair leftClosestPair = ClosestPair(xSortedL, ySortedL);
                Pair rightClosestPair = ClosestPair(xSortedR, ySortedR);

                // determine if the pair on the left or the pair on the right has the smaller distance
                Pair leftRightClosestPair = (rightClosestPair.distance < leftClosestPair.distance) ? rightClosestPair : leftClosestPair;
                
                // Scan the array(sorted by Y-value) and find all of the points within the strip around the vertical line
                List<Point> pointsInVertStrip = new List<Point>();
                foreach (Point point in ySorted)
                {
                    if ((point.x >= verticalLineValue - leftRightClosestPair.distance) && (point.x <= verticalLineValue + leftRightClosestPair.distance))
                    {
                        pointsInVertStrip.Add(point);
                    }
                }

                /* This method takes the array of points within the strip and the current closest pair and determines if there are
                 * any points in the strip that are closer than the current closest pair. This step takes O(n) time.
                 */
                closestPair = FindClosestInStrip(pointsInVertStrip, leftRightClosestPair);
                
            }

            return closestPair;
        }

        private static Pair FindClosestInStrip(List<Point> pointsInVertStrip, Pair closestPair)
        {
            Pair closestPairInStrip = new Pair(closestPair);

            for ( int i = 0; i < pointsInVertStrip.Count; i++)
            {
                Point point = pointsInVertStrip[i];

                // Check the distance to the next 7 points this part takes 7n time in the worst case.
                for (int j = i + 1, k = 0; (j < pointsInVertStrip.Count) && (k < 7); j++, k++)
                {
                    double distance = point.Distance(pointsInVertStrip[j]);
                    if (distance < closestPairInStrip.distance)
                    {
                        closestPairInStrip.Set(point, pointsInVertStrip[j], distance);
                    }
                }
            }

            return closestPairInStrip;
        }
    }
}
