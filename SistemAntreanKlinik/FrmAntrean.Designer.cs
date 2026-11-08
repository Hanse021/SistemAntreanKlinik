namespace SistemAntreanKlinik
{
    partial class FrmAntrean
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
            this.tlpGridKursi = new System.Windows.Forms.TableLayoutPanel();
            this.lblDetailKursi = new System.Windows.Forms.Label();
            this.lblPasien = new System.Windows.Forms.Label();
            this.cmbPasien = new System.Windows.Forms.ComboBox();
            this.lblPoli = new System.Windows.Forms.Label();
            this.cmbPoli = new System.Windows.Forms.ComboBox();
            this.btnDaftar = new System.Windows.Forms.Button();
            this.btnSelesaikan = new System.Windows.Forms.Button();
            this.btnBatalkan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tlpGridKursi
            // 
            this.tlpGridKursi.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpGridKursi.ColumnCount = 2;
            this.tlpGridKursi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGridKursi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGridKursi.Location = new System.Drawing.Point(30, 90);
            this.tlpGridKursi.Name = "tlpGridKursi";
            this.tlpGridKursi.RowCount = 2;
            this.tlpGridKursi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGridKursi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGridKursi.Size = new System.Drawing.Size(560, 360);
            this.tlpGridKursi.TabIndex = 0;
            // 
            // lblDetailKursi
            // 
            this.lblDetailKursi.Location = new System.Drawing.Point(27, 453);
            this.lblDetailKursi.Name = "lblDetailKursi";
            this.lblDetailKursi.Size = new System.Drawing.Size(560, 70);
            this.lblDetailKursi.TabIndex = 0;
            this.lblDetailKursi.Text = "\tPilih kursi untuk melihat detail.";
            this.lblDetailKursi.Click += new System.EventHandler(this.lblDetailKursi_Click);
            // 
            // lblPasien
            // 
            this.lblPasien.Location = new System.Drawing.Point(30, 25);
            this.lblPasien.Name = "lblPasien";
            this.lblPasien.Size = new System.Drawing.Size(77, 16);
            this.lblPasien.TabIndex = 1;
            this.lblPasien.Text = "Pilih Pasien";
            // 
            // cmbPasien
            // 
            this.cmbPasien.FormattingEnabled = true;
            this.cmbPasien.Location = new System.Drawing.Point(33, 44);
            this.cmbPasien.Name = "cmbPasien";
            this.cmbPasien.Size = new System.Drawing.Size(280, 24);
            this.cmbPasien.TabIndex = 2;
            // 
            // lblPoli
            // 
            this.lblPoli.AutoSize = true;
            this.lblPoli.Location = new System.Drawing.Point(320, 25);
            this.lblPoli.Name = "lblPoli";
            this.lblPoli.Size = new System.Drawing.Size(74, 16);
            this.lblPoli.TabIndex = 3;
            this.lblPoli.Text = "Poli Tujuan";
            // 
            // cmbPoli
            // 
            this.cmbPoli.FormattingEnabled = true;
            this.cmbPoli.Location = new System.Drawing.Point(323, 44);
            this.cmbPoli.Name = "cmbPoli";
            this.cmbPoli.Size = new System.Drawing.Size(280, 24);
            this.cmbPoli.TabIndex = 4;
            // 
            // btnDaftar
            // 
            this.btnDaftar.Location = new System.Drawing.Point(410, 458);
            this.btnDaftar.Name = "btnDaftar";
            this.btnDaftar.Size = new System.Drawing.Size(180, 40);
            this.btnDaftar.TabIndex = 5;
            this.btnDaftar.Text = "Daftarkan Antrean";
            this.btnDaftar.UseVisualStyleBackColor = true;
            // 
            // btnSelesaikan
            // 
            this.btnSelesaikan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelesaikan.Location = new System.Drawing.Point(407, 504);
            this.btnSelesaikan.Name = "btnSelesaikan";
            this.btnSelesaikan.Size = new System.Drawing.Size(180, 40);
            this.btnSelesaikan.TabIndex = 6;
            this.btnSelesaikan.Text = "Selesaikan Antrean";
            this.btnSelesaikan.UseVisualStyleBackColor = true;
            // 
            // btnBatalkan
            // 
            this.btnBatalkan.Location = new System.Drawing.Point(224, 458);
            this.btnBatalkan.Name = "btnBatalkan";
            this.btnBatalkan.Size = new System.Drawing.Size(180, 40);
            this.btnBatalkan.TabIndex = 7;
            this.btnBatalkan.Text = "Batalkan Antrean";
            this.btnBatalkan.UseVisualStyleBackColor = true;
            // 
            // FrmAntrean
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 643);
            this.Controls.Add(this.btnSelesaikan);
            this.Controls.Add(this.btnBatalkan);
            this.Controls.Add(this.btnDaftar);
            this.Controls.Add(this.cmbPoli);
            this.Controls.Add(this.lblPoli);
            this.Controls.Add(this.cmbPasien);
            this.Controls.Add(this.lblPasien);
            this.Controls.Add(this.lblDetailKursi);
            this.Controls.Add(this.tlpGridKursi);
            this.Name = "FrmAntrean";
            this.Text = "FrmAntrean";
            this.Load += new System.EventHandler(this.FrmAntrean_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpGridKursi;
        private System.Windows.Forms.Label lblDetailKursi;
        private System.Windows.Forms.Label lblPasien;
        private System.Windows.Forms.ComboBox cmbPasien;
        private System.Windows.Forms.Label lblPoli;
        private System.Windows.Forms.ComboBox cmbPoli;
        private System.Windows.Forms.Button btnDaftar;
        private System.Windows.Forms.Button btnSelesaikan;
        private System.Windows.Forms.Button btnBatalkan;
    }
}