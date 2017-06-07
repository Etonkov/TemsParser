using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

using TemsParser.CustomAttributes;
using TemsParser.Behaviors;
using TemsParser.Processing;
using TemsParser.Common;
using TemsParser.Resources;

namespace TemsParser.ViewModels
{
    public class CalcDistanceViewModel : ViewModelBase
    {
        private double? distance;
        private string _latitudePoint1, _longitudePoint1, _latitudePoint2, _longitudePoint2;


        public CalcDistanceViewModel()
        {
            CalculateCommand = new Command(
                ex =>
                {
                    this.Distance =
                        Calculator.GetDistance(StringParser.ParseStringIntoDouble(LatitudePoint1),
                                                 StringParser.ParseStringIntoDouble(LongitudePoint1),
                                                 StringParser.ParseStringIntoDouble(LatitudePoint2),
                                                 StringParser.ParseStringIntoDouble(LongitudePoint2));
                },
                ce =>
                {
                    return ValidateObject(this);
                });

            CancelCommand = new Command(ex => Close());
        }


        public ICommand CalculateCommand { get; set; }

        [Coordinate(CoordinateType.Latitude)]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string LatitudePoint1
        {
            get
            {
                return _latitudePoint1;
            }
            private set
            {
                if (_latitudePoint1 != value)
                {
                    _latitudePoint1 = value;
                    Distance = null;
                }
            }
        }

        [Coordinate(CoordinateType.Longitude)]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string LongitudePoint1
        {
            get
            {
                return _longitudePoint1;
            }
            private set
            {
                if (_longitudePoint1 != value)
                {
                    _longitudePoint1 = value;
                    Distance = null;
                }
            }
        }

        [Coordinate(CoordinateType.Latitude)]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string LatitudePoint2
        {
            get
            {
                return _latitudePoint2;
            }
            private set
            {
                if (_latitudePoint2 != value)
                {
                    _latitudePoint2 = value;
                    Distance = null;
                }
            }
        }

        [Coordinate(CoordinateType.Longitude)]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "Required")]
        public string LongitudePoint2
        {
            get
            {
                return _longitudePoint2;
            }
            private set
            {
                if (_longitudePoint2 != value)
                {
                    _longitudePoint2 = value;
                    Distance = null;
                }
            }
        }

        public double? Distance
        {
            get { return distance; }
            private set { distance = value; OnPropertyChanged(); }
        }
    }
}
