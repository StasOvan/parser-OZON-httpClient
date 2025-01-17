using parser_OZON;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;


namespace parser_OZON_webview
{

    public partial class Form1 : Form
    {
        const string URL_WORKER = "https://myqu.ru/_turistore/worker.php";
        const string URL_FEED = "https://myqu.ru/_turistore/feed.xml";
        const string URL_XML = "https://www.turistore.ru/marketplace/4241103.xml";
        const string URL_OZON = "https://www.ozon.ru/product/";
        const string URL_ENDPOINT = "https://myqu.ru/_turistore/worker_ozon.php";
        //const string COOKIES = "__Secure-ab-group=45; __Secure-user-id=0; TS0121feed=0187c00a189e5e85614c893e494f069578f27ee212ecdb0b1cb031d93dba740c8f379aba1de950e1f99ab11ba3f2b8ac4dbfa5c2b9; xcid=843e375db001e54ecc38178105ba6106; __Secure-ext_xcid=843e375db001e54ecc38178105ba6106; __Secure-ETC=e1d6a015c027bd6c0ca26433b5b16735; abt_data=7.jmrBXcZKNt1kIH6W6Jo2C8WWMC5vSDpUTRsdgZvhmda9ieGuRoqkDo8YrKD5zBHE8CYpsyKx23A2C3MEG0WdJE_F26JrmgIUhCH1E1o__IeN6Y1DsoB3Wauk13d40ujpDJZISynXHH_varIbkZM0x_yX7u689FZGFg73FLdJVcwgFzeSKxYhLmSewDQ0I0zapZ8XiJ5nn4qGQyrHi4gf8iaflj-OO9AD0mPI0QxPMuKn7MEcUra1AV-Te1xw_usDdXllKnf7B1J4ZSgkFTS2j6R7fthGK-3UCk6CAYcWoMisVrIdQYsO47T_SOG5CW_EPIwWfVk74awro8fknyQ0arLgwiHM1zTQwXRW7vHnkrxxARn-mWEyHXPnNLKZC27nyVMJb35PTTUXN5p8fYUZ6XxtLESMma8Cl3DINvnejwqmKcFhd5uRaoAWOTCZcnN8Y-lEkSuYfVCgLdl2dfpeGBi_qCln4onYMpQTbDvhfA; __Secure-access-token=6.0.mlqDRhLgQqim49lMuNw3Hg.45.AdStCyh2AuTEIRLt7-1Mr92uJzwzD_1ylLIyRSXtty7NTrN8viSnLNLy7jAS6T0IDg..20250109150058.-bO3FRNDOt3ko31YXxKlCf0af4C7MY8_AsofuF6x4rU.1c4d2e3d7b2c3db6a; __Secure-refresh-token=6.0.mlqDRhLgQqim49lMuNw3Hg.45.AdStCyh2AuTEIRLt7-1Mr92uJzwzD_1ylLIyRSXtty7NTrN8viSnLNLy7jAS6T0IDg..20250109150058.KCtvrW_aLjUoq7yhBlw3fmLPfNCFMw_jioRJfpEEw1I.16f338e67704849bb; ADDRESSBOOKBAR_WEB_CLARIFICATION=1736427665; rfuid=NjkyNDcyNDUyLDEyNC4wNDM0NzUyNzUxNjA3NCwxOTA3MzMzOTM0LC0xLC0xNjMwNjYwMTgzLFczc2libUZ0WlNJNklsQkVSaUJXYVdWM1pYSWlMQ0prWlhOamNtbHdkR2x2YmlJNklsQnZjblJoWW14bElFUnZZM1Z0Wlc1MElFWnZjbTFoZENJc0ltMXBiV1ZVZVhCbGN5STZXM3NpZEhsd1pTSTZJbUZ3Y0d4cFkyRjBhVzl1TDNCa1ppSXNJbk4xWm1acGVHVnpJam9pY0dSbUluMHNleUowZVhCbElqb2lkR1Y0ZEM5d1pHWWlMQ0p6ZFdabWFYaGxjeUk2SW5Ca1ppSjlYWDBzZXlKdVlXMWxJam9pUTJoeWIyMWxJRkJFUmlCV2FXVjNaWElpTENKa1pYTmpjbWx3ZEdsdmJpSTZJbEJ2Y25SaFlteGxJRVJ2WTNWdFpXNTBJRVp2Y20xaGRDSXNJbTFwYldWVWVYQmxjeUk2VzNzaWRIbHdaU0k2SW1Gd2NHeHBZMkYwYVc5dUwzQmtaaUlzSW5OMVptWnBlR1Z6SWpvaWNHUm1JbjBzZXlKMGVYQmxJam9pZEdWNGRDOXdaR1lpTENKemRXWm1hWGhsY3lJNkluQmtaaUo5WFgwc2V5SnVZVzFsSWpvaVEyaHliMjFwZFcwZ1VFUkdJRlpwWlhkbGNpSXNJbVJsYzJOeWFYQjBhVzl1SWpvaVVHOXlkR0ZpYkdVZ1JHOWpkVzFsYm5RZ1JtOXliV0YwSWl3aWJXbHRaVlI1Y0dWeklqcGJleUowZVhCbElqb2lZWEJ3YkdsallYUnBiMjR2Y0dSbUlpd2ljM1ZtWm1sNFpYTWlPaUp3WkdZaWZTeDdJblI1Y0dVaU9pSjBaWGgwTDNCa1ppSXNJbk4xWm1acGVHVnpJam9pY0dSbUluMWRmU3g3SW01aGJXVWlPaUpOYVdOeWIzTnZablFnUldSblpTQlFSRVlnVm1sbGQyVnlJaXdpWkdWelkzSnBjSFJwYjI0aU9pSlFiM0owWVdKc1pTQkViMk4xYldWdWRDQkdiM0p0WVhRaUxDSnRhVzFsVkhsd1pYTWlPbHQ3SW5SNWNHVWlPaUpoY0hCc2FXTmhkR2x2Ymk5d1pHWWlMQ0p6ZFdabWFYaGxjeUk2SW5Ca1ppSjlMSHNpZEhsd1pTSTZJblJsZUhRdmNHUm1JaXdpYzNWbVptbDRaWE1pT2lKd1pHWWlmVjE5TEhzaWJtRnRaU0k2SWxkbFlrdHBkQ0JpZFdsc2RDMXBiaUJRUkVZaUxDSmtaWE5qY21sd2RHbHZiaUk2SWxCdmNuUmhZbXhsSUVSdlkzVnRaVzUwSUVadmNtMWhkQ0lzSW0xcGJXVlVlWEJsY3lJNlczc2lkSGx3WlNJNkltRndjR3hwWTJGMGFXOXVMM0JrWmlJc0luTjFabVpwZUdWeklqb2ljR1JtSW4wc2V5SjBlWEJsSWpvaWRHVjRkQzl3WkdZaUxDSnpkV1ptYVhobGN5STZJbkJrWmlKOVhYMWQsV3lKeWRTMVNWU0pkLDAsMSwwLDI0LDIzNzQxNTkzMCw4LDIyNzEyNjUyMCwwLDEsMCwtNDkxMjc1NTIzLFIyOXZaMnhsSUVsdVl5NGdUbVYwYzJOaGNHVWdSMlZqYTI4Z1YybHVNeklnTlM0d0lDaFhhVzVrYjNkeklFNVVJREV3TGpBN0lGZHBialkwT3lCNE5qUXBJRUZ3Y0d4bFYyVmlTMmwwTHpVek55NHpOaUFvUzBoVVRVd3NJR3hwYTJVZ1IyVmphMjhwSUVOb2NtOXRaUzh4TXpFdU1DNHdMakFnVTJGbVlYSnBMelV6Tnk0ek5pQXlNREF6TURFd055Qk5iM3BwYkd4aCxleUpqYUhKdmJXVWlPbnNpWVhCd0lqcDdJbWx6U1c1emRHRnNiR1ZrSWpwbVlXeHpaU3dpU1c1emRHRnNiRk4wWVhSbElqcDdJa1JKVTBGQ1RFVkVJam9pWkdsellXSnNaV1FpTENKSlRsTlVRVXhNUlVRaU9pSnBibk4wWVd4c1pXUWlMQ0pPVDFSZlNVNVRWRUZNVEVWRUlqb2libTkwWDJsdWMzUmhiR3hsWkNKOUxDSlNkVzV1YVc1blUzUmhkR1VpT25zaVEwRk9UazlVWDFKVlRpSTZJbU5oYm01dmRGOXlkVzRpTENKU1JVRkVXVjlVVDE5U1ZVNGlPaUp5WldGa2VWOTBiMTl5ZFc0aUxDSlNWVTVPU1U1SElqb2ljblZ1Ym1sdVp5SjlmWDE5LDY1LC0xMjg1NTUxMywxLDEsLTEsMTY5OTk1NDg4NywxNjk5OTU0ODg3LC0xMTI2MzA0Nzc0LDI=";
        const string COOKIE = "__Secure-ab-group=45; __Secure-user-id=0; TS0121feed=0187c00a189e5e85614c893e494f069578f27ee212ecdb0b1cb031d93dba740c8f379aba1de950e1f99ab11ba3f2b8ac4dbfa5c2b9; xcid=843e375db001e54ecc38178105ba6106; __Secure-ext_xcid=843e375db001e54ecc38178105ba6106; is_cookies_accepted=1; __Secure-ETC=d4738a8c9832fb49afcbb85a056c5cdc; abt_data=7.HtY4LCIlBhfGH4HAEu-wKcUyQIXge3F4yx4tARfY38avm6wyUBRwXLxU7Kir3KVCkEkGcknGD8z-vQhJj_YZqVk0ABJTIOOTuZqmMx08gB9Maky-wuLHwWJ8eFk6_YplhNene58bEdOP_2hDhXNHdgw95KtyAOKJS6_qO5B8Fe4ZJfmtONVD7frmKieXbHRb3p6vvROb68_IUUyUsVZfCg04QttJVkrt5W1CgbJxckTX46vYr1KIE7uUXpgpIjpN19IkXOkHYMMczmNJeQEDyTMyqXAKKczmD-YvkKtRBmTfU3Q0GC_X5mFit5sdbkjpq1qtVK5tKt0MfiUMJ0g2vnyfZ4QfABoKWfc3YrqRt-NxREvih9buVPVckmkygoFIj1ord5_q440MtUNgkEsajjCtQyGLKpZ7kLSzT9M0KoUBbG886jbxDy2XND51SSdIvF9SwPcRlOdtlql8v3VCWrwFfDqWV0VJFBJY-u7mB-pkeIze_iEKtFSyulk2QzE; ADDRESSBOOKBAR_WEB_CLARIFICATION=1736711480; __Secure-refresh-token=6.0.mlqDRhLgQqim49lMuNw3Hg.45.AdStCyh2AuTEIRLt7-1Mr92uJzwzD_1ylLIyRSXtty7NTrN8viSnLNLy7jAS6T0IDg..20250115043930.HZSLHNnB9ETKwrB3KZBhD2eLuxjlYByAvnCB7egH1DQ.124b3905e54bb93a9; __Secure-access-token=6.0.mlqDRhLgQqim49lMuNw3Hg.45.AdStCyh2AuTEIRLt7-1Mr92uJzwzD_1ylLIyRSXtty7NTrN8viSnLNLy7jAS6T0IDg..20250115043930.2kAC7qy9FUleZfNxwmXFvbDnk1wlzsYzBlE0MJrMQ7Y.114e393b0846c15ea; TS0149423d=0187c00a18d851c756bbae4c1dcb2e108e4e96ac40cfdcf9f306223c24bc9644f22067f428601c188bde1136fa98f758d5f02f6796; rfuid=NjkyNDcyNDUyLDEyNC4wNDM0NzUyNzUxNjA3NCwxOTA3MzMzOTM0LC0xLC0xNjMwNjYwMTgzLFczc2libUZ0WlNJNklsQkVSaUJXYVdWM1pYSWlMQ0prWlhOamNtbHdkR2x2YmlJNklsQnZjblJoWW14bElFUnZZM1Z0Wlc1MElFWnZjbTFoZENJc0ltMXBiV1ZVZVhCbGN5STZXM3NpZEhsd1pTSTZJbUZ3Y0d4cFkyRjBhVzl1TDNCa1ppSXNJbk4xWm1acGVHVnpJam9pY0dSbUluMHNleUowZVhCbElqb2lkR1Y0ZEM5d1pHWWlMQ0p6ZFdabWFYaGxjeUk2SW5Ca1ppSjlYWDBzZXlKdVlXMWxJam9pUTJoeWIyMWxJRkJFUmlCV2FXVjNaWElpTENKa1pYTmpjbWx3ZEdsdmJpSTZJbEJ2Y25SaFlteGxJRVJ2WTNWdFpXNTBJRVp2Y20xaGRDSXNJbTFwYldWVWVYQmxjeUk2VzNzaWRIbHdaU0k2SW1Gd2NHeHBZMkYwYVc5dUwzQmtaaUlzSW5OMVptWnBlR1Z6SWpvaWNHUm1JbjBzZXlKMGVYQmxJam9pZEdWNGRDOXdaR1lpTENKemRXWm1hWGhsY3lJNkluQmtaaUo5WFgwc2V5SnVZVzFsSWpvaVEyaHliMjFwZFcwZ1VFUkdJRlpwWlhkbGNpSXNJbVJsYzJOeWFYQjBhVzl1SWpvaVVHOXlkR0ZpYkdVZ1JHOWpkVzFsYm5RZ1JtOXliV0YwSWl3aWJXbHRaVlI1Y0dWeklqcGJleUowZVhCbElqb2lZWEJ3YkdsallYUnBiMjR2Y0dSbUlpd2ljM1ZtWm1sNFpYTWlPaUp3WkdZaWZTeDdJblI1Y0dVaU9pSjBaWGgwTDNCa1ppSXNJbk4xWm1acGVHVnpJam9pY0dSbUluMWRmU3g3SW01aGJXVWlPaUpOYVdOeWIzTnZablFnUldSblpTQlFSRVlnVm1sbGQyVnlJaXdpWkdWelkzSnBjSFJwYjI0aU9pSlFiM0owWVdKc1pTQkViMk4xYldWdWRDQkdiM0p0WVhRaUxDSnRhVzFsVkhsd1pYTWlPbHQ3SW5SNWNHVWlPaUpoY0hCc2FXTmhkR2x2Ymk5d1pHWWlMQ0p6ZFdabWFYaGxjeUk2SW5Ca1ppSjlMSHNpZEhsd1pTSTZJblJsZUhRdmNHUm1JaXdpYzNWbVptbDRaWE1pT2lKd1pHWWlmVjE5TEhzaWJtRnRaU0k2SWxkbFlrdHBkQ0JpZFdsc2RDMXBiaUJRUkVZaUxDSmtaWE5qY21sd2RHbHZiaUk2SWxCdmNuUmhZbXhsSUVSdlkzVnRaVzUwSUVadmNtMWhkQ0lzSW0xcGJXVlVlWEJsY3lJNlczc2lkSGx3WlNJNkltRndjR3hwWTJGMGFXOXVMM0JrWmlJc0luTjFabVpwZUdWeklqb2ljR1JtSW4wc2V5SjBlWEJsSWpvaWRHVjRkQzl3WkdZaUxDSnpkV1ptYVhobGN5STZJbkJrWmlKOVhYMWQsV3lKeWRTMVNWU0pkLDAsMSwwLDI0LDIzNzQxNTkzMCw4LDIyNzEyNjUyMCwwLDEsMCwtNDkxMjc1NTIzLFIyOXZaMnhsSUVsdVl5NGdUbVYwYzJOaGNHVWdSMlZqYTI4Z1YybHVNeklnTlM0d0lDaFhhVzVrYjNkeklFNVVJREV3TGpBN0lGZHBialkwT3lCNE5qUXBJRUZ3Y0d4bFYyVmlTMmwwTHpVek55NHpOaUFvUzBoVVRVd3NJR3hwYTJVZ1IyVmphMjhwSUVOb2NtOXRaUzh4TXpFdU1DNHdMakFnVTJGbVlYSnBMelV6Tnk0ek5pQXlNREF6TURFd055Qk5iM3BwYkd4aCxleUpqYUhKdmJXVWlPbnNpWVhCd0lqcDdJbWx6U1c1emRHRnNiR1ZrSWpwbVlXeHpaU3dpU1c1emRHRnNiRk4wWVhSbElqcDdJa1JKVTBGQ1RFVkVJam9pWkdsellXSnNaV1FpTENKSlRsTlVRVXhNUlVRaU9pSnBibk4wWVd4c1pXUWlMQ0pPVDFSZlNVNVRWRUZNVEVWRUlqb2libTkwWDJsdWMzUmhiR3hsWkNKOUxDSlNkVzV1YVc1blUzUmhkR1VpT25zaVEwRk9UazlVWDFKVlRpSTZJbU5oYm01dmRGOXlkVzRpTENKU1JVRkVXVjlVVDE5U1ZVNGlPaUp5WldGa2VWOTBiMTl5ZFc0aUxDSlNWVTVPU1U1SElqb2ljblZ1Ym1sdVp5SjlmWDE5LDY1LC0xMjg1NTUxMywxLDEsLTEsMTY5OTk1NDg4NywxNjk5OTU0ODg3LC0xMTI2MzA0Nzc0LDI=";

