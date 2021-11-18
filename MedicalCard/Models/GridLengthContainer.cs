using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedicalCard.Models
{
    public class GridLengthContainer
    {
        private GridLength _gridLength;

        public GridLength Length
        {
            get => _gridLength;
            set
            {
                _gridLength = value;
            }
        }

        public double Value
        {
            get => _gridLength.Value;
            set
            {
                _gridLength = new GridLength(value, _gridLength.GridUnitType);
            }
        }

        public GridUnitType GridUnitType
        {
            get => _gridLength.GridUnitType;
            set
            {
                _gridLength = new GridLength(_gridLength.Value, value);
            }
        }

        public GridLengthContainer(double value, GridUnitType type)
        {
            _gridLength = new GridLength(value, type);
        }
    }
}
