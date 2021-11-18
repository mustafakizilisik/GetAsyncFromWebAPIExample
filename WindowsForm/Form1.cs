using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await ListenWebAPI();
        }

        private async Task ListenWebAPI()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Debug.Write("***********************While");
                    using (HttpClient _httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await _httpClient.GetAsync("https://fakestoreapi.com/products");
                        if (response.IsSuccessStatusCode)
                        {
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                            this.Invoke((MethodInvoker)delegate
                            {
                                dataGridView1.DataSource = data;
                            });
                        }
                    }
                }
            });
        }

        class Product
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Price { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
        }
    }
}
