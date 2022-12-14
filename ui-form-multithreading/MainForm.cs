using System.ComponentModel;

namespace ui_form_multithreading
{
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
}