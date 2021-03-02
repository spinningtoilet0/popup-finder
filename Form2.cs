using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace PopupFinderGui
{
    public partial class Form2 : Form
    {
        public static string[] websites = { "youtube", "facebook", "gmail", "google", "microsoft", "twitter", "wikipedia", "amazon", "instagram", "reddit", "ebay", "netflix", "apple", "walmart", "yahoo", "nytimes", "aol", "bing", "fandom", "imdb", "yelp", "craigslist", "healthline", "tripadvisor", "linkedin", "homedepot", "quora", "roblox", "zillow", "paypal" };
        public static string[] extensions = { ".cc", ".pro", ".biz", ".eu", ".ru", ".info", ".uk", ".de", ".ga", ".online", ".xyz", ".org", ".net", ".com" };
        public static bool gorp = false;
        public static int guesses = 1000;
        public static int popups = 20;
        public static double timeout = 1;
        public static bool nonum = false;

        public Form2()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Enabled = true;
                textBox1.Enabled = false;
            }
            else
            {
                textBox2.Enabled = false;
                textBox1.Enabled = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Lines = websites;
            richTextBox2.Lines = extensions;
            textBox1.Text = guesses.ToString();
            textBox2.Text = popups.ToString();
            textBox3.Text = timeout.ToString();
            checkBox1.Checked = gorp;
            checkBox2.Checked = nonum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Saving...";
            button1.Enabled = false;
            websites = richTextBox1.Lines;
            extensions = richTextBox2.Lines;
            guesses = int.Parse(textBox1.Text);
            popups = int.Parse(textBox2.Text);
            timeout = int.Parse(textBox3.Text);
            gorp = checkBox1.Checked;
            nonum = checkBox2.Checked;
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings.Add("websites", arraytostring(websites));
            settings.Add("extensions", arraytostring(extensions));
            settings.Add("guesses", Convert.ToString(guesses));
            settings.Add("popups", Convert.ToString(popups));
            settings.Add("timeout", Convert.ToString(timeout));
            settings.Add("gorp", Convert.ToString(gorp));
            settings.Add("nonum", Convert.ToString(nonum));
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            this.Hide();
            button1.Enabled = true;
            button1.Text = "Save!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;
            richTextBox1.Text = appSettings["websites"] ?? "Not Found";
            richTextBox2.Text = appSettings["extensions"] ?? "Not Found";
            textBox1.Text = appSettings["guesses"] ?? "Not Found";
            textBox2.Text = appSettings["popups"] ?? "Not Found";
            textBox3.Text = appSettings["timeout"] ?? "Not Found";
            checkBox1.Checked = Convert.ToBoolean(appSettings["gorp"] ?? "false");
            checkBox2.Checked = Convert.ToBoolean(appSettings["nonum"] ?? "false");
        }

        private string arraytostring(string[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in array)
            {
                sb.Append($"{s}\n");
            }
            return sb.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings.Clear();
            configFile.Save();
            ConfigurationManager.RefreshSection("appSettings");
            string[] websites2 = { "youtube", "facebook", "gmail", "google", "microsoft", "twitter", "wikipedia", "amazon", "instagram", "reddit", "ebay", "netflix", "apple", "walmart", "yahoo", "nytimes", "aol", "bing", "fandom", "imdb", "yelp", "craigslist", "healthline", "tripadvisor", "linkedin", "homedepot", "quora", "roblox", "zillow", "paypal" };
            richTextBox1.Text = arraytostring(websites2);
            string[] extensions2 = { ".cc", ".pro", ".biz", ".eu", ".ru", ".info", ".uk", ".de", ".ga", ".online", ".xyz", ".org", ".net", ".com" };
            richTextBox2.Text = arraytostring(extensions2);
            textBox1.Text = "1000";
            textBox2.Text = "20";
            textBox3.Text = "1";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            this.Refresh();
        }
    }
}
