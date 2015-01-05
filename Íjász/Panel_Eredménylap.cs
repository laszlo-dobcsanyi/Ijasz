using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Íjász
{
    public sealed class Panel_Eredménylap : Control
    {
        Label lblVersenyAzonosito;
        Label lblVersenyMegnevezes = new Label();
        Label lblVersenyMegnevezes2 = new Label();
        Label lblVersenysorozatAzonosito;
        Label lblVersenysorozatMegnevezes = new Label();
        Label lblVersenysorozatMegnevezes2 = new Label();
        Label lblVersenyekSzama = new Label();
        ComboBox cboVersenyAzonosito;
        ComboBox cboVersenysorozatAzonosito;
        ComboBox cboVersenyekSzama;
        CheckBox teljes;
        CheckBox mísz;

        Label lblVersenyNyomtat;
        Label lblVersenysorozatNyomtat;
        CheckBox chkVerseny;
        CheckBox chkVersenysorozat;

        int VersenyekSzama;

        public
        Panel_Eredménylap()
        {
            InitializeContent();
        }

        public void 
        InitializeContent()
        {
            Button nyomtat = new Button();
            nyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            nyomtat.Text = "Nyomtat";
            nyomtat.Size = new System.Drawing.Size(96, 32);
            nyomtat.Click += Nyomtat_Click;

            lblVersenysorozatAzonosito = new Label();
            lblVersenysorozatAzonosito.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenysorozatAzonosito.Text = "Versenysorozat azonosító:";
            lblVersenysorozatAzonosito.AutoSize = true;

            lblVersenyekSzama.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenyekSzama.Text = "Versenyek száma:";
            lblVersenyekSzama.AutoSize = true;


            lblVersenyAzonosito = new Label();
            lblVersenyAzonosito.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenyAzonosito.Text = "Verseny azonosító:";
            lblVersenyAzonosito.AutoSize = true;

            lblVersenyNyomtat = new Label();
            lblVersenyNyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenyNyomtat.Text = "Versenyt nyomtat:";
            lblVersenyNyomtat.AutoSize = true;

            lblVersenysorozatNyomtat = new Label();
            lblVersenysorozatNyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenysorozatNyomtat.Text = "Versenysorozatot nyomtat:";
            lblVersenysorozatNyomtat.AutoSize = true;

            Label eredmeny = new Label();
            eredmeny.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            eredmeny.Text = "Eredménylap típus:";
            eredmeny.AutoSize = true;

            cboVersenyAzonosito = new ComboBox();
            cboVersenyAzonosito.Size = new System.Drawing.Size(128, 24);
            cboVersenyAzonosito.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            cboVersenyAzonosito.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVersenyAzonosito.SelectedIndexChanged += cboVersenyek_SelectedIndexChanged;


            cboVersenyekSzama = new ComboBox();
            cboVersenyekSzama.Size = new System.Drawing.Size(128, 24);
            cboVersenyekSzama.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            cboVersenyekSzama.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVersenyekSzama.SelectedIndexChanged += cboVersenyekSzama_SelectedIndexChanged;


            List<Verseny> versenyek = Program.database.Versenyek();

            

            foreach (Verseny current in versenyek)
            {
                cboVersenyAzonosito.Items.Add(current.azonosító);
            }

            if (cboVersenyAzonosito.Items.Count != 0) cboVersenyAzonosito.SelectedIndex = 0;


            cboVersenysorozatAzonosito = new ComboBox();
            cboVersenysorozatAzonosito.Size = new System.Drawing.Size(128, 24);
            cboVersenysorozatAzonosito.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            cboVersenysorozatAzonosito.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVersenysorozatAzonosito.SelectedIndexChanged += cboVersenysorozatAzonosito_SelectedIndexChanged;

            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
            foreach (Versenysorozat current in versenysorozatok)
                cboVersenysorozatAzonosito.Items.Add(current.azonosító);

            if (cboVersenysorozatAzonosito.Items.Count != 0) cboVersenysorozatAzonosito.SelectedIndex = 0;




            lblVersenyMegnevezes.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenyMegnevezes.Text = "Verseny megnevezés: ";
            lblVersenyMegnevezes.AutoSize = true;

            lblVersenyMegnevezes2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenyMegnevezes2.AutoSize = true;
            lblVersenyMegnevezes2.Font = new System.Drawing.Font(lblVersenyMegnevezes2.Font, FontStyle.Underline);

            lblVersenysorozatMegnevezes.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenysorozatMegnevezes.Text = "Versenysorozat megnevezés: ";
            lblVersenysorozatMegnevezes.AutoSize = true;

            lblVersenysorozatMegnevezes2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblVersenysorozatMegnevezes2.AutoSize = true;
            lblVersenysorozatMegnevezes2.Font = new System.Drawing.Font(lblVersenyMegnevezes2.Font, FontStyle.Underline);

            teljes = new CheckBox();
            teljes.Text = "Teljes";
            teljes.AutoSize = true;
            teljes.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            teljes.FlatStyle = FlatStyle.Flat;

            mísz = new CheckBox();
            mísz.Text = "MÍSZ";
            mísz.AutoSize = true;
            mísz.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            mísz.FlatStyle = FlatStyle.Flat;

            chkVerseny = new CheckBox();
            chkVerseny.AutoSize = true;
            chkVerseny.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            chkVerseny.FlatStyle = FlatStyle.Flat;

            chkVersenysorozat = new CheckBox();
            chkVersenysorozat.AutoSize = true;
            chkVersenysorozat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            chkVersenysorozat.FlatStyle = FlatStyle.Flat;

            foreach (Verseny current in versenyek)
            {
                if (cboVersenyAzonosito.Text == current.azonosító)
                {
                    lblVersenyMegnevezes2.Text = current.megnevezés;
                }
            }
            
            foreach (Versenysorozat current in versenysorozatok)
            {
                if (cboVersenysorozatAzonosito.Text == current.azonosító)
                {
                    lblVersenysorozatMegnevezes2.Text = current.megnevezés;
                }
            }

            lblVersenyNyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, 
                                                                  ClientRectangle.Height - 32 - 41 * 16);

            lblVersenysorozatNyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, 
                                                                         ClientRectangle.Height - 32 - 39 * 16);

            chkVerseny.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 48 * 16,
                                                           ClientRectangle.Height - 32 - 41 * 16 - 12);

            chkVersenysorozat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 48 * 16, 
                                                                  ClientRectangle.Height - 32 - 39 * 16 - 12);

            lblVersenysorozatAzonosito.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16,
                                                                           ClientRectangle.Height - 32 - 36 * 16);
            //--
            lblVersenyekSzama.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, 
                                                                  ClientRectangle.Height - 32 - 33 * 16);

            lblVersenyAzonosito.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, 
                                                                    ClientRectangle.Height - 32 - 30 * 16);

            eredmeny.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16,
                                                         ClientRectangle.Height - 32 - 27 * 16);

            cboVersenysorozatAzonosito.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16,
                                                                           ClientRectangle.Height - 32 - 36 * 16);

            cboVersenyekSzama.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16,
                                                                  ClientRectangle.Height - 32 - 33 * 16);

            cboVersenyAzonosito.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16, 
                                                                    ClientRectangle.Height - 32 - 30 * 16);

            lblVersenysorozatMegnevezes.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 32 * 16,
                                                                            ClientRectangle.Height - 32 - 36 * 16);

            lblVersenysorozatMegnevezes2.Location = new System.Drawing.Point(lblVersenysorozatMegnevezes.Location.X + lblVersenysorozatMegnevezes.Width + 56, 
                                                                             lblVersenysorozatMegnevezes.Location.Y);

            lblVersenyMegnevezes.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 32 * 16, 
                                                                     ClientRectangle.Height - 32 - 30 * 16);

            lblVersenyMegnevezes2.Location = new System.Drawing.Point(lblVersenyMegnevezes.Location.X + lblVersenyMegnevezes.Width + 18, 
                                                                      lblVersenyMegnevezes.Location.Y);

            teljes.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 45 * 16, 
                                                       ClientRectangle.Height - 32 - 27 * 16 - 8);

            mísz.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 41 * 16, 
                                                     ClientRectangle.Height - 32 - 27 * 16 - 8);

            nyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 150,
                                                        ClientRectangle.Height - 32 - 16);

            teljes.Click += Eredmenylap_Click;
            mísz.Click += Eredmenylap_Click;

            chkVerseny.Click += Versenysorozat_Click;
            chkVersenysorozat.Click += Versenysorozat_Click;

            teljes.CheckState = CheckState.Unchecked;
            mísz.CheckState = CheckState.Unchecked;
            chkVerseny.CheckState = CheckState.Unchecked;
            chkVersenysorozat.CheckState = CheckState.Unchecked;


            Controls.Add(lblVersenyNyomtat);
            Controls.Add(lblVersenysorozatNyomtat);

            Controls.Add(chkVerseny);
            Controls.Add(chkVersenysorozat);

            Controls.Add(lblVersenyekSzama);
            Controls.Add(cboVersenyekSzama);

            Controls.Add(lblVersenysorozatAzonosito);
            Controls.Add(lblVersenyAzonosito);
            Controls.Add(eredmeny);
            Controls.Add(cboVersenysorozatAzonosito);
            Controls.Add(cboVersenyAzonosito);
            Controls.Add(lblVersenysorozatMegnevezes);
            Controls.Add(lblVersenysorozatMegnevezes2);
            Controls.Add(lblVersenyMegnevezes);
            Controls.Add(lblVersenyMegnevezes2);
            Controls.Add(teljes);
            Controls.Add(mísz);
            Controls.Add(nyomtat);
        }

        #region EventHandlers

        private void 
        cboVersenysorozatAzonosito_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            List<Versenysorozat> Versenysorozatok = Program.database.Versenysorozatok();
            foreach (Versenysorozat current in Versenysorozatok)
            {
                if (cboVersenysorozatAzonosito.Text == current.azonosító)
                {
                    lblVersenysorozatMegnevezes2.Text = current.megnevezés;
                }
            }

            cboVersenyekSzama.Items.Clear();
            List<Verseny> Versenyek = Program.database.Versenyek();
            VersenyekSzama = 0;
            foreach (Verseny item in Versenyek)
            {
                if (item.versenysorozat == cboVersenysorozatAzonosito.Text)
                {
                    VersenyekSzama++;
                    cboVersenyekSzama.Items.Add(VersenyekSzama);
                }
            }

            if (VersenyekSzama!=0)
            {
                cboVersenyekSzama.SelectedIndex = 0;
            }
        }

        private void 
        cboVersenyek_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            List<Verseny> Versenyek = Program.database.Versenyek();
            foreach (Verseny current in Versenyek)
            {
                if (cboVersenyAzonosito.Text == current.azonosító)
                {
                    lblVersenyMegnevezes2.Text = current.megnevezés;
                }
            }
        }

        private void 
        cboVersenyekSzama_SelectedIndexChanged(object sender, EventArgs _event)
        {
            //TODO vagy ezmiez??
        }

        private void 
        Nyomtat_Click(object _sender, EventArgs _event)
        {
            if (mísz.Checked == false && teljes.Checked == false)
            {
                MessageBox.Show("Nincs kiválasztva mísz/teljes", "Hiba", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkVerseny.CheckState == CheckState.Checked && teljes.CheckState == CheckState.Checked)
            {
                if (cboVersenyAzonosito.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva verseny", "Hiba", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_verseny_teljes(cboVersenyAzonosito.Text));
                return;
            }

            else if (chkVerseny.CheckState == CheckState.Checked && mísz.CheckState == CheckState.Checked)
            {
                if (cboVersenyAzonosito.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva verseny", "Hiba", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_verseny_misz(cboVersenyAzonosito.Text));
                return;
            }

            else if (chkVersenysorozat.CheckState == CheckState.Checked && teljes.CheckState == CheckState.Checked)
            {
                if (cboVersenysorozatAzonosito.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva versenysorozat", "Hiba",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (VersenyekSzama == 0)
                {
                    MessageBox.Show("Nincs verseny a kiválasztott versenysorozatban", "Hiba", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_versenysorozat_teljes(
                                    cboVersenysorozatAzonosito.Text, Convert.ToInt32(cboVersenyekSzama.Text)));
                return;
            }

            else if (chkVersenysorozat.CheckState == CheckState.Checked && mísz.CheckState == CheckState.Checked)
            {
                if (cboVersenysorozatAzonosito.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva versenysorozat", "Hiba",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (VersenyekSzama == 0)
                {
                    MessageBox.Show("Nincs verseny a kiválasztott versenysorozatban", "Hiba", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_versenysorozat_misz(
                                    cboVersenysorozatAzonosito.Text, Convert.ToInt32(cboVersenyekSzama.Text)));
                return;
            }
        }

        private void 
        Eredmenylap_Click(object _sender, EventArgs _event)
        {
            CheckBox Aktiv = _sender as CheckBox;
            if (Aktiv != teljes && Aktiv != null)
            {
                teljes.Checked = false;
                mísz.CheckState = CheckState.Checked;
            }
            else
            {
                teljes.Checked = true;
                mísz.CheckState = CheckState.Unchecked;
            }
        }

        private void 
        Versenysorozat_Click(object _sender, EventArgs _event)
        {
            CheckBox aktív = _sender as CheckBox;
            if (aktív != chkVerseny && aktív != null)
            {
                cboVersenyAzonosito.Enabled = false;
                cboVersenysorozatAzonosito.Enabled = true;
                chkVerseny.Checked = false;
                cboVersenyekSzama.Enabled = true;
                chkVersenysorozat.Checked = true;
            }
            else
            {
                chkVerseny.Checked = true;
                chkVersenysorozat.Checked = false;
                cboVersenyAzonosito.Enabled = true;
                cboVersenysorozatAzonosito.Enabled = false;
                cboVersenyekSzama.Enabled = false;
            }
        }

        public void 
        VersenyHozzaadas(Verseny _verseny)
        {
            cboVersenyAzonosito.Items.Add(_verseny.azonosító);
        }

        public void
        VersenyTorles(string _verseny)
        {
            cboVersenyAzonosito.Items.Remove(_verseny);
        }

        public void 
        VersenyModositas(string _azonosito, Verseny _verseny)
        {
            if (_azonosito != _verseny.azonosító)
            {
                for (int current = 0; current < cboVersenyAzonosito.Items.Count; ++current)
                {
                    if (_azonosito == cboVersenyAzonosito.Items[current].ToString())
                    {
                        cboVersenyAzonosito.Items[current] = _verseny.azonosító;
                        break;
                    }
                }
            }
        }

        public void 
        VersenysorozatHozzáadás(Versenysorozat _versenysorozat)
        {
            cboVersenysorozatAzonosito.Items.Add(_versenysorozat.azonosító);
        }

        public void 
        VersenysorozatTorles(string _versenysorozat)
        {
            cboVersenysorozatAzonosito.Items.Remove(_versenysorozat);
        }

        public void 
        VersenysorozatModositas(string _azonosító, Versenysorozat _versenysorozat)
        {
            if (_azonosító != _versenysorozat.azonosító)
            {
                for (int current = 0; current < cboVersenysorozatAzonosito.Items.Count; ++current)
                {
                    if (_azonosító == cboVersenysorozatAzonosito.Items[current].ToString())
                    {
                        cboVersenysorozatAzonosito.Items[current] = _versenysorozat.azonosító;
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
