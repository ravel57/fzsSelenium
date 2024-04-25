using System.ComponentModel;

namespace fzsSelenium
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.SelectorColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.SelectorColumn, this.ColumnValue });
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(776, 426);
            this.dataGridView.TabIndex = 0;
            // 
            // SelectorColumn
            // 
            this.SelectorColumn.HeaderText = "typeColumn";
            this.SelectorColumn.Items.AddRange(new object[] { "id", "classes : text", "tag : text", "sleep" });
            this.SelectorColumn.Name = "SelectorColumn";
            this.SelectorColumn.Width = 200;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnValue.Width = 200;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Closed += new System.EventHandler(this.SettingForm_Closed);
            this.Load += new System.EventHandler(this.SettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridViewComboBoxColumn SelectorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;

        private System.Windows.Forms.DataGridView dataGridView;

        #endregion
    }
}