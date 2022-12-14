using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ui_form_multithreading
{
    public partial class FormWithLongRunningTask : Form, INotifyPropertyChanged
    {
        static Random _rando = new Random(8);
        public FormWithLongRunningTask() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _ = runRandomDelayLoop();
        }
        private async Task runRandomDelayLoop()
        {
            while(true)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_rando.NextDouble() * 10));
                    TimeStamp = DateTime.Now;
                    Text = $"@ {TimeStamp.ToLongTimeString()}";
                    BringToFront();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }
        DateTime _timeStamp = DateTime.Now;
        public DateTime TimeStamp
        {
            get => _timeStamp;
            set
            {
                if (!Equals(_timeStamp, value))
                {
                    _timeStamp = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }
        public static event PropertyChangedEventHandler? PropertyChanged;
    }
}
