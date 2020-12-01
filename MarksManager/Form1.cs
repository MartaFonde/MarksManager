using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarksManager
{
    enum Urgencia
    {
        Ya, 
        Hoy, 
        Mañana
    }
    enum TipoNota
    {
        Pública,
        Empresa,
        Personal
    }
    public partial class Form1 : Form
    {
        static int[] pinValido = { 1234, 5678, 0147, 0258, 0369, 0000 };
        Form2 f2;
        int intentos = 3;
        bool valido = false;
        List<Nota> notas = new List<Nota>();
        Urgencia urgencia = Urgencia.Mañana;        //si no está check funcionalidad completa
        TipoNota tipo = TipoNota.Pública;

        public Form1()
        {
            InitializeComponent();

            txtNombre.Tag = Color.LightPink;
            txtDestinatario.Tag = Color.LightPink;
            txtNota.Tag = Color.Aquamarine;

            radioButton1.Text = Urgencia.Ya.ToString();
            radioButton1.Tag = Urgencia.Ya;
            radioButton2.Text = Urgencia.Hoy.ToString();
            radioButton2.Tag = Urgencia.Hoy;
            radioButton3.Text = Urgencia.Mañana.ToString();
            radioButton3.Tag = Urgencia.Mañana;

            radioButton4.Text = TipoNota.Pública.ToString();
            radioButton4.Tag = TipoNota.Pública;
            radioButton5.Text = TipoNota.Personal.ToString();
            radioButton5.Tag = TipoNota.Personal;
            radioButton6.Text = TipoNota.Empresa.ToString();
            radioButton6.Tag = TipoNota.Empresa;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            f2 = new Form2();
            DialogResult res;

            while (!valido && intentos > 0)
            {
                res = f2.ShowDialog();

                switch (res)
                {
                    case DialogResult.OK:
                        if (comprobarPin(f2.txtPin.Text))
                        {
                            if (pinValido.Contains(Convert.ToInt32(f2.txtPin.Text)))
                            {
                                f2.Close();
                                valido = true;
                                if (f2.chkCompleta.Checked)
                                {
                                    groupBox1.Visible = true;
                                    groupBox2.Visible = true;
                                }
                            }
                            else
                            {
                                f2.lblError.Text = "Error. PIN no válido.\n Tienes " + (--intentos) + " intentos";
                            }
                        }
                        break;

                    case DialogResult.Cancel:
                        valido = true;
                        Environment.Exit(0);
                        //f2.Close();
                        //this.Close();
                        break;
                }
            }
            if (intentos == 0)
            {
                f2.Close();
                //this.Close();
                Environment.Exit(0);
            }
        }

        private bool comprobarPin(string pin)
        {
            try
            {
                int n = Convert.ToInt32(pin);
            }
            catch (FormatException)
            {
                f2.lblError.Text = "Error. PIN son números.\n Intentos restantes: " + (--intentos);
                return false;
            }
            catch (ArgumentNullException)
            {
                f2.lblError.Text = "Error. Introduce algo.\n Intentos restantes: " + (--intentos);
                return false;
            }
            catch (OverflowException)
            {
                f2.lblError.Text = "Error. PIN demasiado largo.\n Intentos restantes: " + (--intentos);
                return false;
            }
            return true;
        }

        private void txt_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = (Color)((TextBox)sender).Tag;
        }

        private void txt_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("¿Estás seguro de que quieres salir?", "Confirmación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                 == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cursorEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void cursorLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void btnAlmacenar_Click(object sender, EventArgs e)
        {
            if(txtNombre.Text.Trim().Length > 0 && txtDestinatario.Text.Trim().Length > 0 && txtNota.Text.Trim().Length > 0)
            {
                Nota n = new Nota(txtNombre.Text.Trim(), txtDestinatario.Text.Trim(), txtNota.Text.Trim(), urgencia, tipo);
                notas.Add(n);
                txtNombre.Text = "";
                txtDestinatario.Text = "";
                txtNota.Text = "";
            }
        }

        private void rbCheckUrgencia(object sender, EventArgs e)
        {
            if(((RadioButton)sender).Checked){
                urgencia = (Urgencia)((RadioButton)sender).Tag;
            }
        }

        private void rbCheckTipo(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                tipo = (TipoNota)((RadioButton)sender).Tag;
            }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            txtNota.Text = "";
            foreach (Nota n in notas)
            {
                txtNota.AppendText("Nombre: " + n.Nombre + "\r\nDestinatario: " + n.Destinatario + "\r\nNota: " + n.Texto +
                    "\r\nUrgencia: " + n.Urgencia + "\r\nTipo de nota: " + n.Tipo + "\r\n");                       
                txtNota.AppendText("\r\n********************\r\n");
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            DialogResult res = f3.ShowDialog();
            int ind;

            switch (res)
            {
                case DialogResult.OK:
                    if(Int32.TryParse(f3.textBox1.Text, out ind))
                    {
                        if(ind >= 0 && ind < notas.Count)
                        {
                            notas.RemoveAt(ind);
                        }
                        else
                        {
                            MessageBox.Show("El índice indicado no está definido en la colección", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    f3.Close();
                    break;
                case DialogResult.Cancel:
                    f3.Close();
                    break;
            }
        }

        private void borrarNotas(Object sender, EventArgs e)
        {
            txtNota.Text = "";
        }
    }
}
