using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
     public sealed class Form_Egyesulet : Form
        {
            private Egyesulet? egyesulet;
            TextBox txtAzonosito; 
            TextBox txtCim ;
            TextBox txtVezeto;
            TextBox txtTelefon1;
            TextBox txtTelefon2;
            TextBox txtEmail1;
            TextBox txtEmail2;
            CheckBox chkListazando;

            public 
            Form_Egyesulet()
            {
                InitializeContent();
                InitializeForm();
            }

            public 
            Form_Egyesulet( Egyesulet _egyesulet )
            {
                egyesulet = _egyesulet;
                InitializeContent();
                InitializeForm();
                InitializeData(egyesulet);
            }

            private void 
            InitializeForm()
            {
                Text = "Egyesület";
                ClientSize = new System.Drawing.Size(420, 300);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void 
            InitializeContent()
            {
                Label lblAzonosito = new iLabel("Egyesület neve:",new Point(16, 16 + 0 * 32),this);

                Label lblCim = new iLabel("Egyesület címe:",new Point(16, 16 + 1 * 32),this);

                Label lblVezeto = new iLabel("Vezető:",new Point(16, 16 + 2 * 32),this);

                Label lblTelefon = new iLabel("Telefon:",new Point(16, 16 + 3 * 32),this);

                Label lblEmail = new iLabel("E-mail:",new Point(16, 16 + 5 * 32),this);

                Label lblListazando = new iLabel("Listázandó:",new Point(16, 16 + 7 * 32),this);

                txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Width + 32 + 16, lblAzonosito.Location.Y ),
                                                    30,
                                                    new Size( 128 + 2* 64, 24),
                                                    null,
                                                    this); 

                txtCim = new iTextBox( new Point( txtAzonosito.Location.X,lblCim.Location.Y ),
                                                    30,
                                                    new Size(128 + 2 * 64, 24),
                                                    null,
                                                    this); 

                txtVezeto = new iTextBox( new Point( txtAzonosito.Location.X,lblVezeto.Location.Y ),
                                                    30,
                                                    new Size(128 + 2 * 64, 24),
                                                    null,
                                                    this); 

                txtTelefon1 =new iTextBox( new Point( txtAzonosito.Location.X,lblTelefon.Location.Y ),
                                                    30,
                                                    new Size( 128 + 2* 64, 24),
                                                    null,
                                                    this);

                txtTelefon2 = new iTextBox(new Point(txtAzonosito.Location.X, lblTelefon.Location.Y + 32),
                                                    30,
                                                    new Size(128 + 2* 64, 24),
                                                    null,
                                                    this);

                txtEmail1 = new iTextBox( new Point( txtAzonosito.Location.X,lblEmail.Location.Y ),
                                                    30,
                                                    new Size(128 + 2 * 64, 24),
                                                    null,
                                                    this);

                txtEmail2 = new iTextBox(new Point(txtAzonosito.Location.X, lblEmail.Location.Y + 32),
                                                    30,
                                                    new Size(128 + 2 * 64, 24),
                                                    null,
                                                    this);

                chkListazando = new iCheckBox( "",
                                                new Point(txtAzonosito.Location.X,lblListazando.Location.Y),
                                                null,
                                                this );
                chkListazando.Checked = true;

                Button btnRendben = new iButton("Rendben",
                                                new Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16),
                                                new Size(96, 32),
                                                btnRendben_Click,
                                                this);
                /*teszteléshez

                txtAzonosito.Text = "azonosito";
                txtCim.Text = "cim";
                txtVezeto.Text = "vezeto bela";
                txtTelefon1.Text = "telo1sdafdadfs";
                txtTelefon2.Text = "telo2";
                txtEmail1.Text = "email1";
                txtEmail2.Text = "email2";
               */
            }

            private void 
            InitializeData(Egyesulet? _egyesulet)
            {
                txtAzonosito.Text = _egyesulet.Value.Azonosito;
                if (_egyesulet.Value.TagokSzama != 0) { txtAzonosito.Enabled = false; }
                
                txtCim.Text = _egyesulet.Value.Cim;
                txtVezeto.Text = _egyesulet.Value.Vezeto;
                txtTelefon1.Text = _egyesulet.Value.Telefon1;
                txtTelefon2.Text = _egyesulet.Value.Telefon2;
                txtEmail1.Text = _egyesulet.Value.Email1;
                txtEmail2.Text = _egyesulet.Value.Email2;
                chkListazando.Checked = _egyesulet.Value.Listazando;
            }

            #region EventHandlers

            private void btnRendben_Click(Object _sender, EventArgs _event)
            {
                if (txtAzonosito.Text.Length == 0) { MessageBox.Show("Nem megfelelő az azonosító hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (egyesulet == null)
                {
                    egyesulet = new Egyesulet(txtAzonosito.Text, 
                                                        txtCim.Text, 
                                                        txtVezeto.Text,
                                                        txtTelefon1.Text,
                                                        txtTelefon2.Text,
                                                        txtEmail1.Text,
                                                        txtEmail2.Text,
                                                        chkListazando.Checked, 
                                                        0);

                    Program.mainform.egyesuletek_panel.Egyesulet_Hozzaadas(egyesulet.Value);
                }
                else
                {
                    Egyesulet uj = new Egyesulet(txtAzonosito.Text,
                                    txtCim.Text,
                                    txtVezeto.Text,
                                    txtTelefon1.Text,
                                    txtTelefon2.Text,
                                    txtEmail1.Text,
                                    txtEmail2.Text,
                                    chkListazando.Checked,
                                    egyesulet.Value.TagokSzama);
                    Egyesulet regi = egyesulet.Value;

                    Program.mainform.egyesuletek_panel.Egyesulet_Modositas(regi,uj);
                }

                this.Close();
            }                                 
            #endregion                        
        }                                     
}