        HttpClient httpClient;

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
            dataGridView1.Rows.Clear();
            articles = new List<string>();
            items = new List<Item>();
            
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
                
                //articles[i] = "940811029";
                Debug.WriteLine(articles[i]);

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
                        item.Article_found = "article not found";
                        items.Add(item);
                        dataGridView1.Rows.Add(i + 1, articles[i], "Такой страницы не существует");
                        continue;
                    }

                    item.Article = articles[i];
                    item.Price_card = prices[0];
                    item.Price = prices[1];
                    item.Price_old = prices[2];

                    if (prices[0] != "" || prices[1] != "" || prices[2] != "" )
                        item.Article_found = "true";
                    else
                        item.Article_found = "false";


                    items.Add(item);

                    if (item.Article_found == "true")
                        dataGridView1.Rows.Add(i + 1, articles[i], item.Price_card, item.Price, item.Price_old);
                    else
                        dataGridView1.Rows.Add(i + 1, articles[i], "нет цен");
                }
                else
                {
                    dataGridView1.Rows.Add(i + 1, "нет артикула");
                }
                

            } // for
            

            AddHistoryText("Парсинг завершен.");
            AddHistoryText("Сохранение данных ..");
            PostJsonToEndPoint(); // отправляем данные на worker_ozon
            AddHistoryText("Данные сохранены.");

            AddHistoryText("Запуск обработчика feed.xml ..");
            if (RunWorker() == true)
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
            await Task.Delay(150);

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            // Создаем HttpClient
            httpClient = new HttpClient(handler);

            //article = "1620108309";
            string url = $"{URL_OZON}{article}"; // URL товара
            
            List<string> values = [];

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Cookie", COOKIE);

            values.Add("");
            values.Add("");
            values.Add("");
            values.Add(""); // дополнительное пустое значение

            //HttpResponseMessage response = await httpClient.GetAsync(url);
            var response = await httpClient.GetAsync(url);

            // Проверяем статус ответа
            if (response.IsSuccessStatusCode) flagSTATUS = 1; else return values;
            
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            var body = await response.Content.ReadAsStringAsync();
            //File.WriteAllText("text.html", body);
            htmlDocument.LoadHtml(body);

            if ( body.Contains("Такой страницы не существует") )
            {
                flagSTATUS = -1;
                return values;
            }

            if ( body.Contains("Этот товар закончился") )
            {
                flagSTATUS = 0;
                return values;
            }

            if ( body.Contains("data-widget=\"webPrice\"") )
            {
                // Находим элемент <div data-widget="webPrice">
                var priceContainer = htmlDocument.DocumentNode.SelectSingleNode("//div[@data-widget='webPrice']");
            
                if (priceContainer != null)
                {
                    string[] temp = priceContainer.InnerHtml.Split("₽</span>");

                    for (var i = 0; i < temp.Length; i++)
                        values[i] = ExtractDigits(temp[i].Substring(temp[i].LastIndexOf(">") + 1));

                    if (temp[0].Contains("<button tabindex") == false)
                    {
                        values[2] = values[1];
                        values[1] = values[0];
                        values[0] = "";
                    }
                }    
            }
            
            return values;

        }

        private void LoadArticles()
        {
            httpClient = new HttpClient();

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
                if (response.Contains("true")) return true; else return false;
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

        private void SendMail()
        {
            // Создаем экземпляр EmailSender
            EmailSender emailSender = new EmailSender();

            // Отправляем письмо
            emailSender.SendEmail("recipient@example.com", "Тема письма", "Содержимое письма");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Создаем HttpClientHandler с поддержкой прокси
            var proxy = new WebProxy("178.34.190.6:8080");
            //{
            //    // Если требуется аутентификация на прокси
            //    Credentials = new NetworkCredential("username", "password") // Замените на ваши учетные данные, если необходимо
            //};

            var handler = new HttpClientHandler
            {
                Proxy = proxy,
                UseProxy = true,
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            // Создаем HttpClient один раз
            using var client = new HttpClient(handler);

            // Теперь вы можете использовать client в цикле
            for (int i = 0; i < 1; i++)
            {
                // Ваш код для отправки запросов с использованием client
                var response = await client.GetAsync("https://www.ozon.ru/product/dzhemper-sevenext-1489352754/");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Обработка ответа
                    Debug.WriteLine("Запрос выполнен успешно!");
                }
                else
                {
                    Debug.WriteLine($"error: {response.StatusCode}");
                    // Обработка ошибки
                }

                // Задержка между запросами, если необходимо
                await Task.Delay(2000); // Задержка в 2 секунды
            }
        }
    }


}
