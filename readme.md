One of many ways to look at this [question](https://stackoverflow.com/q/74798238/5438626) is that there's only one display area (albeit which might be comprised of many screens) and only one element of it can change at any moment in time. To my way of thinking, this means that having more than one UI thread can often be self defeating (unless your UI is testing another UI). And since the machine has some finite number of cores, having a very large number of threads (whether of the UI or worker variety) means you can start to have a lot of overhead marshalling the context as threads switch off.

Here's one way of achieving the kind of result you describe: this mock sample opens 10 forms that will run their continuous long-running tasks in parallel. The approach that I'm taking here is to have the forms fire an event whenever a task completes. Now, the main form can subscribe to that event and marshal the received data onto the one-and-only UI thread to display the result. I have also included a textbox with a data binding to try and address that aspect of your question.

    public partial class MainForm : Form, INotifyPropertyChanged
    {
        public MainForm()
        {
            InitializeComponent();
            BackColor = Color.CornflowerBlue;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Exercise a data binding
            textBox1.DataBindings.Add(
                nameof(TextBox.Text),
                this,
                nameof(ReceivedTimestamp)
            );
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
                    // Test the data binding
                    ReceivedTimestamp = e.TimeStamp.ToLongTimeString(); 
                });
            }
        }
        string _ReceivedTimestamp = string.Empty;
        public string ReceivedTimestamp
        {
            get => _ReceivedTimestamp;
            set
            {
                if (!Equals(_ReceivedTimestamp, value))
                {
                    _ReceivedTimestamp = value;
                    PropertyChanged?
                        .Invoke(this, new PropertyChangedEventArgs(nameof(ReceivedTimestamp)));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        Color[] _colors = new Color[]
        {
            Color.Black, Color.Blue, Color.Green, Color.LightSalmon, Color.SeaGreen,
            Color.BlueViolet, Color.DarkCyan, Color.Maroon, Color.Chocolate, Color.DarkKhaki
        };
    }
***

The 'other'' forms are mocked like this:

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
***

[![enter image description here][1]][1]

I believe that there's no 'right' answer to a question but I hope there's something here that might move things forward for you.


  [1]: https://i.stack.imgur.com/zpLGs.png