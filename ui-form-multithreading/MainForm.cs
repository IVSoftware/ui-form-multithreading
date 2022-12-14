namespace ui_form_multithreading
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            BackColor = Color.CornflowerBlue;
        }
        Color[] _colors = new Color[]
        {
            Color.Black, Color.Blue, Color.Green, Color.LightSalmon, Color.SeaGreen,
            Color.BlueViolet, Color.DarkCyan, Color.Maroon, Color.Chocolate, Color.DarkKhaki
        };

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
    }
}