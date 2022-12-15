using System.ComponentModel;

namespace ui_form_multithreading
{
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
            FormWithLongRunningTask.PropertyChanged += onAnyFWLRTPropertyChanged;
            for (int i = 0; i < 10; i++)
            {
                new FormWithLongRunningTask { Name = $"Form{i}" }.Show(this);
            }
            // Exercise a data binding
            textBoxWithDataBinding.DataBindings.Add(
                nameof(TextBox.Text),
                this,
                nameof(ReceivedTimestamp)
            );
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
                            richTextBox.SelectionColor = _colors[int.Parse(form.Name.Replace("Form", string.Empty))];
                            richTextBox
                            .AppendText(
                                $"Sender: {form.Name} @ {form.TimeStamp}{Environment.NewLine}");
                            richTextBox.Select(richTextBox.TextLength, 0);
                            richTextBox.ScrollToCaret();
                            // Test the textbox data binding
                            ReceivedTimestamp = form.TimeStamp.ToLongTimeString();

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
    class DataGridViewEx : DataGridView
    {
        public new BindingList<Record> DataSource { get; } = new BindingList<Record>();
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Format columns
            base.DataSource = this.DataSource;
            DataSource.Add(new Record());
            Columns[nameof(Record.Sender)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            var col =  Columns[nameof(Record.TimeStamp)];
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            col.DefaultCellStyle.Format = "hh:mm:ss tt";
            DataSource.Clear();
        }
    }
    class Record
    {
        public string Sender { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
    }
}