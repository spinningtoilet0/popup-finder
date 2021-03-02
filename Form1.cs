using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable CS0168 // Variable is declared but never used

namespace PopupFinderGui
{
    public partial class Form1 : Form
    {
        private static bool nonum = Form2.nonum;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(start);
            button2.Enabled = true;
            button1.Text = "Start";
        }

        private void start(object state)
        {
            button1.Invoke(new Action(() => button1.Text = "Working..."));
            button1.Invoke(new Action(() => button1.Enabled = false));
            button2.Invoke(new Action(() => button2.Enabled = false));
            string[] websites = Form2.websites;
            string[] webext = Form2.extensions;
            int amountofguesses = Form2.guesses;
            bool gorp = Form2.gorp;
            List<string> goodurls = new List<string>();
            List<string> triedurls = new List<string>();
            HttpClient httpClient = new HttpClient();
            Random random = new Random();
            httpClient.Timeout = TimeSpan.FromSeconds(Form2.timeout);
            double speed = 0;
            double seconds = 0;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Reset();
                stopwatch.Start();
                byte[] bytes = Encoding.UTF8.GetBytes(httpClient.GetAsync("http://google.com/").Result.Content.ReadAsStringAsync().Result);
                stopwatch.Stop();
                seconds = stopwatch.Elapsed.TotalSeconds;
                speed = bytes.Count() / seconds;
            }
            catch (PingException e)
            {
                Environment.Exit(1);
            }
            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"Ping: {seconds}\nDownload speed: {speed} bytes per second")));
            richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
            if (!gorp)
            {
                while (amountofguesses >= 1)
                {
                    this.Invoke(new Action(() => Text = $"PopupFinderGui - {amountofguesses} to try left!"));
                trynewurl:
                    int webe = webext.Length;
                    int rwebr = random.Next(0, websites.Length);
                    string website = websites[rwebr];

                    string malurl = website;
                    int amountofchanges = random.Next(1, 5);
                    while (amountofchanges >= 1)
                    {
                        int change = random.Next(1, 3);
                        int offset = random.Next(0, malurl.Length);
                        malurl = GenUrl(malurl, change, offset);
                        amountofchanges = amountofchanges - 1;
                    }
                    if (triedurls.Contains(malurl) || websites.Contains(malurl))
                    {
                        goto trynewurl;
                    }
                    triedurls.Add(malurl);
                    while (webe >= 1)
                    {
                        var extension = webext[webe - 1];
                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nTrying {malurl}{extension}...")));
                        richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                        try
                        {
                            var uri = new Uri($"http://{malurl}{extension}");
                            var response = httpClient.GetAsync(uri).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = response.Content;
                                string responseString = responseContent.ReadAsStringAsync().Result;
                                if (responseString.ToUpper().Contains("404") || responseString.ToUpper().Contains("403") || responseString.ToUpper().Contains("ERROR") || responseString.ToUpper().Contains("TEST"))
                                {
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nBad url: {malurl}{extension}")));
                                    richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                                }
                                else
                                {
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\n--------------------\nGOOD URL! {malurl}{extension}\n--------------------")));
                                    richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                                    goodurls.Add($"{malurl}{extension}");
                                }
                            }
                            else
                            {
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nBad url: {malurl}{extension}")));
                                richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                            }
                        }
                        catch (Exception e)
                        {
                            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nBad url: {malurl}{extension}")));
                            richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                        }
                        webe--;
                    }
                    amountofguesses = amountofguesses - 1;
                }
            }
            else
            {
                amountofguesses = Form2.popups;
                while (amountofguesses >= 1)
                {
                    this.Invoke(new Action(() => Text = $"PopupFinderGui - {amountofguesses} to try left!"));
                trynewurl:
                    int webe = webext.Length;
                    int rwebr = random.Next(0, websites.Length);
                    string website = websites[rwebr];

                    string malurl = website;
                    int amountofchanges = random.Next(1, 5);
                    while (amountofchanges >= 1)
                    {
                        int change = random.Next(1, 3);
                        int offset = random.Next(0, malurl.Length);
                        malurl = GenUrl(malurl, change, offset);
                        amountofchanges = amountofchanges - 1;
                    }
                    if (triedurls.Contains(malurl) || websites.Contains(malurl))
                    {
                        goto trynewurl;
                    }
                    triedurls.Add(malurl);
                    while (webe >= 1)
                    {
                        var extension = webext[webe - 1];
                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nTrying {malurl}{extension}...")));
                        richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                        try
                        {
                            var uri = new Uri($"http://{malurl}{extension}");
                            var response = httpClient.GetAsync(uri).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = response.Content;
                                string responseString = responseContent.ReadAsStringAsync().Result;
                                if (responseString.ToUpper().Contains("404") || responseString.ToUpper().Contains("403") || responseString.ToUpper().Contains("ERROR") || responseString.ToUpper().Contains("TEST"))
                                {
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nBad url: {malurl}{extension}")));
                                    richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                                }
                                else
                                {
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\n--------------------\nGOOD URL! {malurl}{extension}\n--------------------")));
                                    richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                                    goodurls.Add($"{malurl}{extension}");
                                    amountofguesses = amountofguesses - 1;
                                }
                            }
                            else
                            {
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nBad url: {malurl}{extension}")));
                                richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                            }
                        }
                        catch (Exception e)
                        {
                            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nBad url: {malurl}{extension}")));
                            richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
                        }
                        webe--;
                    }
                }
            }
            this.Invoke(new Action(() => Text = "PopupFinderGui - Good URLS!"));
            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\nGood urls: ")));
            richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
            foreach (string goodurl in goodurls)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"\n{goodurl}")));
                richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
            }
            richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
            richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
            button1.Invoke(new Action(() => button1.Text = "Start"));
            button1.Invoke(new Action(() => button1.Enabled = true));
            button2.Invoke(new Action(() => button2.Enabled = true));
        }
        public static string RandomString(int length)
        {
            if (!nonum)
            {
                Random random = new Random();
                const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                Random random = new Random();
                const string chars = "abcdefghijklmnopqrstuvwxyz";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        public static char RandomCharacter()
        {
            return Convert.ToChar(RandomString(1));
        }
        public static string GenUrl(string inwebsite, int change, int offset)
        {
            if (change == 1)
            {
                inwebsite = ReplaceAt(inwebsite, offset, RandomCharacter());
            }
            if (change == 2)
            {
                inwebsite = inwebsite.Insert(offset, RandomString(1));
            }
            if (change == 3)
            {
                inwebsite = inwebsite.Remove(offset);
            }
            return inwebsite;
        }
        public static string ReplaceAt(string input, int index, char newChar)
        {
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Hide();
            frm2.Show();
            frm2.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            frm2.Hide();
        }
    }
}
