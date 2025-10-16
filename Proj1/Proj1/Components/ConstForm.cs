namespace Proj1.Components
{
    public partial class ConstForm : Form
    {
        public decimal SelectedLength => NUP1.Value;
        public ConstForm()
        {
            InitializeComponent();
        }
        private void B1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        private void B2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}