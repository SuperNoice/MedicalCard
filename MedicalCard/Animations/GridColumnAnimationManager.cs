using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalCard.Animations
{
    public class GridColumnAnimationManager
    {
        private List<GridColumnAnimation> _animations;
        private ColumnDefinition _column;
        private double _openWidth;

        public int Duration { set; get; }
        public int Fps { set; get; }
        public AnimationState State { get; private set; }

        public GridColumnAnimationManager(ColumnDefinition column, double openWidth, AnimationState state, int duration, int fps)
        {
            _column = column;
            _openWidth = openWidth;
            _animations = new List<GridColumnAnimation>();
            State = state;
            Duration = duration;
            Fps = fps;

            switch (State)
            {
                case AnimationState.Open:
                    Dispatcher.CurrentDispatcher.Invoke(() => { _column.Width = new GridLength(openWidth, GridUnitType.Star); });
                    break;
                case AnimationState.Close:
                    Dispatcher.CurrentDispatcher.Invoke(() => { _column.Width = new GridLength(0.0, GridUnitType.Star); });
                    break;
                default:
                    throw new ArgumentException("Изначальное состояние меню может быть только Open или Close!");
            }
        }

        public async void Open()
        {
            if (State == AnimationState.Open || State == AnimationState.Opening)
            {
                return;
            }
            if (State == AnimationState.Closing)
            {
                foreach (var anim in _animations)
                {
                    anim.Cancel();
                }
            }

            var animation = new GridColumnAnimation(_column);
            _animations.Add(animation);
            State = AnimationState.Opening;

            await animation.AnimateWidth(_openWidth, Convert.ToInt32(Duration - Duration * (_column.Width.Value / _openWidth)), Fps);

            if (!animation.IsCancelled)
            {
                State = AnimationState.Open;
            }
            _animations.Remove(animation);
        }

        public async void Close()
        {
            if (State == AnimationState.Close || State == AnimationState.Closing)
            {
                return;
            }
            if (State == AnimationState.Opening)
            {
                foreach (var anim in _animations)
                {
                    anim.Cancel();
                }
            }

            var animation = new GridColumnAnimation(_column);
            _animations.Add(animation);
            State = AnimationState.Closing;

            await animation.AnimateWidth(0.0, Convert.ToInt32(Duration * (_column.Width.Value / _openWidth)), Fps);

            if (!animation.IsCancelled)
            {
                State = AnimationState.Close;
            }
            _animations.Remove(animation);
        }

    }
}
