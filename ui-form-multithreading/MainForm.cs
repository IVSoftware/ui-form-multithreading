using System.ComponentModel;
using System.Windows.Forms;

namespace ui_form_multithreading
{
    public partial class MainForm : Form, INotifyPropertyChanged
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormWithLongRunningTask.PropertyChanged += onAnyFWLRTPropertyChanged;
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
    class Record
    {
        public string Sender { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
    }
}