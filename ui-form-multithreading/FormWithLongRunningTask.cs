namespace ui_form_multithreading
{
    public partial class FormWithLongRunningTask : Form
    {
        static Random _rando = new Random();
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
                    var e = new TaskCompleteEventArgs();
                    Text = $"@ {e.TimeStamp.ToLongTimeString()}";
                    TaskComplete?.Invoke(this, e);
                    BringToFront();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }
        public static event TaskCompleteEventHandler TaskComplete;
    }
    public delegate void TaskCompleteEventHandler(Object sender, TaskCompleteEventArgs e);
    public class TaskCompleteEventArgs : EventArgs
    {
        public DateTime TimeStamp { get; } = DateTime.Now;
    }
}
