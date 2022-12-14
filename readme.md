One of many ways to look at this [question](https://stackoverflow.com/q/74798238/5438626) is that there's only one display area (albeit which might consist of many screens) and only one element of it can change at any given moment. To my way of thinking, this means that having more than one UI thread can often be self defeating (unless your UI is testing another UI). And since the machine has some finite number of cores, having a very large number of threads (whether of the UI or worker variety) means you can start to have a lot of overhead marshalling the context as threads switch off.

Here's one way of achieving the kind of result you describe: This mock sample opens 10 forms that will run their continuous long-running tasks in parallel. To mock data binding where FormWithLongRunningTask is the binding source, the MainForm subscribes to the `PropertyChanged` event. When a `PropertyChanged` event is received, the main form can identify the sender and inspect `e` to determine which property has changed. In this case, if the property is `TimeStamp`, the received data is marshalled onto the one-and-only UI thread to display the result.

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            BackColor = Color.CornflowerBlue;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormWithLongRunningTask.PropertyChanged += onAnyPropertyChanged;
            for (int i = 0; i < 10; i++)
            {
                new FormWithLongRunningTask { Name = $"Form{i}" }.Show(this);
            }
        }
        private void onAnyPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is FormWithLongRunningTask form)
            {
                BeginInvoke(() =>
                {
                    switch (e.PropertyName)
                    {
                        case nameof(FormWithLongRunningTask.TimeStamp):
                            break;
                        default:
                            break;
                    }
                    richTextBox.SelectionColor = _colors[int.Parse(form.Name.Replace("Form", string.Empty))];
                    richTextBox
                    .AppendText(
                        $"Sender: {form.Name} @ {form.TimeStamp}{Environment.NewLine}");
                    richTextBox.Select(richTextBox.TextLength, 0);
                    richTextBox.ScrollToCaret();
                    textBoxWithDataBinding.Text = form.TimeStamp.ToLongTimeString();
                });
            }
        }
        Color[] _colors = new Color[]
        {
            Color.Black, Color.Blue, Color.Green, Color.LightSalmon, Color.SeaGreen,
            Color.BlueViolet, Color.DarkCyan, Color.Maroon, Color.Chocolate, Color.DarkKhaki
        };
    }
***

The 'other'' 10 forms use this class which mocks a binding source like this:

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
***

[![enter image description here][1]][1]

I believe that there's no 'right' answer to your question but I hope there's something here that might move things forward for you.


  [1]: https://i.stack.imgur.com/Cahr7.png