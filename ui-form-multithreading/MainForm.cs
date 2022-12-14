namespace ui_form_multithreading
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            BackColor = Color.AliceBlue;
            StartPosition = FormStartPosition.Manual;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormWithLongRunningTask.TaskComplete += onAnyTaskComplete;
            for (int i = 0; i < 10; i++)
            {
                new FormWithLongRunningTask().Show(this);
            }
        }

        private void onAnyTaskComplete(object sender, TaskCompleteEventArgs e)
        {
        }
    }
}