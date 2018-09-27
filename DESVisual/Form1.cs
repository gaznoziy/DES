using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace DESVisual
{
    public partial class Form1 : Form
    { 
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var des = new DES();

            var text = this.textBox1.Text;
            var key = textBox2.Text;

            var result = des.Encrypt(text, key);
            this.textBox3.Text = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var des = new DES();
            var text = this.textBox1.Text;
            var key = textBox2.Text;

            var result = des.Decrypt(text, key);
            this.textBox3.Text = result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
