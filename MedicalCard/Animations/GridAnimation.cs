using MedicalCard.Models;
using MedicalCard.ViewModels;
using System.Threading;
using System.Windows;

namespace MedicalCard.Animations
{
    public class GridAnimation
    {
        ApplicationViewModel _propertyOwner;

        public GridAnimation(ApplicationViewModel vm)
        {
            _propertyOwner = vm;
        }

        public void Animate(GridLengthContainer propertyVar, string propertyName, double endLength, int time, int fps)
        {
            int sleepTime = 1000 / fps;

            double step = (endLength - propertyVar.Value) / (time / (double)sleepTime);
            int timeCounter = time;
            while (timeCounter > 0)
            {
                GridLength width;
                width = new GridLength(propertyVar.Value + step, propertyVar.GridUnitType);
                if (width.Value >= 0)
                {
                    propertyVar.Length = width;
                    _propertyOwner.OnPropetryChanged(propertyName);
                }

                timeCounter -= sleepTime;
                Thread.Sleep(sleepTime);
            }

            propertyVar.Length = new GridLength(endLength, propertyVar.GridUnitType);
            _propertyOwner.OnPropetryChanged(propertyName);
        }
    }
}
