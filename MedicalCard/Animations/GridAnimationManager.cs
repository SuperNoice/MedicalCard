using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MedicalCard.Models;
using MedicalCard.ViewModels;

namespace MedicalCard.Animations
{
    enum MenuState
    {
        Open, Close
    }

    public class GridAnimationManager
    {
        private GridAnimation _animation;
        private ApplicationViewModel _propertyOwner;
        private GridLengthContainer _propertyVar;
        string _propertyName;
        double _minLength;
        double _maxLength;
        int _time;
        int _fps;
        MenuState _menuState;

        public GridAnimationManager(ApplicationViewModel vm, GridLengthContainer propertyVar, string propertyName, double minLength, double maxLength, int time, int fps)
        {
            _propertyOwner = vm;
            _propertyVar = propertyVar;
            _animation = new GridAnimation(vm);
            _minLength = minLength;
            _maxLength = maxLength;
            _time = time;
            _fps = fps;
            _menuState = (propertyVar.Value == 0) ? MenuState.Close : MenuState.Open;
        }

        public void Open()
        {
            if (_menuState == MenuState.Open)
            {
                return;
            }

            _animation.Animate(_propertyVar, _propertyName, _maxLength, _time, _fps);

            _menuState = MenuState.Open;
        }

        public void Close()
        {
            if (_menuState == MenuState.Close)
            {
                return;
            }

            _animation.Animate(_propertyVar, _propertyName, _minLength, _time, _fps);

            _menuState = MenuState.Close;
        }

    }


}

