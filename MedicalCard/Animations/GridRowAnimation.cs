using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalCard.Animations
{
    public class GridRowAnimation : DispatcherObject
    {
        private RowDefinition _row;
        private CancellationTokenSource _cancellationTokenSource;

        public event EventHandler Cancelled;
        public event EventHandler Compleated;

        public bool IsCancelled { get; private set; }

        public GridRowAnimation(RowDefinition row)
        {
            _row = row;
            _cancellationTokenSource = new CancellationTokenSource();
            IsCancelled = false;
        }

        public async Task AnimateHeight(double endHeight, int time, int fps)
        {
            if (endHeight < 0)
            {
                throw new ArgumentException("Ширина не должна быть отрицательной!");
            }
            Task t = Task.Run(() =>
            {
                try
                {
                    Animation(endHeight, time, fps);
                }
                catch (Exception)
                {
                    return;
                }

            });
            await t;
            return;
        }

        private void Animation(double endHeight, int time, int fps)
        {
            int sleepTime = 1000 / fps;
            double startWidth = 0;

            Dispatcher.Invoke(() =>
            {
                startWidth = _row.Height.Value;
            });

            double step = (endHeight - startWidth) / (time / (double)sleepTime);
            int timeCounter = time;
            while (timeCounter > 0)
            {
                if (_cancellationTokenSource?.IsCancellationRequested ?? false)
                {
                    IsCancelled = true;
                    Cancelled?.Invoke(this, EventArgs.Empty);
                    return;
                }


                GridLength width;
                Dispatcher.Invoke(() =>
                {
                    width = new GridLength(_row.Height.Value + step, GridUnitType.Star);
                    if (width.Value >= 0)
                    {
                        _row.Height = width;
                    }
                });

                timeCounter -= sleepTime;
                Thread.Sleep(sleepTime);
            }
            Dispatcher.Invoke(() =>
            {
                _row.Height = new GridLength(endHeight, GridUnitType.Star);
            });
            Compleated?.Invoke(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

    }
}
