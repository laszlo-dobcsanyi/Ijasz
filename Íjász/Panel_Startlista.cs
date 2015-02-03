using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Íjász
{
    public sealed class Panel_Startlista : Control
    {
        private ComboBox cboVersenyAzonosito;
        Label lblVersenyAzonosito;
        Label lblMegnevezes = new Label();
        Label lblMegnevezes2 = new Label();
        CheckBox chkCsapatlista;
        CheckBox chkNevezesiLista;
        CheckBox chkNemMegjelent;

        public
        Panel_Startlista()
        {
            InitializeContent();
        }

        public void
        InitializeContent()
        {
            Button btnNyomtat = new Button();
            btnNyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnNyomtat.Text = "Nyomtat";
            btnNyomtat.Size = new System.Drawing.Size(96, 32);
            btnNyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 150,
                                                           ClientRectangle.Height - 32 - 16);
            btnNyomtat.Click += btnNyomtat_Click;

            lblVersenyAzonosito = new Label();
            lblVersenyAzonosito.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenyAzonosito.Text = "Verseny azonosító:";
            lblVersenyAzonosito.AutoSize = true;
            lblVersenyAzonosito.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 50 * 16,
                                                                    ClientRectangle.Height - 32 - 40 * 16);

            Label lblStart = new Label();
            lblStart.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblStart.Text = "Startlista típus:";
            lblStart.AutoSize = true;
            lblStart.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 50 * 16,
                                                      ClientRectangle.Height - 32 - 37 * 16 - 7);

            cboVersenyAzonosito = new ComboBox();
            cboVersenyAzonosito.Size = new System.Drawing.Size(128, 24);
            cboVersenyAzonosito.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            cboVersenyAzonosito.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16,
                                                                    ClientRectangle.Height - 32 - 40 * 16);
            cboVersenyAzonosito.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVersenyAzonosito.SelectedIndexChanged += cboVersenyAzonosito_SelectedIndexChanged;

            List<Verseny> Versenyek = Program.database.Versenyek();
            foreach (Verseny current in Versenyek)
            {
                cboVersenyAzonosito.Items.Add(current.Azonosito);
            }

            if (cboVersenyAzonosito.Items.Count != 0)
            {
                cboVersenyAzonosito.SelectedIndex = 0;
            }

            lblMegnevezes.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblMegnevezes.Text = "Verseny Megnevezés: ";
            lblMegnevezes.AutoSize = true;
            lblMegnevezes.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 32 * 16,
                                                              ClientRectangle.Height - 32 - 40 * 16);

            lblMegnevezes2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblMegnevezes2.AutoSize = true;
            lblMegnevezes2.Location = new System.Drawing.Point(lblMegnevezes.Location.X + lblMegnevezes.Size.Width + 18,
                                                               lblMegnevezes.Location.Y);
            lblMegnevezes2.Font = new System.Drawing.Font(lblMegnevezes2.Font, FontStyle.Underline);

            foreach (Verseny current in Versenyek)
            {
                if (cboVersenyAzonosito.Text == current.Azonosito)
                {
                    lblMegnevezes2.Text = current.Megnevezes;
                }
            }
            chkNevezesiLista = new CheckBox();
            chkNevezesiLista.Text = "Nevezési";
            chkNevezesiLista.AutoSize = true;
            chkNevezesiLista.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            chkNevezesiLista.FlatStyle = FlatStyle.Flat;
            chkNevezesiLista.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 44 * 16,
                                                                 ClientRectangle.Height - 32 - 38 * 16);

            chkCsapatlista = new CheckBox();
            chkCsapatlista.Text = "Csapatlista";
            chkCsapatlista.AutoSize = true;
            chkCsapatlista.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            chkCsapatlista.FlatStyle = FlatStyle.Flat;
            chkCsapatlista.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 39 * 16,
                                                               ClientRectangle.Height - 32 - 38 * 16);

            chkNemMegjelent = new CheckBox();
            chkNemMegjelent.Text = "Nem Megjelent";
            chkNemMegjelent.AutoSize = true;
            chkNemMegjelent.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            chkNemMegjelent.FlatStyle = FlatStyle.Flat;
            chkNemMegjelent.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 33 * 16,
                                                                ClientRectangle.Height - 32 - 38 * 16);

            chkNevezesiLista.Click += CheckBox_Click;
            chkCsapatlista.Click += CheckBox_Click;
            chkNemMegjelent.Click += CheckBox_Click;

            Controls.Add(chkNevezesiLista);
            Controls.Add(chkCsapatlista);
            Controls.Add(chkNemMegjelent);
            Controls.Add(lblMegnevezes);
            Controls.Add(lblMegnevezes2);
            Controls.Add(lblVersenyAzonosito);
            Controls.Add(lblStart);
            Controls.Add(cboVersenyAzonosito);
            Controls.Add(btnNyomtat);
        }

        #region EventHandlers

        private void
        cboVersenyAzonosito_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            List<Verseny> Versenyek = Program.database.Versenyek();
            foreach (Verseny current in Versenyek)
            {
                if (cboVersenyAzonosito.Text == current.Azonosito)
                {
                    lblMegnevezes2.Text = current.Megnevezes;
                }
            }
        }

        private void
        btnNyomtat_Click(object _sender, EventArgs _event)
        {
            if (cboVersenyAzonosito.Text == "") return;

            if (chkCsapatlista.Checked == false && chkNevezesiLista.Checked == false && chkNemMegjelent.Checked == false) return;

            if (chkCsapatlista.Checked == true)
            {
                Nyomtat.Dialog(Nyomtat.NyomtatCsapatlista(cboVersenyAzonosito.Text));

                return;
                // Nyomtat.print("NEVEZLISTA.docx");
            }
            if (chkNevezesiLista.Checked == true)
            {
                Nyomtat.Dialog( Nyomtat.NyomtatNevezesiLista( cboVersenyAzonosito.Text, false ) );
                return;
                // Nyomtat.print("NEVEZLISTA.docx");
            }
            if (chkNemMegjelent.Checked == true)
            {
                Nyomtat.Dialog( Nyomtat.NyomtatNevezesiLista( cboVersenyAzonosito.Text, true ) );
                return;
                // Nyomtat.print("CSAPATLISTA.docx");
            }

            Nyomtat.Dialog(cboVersenyAzonosito.Text);
        }

        private void
        CheckBox_Click(object _sender, EventArgs _event)
        {
            CheckBox chkAktiv = _sender as CheckBox;
            if (chkAktiv == chkCsapatlista)
            {
                chkCsapatlista.Checked = true;
                chkNevezesiLista.Checked = false;
                chkNemMegjelent.Checked = false;
            }
            else if (chkAktiv == chkNevezesiLista)
            {
                chkNevezesiLista.Checked = true;
                chkCsapatlista.Checked = false;
                chkNemMegjelent.Checked = false;
            }
            else if (chkAktiv == chkNemMegjelent)
            {
                chkNemMegjelent.Checked = true;
                chkNevezesiLista.Checked = false;
                chkCsapatlista.Checked = false;
            }
        }

        public void
        VersenyHozzaadas(Verseny _verseny)
        {
            cboVersenyAzonosito.Items.Add(_verseny.Azonosito);
        }

        public void
        VersenyTorles(string _verseny)
        {
            cboVersenyAzonosito.Items.Remove(_verseny);
        }

        public void
        VersenyModositas(string _versenyazonosito, Verseny _verseny)
        {
            if (_versenyazonosito != _verseny.Azonosito)
            {
                for (int current = 0; current < cboVersenyAzonosito.Items.Count; ++current)
                {
                    if (_versenyazonosito == cboVersenyAzonosito.Items[current].ToString())
                    {
                        cboVersenyAzonosito.Items[current] = _verseny.Azonosito;
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
