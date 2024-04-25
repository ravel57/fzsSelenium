using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Spreadsheet;

namespace fzsSelenium
{
    public partial class SettingForm : Form
    {
        
        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Closed(object sender, EventArgs e)
        {
            var list = new ArrayList();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!string.IsNullOrEmpty((string)row.Cells[0].Value))
                {
                    list.Add(new ValueRow((string)row.Cells[0].Value, (string)row.Cells[1].Value));
                }
            }

            MainForm.constructorList = (ValueRow[])list.ToArray(typeof(ValueRow));
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            for (var i = 0; i < MainForm.constructorList.Length; i++)
            {
                var valueRow = MainForm.constructorList[i];
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells["SelectorColumn"].Value = valueRow.Type;
                dataGridView.Rows[i].Cells["ColumnValue"].Value = valueRow.Value;
            }
        }


        public class ValueRow(string type, string value)
        {
            public string Type { get; set; } = type;
            public string Value { get; set; } = value;
        }
    }
}