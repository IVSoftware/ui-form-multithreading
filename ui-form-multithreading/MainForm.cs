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
                    richTextBox.Select(richTextBox.TextLength, 0);
                    richTextBox.ScrollToCaret();
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
}