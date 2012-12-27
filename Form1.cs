using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.IO;

struct Musique
{
    public String title;
    public String url;
};


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<Musique> parsePage(String url)
        {
            List<Musique> mu = new List<Musique>();
            List<String> urls = new List<String>();
            List<String> titles = new List<String>();
            WebClient client = new WebClient();
            //client.DownloadFile("https://soundcloud.com/thecrimsonchin", this.textBox2.Text + "\\" + "blah.html");
            String src = client.DownloadString(url);
            Regex regu = new Regex("http://media.soundcloud.com/stream/[-\u002E=?A-Z\u005Fa-z0-9]*");
            Regex regt = new Regex("\"title\":\"[A-Za-z0-9_:/\u002E() \t\"@'+-]*");

            foreach (Match m in regu.Matches(src))
            {
                urls.Add(m.ToString());
            }


            foreach (Match m in regt.Matches(src))
            {
                String res = m.ToString();
                res = res.Remove(0, 9);
                res = res.Substring(0, (res.Length) - 1);
                titles.Add(res);
            }

            for (int i = 0; i < titles.Count; i++)
            {
                Musique m = new Musique();
                m.title = titles.ElementAt(i);
                m.url = urls.ElementAt(i);
                mu.Add(m);
            }

            return mu;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = this.folderBrowserDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                this.textBox2.Clear();
                this.textBox2.AppendText(this.folderBrowserDialog.SelectedPath);
            }
            else
            {
                this.textBox2.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("Vous devez entrer une url !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(!((this.textBox1.Text.Contains("https://soundcloud.com/")) | (this.textBox1.Text.Contains("http://soundcloud.com/"))))
            {
                MessageBox.Show("Seul le site de soundcloud est supporté !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (this.textBox2.Text == "")
            {
                MessageBox.Show("Vous devez choisir un dossier de sortie !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                String path = this.textBox1.Text.Remove(0, this.textBox1.Text.LastIndexOf('/'));
                Directory.CreateDirectory((textBox2.Text + "\\" + path));
                List<Musique> mu = parsePage(this.textBox1.Text);
                ListMusic b = new ListMusic(mu, (textBox2.Text + "\\" + path));
                if (b.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                {
                    this.Close();
                }
            }
        }
    }
}
