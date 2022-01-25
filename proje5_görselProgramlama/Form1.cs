using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace topSektirme
{
    public partial class Form1 : Form
    {
        public List<Ball> bList = new List<Ball>();
        public int stickLocation = 282;
        string pathJson = @"yourproject\jsonFilem.json";
        Random rnd = new Random();
        Thread thrd;

        //Tuslar A ve D'dir

        public Form1()
        {
            InitializeComponent();
            thrd = new Thread(new ThreadStart(threadFunc));
            thrd.Start();
            timer1.Interval = 10;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        { 
            for (int i = 0; i < bList.Count; i++)
            {
                bList[i].Draw(e.Graphics);
                bList[i].Move(stickLocation, this);
                if (bList[i].cezaPuani == true)
                {
                    bList.Remove(bList[i]);
                    addBall();
                    addBall();
                }
                else if (bList[i].durum == false)
                {
                    bList.Remove(bList[i]);
                }
                label1.Text = "SCORE: " + Ball.score;
            }

        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        void addBall()
        {
            Ball ball = new Ball
            {
                kord_x = rnd.Next(100, this.ClientSize.Width - 100),
                kord_y = rnd.Next(100, this.ClientSize.Height - 100),
                renk = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255))),
                yon_bilgisi = rnd.Next(0, 2),
            };
            bList.Add(ball);
        }
        private void threadFunc()
        {
            while(bList.Count <= 5)
            {
                addBall();
                Thread.Sleep(10000);
            }
           
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A)
            {
                if(stickLocation > 30) 
                { 
                    stickLocation -= 20;
                }       
            }
            else if(e.KeyCode == Keys.D)
            {
                if(stickLocation < this.ClientSize.Width-230) 
                {  
                    stickLocation += 20;
                }
               
            }
            picStick.Location = new Point(stickLocation, 295);   

        }

        public void writeFileJson()
        {
            var jsonFile = JsonConvert.SerializeObject(bList);
            var jsonFiledata = Encryptor.Encrypt(jsonFile);
            using (var writer = new StreamWriter(pathJson))
            {
                writer.Write(jsonFiledata);
            }
        }
        public List<Ball> readFileJson()
        {
            string resultFile = File.ReadAllText(pathJson);
            var decrptedjsonFile = Encryptor.Decrypt(resultFile);
            bList = JsonConvert.DeserializeObject<List<Ball>>(decrptedjsonFile);
            return bList;
        }

        public bool controlJsonFile()
        {   
            return (File.Exists(pathJson) == true) ? true : false;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            bList = readFileJson();
        }
        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (controlJsonFile() == true)
            {
                File.Delete(pathJson);
            }
            writeFileJson();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (controlJsonFile() == true)
            {
                File.Delete(pathJson);
            }
            writeFileJson();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (controlJsonFile() == true)
            {
                DialogResult dialogResult = MessageBox.Show("Yeniden Yükleme Yapılsın mı?", "Pencere", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    bList = readFileJson();
                }
                else if (DialogResult == DialogResult.No)
                {
                    bList = new List<Ball>();
                }
            }
        }
    }
}
