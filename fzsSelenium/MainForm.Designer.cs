namespace fzsSelenium {
	partial class MainForm {
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.selectFile_button = new System.Windows.Forms.Button();
			this.fileName_label = new System.Windows.Forms.Label();
			this.citys_groupBox = new System.Windows.Forms.GroupBox();
			this.cityMO_radioButton = new System.Windows.Forms.RadioButton();
			this.cityMejReg_radioButton = new System.Windows.Forms.RadioButton();
			this.cityMos_radioButton = new System.Windows.Forms.RadioButton();
			this.startFzs_button = new System.Windows.Forms.Button();
			this.fileView_dataGridView = new System.Windows.Forms.DataGridView();
			this.type_groupBox = new System.Windows.Forms.GroupBox();
			this.roleUL2_radioButton = new System.Windows.Forms.RadioButton();
			this.roleUL_radioButton = new System.Windows.Forms.RadioButton();
			this.roleFL_radioButton = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.genderF_radioButton = new System.Windows.Forms.RadioButton();
			this.genderM_radioButton = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.certTimeContu_radioButton = new System.Windows.Forms.RadioButton();
			this.certTimeRe_radioButton = new System.Windows.Forms.RadioButton();
			this.certTimeFirst_radioButton = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.powerOfAttorneyHasNumber_checkBox = new System.Windows.Forms.CheckBox();
			this.customReleaseBasis_textBox = new System.Windows.Forms.TextBox();
			this.customReleaseBasis_radioButton = new System.Windows.Forms.RadioButton();
			this.powerOfAttorneyReleaseBasis_radioButton = new System.Windows.Forms.RadioButton();
			this.settingsButton = new System.Windows.Forms.Button();
			this.citys_groupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileView_dataGridView)).BeginInit();
			this.type_groupBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// selectFile_button
			// 
			this.selectFile_button.Location = new System.Drawing.Point(12, 10);
			this.selectFile_button.Name = "selectFile_button";
			this.selectFile_button.Size = new System.Drawing.Size(110, 23);
			this.selectFile_button.TabIndex = 0;
			this.selectFile_button.Text = "Открыть файл";
			this.selectFile_button.UseVisualStyleBackColor = true;
			this.selectFile_button.Click += new System.EventHandler(this.selectFile_button_Click);
			// 
			// fileName_label
			// 
			this.fileName_label.AutoSize = true;
			this.fileName_label.Location = new System.Drawing.Point(128, 15);
			this.fileName_label.Name = "fileName_label";
			this.fileName_label.Size = new System.Drawing.Size(95, 13);
			this.fileName_label.TabIndex = 1;
			this.fileName_label.Text = "(файл не выбран)";
			// 
			// citys_groupBox
			// 
			this.citys_groupBox.Controls.Add(this.cityMO_radioButton);
			this.citys_groupBox.Controls.Add(this.cityMejReg_radioButton);
			this.citys_groupBox.Controls.Add(this.cityMos_radioButton);
			this.citys_groupBox.Location = new System.Drawing.Point(148, 41);
			this.citys_groupBox.Name = "citys_groupBox";
			this.citys_groupBox.Size = new System.Drawing.Size(152, 93);
			this.citys_groupBox.TabIndex = 2;
			this.citys_groupBox.TabStop = false;
			this.citys_groupBox.Text = "Регион - ТОФК";
			// 
			// cityMO_radioButton
			// 
			this.cityMO_radioButton.AutoSize = true;
			this.cityMO_radioButton.Location = new System.Drawing.Point(6, 66);
			this.cityMO_radioButton.Name = "cityMO_radioButton";
			this.cityMO_radioButton.Size = new System.Drawing.Size(125, 17);
			this.cityMO_radioButton.TabIndex = 1;
			this.cityMO_radioButton.TabStop = true;
			this.cityMO_radioButton.Text = "Мос Область - 4800";
			this.cityMO_radioButton.UseVisualStyleBackColor = true;
			this.cityMO_radioButton.CheckedChanged += new System.EventHandler(this.city3_radioButton_CheckedChanged);
			// 
			// cityMejReg_radioButton
			// 
			this.cityMejReg_radioButton.AutoSize = true;
			this.cityMejReg_radioButton.Location = new System.Drawing.Point(6, 20);
			this.cityMejReg_radioButton.Name = "cityMejReg_radioButton";
			this.cityMejReg_radioButton.Size = new System.Drawing.Size(125, 17);
			this.cityMejReg_radioButton.TabIndex = 0;
			this.cityMejReg_radioButton.TabStop = true;
			this.cityMejReg_radioButton.Text = "Межрегион    - 9500";
			this.cityMejReg_radioButton.UseVisualStyleBackColor = true;
			this.cityMejReg_radioButton.CheckedChanged += new System.EventHandler(this.city1_radioButton_CheckedChanged);
			// 
			// cityMos_radioButton
			// 
			this.cityMos_radioButton.AutoSize = true;
			this.cityMos_radioButton.Location = new System.Drawing.Point(6, 43);
			this.cityMos_radioButton.Name = "cityMos_radioButton";
			this.cityMos_radioButton.Size = new System.Drawing.Size(124, 17);
			this.cityMos_radioButton.TabIndex = 0;
			this.cityMos_radioButton.TabStop = true;
			this.cityMos_radioButton.Text = "Москва          - 7300";
			this.cityMos_radioButton.UseVisualStyleBackColor = true;
			this.cityMos_radioButton.CheckedChanged += new System.EventHandler(this.city2_radioButton_CheckedChanged);
			// 
			// startFzs_button
			// 
			this.startFzs_button.Enabled = false;
			this.startFzs_button.Location = new System.Drawing.Point(721, 41);
			this.startFzs_button.Name = "startFzs_button";
			this.startFzs_button.Size = new System.Drawing.Size(110, 93);
			this.startFzs_button.TabIndex = 7;
			this.startFzs_button.Text = "НАЧАТЬ";
			this.startFzs_button.UseVisualStyleBackColor = true;
			this.startFzs_button.Click += new System.EventHandler(this.startFzs_button_Click);
			// 
			// fileView_dataGridView
			// 
			this.fileView_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.fileView_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fileView_dataGridView.Location = new System.Drawing.Point(12, 140);
			this.fileView_dataGridView.MultiSelect = false;
			this.fileView_dataGridView.Name = "fileView_dataGridView";
			this.fileView_dataGridView.Size = new System.Drawing.Size(819, 291);
			this.fileView_dataGridView.TabIndex = 4;
			// 
			// type_groupBox
			// 
			this.type_groupBox.Controls.Add(this.roleUL2_radioButton);
			this.type_groupBox.Controls.Add(this.roleUL_radioButton);
			this.type_groupBox.Controls.Add(this.roleFL_radioButton);
			this.type_groupBox.Location = new System.Drawing.Point(312, 41);
			this.type_groupBox.Name = "type_groupBox";
			this.type_groupBox.Size = new System.Drawing.Size(110, 93);
			this.type_groupBox.TabIndex = 4;
			this.type_groupBox.TabStop = false;
			this.type_groupBox.Text = "Лицо";
			// 
			// roleUL2_radioButton
			// 
			this.roleUL2_radioButton.AutoSize = true;
			this.roleUL2_radioButton.Location = new System.Drawing.Point(6, 67);
			this.roleUL2_radioButton.Name = "roleUL2_radioButton";
			this.roleUL2_radioButton.Size = new System.Drawing.Size(93, 17);
			this.roleUL2_radioButton.TabIndex = 1;
			this.roleUL2_radioButton.TabStop = true;
			this.roleUL2_radioButton.Text = "ЮЛ без ФИО";
			this.roleUL2_radioButton.UseVisualStyleBackColor = true;
			this.roleUL2_radioButton.CheckedChanged += new System.EventHandler(this.role3_radioButton_CheckedChanged);
			// 
			// roleUL_radioButton
			// 
			this.roleUL_radioButton.AutoSize = true;
			this.roleUL_radioButton.Location = new System.Drawing.Point(7, 44);
			this.roleUL_radioButton.Name = "roleUL_radioButton";
			this.roleUL_radioButton.Size = new System.Drawing.Size(42, 17);
			this.roleUL_radioButton.TabIndex = 1;
			this.roleUL_radioButton.TabStop = true;
			this.roleUL_radioButton.Text = "ЮЛ";
			this.roleUL_radioButton.UseVisualStyleBackColor = true;
			this.roleUL_radioButton.CheckedChanged += new System.EventHandler(this.role2_radioButton_CheckedChanged);
			// 
			// roleFL_radioButton
			// 
			this.roleFL_radioButton.AutoSize = true;
			this.roleFL_radioButton.Location = new System.Drawing.Point(7, 20);
			this.roleFL_radioButton.Name = "roleFL_radioButton";
			this.roleFL_radioButton.Size = new System.Drawing.Size(44, 17);
			this.roleFL_radioButton.TabIndex = 0;
			this.roleFL_radioButton.TabStop = true;
			this.roleFL_radioButton.Text = "ФЛ";
			this.roleFL_radioButton.UseVisualStyleBackColor = true;
			this.roleFL_radioButton.CheckedChanged += new System.EventHandler(this.role1_radioButton_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.genderF_radioButton);
			this.groupBox1.Controls.Add(this.genderM_radioButton);
			this.groupBox1.Location = new System.Drawing.Point(434, 41);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(110, 93);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Пол";
			// 
			// genderF_radioButton
			// 
			this.genderF_radioButton.AutoSize = true;
			this.genderF_radioButton.Location = new System.Drawing.Point(7, 44);
			this.genderF_radioButton.Name = "genderF_radioButton";
			this.genderF_radioButton.Size = new System.Drawing.Size(72, 17);
			this.genderF_radioButton.TabIndex = 1;
			this.genderF_radioButton.TabStop = true;
			this.genderF_radioButton.Text = "Женский";
			this.genderF_radioButton.UseVisualStyleBackColor = true;
			this.genderF_radioButton.CheckedChanged += new System.EventHandler(this.genderF_radioButton_CheckedChanged);
			// 
			// genderM_radioButton
			// 
			this.genderM_radioButton.AutoSize = true;
			this.genderM_radioButton.Location = new System.Drawing.Point(7, 20);
			this.genderM_radioButton.Name = "genderM_radioButton";
			this.genderM_radioButton.Size = new System.Drawing.Size(71, 17);
			this.genderM_radioButton.TabIndex = 0;
			this.genderM_radioButton.TabStop = true;
			this.genderM_radioButton.Text = "Мужской";
			this.genderM_radioButton.UseVisualStyleBackColor = true;
			this.genderM_radioButton.CheckedChanged += new System.EventHandler(this.genderM_radioButton_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.certTimeContu_radioButton);
			this.groupBox2.Controls.Add(this.certTimeRe_radioButton);
			this.groupBox2.Controls.Add(this.certTimeFirst_radioButton);
			this.groupBox2.Location = new System.Drawing.Point(12, 41);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(124, 93);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Тип выпуска";
			// 
			// certTimeContu_radioButton
			// 
			this.certTimeContu_radioButton.AutoSize = true;
			this.certTimeContu_radioButton.Location = new System.Drawing.Point(7, 68);
			this.certTimeContu_radioButton.Name = "certTimeContu_radioButton";
			this.certTimeContu_radioButton.Size = new System.Drawing.Size(88, 17);
			this.certTimeContu_radioButton.TabIndex = 2;
			this.certTimeContu_radioButton.TabStop = true;
			this.certTimeContu_radioButton.Text = "Продолжить";
			this.certTimeContu_radioButton.UseVisualStyleBackColor = true;
			this.certTimeContu_radioButton.CheckedChanged += new System.EventHandler(this.certTime3_radioButton_CheckedChanged);
			// 
			// certTimeRe_radioButton
			// 
			this.certTimeRe_radioButton.AutoSize = true;
			this.certTimeRe_radioButton.Location = new System.Drawing.Point(7, 44);
			this.certTimeRe_radioButton.Name = "certTimeRe_radioButton";
			this.certTimeRe_radioButton.Size = new System.Drawing.Size(88, 17);
			this.certTimeRe_radioButton.TabIndex = 1;
			this.certTimeRe_radioButton.Text = "Перевыпуск";
			this.certTimeRe_radioButton.UseVisualStyleBackColor = true;
			this.certTimeRe_radioButton.CheckedChanged += new System.EventHandler(this.certTime2_radioButton_CheckedChanged);
			// 
			// certTimeFirst_radioButton
			// 
			this.certTimeFirst_radioButton.AutoSize = true;
			this.certTimeFirst_radioButton.Checked = true;
			this.certTimeFirst_radioButton.Location = new System.Drawing.Point(7, 20);
			this.certTimeFirst_radioButton.Name = "certTimeFirst_radioButton";
			this.certTimeFirst_radioButton.Size = new System.Drawing.Size(82, 17);
			this.certTimeFirst_radioButton.TabIndex = 0;
			this.certTimeFirst_radioButton.TabStop = true;
			this.certTimeFirst_radioButton.Text = "Первичный";
			this.certTimeFirst_radioButton.UseVisualStyleBackColor = true;
			this.certTimeFirst_radioButton.CheckedChanged += new System.EventHandler(this.certTime1_radioButton_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.powerOfAttorneyHasNumber_checkBox);
			this.groupBox3.Controls.Add(this.customReleaseBasis_textBox);
			this.groupBox3.Controls.Add(this.customReleaseBasis_radioButton);
			this.groupBox3.Controls.Add(this.powerOfAttorneyReleaseBasis_radioButton);
			this.groupBox3.Location = new System.Drawing.Point(550, 41);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(165, 93);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "На основании...";
			// 
			// powerOfAttorneyHasNumber_checkBox
			// 
			this.powerOfAttorneyHasNumber_checkBox.AutoSize = true;
			this.powerOfAttorneyHasNumber_checkBox.Checked = true;
			this.powerOfAttorneyHasNumber_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.powerOfAttorneyHasNumber_checkBox.Location = new System.Drawing.Point(7, 67);
			this.powerOfAttorneyHasNumber_checkBox.Name = "powerOfAttorneyHasNumber_checkBox";
			this.powerOfAttorneyHasNumber_checkBox.Size = new System.Drawing.Size(158, 17);
			this.powerOfAttorneyHasNumber_checkBox.TabIndex = 3;
			this.powerOfAttorneyHasNumber_checkBox.Text = "Доверенность с номером";
			this.powerOfAttorneyHasNumber_checkBox.UseVisualStyleBackColor = true;
			// 
			// customReleaseBasis_textBox
			// 
			this.customReleaseBasis_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.customReleaseBasis_textBox.Location = new System.Drawing.Point(25, 41);
			this.customReleaseBasis_textBox.Name = "customReleaseBasis_textBox";
			this.customReleaseBasis_textBox.Size = new System.Drawing.Size(88, 20);
			this.customReleaseBasis_textBox.TabIndex = 2;
			this.customReleaseBasis_textBox.Click += new System.EventHandler(this.customReleaseBasis_textBox_TextChanged);
			this.customReleaseBasis_textBox.TextChanged += new System.EventHandler(this.customReleaseBasis_textBox_TextChanged);
			// 
			// customReleaseBasis_radioButton
			// 
			this.customReleaseBasis_radioButton.AutoSize = true;
			this.customReleaseBasis_radioButton.Location = new System.Drawing.Point(7, 44);
			this.customReleaseBasis_radioButton.Name = "customReleaseBasis_radioButton";
			this.customReleaseBasis_radioButton.Size = new System.Drawing.Size(14, 13);
			this.customReleaseBasis_radioButton.TabIndex = 1;
			this.customReleaseBasis_radioButton.UseVisualStyleBackColor = true;
			this.customReleaseBasis_radioButton.CheckedChanged += new System.EventHandler(this.customReleaseBasis_radioButton_CheckedChanged);
			// 
			// powerOfAttorneyReleaseBasis_radioButton
			// 
			this.powerOfAttorneyReleaseBasis_radioButton.AutoSize = true;
			this.powerOfAttorneyReleaseBasis_radioButton.Checked = true;
			this.powerOfAttorneyReleaseBasis_radioButton.Location = new System.Drawing.Point(7, 20);
			this.powerOfAttorneyReleaseBasis_radioButton.Name = "powerOfAttorneyReleaseBasis_radioButton";
			this.powerOfAttorneyReleaseBasis_radioButton.Size = new System.Drawing.Size(67, 17);
			this.powerOfAttorneyReleaseBasis_radioButton.TabIndex = 0;
			this.powerOfAttorneyReleaseBasis_radioButton.TabStop = true;
			this.powerOfAttorneyReleaseBasis_radioButton.Text = "приказа";
			this.powerOfAttorneyReleaseBasis_radioButton.UseVisualStyleBackColor = true;
			this.powerOfAttorneyReleaseBasis_radioButton.CheckedChanged += new System.EventHandler(this.powerOfAttorneyReleaseBasis_radioButton_CheckedChanged);
			// 
			// settingsButton
			// 
			this.settingsButton.Location = new System.Drawing.Point(722, 10);
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(108, 22);
			this.settingsButton.TabIndex = 8;
			this.settingsButton.Text = "Настройки";
			this.settingsButton.UseVisualStyleBackColor = true;
			this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(843, 445);
			this.Controls.Add(this.settingsButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.type_groupBox);
			this.Controls.Add(this.fileView_dataGridView);
			this.Controls.Add(this.startFzs_button);
			this.Controls.Add(this.citys_groupBox);
			this.Controls.Add(this.fileName_label);
			this.Controls.Add(this.selectFile_button);
			this.Name = "MainForm";
			this.Text = "mainForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
			this.Load += new System.EventHandler(this.mainForm_Load);
			this.citys_groupBox.ResumeLayout(false);
			this.citys_groupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileView_dataGridView)).EndInit();
			this.type_groupBox.ResumeLayout(false);
			this.type_groupBox.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.Button settingsButton;

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button selectFile_button;
		private System.Windows.Forms.Label fileName_label;
		private System.Windows.Forms.GroupBox citys_groupBox;
		private System.Windows.Forms.RadioButton cityMO_radioButton;
		private System.Windows.Forms.RadioButton cityMos_radioButton;
		private System.Windows.Forms.Button startFzs_button;
		private System.Windows.Forms.DataGridView fileView_dataGridView;
		private System.Windows.Forms.GroupBox type_groupBox;
		private System.Windows.Forms.RadioButton roleUL_radioButton;
		private System.Windows.Forms.RadioButton roleFL_radioButton;
		private System.Windows.Forms.RadioButton roleUL2_radioButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton genderF_radioButton;
		private System.Windows.Forms.RadioButton genderM_radioButton;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton certTimeRe_radioButton;
		private System.Windows.Forms.RadioButton certTimeFirst_radioButton;
		private System.Windows.Forms.RadioButton certTimeContu_radioButton;
		private System.Windows.Forms.RadioButton cityMejReg_radioButton;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox customReleaseBasis_textBox;
		private System.Windows.Forms.RadioButton customReleaseBasis_radioButton;
		private System.Windows.Forms.RadioButton powerOfAttorneyReleaseBasis_radioButton;
		private System.Windows.Forms.CheckBox powerOfAttorneyHasNumber_checkBox;
	}
}

