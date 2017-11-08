using DAL;
using Entity.Models;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnketV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Soru s = new Soru();
            s.SoruCumlesi = textBox2.Text;
            AnketContext db = new AnketContext();
            db.Sorular.Add(s);
            db.SaveChanges();
            YenileSorular();
        }
        AnketContext db = new AnketContext();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            YenileSorular();
            YenileCevaplar();
        }
        public void YenileSorular()
        {
            AnketContext db = new AnketContext();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = db.Sorular.ToList();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Clear();
            foreach (Soru soru in db.Sorular)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = soru.SoruCumlesi;
                flowLayoutPanel1.Controls.Add(lbl);

                RadioButton r1 = new RadioButton();
                r1.Name = "Soru_" + soru.SoruID;
                r1.Text = "Evet";
                //flowLayoutPanel1.Controls.Add(r1);

                RadioButton r2 = new RadioButton();
                r2.Name = "Soru_" + soru.SoruID;
                r2.Text = "Hayir";
                //flowLayoutPanel1.Controls.Add(r2);

                FlowLayoutPanel P = new FlowLayoutPanel();
                P.Width = 300;
                P.Height = 100;
                P.AutoScroll = false;
                P.Controls.Add(r1);
                P.Controls.Add(r2);
                flowLayoutPanel1.Controls.Add(P);
                flowLayoutPanel1.SetFlowBreak(P, true);


                //ComboBox c1 = new ComboBox();
                //c1.name="Soru_"+soru.soruID;
                //c1.Items.Add("Evet");                
                //c1.Items.Add("Hayir");
                //flowLayoutPanel1.Controls.Add(c1);
                //flowLayoutPanel1.SetFlowBreak(c1, true);

            }
        }
        public void YenileCevaplar()
        {
            AnketContext db = new AnketContext();
            dataGridView2.DataSource = null;
            // dataGridView2.DataSource = db.Cevaplar.ToList();
            dataGridView2.DataSource = db.Cevaplar.Select(x => new CevapViewModel()
            {
                CevapID = x.CevapID,
                AdSoyad = x.CevabiVerenKisi.AdSoyad,
                Soru = x.Sorusu.SoruCumlesi,
                Cevap = x.Yanit.ToString()
            }).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //{//combobox
            //    foreach (Control item in flowLayoutPanel1.Controls)
            //    {
            //        if (item is ComboBox)
            //        {
            //            string soruID = (item.Name.Replace("Soru_", ""));
            //            int SID = Convert.ToInt32(soruID);
            //            Cevap C = new Cevap();
            //            C.SoruID = SID;
            //            int y = (((ComboBox)item).SelectedIndex + 1) % 2;
            //            C.Yanit = (Yanit)y;

            //        }
            //    }
                // radiobutton
                foreach (Control pnl in flowLayoutPanel1.Controls)
                {
                    if (pnl is FlowLayoutPanel)
                    {
                        foreach (RadioButton item in ((FlowLayoutPanel)pnl).Controls)
                        {


                            RadioButton r = item;
                            if (r.Checked)
                            {
                                string soruID = (item.Name.Replace("Soru_", ""));
                                int SID = Convert.ToInt32(soruID);
                                Cevap C = new Cevap();
                                C.SoruID = SID;
                                C.Yanit = r.Text == "Evet" ? Yanit.Evet : Yanit.Hayir;

                                Kisi k = db.Kisiler.Where(x => x.AdSoyad == textBox1.Text).FirstOrDefault();

                                if (k != null)
                                    C.KisiID = k.KisiID;
                                else
                                {
                                    k = new Kisi();
                                    k.AdSoyad = textBox1.Text;
                                    db.Kisiler.Add(k);
                                    db.SaveChanges();
                                    C.KisiID = k.KisiID;
                                }

                                db.Cevaplar.Add(C);
                                db.SaveChanges();
                                MessageBox.Show("Eklendi");
                                YenileCevaplar();
                            }



                        }
                    }
                }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            //soru sil 
            if (dataGridView1.SelectedRows.Count == 0) //seçili birşey yok ise
                MessageBox.Show("Soru seçiniz");
            else
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    int SoruID = (int)item.Cells[0].Value; //silinecek satırın ID'sine ulaş
                    Soru silinecek = db.Sorular.Find(SoruID);
                    db.Sorular.Remove(silinecek);
                }
                db.SaveChanges();
                YenileSorular();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//sorudüzenle butonu
            if (dataGridView1.SelectedRows.Count == 0) //seçili birşey yok ise
                MessageBox.Show("Soru seçiniz");
            else
            {
                SoruDuzenle form = new SoruDuzenle();
                int secilenID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                Soru Duzenlenecek = db.Sorular.Find(secilenID);
                form.GelenSoru = Duzenlenecek;
                form.Show();

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //cevap sil 
            if (dataGridView2.SelectedRows.Count == 0) 
                MessageBox.Show("Cevap seçiniz");
            else
            {
                foreach (DataGridViewRow item in dataGridView2.SelectedRows)
                {
                    int CevapID = (int)item.Cells[0].Value;
                    Cevap silinecek = db.Cevaplar.Find(CevapID);
                    db.Cevaplar.Remove(silinecek);
                }
                db.SaveChanges();
                YenileCevaplar();
            }
        }
    }
    }

