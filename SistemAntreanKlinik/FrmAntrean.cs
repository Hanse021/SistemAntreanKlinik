
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SistemAntreanKlinik
{
    public partial class FrmAntrean : Form
    {
        // DATA KURSI DAN ANTREAN YANG DIPILIH
        private int? selectedSeatId = null;
        private int? selectedQueueTicketId = null;

        public FrmAntrean()
        {
            InitializeComponent();

            this.Load += FrmAntrean_Load;

            btnDaftar.Click += btnDaftar_Click;
            btnSelesaikan.Click += btnSelesaikan_Click;
            btnBatalkan.Click += btnBatalkan_Click;

            btnDaftar.Enabled = false;
            btnSelesaikan.Enabled = false;
            btnBatalkan.Enabled = false;
        }

        // ==========================================
        // 1. FORM LOAD
        // ==========================================

        private void FrmAntrean_Load(object sender, EventArgs e)
        {
            SetupGrid();
            LoadPatients();
            LoadPolyclinics();
            LoadSeats();
        }

        // ==========================================
        // 2. SETUP GRID KURSI
        // ==========================================

        private void SetupGrid()
        {
            tlpGridKursi.SuspendLayout();

            tlpGridKursi.RowCount = 3;
            tlpGridKursi.ColumnCount = 4;

            tlpGridKursi.RowStyles.Clear();
            tlpGridKursi.ColumnStyles.Clear();

            tlpGridKursi.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            for (int i = 0; i < 3; i++)
            {
                tlpGridKursi.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100F / 3)
                );
            }

            for (int i = 0; i < 4; i++)
            {
                tlpGridKursi.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 25F)
                );
            }

            tlpGridKursi.ResumeLayout();
        }

        // ==========================================
        // 3. LOAD DATA PASIEN
        // ==========================================

        private void LoadPatients()
        {
            string query = @"
                SELECT
                    PatientId,
                    FullName + N' (' + MedicalRecordNo + N')' AS DisplayName
                FROM dbo.Patients
                WHERE IsActive = 1
                ORDER BY FullName;
            ";

            try
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    conn.Open();

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        cmbPasien.DataSource = dt;
                        cmbPasien.DisplayMember = "DisplayName";
                        cmbPasien.ValueMember = "PatientId";
                        cmbPasien.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat pasien:\n" + ex.Message
                );
            }
        }

        // ==========================================
        // 4. LOAD DATA POLI
        // ==========================================

        private void LoadPolyclinics()
        {
            string query = @"
                SELECT PolyclinicId, PolyName
                FROM dbo.Polyclinics
                WHERE IsActive = 1
                ORDER BY PolyName;
            ";

            try
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    conn.Open();

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        cmbPoli.DataSource = dt;
                        cmbPoli.DisplayMember = "PolyName";
                        cmbPoli.ValueMember = "PolyclinicId";
                        cmbPoli.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat poli:\n" + ex.Message
                );
            }
        }

        // ==========================================
        // 5. RESET PILIHAN
        // ==========================================

        private void ResetSelection()
        {
            selectedSeatId = null;
            selectedQueueTicketId = null;

            btnDaftar.Enabled = false;
            btnSelesaikan.Enabled = false;
            btnBatalkan.Enabled = false;
        }

        // ==========================================
        // 6. LOAD DATA KURSI
        // ==========================================

        private void LoadSeats()
        {
            ResetSelection();

            while (tlpGridKursi.Controls.Count > 0)
            {
                Control control = tlpGridKursi.Controls[0];

                tlpGridKursi.Controls.RemoveAt(0);

                control.Dispose();
            }

            string query = @"
                SELECT
                    s.SeatId,
                    s.SeatCode,
                    s.GridRow,
                    s.GridColumn,
                    qt.QueueTicketId,

                    CASE
                        WHEN sa.AssignmentId IS NULL
                        THEN CAST(0 AS BIT)
                        ELSE CAST(1 AS BIT)
                    END AS IsOccupied,

                    qt.QueueNumber,
                    p.FullName,
                    pc.PolyName

                FROM dbo.Seats s

                LEFT JOIN dbo.SeatAssignments sa
                    ON sa.SeatId = s.SeatId
                    AND sa.ReleasedAt IS NULL

                LEFT JOIN dbo.QueueTickets qt
                    ON qt.QueueTicketId = sa.QueueTicketId

                LEFT JOIN dbo.Patients p
                    ON p.PatientId = qt.PatientId

                LEFT JOIN dbo.Polyclinics pc
                    ON pc.PolyclinicId = qt.PolyclinicId

                WHERE s.IsActive = 1

                ORDER BY s.GridRow, s.GridColumn;
            ";

            try
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SeatInfo seat = new SeatInfo()
                            {
                                SeatId = Convert.ToInt32(reader["SeatId"]),
                                SeatCode = Convert.ToString(reader["SeatCode"]),

                                GridRow = Convert.ToInt32(reader["GridRow"]),
                                GridColumn = Convert.ToInt32(reader["GridColumn"]),

                                QueueTicketId =
                                    reader["QueueTicketId"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["QueueTicketId"]),

                                IsOccupied = Convert.ToBoolean(
                                    reader["IsOccupied"]
                                ),

                                QueueNumber =
                                    reader["QueueNumber"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["QueueNumber"]),

                                PatientName =
                                    reader["FullName"] == DBNull.Value
                                    ? "-"
                                    : Convert.ToString(reader["FullName"]),

                                PolyName =
                                    reader["PolyName"] == DBNull.Value
                                    ? "-"
                                    : Convert.ToString(reader["PolyName"])
                            };

                            Button btn = new Button();

                            btn.Name = "btn" + seat.SeatCode;

                            btn.Text = seat.SeatCode +
                                (seat.IsOccupied
                                    ? "\nTERISI\nAntrean " + seat.QueueNumber
                                    : "\nKOSONG");

                            btn.Dock = DockStyle.Fill;
                            btn.Margin = new Padding(8);

                            btn.Font = new Font(
                                "Segoe UI",
                                10,
                                FontStyle.Bold
                            );

                            btn.FlatStyle = FlatStyle.Flat;
                            btn.UseVisualStyleBackColor = false;

                            btn.Tag = seat;

                            btn.BackColor = seat.IsOccupied
                                ? Color.LightCoral
                                : Color.LightGreen;

                            btn.Click += SeatButton_Click;

                            tlpGridKursi.Controls.Add(
                                btn,
                                seat.GridColumn - 1,
                                seat.GridRow - 1
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat kursi:\n" + ex.Message
                );
            }
        }

        // ==========================================
        // 7. EVENT KLIK KURSI
        // ==========================================

        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            SeatInfo seat = (SeatInfo)btn.Tag;

            if (seat.IsOccupied)
            {
                selectedSeatId = null;
                selectedQueueTicketId = seat.QueueTicketId;

                lblDetailKursi.Text =
                    "Kursi: " + seat.SeatCode +
                    "\nStatus: Terisi" +
                    "\nNomor Antrean: " + seat.QueueNumber +
                    "\nNama Pasien: " + seat.PatientName +
                    "\nPoli: " + seat.PolyName;
            }
            else
            {
                selectedSeatId = seat.SeatId;
                selectedQueueTicketId = null;

                lblDetailKursi.Text =
                    "Kursi: " + seat.SeatCode +
                    "\nStatus: Kosong" +
                    "\nKursi dipilih untuk pendaftaran.";
            }

            btnDaftar.Enabled = selectedSeatId.HasValue;

            btnSelesaikan.Enabled =
                selectedQueueTicketId.HasValue;

            btnBatalkan.Enabled =
                selectedQueueTicketId.HasValue;

            UpdateSeatButtonColors();
        }

        // ==========================================
        // 8. UPDATE WARNA KURSI
        // ==========================================

        private void UpdateSeatButtonColors()
        {
            foreach (Control control in tlpGridKursi.Controls)
            {
                Button btn = control as Button;

                if (btn == null)
                    continue;

                SeatInfo seat = btn.Tag as SeatInfo;

                if (seat == null)
                    continue;

                if (seat.IsOccupied)
                {
                    btn.BackColor = Color.LightCoral;
                }
                else if (selectedSeatId == seat.SeatId)
                {
                    btn.BackColor = Color.LightBlue;
                }
                else
                {
                    btn.BackColor = Color.LightGreen;
                }
            }
        }

        // ==========================================
        // 9. PENDAFTARAN ANTREAN
        // ==========================================

        private void btnDaftar_Click(object sender, EventArgs e)
        {
            if (cmbPasien.SelectedIndex < 0)
            {
                MessageBox.Show("Silakan pilih pasien.");
                return;
            }

            if (cmbPoli.SelectedIndex < 0)
            {
                MessageBox.Show("Silakan pilih poli.");
                return;
            }

            if (!selectedSeatId.HasValue)
            {
                MessageBox.Show("Silakan pilih kursi kosong.");
                return;
            }

            int patientId = Convert.ToInt32(cmbPasien.SelectedValue);
            int polyclinicId = Convert.ToInt32(cmbPoli.SelectedValue);
            int seatId = selectedSeatId.Value;

            DateTime queueDate = DateTime.Today;

            int queueNumber = 0;

            try
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction transaction =
                        conn.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            // PERIKSA KETERSEDIAAN KURSI

                            string checkSeatQuery = @"
                                SELECT COUNT(*)
                                FROM dbo.Seats s WITH (UPDLOCK, HOLDLOCK)
                                WHERE s.SeatId = @SeatId
                                  AND s.IsActive = 1
                                  AND NOT EXISTS (
                                      SELECT 1
                                      FROM dbo.SeatAssignments sa
                                          WITH (UPDLOCK, HOLDLOCK)
                                      WHERE sa.SeatId = s.SeatId
                                        AND sa.ReleasedAt IS NULL
                                  );
                            ";

                            using (SqlCommand cmd = new SqlCommand(
                                checkSeatQuery, conn, transaction))
                            {
                                cmd.Parameters.Add(
                                    "@SeatId", SqlDbType.Int
                                ).Value = seatId;

                                int available = Convert.ToInt32(
                                    cmd.ExecuteScalar()
                                );

                                if (available == 0)
                                {
                                    throw new Exception(
                                        "Kursi sudah tidak tersedia."
                                    );
                                }
                            }

                            // GENERATE NOMOR ANTREAN

                            string queueQuery = @"
                                SELECT ISNULL(MAX(QueueNumber), 0) + 1
                                FROM dbo.QueueTickets
                                    WITH (UPDLOCK, HOLDLOCK)
                                WHERE PolyclinicId = @PolyclinicId
                                  AND QueueDate = @QueueDate;
                            ";

                            using (SqlCommand cmd = new SqlCommand(
                                queueQuery, conn, transaction))
                            {
                                cmd.Parameters.Add(
                                    "@PolyclinicId", SqlDbType.Int
                                ).Value = polyclinicId;

                                cmd.Parameters.Add(
                                    "@QueueDate", SqlDbType.Date
                                ).Value = queueDate;

                                queueNumber = Convert.ToInt32(
                                    cmd.ExecuteScalar()
                                );
                            }

                            // INSERT ANTREAN

                            string insertQueueQuery = @"
                                INSERT INTO dbo.QueueTickets
                                (
                                    PatientId,
                                    PolyclinicId,
                                    QueueDate,
                                    QueueNumber,
                                    Status
                                )
                                OUTPUT INSERTED.QueueTicketId
                                VALUES
                                (
                                    @PatientId,
                                    @PolyclinicId,
                                    @QueueDate,
                                    @QueueNumber,
                                    'Waiting'
                                );
                            ";

                            int queueTicketId;

                            using (SqlCommand cmd = new SqlCommand(
                                insertQueueQuery, conn, transaction))
                            {
                                cmd.Parameters.Add(
                                    "@PatientId", SqlDbType.Int
                                ).Value = patientId;

                                cmd.Parameters.Add(
                                    "@PolyclinicId", SqlDbType.Int
                                ).Value = polyclinicId;

                                cmd.Parameters.Add(
                                    "@QueueDate", SqlDbType.Date
                                ).Value = queueDate;

                                cmd.Parameters.Add(
                                    "@QueueNumber", SqlDbType.Int
                                ).Value = queueNumber;

                                queueTicketId = Convert.ToInt32(
                                    cmd.ExecuteScalar()
                                );
                            }

                            // INSERT PENEMPATAN KURSI

                            string insertSeatQuery = @"
                                INSERT INTO dbo.SeatAssignments
                                (
                                    QueueTicketId,
                                    SeatId
                                )
                                VALUES
                                (
                                    @QueueTicketId,
                                    @SeatId
                                );
                            ";

                            using (SqlCommand cmd = new SqlCommand(
                                insertSeatQuery, conn, transaction))
                            {
                                cmd.Parameters.Add(
                                    "@QueueTicketId", SqlDbType.Int
                                ).Value = queueTicketId;

                                cmd.Parameters.Add(
                                    "@SeatId", SqlDbType.Int
                                ).Value = seatId;

                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch
                            {
                                // Transaksi mungkin sudah selesai.
                            }

                            throw;
                        }
                    }
                }

                ResetSelection();
                LoadSeats();

                lblDetailKursi.Text =
                    "Pendaftaran antrean berhasil!";

                MessageBox.Show(
                    "Pendaftaran berhasil!\n" +
                    "Nomor Antrean: " + queueNumber,
                    "Berhasil",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (SqlException ex)
            {
                ResetSelection();
                LoadSeats();

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    MessageBox.Show(
                        "Kursi atau nomor antrean sudah digunakan."
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Kesalahan database:\n" + ex.Message
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Pendaftaran gagal:\n" + ex.Message
                );
            }
        }

        // ==========================================
        // 10. SELESAIKAN / BATALKAN ANTREAN
        // ==========================================

        private void ChangeQueueStatus(string newStatus)
        {
            if (!selectedQueueTicketId.HasValue)
            {
                MessageBox.Show("Pilih kursi yang terisi.");
                return;
            }

            if (newStatus == "Cancelled")
            {
                DialogResult result = MessageBox.Show(
                    "Yakin ingin membatalkan antrean ini?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    return;
            }

            int queueTicketId = selectedQueueTicketId.Value;

            try
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction transaction =
                        conn.BeginTransaction())
                    {
                        try
                        {
                            // UPDATE STATUS ANTREAN

                            string updateQueueQuery = @"
                                UPDATE dbo.QueueTickets
                                SET Status = @Status
                                WHERE QueueTicketId = @QueueTicketId
                                  AND Status IN ('Waiting', 'InService');
                            ";

                            using (SqlCommand cmd = new SqlCommand(
                                updateQueueQuery, conn, transaction))
                            {
                                cmd.Parameters.Add(
                                    "@Status", SqlDbType.VarChar, 15
                                ).Value = newStatus;

                                cmd.Parameters.Add(
                                    "@QueueTicketId", SqlDbType.Int
                                ).Value = queueTicketId;

                                if (cmd.ExecuteNonQuery() != 1)
                                {
                                    throw new Exception(
                                        "Antrean sudah diproses atau tidak tersedia."
                                    );
                                }
                            }

                            // LEPASKAN KURSI

                            string releaseSeatQuery = @"
                                UPDATE dbo.SeatAssignments
                                SET ReleasedAt = SYSDATETIME()
                                WHERE QueueTicketId = @QueueTicketId
                                  AND ReleasedAt IS NULL;
                            ";

                            using (SqlCommand cmd = new SqlCommand(
                                releaseSeatQuery, conn, transaction))
                            {
                                cmd.Parameters.Add(
                                    "@QueueTicketId", SqlDbType.Int
                                ).Value = queueTicketId;

                                if (cmd.ExecuteNonQuery() != 1)
                                {
                                    throw new Exception(
                                        "Penempatan kursi aktif tidak ditemukan."
                                    );
                                }
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch
                            {
                                // Transaksi mungkin sudah selesai.
                            }

                            throw;
                        }
                    }
                }

                ResetSelection();
                LoadSeats();

                lblDetailKursi.Text =
                    "Antrean berhasil diproses.";

                MessageBox.Show(
                    newStatus == "Completed"
                        ? "Antrean berhasil diselesaikan."
                        : "Antrean berhasil dibatalkan.",
                    "Berhasil",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memproses antrean:\n" + ex.Message
                );
            }
        }

        private void btnSelesaikan_Click(object sender, EventArgs e)
        {
            ChangeQueueStatus("Completed");
        }

        private void btnBatalkan_Click(object sender, EventArgs e)
        {
            ChangeQueueStatus("Cancelled");
        }

        // ==========================================
        // 11. EVENT LABEL
        // ==========================================

        private void lblDetailKursi_Click(object sender, EventArgs e)
        {
            // Tidak ada aksi ketika label diklik.
        }

        // ==========================================
        // 12. STRUKTUR DATA KURSI
        // ==========================================

        private class SeatInfo
        {
            public int SeatId { get; set; }
            public int? QueueTicketId { get; set; }

            public string SeatCode { get; set; }

            public int GridRow { get; set; }
            public int GridColumn { get; set; }

            public bool IsOccupied { get; set; }

            public int? QueueNumber { get; set; }

            public string PatientName { get; set; }
            public string PolyName { get; set; }
        }
    }
}
