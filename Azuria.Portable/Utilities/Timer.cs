using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace System.Timers
{
    /// <summary>
    ///     Stellt den EventHandler der Elapsed-Ereignisses des <see cref="Timer" /> dar.
    /// </summary>
    /// <param name="sender">Der Auslöser des Ereignisse.s</param>
    /// <param name="e">Zusätliche Informationen über das ausgelöste Ereigniss.</param>
    public delegate void ElapsedEventHandler(object sender, EventArgs e);

    internal sealed class Timer
    {
        private readonly CancellationTokenSource _ct = new CancellationTokenSource();
        private Task _task;

        internal Timer()
        {
            this.CreateTask();
        }

        internal Timer(double period)
        {
            this.Interval = period;
            this.CreateTask();
        }

        #region Properties

        internal bool AutoReset { get; set; }

        public bool Enabled
        {
            get { return this._task.Status == TaskStatus.Running; }
            set
            {
                if (value) this.Start();
                else this.Stop();
            }
        }

        internal double Interval { get; set; }

        #endregion

        #region

        private void CreateTask()
        {
            this._task = new Task(async () =>
            {
                do
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(double.IsNaN(this.Interval) ? 1000.0 : this.Interval));
                    this.Elapsed?.Invoke(this, EventArgs.Empty);
                } while (this.AutoReset);
            }, this._ct.Token);
        }

        internal event ElapsedEventHandler Elapsed;

        internal void Start()
        {
            if (this._task.IsCompleted) this.CreateTask();
            if (this._task.Status != TaskStatus.Running) this._task.Start();
        }

        internal void Stop()
        {
            if (this._task.Status == TaskStatus.Running && !this._ct.IsCancellationRequested &&
                this._ct.Token.CanBeCanceled) this._ct.Cancel();
        }

        #endregion
    }
}