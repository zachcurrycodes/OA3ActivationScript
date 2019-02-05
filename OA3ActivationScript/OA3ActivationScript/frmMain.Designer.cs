namespace OA3ActivationScript
{
  partial class FrmMain
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.lblChooseOs = new System.Windows.Forms.Label();
      this.gbOsRadio = new System.Windows.Forms.GroupBox();
      this.lblNetwork = new System.Windows.Forms.Label();
      this.lblShare = new System.Windows.Forms.Label();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.SystemInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DmiManufacturer = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DmiModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DmiSerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DmiChassisSerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ProcCores = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.IpAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BiosManufacturer = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.gbMB = new System.Windows.Forms.GroupBox();
      this.gbActions = new System.Windows.Forms.GroupBox();
      this.btnSubmit = new System.Windows.Forms.Button();
      this.cbDefaultKey = new System.Windows.Forms.CheckBox();
      this.btnDefault = new System.Windows.Forms.Button();
      this.btnClearKey = new System.Windows.Forms.Button();
      this.btnProductKey = new System.Windows.Forms.Button();
      this.menuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.gbActions.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(984, 24);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
      // 
      // lblChooseOs
      // 
      this.lblChooseOs.AutoSize = true;
      this.lblChooseOs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblChooseOs.Location = new System.Drawing.Point(15, 28);
      this.lblChooseOs.Name = "lblChooseOs";
      this.lblChooseOs.Size = new System.Drawing.Size(522, 20);
      this.lblChooseOs.TabIndex = 2;
      this.lblChooseOs.Text = "Choose the BOXX part number for Operating System (Required)";
      // 
      // gbOsRadio
      // 
      this.gbOsRadio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.gbOsRadio.Location = new System.Drawing.Point(15, 100);
      this.gbOsRadio.Name = "gbOsRadio";
      this.gbOsRadio.Size = new System.Drawing.Size(415, 450);
      this.gbOsRadio.TabIndex = 3;
      this.gbOsRadio.TabStop = false;
      this.gbOsRadio.Text = "Choose the BOXX part number for Operating System (Required)";
      // 
      // lblNetwork
      // 
      this.lblNetwork.Location = new System.Drawing.Point(555, 34);
      this.lblNetwork.Name = "lblNetwork";
      this.lblNetwork.Size = new System.Drawing.Size(100, 13);
      this.lblNetwork.TabIndex = 4;
      this.lblNetwork.Text = "Network Status";
      // 
      // lblShare
      // 
      this.lblShare.Location = new System.Drawing.Point(660, 34);
      this.lblShare.Name = "lblShare";
      this.lblShare.Size = new System.Drawing.Size(100, 13);
      this.lblShare.TabIndex = 5;
      this.lblShare.Text = "Shared Folder Status";
      // 
      // dataGridView1
      // 
      dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
      this.dataGridView1.ColumnHeadersHeight = 18;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SystemInformation,
            this.DmiManufacturer,
            this.DmiModel,
            this.DmiSerNo,
            this.DmiChassisSerNo,
            this.ProcCores,
            this.IpAddress,
            this.BiosManufacturer});
      this.dataGridView1.Enabled = false;
      this.dataGridView1.GridColor = System.Drawing.SystemColors.Control;
      this.dataGridView1.Location = new System.Drawing.Point(15, 52);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.Size = new System.Drawing.Size(845, 42);
      this.dataGridView1.TabIndex = 6;
      // 
      // SystemInformation
      // 
      this.SystemInformation.HeaderText = "System Information";
      this.SystemInformation.Name = "SystemInformation";
      // 
      // DmiManufacturer
      // 
      this.DmiManufacturer.HeaderText = "DMI Manufacturer";
      this.DmiManufacturer.Name = "DmiManufacturer";
      // 
      // DmiModel
      // 
      this.DmiModel.HeaderText = "DMI Model";
      this.DmiModel.Name = "DmiModel";
      // 
      // DmiSerNo
      // 
      this.DmiSerNo.HeaderText = "DMI BIOS Serial Number";
      this.DmiSerNo.Name = "DmiSerNo";
      // 
      // DmiChassisSerNo
      // 
      this.DmiChassisSerNo.HeaderText = "DMI Chassis Serial Number";
      this.DmiChassisSerNo.Name = "DmiChassisSerNo";
      // 
      // ProcCores
      // 
      this.ProcCores.HeaderText = "Processor Cores (Total)";
      this.ProcCores.Name = "ProcCores";
      // 
      // IpAddress
      // 
      this.IpAddress.HeaderText = "IP Address";
      this.IpAddress.Name = "IpAddress";
      // 
      // BiosManufacturer
      // 
      this.BiosManufacturer.HeaderText = "BIOS Manufacturer";
      this.BiosManufacturer.Name = "BiosManufacturer";
      // 
      // gbMB
      // 
      this.gbMB.Location = new System.Drawing.Point(445, 100);
      this.gbMB.Name = "gbMB";
      this.gbMB.Size = new System.Drawing.Size(415, 90);
      this.gbMB.TabIndex = 7;
      this.gbMB.TabStop = false;
      this.gbMB.Text = "Choose the Mainboard Manufacturer (Required)";
      // 
      // gbActions
      // 
      this.gbActions.Controls.Add(this.btnSubmit);
      this.gbActions.Controls.Add(this.cbDefaultKey);
      this.gbActions.Controls.Add(this.btnDefault);
      this.gbActions.Controls.Add(this.btnClearKey);
      this.gbActions.Controls.Add(this.btnProductKey);
      this.gbActions.Location = new System.Drawing.Point(445, 200);
      this.gbActions.Name = "gbActions";
      this.gbActions.Size = new System.Drawing.Size(415, 165);
      this.gbActions.TabIndex = 8;
      this.gbActions.TabStop = false;
      // 
      // btnSubmit
      // 
      this.btnSubmit.Location = new System.Drawing.Point(10, 130);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new System.Drawing.Size(395, 25);
      this.btnSubmit.TabIndex = 4;
      this.btnSubmit.Text = "Submit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
      // 
      // cbDefaultKey
      // 
      this.cbDefaultKey.AutoSize = true;
      this.cbDefaultKey.Checked = true;
      this.cbDefaultKey.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbDefaultKey.Location = new System.Drawing.Point(10, 20);
      this.cbDefaultKey.Name = "cbDefaultKey";
      this.cbDefaultKey.Size = new System.Drawing.Size(221, 17);
      this.cbDefaultKey.TabIndex = 3;
      this.cbDefaultKey.Text = "Change Product Key to OA3 default key?";
      this.cbDefaultKey.UseVisualStyleBackColor = true;
      // 
      // btnDefault
      // 
      this.btnDefault.Location = new System.Drawing.Point(10, 100);
      this.btnDefault.Name = "btnDefault";
      this.btnDefault.Size = new System.Drawing.Size(395, 25);
      this.btnDefault.TabIndex = 2;
      this.btnDefault.Text = "Set default OA3 Key";
      this.btnDefault.UseVisualStyleBackColor = true;
      this.btnDefault.Click += new System.EventHandler(this.BtnDefault_Click);
      // 
      // btnClearKey
      // 
      this.btnClearKey.Location = new System.Drawing.Point(10, 70);
      this.btnClearKey.Name = "btnClearKey";
      this.btnClearKey.Size = new System.Drawing.Size(395, 25);
      this.btnClearKey.TabIndex = 1;
      this.btnClearKey.Text = "Clear Key";
      this.btnClearKey.UseVisualStyleBackColor = true;
      this.btnClearKey.Click += new System.EventHandler(this.BtnClearKey_Click);
      // 
      // btnProductKey
      // 
      this.btnProductKey.Location = new System.Drawing.Point(10, 40);
      this.btnProductKey.Name = "btnProductKey";
      this.btnProductKey.Size = new System.Drawing.Size(395, 25);
      this.btnProductKey.TabIndex = 0;
      this.btnProductKey.Text = "Retrieve Current Product Key";
      this.btnProductKey.UseVisualStyleBackColor = true;
      this.btnProductKey.Click += new System.EventHandler(this.BtnProductKey_Click);
      // 
      // frmMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(984, 561);
      this.Controls.Add(this.gbActions);
      this.Controls.Add(this.gbMB);
      this.Controls.Add(this.dataGridView1);
      this.Controls.Add(this.lblShare);
      this.Controls.Add(this.lblNetwork);
      this.Controls.Add(this.gbOsRadio);
      this.Controls.Add(this.lblChooseOs);
      this.Controls.Add(this.menuStrip1);
      this.Name = "frmMain";
      this.Text = "BOXX OA3 Activation Script";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_Close);
      this.Load += new System.EventHandler(this.FrmMain_Load);
      this.Resize += new System.EventHandler(this.FrmMain_Resize_1);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.gbActions.ResumeLayout(false);
      this.gbActions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.Label lblChooseOs;
    private System.Windows.Forms.GroupBox gbOsRadio;
    private System.Windows.Forms.Label lblNetwork;
    private System.Windows.Forms.Label lblShare;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.DataGridViewTextBoxColumn SystemInformation;
    private System.Windows.Forms.DataGridViewTextBoxColumn DmiManufacturer;
    private System.Windows.Forms.DataGridViewTextBoxColumn DmiModel;
    private System.Windows.Forms.DataGridViewTextBoxColumn DmiSerNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn DmiChassisSerNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn ProcCores;
    private System.Windows.Forms.DataGridViewTextBoxColumn IpAddress;
    private System.Windows.Forms.DataGridViewTextBoxColumn BiosManufacturer;
    private System.Windows.Forms.GroupBox gbMB;
    private System.Windows.Forms.GroupBox gbActions;
    private System.Windows.Forms.Button btnSubmit;
    private System.Windows.Forms.CheckBox cbDefaultKey;
    private System.Windows.Forms.Button btnDefault;
    private System.Windows.Forms.Button btnClearKey;
    private System.Windows.Forms.Button btnProductKey;
  }
}

