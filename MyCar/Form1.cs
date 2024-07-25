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
            //LoadCategories(); // Загрузите категории в ComboBox
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
                    if (!string.IsNullOrEmpty(category))
                    {
                        query += " WHERE Category = @category";
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(category))
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                }
            }
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
                        while (reader.Read())
                        {
                            categoryTextBox.Items.Add(reader["Category"].ToString());
                        }
                        categoryTextBox.Items.Insert(0, "Все"); // Добавляем опцию для отображения всех категорий
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
    }
}