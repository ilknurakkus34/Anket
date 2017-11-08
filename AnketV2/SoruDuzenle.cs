using DAL;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnketV2
{
    public partial class SoruDuzenle : Form
    {
        public Soru GelenSoru { get; set; }

        public SoruDuzenle()
        {
            InitializeComponent();
        }
        private void SoruDuzenle_Load(object sender, EventArgs e)
        {
            textBox1.Text = GelenSoru.SoruCumlesi;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //sorudüzenle kaydet
            //EF bir kayıtta değişiklik yapabilmesi için CONTEXT üzerinden geliyorsa mümkün
            AnketContext db = new AnketContext();
            var Duzenlenecek = db.Sorular.Find(GelenSoru.SoruID);
            Duzenlenecek.SoruCumlesi = textBox1.Text;
            db.Entry(Duzenlenecek).State = EntityState.Modified;
            db.SaveChanges();
            Form1 f = (Form1)Application.OpenForms["Form1"];
            f.YenileSorular();
            f.YenileCevaplar();
            this.Close();
        }
    }
}
