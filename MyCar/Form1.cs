using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace MyCar
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=mycar.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists("mycar.db"))
            {
                SQLiteConnection.CreateFile("mycar.db");
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS mycar (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                mileage INTEGER,
                price_blr REAL,
                price_usd REAL,
                Description TEXT,
                Category TEXT
            )";
                using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadData()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT id, mileage, price_blr, price_usd, Description, Category FROM mycar";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Нет записей в базе данных");
                    }
                    else
                    {
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                }
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            using (var form = new Form2())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}