using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using TemsParser.Models;
using TemsParser.Models.Parsing;
using TemsParser.Models.Parsing.Area;

namespace TemsParser.Processing
{
    public class Calculator
    {
        /// <summary>
        /// Radius of the Earth in meters.
        /// </summary>
        private const double EarthRadius = 6378137;
        private readonly double LongitudeCoeficient;

        public Calculator(double latitude)
        {
            LongitudeCoeficient = Math.Cos(latitude * Math.PI / 180);
        }


        /// <summary>
        /// Returns the distance in meters between two points with the given coordinates.
        /// </summary>
        /// <param name="latitudePoint1">Latitude of point 1 in degrees. </param>
        /// <param name="longitudePoint1">Longitude of point 1 in degrees.</param>
        /// <param name="latitudePoint2">Latitude of point 2 in degrees.</param>
        /// <param name="longitudePoint2">Longitude of point 2 in degrees.</param>
        /// <returns>Distance between two points in meters.</returns>
        public static double GetDistance(double latitudePoint1,
                                         double longitudePoint1,
                                         double latitudePoint2,
                                         double longitudePoint2)
        {
            // Latitude of point 1 in radians.
            double y1 = latitudePoint1 * Math.PI / 180;

            // Longitude of point 1 in radians.
            double x1 = longitudePoint1 * Math.PI / 180;

            // Latitude of point 2 in radians.
            double y2 = latitudePoint2 * Math.PI / 180;

            // Longitude of point 2 in radians.
            double x2 = longitudePoint2 * Math.PI / 180;

            // Distance between two points in radians.
            double d = Math.Acos((Math.Sin(y1) * Math.Sin(y2)) + (Math.Cos(y1) * Math.Cos(y2) * Math.Cos(x1 - x2)));

            // Distance between two points in meters.
            double l = d * EarthRadius;

            return l;
        }


        public double GetAngle(double length, bool isLongitude)
        {
            if (isLongitude)
            {
                return (length * 180) / (Math.PI * EarthRadius * LongitudeCoeficient);
            }
            else
            {
                return (length * 180) / (Math.PI * EarthRadius);
            }
        }

        public double GetLength(double angle, bool isLongitude)
        {
            if (isLongitude)
            {
                return (Math.PI * EarthRadius * LongitudeCoeficient * angle) / 180;
            }
            else
            {
                return (Math.PI * EarthRadius * angle) / 180;
            }
        }

        public static FreqLevelPairModel GetBestFreqLevel(IEnumerable<FreqLevelPairModel> freqLevelList)
        {
            var values = freqLevelList.ToArray();
            var bestFreqLevel = values.First();

            for (int i = 1; i < values.Length; i++)
            {
                var value = values[i];

                if (value.Level > bestFreqLevel.Level)
                {
                    bestFreqLevel = value;
                }
            }

            return bestFreqLevel;
        }

        public CoordinateModel GetFirstBasePoint(CoordinateModel firstPoint, int dimentions, int binningSize)
        {
            var initialOffsetDistance = ((double)dimentions / 2) * binningSize + (double)binningSize / 2;
            var initialOffsetAngleLatitude = GetAngle(initialOffsetDistance, isLongitude: false);
            var initialOffsetAngleLongitude = GetAngle(initialOffsetDistance, isLongitude: true);

            double baseLatitude = firstPoint.Latitude - initialOffsetAngleLatitude;
            double baseLongitude = firstPoint.Longitude - initialOffsetAngleLongitude;
            return new CoordinateModel(baseLatitude, baseLongitude);
        }

        public static CoordinateModel GetAreaBasePoint(CoordinateModel basePoint,
                                                       CoordinateModel currentPoint,
                                                       double areaLatitudeSizeAngle,
                                                       double areaLongitudeSizeAngle)
        {
            var latitudeStepOffset = Math.Floor((currentPoint.Latitude - basePoint.Latitude) / areaLatitudeSizeAngle);
            var areaBaseLatitude = basePoint.Latitude + areaLatitudeSizeAngle * latitudeStepOffset;

            var longitudeStepOffset = Math.Floor((currentPoint.Longitude - basePoint.Longitude) / areaLongitudeSizeAngle);
            var areaBaseLongitude = basePoint.Longitude + areaLongitudeSizeAngle * longitudeStepOffset;

            return new CoordinateModel(areaBaseLatitude, areaBaseLongitude);
        }
    }
}
