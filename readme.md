One of many ways to look at this is that there's only one display area (albeit which might be comprised of many screens) and only one element of it can change at any moment in time. To my way of thinking, this means that having more than one UI thread is self-defeating. And since the machine has some finite number of cores and with too many threads (UI or worker) there can start to be a lot of overhead marshalling the context as threads switch off.

The way I mocked this out is to open 10 forms that are all performing some long-running task. The approach that I'm taking here is to have the forms fire an event whenever a task completes. Now, the main form can subscribe to that event and marshall the received data onto the one-and-only UI thread to display the result.

        public MainForm()
        {
            InitializeComponent();
            BackColor = Color.CornflowerBlue;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormWithLongRunningTask.TaskComplete += onAnyTaskComplete;
            for (int i = 0; i < 10; i++)
            {
                new FormWithLongRunningTask { Name = $"Form{i}" }.Show(this);
            }
        }
        private void onAnyTaskComplete(object sender, TaskCompleteEventArgs e)
        {
            if (sender is Control control)
            {
                BeginInvoke(() =>
                {
                    richTextBox.SelectionColor = _colors[int.Parse(control.Name.Replace("Form", string.Empty))];
                    richTextBox
                    .AppendText($"Sender: {control.Name} @ {e.TimeStamp}{Environment.NewLine}");    
                });
            }
        }
        Color[] _colors = new Color[]
        {
            Color.Black, Color.Blue, Color.Green, Color.LightSalmon, Color.SeaGreen,
            Color.BlueViolet, Color.DarkCyan, Color.Maroon, Color.Chocolate, Color.DarkKhaki
        };
    }

Where the 'other'' forms are mocked like this:

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
                    Text = $"Timestamp @ {e.TimeStamp.ToString("o")}";
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

