using System;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace MyCar
{
    public partial class Form2 : Form
    {
        private string connectionString = "Data Source=mycar.db;Version=3;";

        public Form2()
        {
            InitializeComponent();
            LoadExchangeRateAsync();
        }

        private async void LoadExchangeRateAsync()
        {
            string url = "https://api.nbrb.by/exrates/rates/431";
            string filePath = "cur.txt";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Проверяет, что HTTP-ответ успешен

                    string json = await response.Content.ReadAsStringAsync();

                    // Разбор JSON
                    JObject jsonObj = JObject.Parse(json);
                    double rate = jsonObj["Cur_OfficialRate"].Value<double>();

                    // Запись значения в файл
                    File.WriteAllText(filePath, rate.ToString("0.00"));

                    Console.WriteLine("Курс успешно сохранен в файл cur.txt");
                    label6.Text = "Текущий курс НБ: "+ rate.ToString();
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {httpEx.Message}");
            }
            catch (TaskCanceledException tcEx)
            {
                MessageBox.Show($"Запрос отменен: {tcEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO mycar (mileage, price_blr, price_usd, Description, Category) VALUES (@mileage, @price_blr, @price_usd, @description, @category)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@mileage", mileageTextBox.Text);
                        cmd.Parameters.AddWithValue("@price_blr", priceBlrTextBox.Text);
                        cmd.Parameters.AddWithValue("@price_usd", priceUsdTextBox.Text);
                        cmd.Parameters.AddWithValue("@description", descriptionTextBox.Text);
                        cmd.Parameters.AddWithValue("@category", categoryTextBox.Text);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Данные успешно сохранены");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
                }
            }
        }

        private void priceBlrTextBox_TextChanged(object sender, EventArgs e)
        {
            double kurs = 3.12;
            double priceBlr;
            if (double.TryParse(priceBlrTextBox.Text, out priceBlr))
            {
                double priceUsd = priceBlr / kurs;
                priceUsdTextBox.Text = priceUsd.ToString("0.00");
            }
            else
            {
                priceUsdTextBox.Text = "";
            }
        }
    }
}