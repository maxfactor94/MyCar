﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using System.Text;
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
            // Проверяем подключение к серверу при старте приложения
            CheckServerConnection();

            LoadData(); // Загружаем данные при старте формы
        }

        private void CheckServerConnection()
        {
            string serverUrl = File.ReadAllText("apiServer.data").Trim();

            if (string.IsNullOrEmpty(serverUrl))
            {
                MessageBox.Show("Не указан сервер. Используется локальная база данных.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Попробуем установить соединение с сервером
            string testUrl = $"{serverUrl.TrimEnd('/')}/testConnection.php"; // Путь к тестовому файлу на сервере

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(testUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Подключение к серверу успешно. Можно загрузить данные с сервера.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось подключиться к серверу. Будет использована локальная база данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к серверу: {ex.Message}. Будет использована локальная база данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        }

                        // Вызываем метод для вычисления сумм после загрузки данных
                        CalculateTotalSums();
                    }
                }
                catch (Exception ex)
                {
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
            Form3 form3 = new Form3();
            form3.Show();
        }

        private async void ЗагрузитьДанныеНаСерверToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Читаем параметры из файлов
            string serverUrl = File.ReadAllText("apiServer.data").Trim();
            string tableName = File.ReadAllText("apiCarId.data").Trim();

            if (string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Не указаны настройки сервера или таблицы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Подготовка данных для отправки на сервер
            var dataToSend = new List<Dictionary<string, object>>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                var data = new Dictionary<string, object>
                {
                    { "mileage", row.Cells["mileage"].Value },
                    { "price_blr", row.Cells["price_blr"].Value },
                    { "price_usd", row.Cells["price_usd"].Value },
                    { "Description", row.Cells["Description"].Value },
                    { "Category", row.Cells["Category"].Value },
                    { "date", row.Cells["date"].Value }
                };

                dataToSend.Add(data);
            }

            // Преобразуем данные в JSON
            string jsonData = JsonConvert.SerializeObject(dataToSend);

            // Подготовим URL для отправки данных на сервер
            string updateUrl = $"{serverUrl.TrimEnd('/')}/updateToServer.php";

            // Отправляем POST-запрос
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent($"table={tableName}&data={jsonData}", Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync(updateUrl, content);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Данные успешно загружены: {responseContent}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных на сервер: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ПолучитьДанныеССервераToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Читаем параметры подключения из файлов
            string serverUrl = File.ReadAllText("apiServer.data").Trim();
            string tableName = File.ReadAllText("apiCarId.data").Trim();

            if (string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Не указаны настройки сервера или таблицы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Подготовка URL для запроса данных с сервера
            string dataUrl = $"{serverUrl.TrimEnd('/')}/getDataFromServer.php?table={tableName}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Отправляем GET-запрос на сервер
                    HttpResponseMessage response = await client.GetAsync(dataUrl);

                    // Проверяем успешность запроса
                    response.EnsureSuccessStatusCode();

                    // Читаем данные из ответа
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Десериализуем JSON в объект
                    var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonResponse);

                    if (data != null && data.Count > 0)
                    {
                        // Очистим локальную таблицу перед вставкой новых данных
                        ClearLocalDatabase();

                        // Вставляем новые данные в локальную базу данных
                        InsertDataToLocalDatabase(data);

                        // Обновляем DataGridView локальными данными
                        LoadData();

                        MessageBox.Show("Данные успешно обновлены в локальной базе данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Нет данных для загрузки.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных с сервера: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearLocalDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM mycar";  // Удаляем все записи в таблице
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при очистке локальной базы данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InsertDataToLocalDatabase(List<Dictionary<string, object>> data)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    foreach (var row in data)
                    {
                        string query = "INSERT INTO mycar (mileage, price_blr, price_usd, Description, Category, date) VALUES (@mileage, @price_blr, @price_usd, @Description, @Category, @date)";

                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            // Добавляем параметры для вставки данных
                            cmd.Parameters.AddWithValue("@mileage", row.ContainsKey("mileage") ? row["mileage"] : DBNull.Value);
                            cmd.Parameters.AddWithValue("@price_blr", row.ContainsKey("price_blr") ? row["price_blr"] : DBNull.Value);
                            cmd.Parameters.AddWithValue("@price_usd", row.ContainsKey("price_usd") ? row["price_usd"] : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Description", row.ContainsKey("Description") ? row["Description"] : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Category", row.ContainsKey("Category") ? row["Category"] : DBNull.Value);
                            cmd.Parameters.AddWithValue("@date", row.ContainsKey("date") ? row["date"] : DBNull.Value);

                            // Выполняем запрос на вставку данных
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при вставке данных в локальную базу данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
