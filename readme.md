One of many ways to look at this [question](https://stackoverflow.com/q/74798238/5438626) is that there's only one display area (albeit which might consist of many screens) and only one element of it can change at any given moment. To my way of thinking, this means that having more than one UI thread can often be self defeating (unless your UI is testing another UI). And since the machine has some finite number of cores, having a very large number of threads (whether of the UI or worker variety) means you can start to have a lot of overhead marshalling the context as threads switch off.

If we wanted to make a [Minimal Reproducible Example](https://stackoverflow.com/help/minimal-reproducible-example) that has 10 `Form` objects executing continuous "mock update" tasks in parallel, what we could do instead of the "data property change hub" you mentioned is to implement INotifyPropertyChanged in those form classes with static `PropertyChanged` event that gets fired when the update occurs. To mock data binding where FormWithLongRunningTask is the binding source, the MainForm subscribes to the `PropertyChanged` event and adds a new `Record` to the `BindingList<Record>` by identifying the sender and inspecting `e` to determine which property has changed. In this case, if the property is `TimeStamp`, the received data is marshalled onto the one-and-only UI thread to display the result in the DataGridView.

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
            // Subscribe to the static event here.
            FormWithLongRunningTask.PropertyChanged += onAnyFWLRTPropertyChanged;
            // Start up the 10 forms which will do "popcorn" updates.
            for (int i = 0; i < 10; i++)
            {
                new FormWithLongRunningTask { Name = $"Form{i}" }.Show(this);
            }
        }
        private void onAnyFWLRTPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is FormWithLongRunningTask form)
            {
                BeginInvoke(() =>
                {
                    switch (e.PropertyName)
                    {
                        case nameof(FormWithLongRunningTask.TimeStamp):
                            dataGridViewEx.DataSource.Add(new Record
                            {
                                Sender = form.Name,
                                TimeStamp = form.TimeStamp,
                            });
                            break;
                        default:
                            break;
                    }
                });
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
    
[![main form with DataGridView][1]][1]
***

The `DataGridView` on the main form uses this custom class:

    class DataGridViewEx : DataGridView
    {
        public new BindingList<Record> DataSource { get; } = new BindingList<Record>();
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode)
            {
                base.DataSource = this.DataSource;
                AllowUserToAddRows = false;

                #region F O R M A T    C O L U M N S
                DataSource.Add(new Record());
                Columns[nameof(Record.Sender)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                var col = Columns[nameof(Record.TimeStamp)];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col.DefaultCellStyle.Format = "hh:mm:ss tt";
                DataSource.Clear();
                #endregion F O R M A T    C O L U M N S
            }
        }
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if ((e.RowIndex > -1) && (e.RowIndex < DataSource.Count))
            {
                var record = DataSource[e.RowIndex];
                var color = _colors[int.Parse(record.Sender.Replace("Form", string.Empty))];
                e.CellStyle.ForeColor = color;
                if (e.ColumnIndex > 0)
                {
                    CurrentCell = this[0, e.RowIndex];
                }
            }
        }
        Color[] _colors = new Color[]
        {
            Color.Black, Color.Blue, Color.Green, Color.LightSalmon, Color.SeaGreen,
            Color.BlueViolet, Color.DarkCyan, Color.Maroon, Color.Chocolate, Color.DarkKhaki
        };
    }


***
The 'other' 10 forms use this class which mocks a binding source like this:

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

I believe that there's no 'right' answer to your question but I hope there's something here that might move things forward for you.


  [1]: https://i.stack.imgur.com/riKdF.png