using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace parser_OZON_webview
{

    public partial class Form1 : Form
    {
        const string URL_WORKER = "https://myqu.ru/_turistore/worker.php";
        const string URL_FEED = "https://myqu.ru/_turistore/feed.xml";
        const string URL_XML = "https://www.turistore.ru/marketplace/4241103.xml";
        const string URL_OZON = "https://www.ozon.ru/product/";
        const string URL_ENDPOINT = "https://myqu.ru/_turistore/worker_ozon.php";
        const string COOKIES = "__Secure-ETC=5f7254d476bad14654f34a9db4926b84; abt_data=7.PiM4AFbZG4NUzgayVZ06Drd9UajRWBJ0b-JCjj1WtQ_aHR3HMoDE_f_P8NF0JfkSuYClNCRjqHh90Qwi43s_OVApILmfiSZ77BSNUm79FJd_ytntS-xgBTxRqgDckfhzQBWQ2Piq0X5ZEe-cgl-BGfjdO5IpqC6T4voIC4ezuwcPonM9U7vhcSm_AEXKQmfoW6LePe0mJjzBcVTe1PjAs1u67UWiuvhKTgQTYHKTWqtwZWIy3x982CsN7oNnW1iQvDCmJjMC9Hi3dBmlrcvQCl46h_NERkOW4q71aIvGcUTYfR-txnqaZ14iUtiSJkTtIerI8UAZPJxeKtjcjwssXYZ2JGTUcxOsjLVA4MWuQk7qe8PqIJVUDnUfBXbwQA";

        private static readonly HttpClient httpClient = new HttpClient();

        int flagSTATUS;
        List<string> articles;
        List<Item> items = new List<Item>();


        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            articles = [];
            textBox1.Text = URL_XML;
        }

        private void InitializeTimer()
        {
            timer1.Interval = 3600000; // 7200000 2 часа в миллисекундах
            timer1.Stop();
            timer1.Enabled = false;
        }


        private async void StartParser()
        {
            flagSTATUS = 1;
            dataGridView1.Rows.Clear();
            articles = [];
            items = [];
            //if (InvokeRequired) this.Invoke(new Action(() => btnStartParse.Enabled = false)); else btnStartParse.Enabled = false;
            //if (InvokeRequired) this.Invoke(new Action(() => groupBox1.Text = "История действий")); else groupBox1.Text = "История действий";
            btnStartParse.Enabled = false;
            groupBox1.Text = "История действий";


            AddHistoryText("Инициализация запроса ..");
            AddHistoryText("Выполнение запроса ..");
            LoadArticles();
            AddHistoryText("Запрос выполнен.");
            var c = 0;
            for (var i = 0; i < articles.Count; i++)
                if (articles[i] != "") c++;
            AddHistoryText($"Найдено записей: {articles.Count}");
            AddHistoryText($"Найдено артикулов: {c}");

            // парсинг
            AddHistoryText($"Идет парсинг ..");
            for (var i = 0; i < articles.Count; i++)
            {
                //if (i == 10) break;
                //Application.DoEvents();
                Item item = new();
                groupBox2.Text = $"Результат парсинга ({i + 1} из {articles.Count}):";

                if (articles[i] != "")
                {
                    flagSTATUS = 1;
                    List<string> prices = await GetPrices(articles[i]);

                    if (flagSTATUS == -1)
                    {
                        item.Article = articles[i];
                        item.Price_card = "";
                        item.Price = "";
                        item.Price_old = "";
                        item.Article_found = "false";
                        items.Add(item);
                        dataGridView1.Rows.Add(i + 1, articles[i], "Такой страницы не существует");
                        continue;
                    }

                    if (prices != null)
                    {
                        item.Article = articles[i];
                        item.Price_card = prices[0];
                        item.Price = prices[1];
                        item.Price_old = prices[2];
                        item.Article_found = "true";
                    }
                    else
                    {
                        item.Article = articles[i];
                        item.Price_card = "";
                        item.Price = "";
                        item.Price_old = "";
                        item.Article_found = "false";
                    }
                }

                items.Add(item);

                if (articles[i] != "")
                    if (item.Article_found == "true")
                        dataGridView1.Rows.Add(i + 1, articles[i], item.Price_card, item.Price, item.Price_old);
                    else
                        dataGridView1.Rows.Add(i + 1, articles[i], "нет цен");
                else
                    dataGridView1.Rows.Add(i + 1, "нет артикула");

            }

            AddHistoryText("Парсинг завершен.");
            AddHistoryText("Сохранение данных ..");
            PostJsonToEndPoint(); // отправляем данные на worker_ozon
            AddHistoryText("Данные сохранены.");

            AddHistoryText("Запуск обработчика feed.xml ..");
            if ( RunWorker() == true )
                AddHistoryText("Фид feed.xml успешно изменен.");
            else
                AddHistoryText("Ошибка работы worker!");


            // завершение парсинга
            btnStartParse.Enabled = true;
            switch (flagSTATUS)
            {
                case -1:
                    label1.Text = DateTime.Now + " ВНИМАНИЕ! Парсинг не выполнен. Произошла ошибка.";
                    label1.BackColor = Color.IndianRed;
                    label1.ForeColor = Color.White;
                    break;
                case 0:
                    label1.Text = DateTime.Now + " что-то пошло не так ..";
                    label1.BackColor = Color.Yellow;
                    label1.ForeColor = Color.Black;
                    break;
                case 1:
                    label1.Text = " Последнее сканирование: " + DateTime.Now;
                    label1.BackColor = Color.DarkGreen;
                    label1.ForeColor = Color.White;
                    break;
            }

            AddHistoryText("### ЗАВЕРШЕНО ###");
            GC.Collect();

        }


        private async Task<List<string>> GetPrices(string article)
        {
            //article = "1746727978";
            string url = $"{URL_OZON}{article}"; // URL товара
            string response = "";

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            //httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Cookie", COOKIES);

            try
            {
                response = await httpClient.GetStringAsync(url);
            }
            catch
            {
                //Debug.WriteLine(article);
                //MessageBox.Show("fghk");
            }

            List<string> values = [];

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();

            htmlDocument.LoadHtml(response);

            // Находим элемент <div data-widget="webPrice">
            var priceContainer = htmlDocument.DocumentNode.SelectSingleNode("//div[@data-widget='webPrice']");
            if (priceContainer != null)
            {
                // Находим дочерний <div>
                var innerDiv = priceContainer.SelectSingleNode(".//div");
                // Находим коллекцию следующих <div>
                var divs = innerDiv.SelectNodes("./div");

                if (divs[0].SelectSingleNode(".//button") != null)
                    values.Add(ExtractDigits(divs[0].InnerText));
                else
                    values.Add("");

                if (divs.Count > 1)
                {
                    var spanNodes = divs[1].SelectNodes(".//span");
                    if (spanNodes[0] != null)
                        values.Add(ExtractDigits(spanNodes[0].InnerText));
                    else
                        values.Add("");

                    if (spanNodes[1] != null)
                        values.Add(ExtractDigits(spanNodes[1].InnerText));
                    else
                        values.Add("");
                }
                else
                {
                    values.Add("");
                    values.Add("");
                }

                // тут проверяем если все пустые (то есть button не нашел), то первый div
                if (values[0] == "" && values[1] == "" && values[2] == "")
                    values[1] = ExtractDigits(divs[0].InnerText);

                return values;
            }
            else
            {
                flagSTATUS = -1;
                return null;
            }
        }

        private void LoadArticles()
        {
            httpClient.DefaultRequestHeaders.Clear();

            try
            {
                var response = httpClient.GetStringAsync(URL_XML).Result;
                XDocument xdoc = XDocument.Parse(response);

                foreach (var offerElement in xdoc.Descendants("offer"))
                {
                    var articleOzon = offerElement.Element("article_ozon")?.Value;
                    if (articleOzon != null || articleOzon != "") articles.Add(articleOzon);
                }
            }
            catch (Exception ex)
            {
                AddHistoryText($"Ошибка загрузки данных: {ex.Message}");
                flagSTATUS = 0;
            }
        }

        private void PostJsonToEndPoint() // работает с списком items
        {
            // URL для отправки данных
            string url = URL_ENDPOINT;

            // Отправка POST-запроса
            using (HttpClient client = new HttpClient())
            {
                // Преобразование в JSON
                string json = JsonSerializer.Serialize(items);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Выполнение POST-запроса
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // Обработка ответа
                flagSTATUS = 1;
                string responseBody;
                if (response.IsSuccessStatusCode)
                    responseBody = response.Content.ReadAsStringAsync().Result;
                else
                    flagSTATUS = -1;
            }
        }

        private bool RunWorker()
        {
            string url = URL_WORKER;

            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                var r = httpClient.GetAsync(url).GetAwaiter().GetResult();
                r.EnsureSuccessStatusCode(); // выбросит исключение, если код ответа не 2xx
                var response = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if ( response.Contains("true") ) return true; else return false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Ошибка: {ex.Message}");
                return false;
            }

        }

        private void btnStartParse_Click(object sender, EventArgs e)
        {
            StartParser();
            timer1.Enabled = true;
            timer1.Start();
        }

        public void AddHistoryText(string text)
        {
            this.txtHistoryText.Invoke((MethodInvoker)delegate
            {
                txtHistoryText.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " + text + Environment.NewLine);
            });
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate(); // Активировать форму
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide(); // Скрываем форму
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public string ExtractDigits(string input)
        {
            string digitsOnly = "";
            foreach (char c in input)
                if (char.IsDigit(c))
                    digitsOnly += c;
            return digitsOnly;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartParser();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Закрыть парсер OZON ?",
                                           "Подтверждение закрытия",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);
            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var telegramUrl = "tg://resolve?domain=cmacuk";
            Process.Start(new ProcessStartInfo(telegramUrl) { UseShellExecute = true });
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = URL_FEED;
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
