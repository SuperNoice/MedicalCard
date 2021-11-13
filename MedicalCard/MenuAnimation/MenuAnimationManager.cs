using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1.MenuAnimation
{
    public enum MenuState
    {
        Open, Close, Opening, Closing
    }

    public class MenuAnimationManager
    {
        private List<GridColumnAnimation> _animations;
        private ColumnDefinition _column;
        private double _openWidth;

        public int Duration { set; get; }
        public int Fps { set; get; }
        public MenuState State { get; private set; }

        public MenuAnimationManager(ColumnDefinition column, double openWidth, MenuState state, int duration, int fps)
        {
            _column = column;
            _openWidth = openWidth;
            _animations = new List<GridColumnAnimation>();
            State = state;
            Duration = duration;
            Fps = fps;

            switch (State)
            {
                case MenuState.Open:
                    Dispatcher.CurrentDispatcher.Invoke(() => { _column.Width = new GridLength(openWidth, GridUnitType.Star); });
                    break;
                case MenuState.Close:
                    Dispatcher.CurrentDispatcher.Invoke(() => { _column.Width = new GridLength(0.0, GridUnitType.Star); });
                    break;
                default:
                    throw new ArgumentException("Изначальное состояние меню может быть только Open или Close!");
            }
        }

        public async void Open()
        {
            if (State == MenuState.Open || State == MenuState.Opening)
            {
                return;
            }
            if (State == MenuState.Closing)
            {
                foreach (var anim in _animations)
                {
                    anim.Cancel();
                }
            }

            var animation = new GridColumnAnimation(_column);
            _animations.Add(animation);
            State = MenuState.Opening;

            await animation.AnimateWidth(_openWidth, Convert.ToInt32(Duration - Duration * (_column.Width.Value / _openWidth)), Fps);

            if (!animation.IsCancelled)
            {
                State = MenuState.Open;
            }
            _animations.Remove(animation);
        }

        public async void Close()
        {
            if (State == MenuState.Close || State == MenuState.Closing)
            {
                return;
            }
            if (State == MenuState.Opening)
            {
                foreach (var anim in _animations)
                {
                    anim.Cancel();
                }
            }

            var animation = new GridColumnAnimation(_column);
            _animations.Add(animation);
            State = MenuState.Closing;

            await animation.AnimateWidth(0.0, Convert.ToInt32(Duration * (_column.Width.Value / _openWidth)), Fps);

            if (!animation.IsCancelled)
            {
                State = MenuState.Close;
            }
            _animations.Remove(animation);
        }

    }
}
