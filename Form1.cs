using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Xml.Linq;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.ComponentModel;

namespace parser_OZON_webview
{

    public partial class Form1 : Form
    {
        //const string URL_XML = "https://myqu.ru/_turistore/feed.xml";
        const string URL_XML = "https://www.turistore.ru/marketplace/4241103.xml";
        const string URL_OZON = "https://www.ozon.ru/product/";
        const string URL_ENDPOINT = "https://myqu.ru/_turistore/worker_ozon.php";
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

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            StartParser(); // Вызов вашей функции parser
        }

        private void StartParser()
        {
            flagSTATUS = 1;
            dataGridView1.Rows.Clear();
            articles = [];
            items = [];
            if (InvokeRequired) this.Invoke(new Action(() => btnStartParse.Enabled = false)); else btnStartParse.Enabled = false;
            if (InvokeRequired) this.Invoke(new Action(() => groupBox1.Text = "История действий")); else groupBox1.Text = "История действий";

            AddHistoryText("Инициализация запроса ..");
            AddHistoryText("Выполнение запроса ..");
            LoadArticles();
            AddHistoryText("Запрос выполнен.");
            var c = 1;
            for (var i = 0; i < articles.Count; i++)
                if (articles[i] != "") c++;
            AddHistoryText($"Найдено записей: {articles.Count.ToString()}");
            AddHistoryText($"Найдено артикулов: {c.ToString()}");

            // парсинг
            AddHistoryText($"Идет парсинг ..");
            for (var i = 0; i < articles.Count; i++)
            {
                Item item = new Item();
                if (articles[i] != "")
                {
                    List<string> prices = GetPrices(articles[i]);
                    if (prices != null)
                    {
                        item.Article = articles[i];
                        item.Price_card = prices[0];
                        item.Price = prices[1];
                        item.Price_old = prices[2];
                    }
                    else
                    {
                        item.Article = articles[i];
                        item.Error = "not found";
                    }
                }

                items.Add(item);
                Application.DoEvents();

                groupBox2.Text = $"Результат парсинга ({i + 1} из {articles.Count}):";
                if (items[i].Error == null)
                    if (articles[i] != "")
                        dataGridView1.Rows.Add(i, articles[i], items[i].Price_card, items[i].Price, items[i].Price_old);
                    else
                        dataGridView1.Rows.Add(i, "нет артикула");
                else
                    dataGridView1.Rows.Add(i, articles[i], items[i].Error);

                //if (i == 10) break;

            }
            AddHistoryText("Парсинг завершен.");
            AddHistoryText("Сохранение данных ..");
            PostJsonToEndPoint(); // отправляем данные на worker_ozon
            AddHistoryText("Данные сохранены.");

            // завершение парсинга
            if (InvokeRequired) this.Invoke(new Action(() => btnStartParse.Enabled = true)); else btnStartParse.Enabled = true;
            switch (flagSTATUS)
            {
                case -1:
                    this.label1.Invoke((MethodInvoker)delegate
                    {
                        label1.Text = DateTime.Now + " ВНИМАНИЕ! Парсинг не выполнен. Произошла ошибка.";
                        label1.BackColor = Color.IndianRed;
                        label1.ForeColor = Color.White;
                    });
                    break;
                case 0:
                    this.label1.Invoke((MethodInvoker)delegate
                    {
                        label1.Text = DateTime.Now + " что-то пошло не так ..";
                        label1.BackColor = Color.Yellow;
                        label1.ForeColor = Color.Black;
                    });
                    break;
                case 1:
                    this.label1.Invoke((MethodInvoker)delegate
                    {
                        label1.Text = " Последнее сканирование: " + DateTime.Now;
                        label1.BackColor = Color.DarkGreen;
                        label1.ForeColor = Color.White;
                    });
                    break;
            }

            AddHistoryText("### ЗАВЕРШЕНО ###");
            GC.Collect();
        }



        private List<string> GetPrices(string article)
        {
            //article = "555567484";
            var url = $"{URL_OZON}{article}"; // URL товара
            var httpClient = new HttpClient();
            var response = httpClient.GetStringAsync(url).Result;
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

                Debug.WriteLine(article);
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

        static List<string> ExtractPricesContainingRuble(HtmlNode container)
        {
            // Находим все элементы <div> внутри priceContainer
            var divs = container.SelectNodes(".//div");
            // Фильтруем и извлекаем элементы, содержащие "₽"
            var prices = divs
                .Select(div => div.InnerText.Trim())
                .Where(text => text.Contains("₽"))
                .ToList();
            return prices;
        }

        static List<string> ExtractUniquePrices(List<string> inputList)
        {
            var regex = new Regex(@"[\d\s]+₽");
            var uniquePrices = new HashSet<string>();
            foreach (var input in inputList)
            {
                var matches = regex.Matches(input);
                foreach (Match match in matches)
                    uniquePrices.Add(match.Value.Trim().Replace("₽", "").Replace(" ", ""));
            }
            return uniquePrices.ToList();
        }

        private void LoadArticles()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = client.GetStringAsync(URL_XML).Result;
                    XDocument xdoc = XDocument.Parse(response);

                    foreach (var offerElement in xdoc.Descendants("offer"))
                    {
                        var articleOzon = offerElement.Element("article_ozon")?.Value;
                        if (articleOzon != null) articles.Add(articleOzon);
                    }
                }
                catch (Exception ex)
                {
                    AddHistoryText($"Ошибка загрузки данных: {ex.Message}");
                    flagSTATUS = 0;
                }
            }
        }



        private void PostJsonToEndPoint() // работает с скписком items
        {

            // URL для отправки данных
            string url = URL_ENDPOINT;

            // Отправка POST-запроса
            using (HttpClient client = new HttpClient())
            {

                // Преобразование словаря в JSON
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string telegramUrl = "tg://resolve?domain=cmacuk";
            Process.Start(new ProcessStartInfo(telegramUrl) { UseShellExecute = true });
        }

        private void btnStartParse_Click(object sender, EventArgs e)
        {
            StartParser();
            timer1.Start();
            timer1.Enabled = true;
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
    }
}
