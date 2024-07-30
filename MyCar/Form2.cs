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
        private int? recordId = null;

        public Form2(int? id = null)
        {
            InitializeComponent();
            LoadExchangeRateAsync();
            dateTimePicker.Value = DateTime.Now;

            if (id.HasValue)
            {
                recordId = id;
                LoadRecord(id.Value);
            }
        }

        private void LoadRecord(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM mycar WHERE id = @id";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            mileageTextBox.Text = reader["mileage"].ToString();
                            priceBlrTextBox.Text = reader["price_blr"].ToString();
                            priceUsdTextBox.Text = reader["price_usd"].ToString();
                            descriptionTextBox.Text = reader["Description"].ToString();
                            categoryTextBox.Text = reader["Category"].ToString();
                            dateTimePicker.Value = Convert.ToDateTime(reader["date"]);
                        }
                    }
                }
            }
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
                    File.WriteAllText(filePath, rate.ToString("0.0000"));

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
            if (categoryTextBox.Text == "")
            {
                MessageBox.Show("Заполните поле Категория");
            }
            else if (mileageTextBox.Text == "")
            {
                MessageBox.Show("Заполните поле Пробег");
            }
            else if (descriptionTextBox.Text == "")
            {
                MessageBox.Show("Заполните поле Описание");
            }
            else if (priceBlrTextBox.Text == "")
            {
                MessageBox.Show("Заполните поле Цена BYN");
            }
            else if (priceUsdTextBox.Text == "")
            {
                MessageBox.Show("Заполните поле Цена USD");
            }
            else
            {
                priceBlrTextBox_TextChanged(sender, e);
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query;

                        if (recordId.HasValue)
                        {
                            query = "UPDATE mycar SET mileage = @mileage, price_blr = @price_blr, price_usd = @price_usd, Description = @description, Category = @category, date = @date WHERE id = @id";
                        }
                        else
                        {
                            query = "INSERT INTO mycar (mileage, price_blr, price_usd, Description, Category, date) VALUES (@mileage, @price_blr, @price_usd, @description, @category, @date)";
                        }

                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@mileage", mileageTextBox.Text);
                            cmd.Parameters.AddWithValue("@price_blr", priceBlrTextBox.Text);
                            cmd.Parameters.AddWithValue("@price_usd", priceUsdTextBox.Text);
                            cmd.Parameters.AddWithValue("@description", descriptionTextBox.Text);
                            cmd.Parameters.AddWithValue("@category", categoryTextBox.Text);
                            cmd.Parameters.AddWithValue("@date", dateTimePicker.Value);

                            if (recordId.HasValue)
                            {
                                cmd.Parameters.AddWithValue("@id", recordId.Value);
                            }

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
        }

        private void priceBlrTextBox_TextChanged(object sender, EventArgs e)
        {
            double kurs = Convert.ToDouble(File.ReadAllText("cur.txt"));
            double priceBlr;
            if (double.TryParse(priceBlrTextBox.Text, out priceBlr))
            {
                double priceUsd = priceBlr / kurs;
                priceUsdTextBox.Text = priceUsd.ToString("0.0000");
            }
            else
            {
                priceUsdTextBox.Text = "";
            }
        }
    }
}