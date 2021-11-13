using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1
{
    public class GridColumnAnimation : DispatcherObject
    {
        private ColumnDefinition _column;
        private CancellationTokenSource _cancellationTokenSource;

        public event EventHandler Cancelled;
        public event EventHandler Compleated;

        public bool IsCancelled { get; private set; }

        public GridColumnAnimation(ColumnDefinition column)
        {
            _column = column;
            _cancellationTokenSource = new CancellationTokenSource();
            IsCancelled = false;
        }

        public async Task AnimateWidth(double endWidth, int time, int fps)
        {
            if (endWidth < 0)
            {
                throw new ArgumentException("Ширина не должна быть отрицательной!");
            }
            Task t = Task.Run(() =>
            {
                try
                {
                    Animation(endWidth, time, fps);
                }
                catch (Exception)
                {
                    return;
                }

            });
            await t;
            return;
        }

        private void Animation(double endWidth, int time, int fps)
        {
            int sleepTime = 1000 / fps;
            double startWidth = 0;

            Dispatcher.Invoke(() =>
            {
                startWidth = _column.Width.Value;
            });

            double step = (endWidth - startWidth) / (time / (double)sleepTime);
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
                    width = new GridLength(_column.Width.Value + step, GridUnitType.Star);
                    if (width.Value >= 0)
                    {
                        _column.Width = width;
                    }
                });

                timeCounter -= sleepTime;
                Thread.Sleep(sleepTime);
            }
            Dispatcher.Invoke(() =>
            {
                _column.Width = new GridLength(endWidth, GridUnitType.Star);
            });
            Compleated?.Invoke(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

    }
}
