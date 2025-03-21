using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq; // Убедитесь, что установили Newtonsoft.Json через NuGet

namespace MyCar
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            try
            {
                apiServerTextBox.Text = File.ReadAllText("apiServer.data");
                apiCarIdTextBox.Text = File.ReadAllText("apiCarId.data");
            }
            catch
            {

            }
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText("apiServer.data", apiServerTextBox.Text.Trim());
            File.WriteAllText("apiCarId.data", apiCarIdTextBox.Text.Trim());
            string serverUrl = apiServerTextBox.Text.Trim();  // URL сервера
            string tableName = apiCarIdTextBox.Text.Trim();   // Название таблицы

            if (string.IsNullOrEmpty(serverUrl))
            {
                MessageBox.Show("Введите адрес сервера!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string checkUrl = $"{serverUrl.TrimEnd('/')}/check.php";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(checkUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonResponse);

                    if (json["status"]?.ToString() == "success")
                    {
                        MessageBox.Show(json["message"]?.ToString(), "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.BackColor = Color.LightGreen;

                        // Если поле ApiCarID пустое — просто включаем кнопку "Добавить новую машину" и выходим
                        if (string.IsNullOrEmpty(tableName))
                        {
                            addButton.Enabled = true;
                            return;
                        }

                        // Если поле ApiCarID заполнено — проверяем таблицу
                        string tableCheckUrl = $"{serverUrl.TrimEnd('/')}/check.php?table={tableName}";
                        response = await client.GetAsync(tableCheckUrl);
                        response.EnsureSuccessStatusCode();

                        jsonResponse = await response.Content.ReadAsStringAsync();
                        json = JObject.Parse(jsonResponse);

                        bool tableExists = json["table_exists"]?.ToObject<bool>() ?? false;

                        if (tableExists)
                        {
                            MessageBox.Show("Данная машина есть в списке!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            addButton.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Такой машины нет в базе данных!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            addButton.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(json["message"]?.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.BackColor = Color.LightCoral;
                        addButton.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.BackColor = Color.LightCoral;
                addButton.Enabled = false;
            }
        }

        private async void AddButton_Click(object sender, EventArgs e)
        {
            string serverUrl = apiServerTextBox.Text.Trim();
            string tableName = apiCarIdTextBox.Text.Trim();

            if (string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Введите сервер и название таблицы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string createUrl = $"{serverUrl.TrimEnd('/')}/createNewCar.php";
            var postData = new StringContent($"table={tableName}", Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(createUrl, postData);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Проверка, начинается ли ответ с символа "<"
                    if (jsonResponse.StartsWith("<"))
                    {
                        MessageBox.Show("Получен некорректный ответ от сервера (HTML вместо JSON).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Попытка десериализовать JSON
                    JObject json = JObject.Parse(jsonResponse);

                    if (json["status"]?.ToString() == "success")
                    {
                        MessageBox.Show(json["message"]?.ToString(), "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        addButton.Visible = false; // Скрываем кнопку после создания
                    }
                    else
                    {
                        MessageBox.Show(json["message"]?.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
