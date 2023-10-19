using HumanMachineInterface.App;
using System.Data.Common;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace HumanMachineInterface.UI2
{
    public partial class Form1 : Form
    {
        Catalog catalog;

        public Form1()
        {
            InitializeComponent();
            catalog = new Catalog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var v = dataGridView1[1, e.RowIndex].Value.ToString();
                if (v == "f ")
                    MessageBox.Show("Вы не можите перейти в файл");
                else
                {
                    var directory = dataGridView1[0, e.RowIndex].Value.ToString();
                    bool isAbsolute = false;

                    var result = catalog.ChangeDirectory(directory, isAbsolute);

                    if (!result.Item1)
                    {
                        MessageBox.Show("Операция невыполнена");
                    }
                    else
                    {
                        Init();
                    }
                }
            }
        }

        private void Init()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            label1.Text = catalog.MyPath;

            dataGridView1.Columns[0].Width = 210;
            dataGridView1.Columns[1].Width = 160;

            var column2 = dataGridView1.Columns[1];
            column2.Width = 120;

            int i = 0;
            foreach (var element in catalog.GetAllItems())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = element.Name;
                dataGridView1.Rows[i].Cells[1].Value = element.Type == CatalogItemType.File ? "f " : "d ";
                i++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = catalog.ChangeDirectoryToBack();

            if (!result.Item1)
            {
                MessageBox.Show("Операция невыполнена");
            }
            else
            {
                Init();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAbsoluteAddress formAddress = new FormAbsoluteAddress();

            if (formAddress.ShowDialog() == DialogResult.OK)
            {
                var adress = formAddress.Address;

                var result = catalog.ChangeDirectory(adress, true);

                if (!result.Item1)
                {
                    MessageBox.Show("Операция невыполнена");
                }
                else
                {
                    Init();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormMoveFiles formAddress = new FormMoveFiles();

            if (formAddress.ShowDialog() == DialogResult.OK)
            {
                var adress = formAddress.Address;
                var listFiles = new List<string>(); 

                for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                {
                    int selRowNum = dataGridView1.SelectedCells[i].RowIndex;
                    if (dataGridView1.Rows[selRowNum].Cells[1].Value.ToString() == "d ")
                    {
                        MessageBox.Show("Вы не можите переместить директорию");
                        return;
                    }

                    listFiles.Add(dataGridView1.Rows[selRowNum].Cells[0].Value.ToString());

                }

                var result = catalog.MoveFiles(adress, listFiles);

                if (!result.Item1)
                {
                    MessageBox.Show("Операция невыполнена");
                }
                else
                {
                    Init();
                }
            }
        }
    }
}