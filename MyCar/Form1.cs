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
            InitializeDataGridView();
            InitializeDatabase();
            LoadCategories(); // Загрузите категории в ComboBox
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData(); // Загружаем данные при старте формы
        }

        private void InitializeDatabase()
        {
            if (!File.Exists("mycar.db"))
            {
                SQLiteConnection.CreateFile("mycar.db");
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string createTableQuery = @"
                        CREATE TABLE mycar (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            mileage INTEGER,
                            price_blr REAL,
                            price_usd REAL,
                            Description TEXT,
                            Category TEXT,
                            date DATETIME
                        )";
                    using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void LoadData(string category = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT id, mileage, price_blr, price_usd, Description, Category, date FROM mycar";
                    if (!string.IsNullOrEmpty(category) && category != "Все")
                    {
                        query += " WHERE Category = @category";
                    }

                    query += " ORDER BY mileage DESC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(category) && category != "Все")
                        {
                            cmd.Parameters.AddWithValue("@category", category);
                        }
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Нет записей в базе данных");
                        }
                        else
                        {
                            dataGridView1.DataSource = dt;
                            // Установка ширины столбца id
                            dataGridView1.Columns["id"].Width = 30;
                            dataGridView1.Columns["mileage"].Width = 50;
                            dataGridView1.Columns["price_blr"].Width = 55;
                            dataGridView1.Columns["price_usd"].Width = 55;
                            //dataGridView1.Columns["Category"].Width = 100;
                            //dataGridView1.Columns["date"].Width = 80;
                        }

                        // Вызываем метод для вычисления сумм после загрузки данных
                        CalculateTotalSums();
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                    Console.WriteLine("Ошибка при загрузке данных: " + ex.Message);
                }
            }
        }

        private void CalculateTotalSums()
        {
            decimal totalBlr = 0;
            decimal totalUsd = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["price_blr"].Value != DBNull.Value)
                {
                    totalBlr += Convert.ToDecimal(row.Cells["price_blr"].Value);
                }
                if (row.Cells["price_usd"].Value != DBNull.Value)
                {
                    totalUsd += Convert.ToDecimal(row.Cells["price_usd"].Value);
                }
            }

            label1.Text = $"Всего: {totalBlr:0.00} BYN ({totalUsd:0.00} USD)";
        }

        private void LoadCategories()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT DISTINCT Category FROM mycar";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        categoryTextBox.Items.Clear();
                        categoryTextBox.Items.Add("Все"); // Добавляем опцию для отображения всех категорий
                        while (reader.Read())
                        {
                            categoryTextBox.Items.Add(reader["Category"].ToString());
                        }
                        categoryTextBox.SelectedIndex = 0; // Устанавливаем "Все" по умолчанию
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке категорий: " + ex.Message);
                }
            }
        }

        private void categoryTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = categoryTextBox.SelectedItem.ToString();
            if (selectedCategory == "Все")
            {
                LoadData();
            }
            else
            {
                LoadData(selectedCategory);
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            using (var form = new Form2())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData(); // Обновляем данные после создания новой записи
                }
            }
        }

        private void InitializeDataGridView()
        {
            // Установите свойства DataGridView для заполнения всего окна
            dataGridView1.Dock = DockStyle.Fill;
            // Автоматическая настройка ширины колонок по заполнению
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Автоматическая настройка высоты строк
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false; // Запретить изменение порядка столбцов
        }

        private void РедактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int id = Convert.ToInt32(selectedRow.Cells["id"].Value);

                using (var form = new Form2(id))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData(); // Обновляем данные после редактирования записи
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите ячейку для редактирования");
            }
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void НастройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}