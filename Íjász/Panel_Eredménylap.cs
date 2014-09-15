using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Íjász
{
    public sealed class Panel_Eredménylap : Control
    {
        Label verseny_azon;
        Label verseny_megn = new Label();
        Label verseny_megn2 = new Label();
        Label versenysorozat_azon;
        Label versenysorozat_megn = new Label();
        Label versenysorozat_megn2 = new Label();
        Label versenyekszama = new Label();
        ComboBox combo_versenyek;
        ComboBox combo_versenysorozat;
        ComboBox combo_versenyekszama;
        CheckBox teljes;
        CheckBox mísz;

        Label verseny_nyomtat;
        Label versenysor_nyomtat;
        CheckBox verseny;
        CheckBox versenysor;

        int _versenyekszama;

        public Panel_Eredménylap()
        {
            InitializeContent();
        }

        public void InitializeContent()
        {
            Button nyomtat = new Button();
            nyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            nyomtat.Text = "Nyomtat";
            nyomtat.Size = new System.Drawing.Size(96, 32);
            nyomtat.Click += nyomtat_Click;

            versenysorozat_azon = new Label();
            versenysorozat_azon.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            versenysorozat_azon.Text = "Versenysorozat azonosító:";
            versenysorozat_azon.AutoSize = true;

            versenyekszama.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            versenyekszama.Text = "Versenyek száma:";
            versenyekszama.AutoSize = true;


            verseny_azon = new Label();
            verseny_azon.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            verseny_azon.Text = "Verseny azonosító:";
            verseny_azon.AutoSize = true;

            verseny_nyomtat = new Label();
            verseny_nyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            verseny_nyomtat.Text = "Versenyt nyomtat:";
            verseny_nyomtat.AutoSize = true;

            versenysor_nyomtat = new Label();
            versenysor_nyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            versenysor_nyomtat.Text = "Versenysorozatot nyomtat:";
            versenysor_nyomtat.AutoSize = true;

            Label eredmeny = new Label();
            eredmeny.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            eredmeny.Text = "Eredménylap típus:";
            eredmeny.AutoSize = true;

            combo_versenyek = new ComboBox();
            combo_versenyek.Size = new System.Drawing.Size(128, 24);
            combo_versenyek.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            combo_versenyek.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_versenyek.SelectedIndexChanged += combo_versenyek_SelectedIndexChanged;


            combo_versenyekszama = new ComboBox();
            combo_versenyekszama.Size = new System.Drawing.Size(128, 24);
            combo_versenyekszama.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            combo_versenyekszama.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_versenyekszama.SelectedIndexChanged += combo_versenyekszama_SelectedIndexChanged;


            List<Verseny> versenyek = Program.database.Versenyek();

            

            foreach (Verseny current in versenyek)
            {
                combo_versenyek.Items.Add(current.azonosító);
            }

            if (combo_versenyek.Items.Count != 0) combo_versenyek.SelectedIndex = 0;


            combo_versenysorozat = new ComboBox();
            combo_versenysorozat.Size = new System.Drawing.Size(128, 24);
            combo_versenysorozat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            combo_versenysorozat.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_versenysorozat.SelectedIndexChanged += combo_versenysorozat_SelectedIndexChanged;

            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
            foreach (Versenysorozat current in versenysorozatok)
                combo_versenysorozat.Items.Add(current.azonosító);

            if (combo_versenysorozat.Items.Count != 0) combo_versenysorozat.SelectedIndex = 0;




            verseny_megn.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            verseny_megn.Text = "Verseny megnevezés: ";
            verseny_megn.AutoSize = true;

            verseny_megn2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            verseny_megn2.AutoSize = true;
            verseny_megn2.Font = new System.Drawing.Font(verseny_megn2.Font, FontStyle.Underline);

            versenysorozat_megn.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            versenysorozat_megn.Text = "Versenysorozat megnevezés: ";
            versenysorozat_megn.AutoSize = true;

            versenysorozat_megn2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            versenysorozat_megn2.AutoSize = true;
            versenysorozat_megn2.Font = new System.Drawing.Font(verseny_megn2.Font, FontStyle.Underline);

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

            verseny = new CheckBox();
            verseny.AutoSize = true;
            verseny.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            verseny.FlatStyle = FlatStyle.Flat;

            versenysor = new CheckBox();
            versenysor.AutoSize = true;
            versenysor.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            versenysor.FlatStyle = FlatStyle.Flat;

            foreach (Verseny current in versenyek)
            {
                if (combo_versenyek.Text == current.azonosító)
                {
                    verseny_megn2.Text =  current.megnevezés;
                }
            }
            
            foreach (Versenysorozat current in versenysorozatok)
            {
                if (combo_versenysorozat.Text == current.azonosító)
                {
                    versenysorozat_megn2.Text = current.megnevezés;
                }
            }

            verseny_nyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, ClientRectangle.Height - 32 - 41 * 16);
            versenysor_nyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, ClientRectangle.Height - 32 - 39 * 16);

            verseny.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 48 * 16, ClientRectangle.Height - 32 - 41 * 16 - 12);
            versenysor.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 48 * 16, ClientRectangle.Height - 32 - 39 * 16 - 12);

            versenysorozat_azon.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, ClientRectangle.Height - 32 - 36 * 16);
            //--
            versenyekszama.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, ClientRectangle.Height - 32 - 33 * 16);


            verseny_azon.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, ClientRectangle.Height - 32 - 30 * 16);
            eredmeny.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 52 * 16, ClientRectangle.Height - 32 - 27 * 16);

            combo_versenysorozat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16, ClientRectangle.Height - 32 - 36 * 16);
            combo_versenyekszama.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16, ClientRectangle.Height - 32 - 33 * 16);
            combo_versenyek.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16, ClientRectangle.Height - 32 - 30 * 16);

            versenysorozat_megn.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 32 * 16, ClientRectangle.Height - 32 - 36 * 16);
            versenysorozat_megn2.Location = new System.Drawing.Point(versenysorozat_megn.Location.X + versenysorozat_megn.Width + 56, versenysorozat_megn.Location.Y);
            verseny_megn.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 32 * 16, ClientRectangle.Height - 32 - 30 * 16);
            verseny_megn2.Location = new System.Drawing.Point(verseny_megn.Location.X + verseny_megn.Width + 18, verseny_megn.Location.Y);

            teljes.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 45 * 16, ClientRectangle.Height - 32 - 27 * 16 - 8);
            mísz.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 41 * 16, ClientRectangle.Height - 32 - 27 * 16 - 8);

            nyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 150, ClientRectangle.Height - 32 - 16);

            teljes.Click += erlap_checkbox_klikk;
            mísz.Click += erlap_checkbox_klikk;

            verseny.Click += vs_checkbox_klikk;
            versenysor.Click += vs_checkbox_klikk;

            teljes.CheckState = CheckState.Unchecked;
            mísz.CheckState = CheckState.Unchecked;
            verseny.CheckState = CheckState.Unchecked;
            versenysor.CheckState = CheckState.Unchecked;


            Controls.Add(verseny_nyomtat);
            Controls.Add(versenysor_nyomtat);

            Controls.Add(verseny);
            Controls.Add(versenysor);

            Controls.Add(versenyekszama);
            Controls.Add(combo_versenyekszama);

            Controls.Add(versenysorozat_azon);
            Controls.Add(verseny_azon);
            Controls.Add(eredmeny);
            Controls.Add(combo_versenysorozat);
            Controls.Add(combo_versenyek);
            Controls.Add(versenysorozat_megn);
            Controls.Add(versenysorozat_megn2);
            Controls.Add(verseny_megn);
            Controls.Add(verseny_megn2);
            Controls.Add(teljes);
            Controls.Add(mísz);
            Controls.Add(nyomtat);
        }

        #region EventHandlers

        private void combo_versenysorozat_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
            foreach (Versenysorozat current in versenysorozatok)
            {
                if (combo_versenysorozat.Text == current.azonosító)
                {
                    versenysorozat_megn2.Text = current.megnevezés;
                }
            }

            combo_versenyekszama.Items.Clear();
            List<Verseny> versenyek = Program.database.Versenyek();
            _versenyekszama = 0;
            foreach (Verseny item in versenyek)
            {
                if (item.versenysorozat == combo_versenysorozat.Text)
                {
                    _versenyekszama++;
                    combo_versenyekszama.Items.Add(_versenyekszama);
                }
            }

            if (_versenyekszama!=0)
            {
                combo_versenyekszama.SelectedIndex = 0;
            }

        }

        private void combo_versenyek_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny current in versenyek)
            {
                if (combo_versenyek.Text == current.azonosító)
                {
                    verseny_megn2.Text = current.megnevezés;
                }
            }
        }

        private void combo_versenyekszama_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void nyomtat_Click(object _sender, EventArgs _event)
        {
            if (mísz.Checked == false && teljes.Checked == false)
            {
                MessageBox.Show("Nincs kiválasztva mísz/teljes", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (verseny.CheckState == CheckState.Checked && teljes.CheckState == CheckState.Checked)
            {
                if (combo_versenyek.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva verseny", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_verseny_teljes(combo_versenyek.Text));
                return;
            }

            else if (verseny.CheckState == CheckState.Checked && mísz.CheckState == CheckState.Checked)
            {
                if (combo_versenyek.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva verseny", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_verseny_misz(combo_versenyek.Text));
                return;
            }

            else if (versenysor.CheckState == CheckState.Checked && teljes.CheckState == CheckState.Checked)
            {
                if (combo_versenysorozat.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva versenysorozat", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (_versenyekszama == 0)
                {
                    MessageBox.Show("Nincs verseny a kiválasztott versenysorozatban", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_versenysorozat_teljes(combo_versenysorozat.Text, Convert.ToInt32(combo_versenyekszama.Text)));
                return;
            }

            else if (versenysor.CheckState == CheckState.Checked && mísz.CheckState == CheckState.Checked)
            {
                if (combo_versenysorozat.SelectedItem == null)
                {
                    MessageBox.Show("Nincs kiválasztva versenysorozat", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_versenyekszama == 0)
                {
                    MessageBox.Show("Nincs verseny a kiválasztott versenysorozatban", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Nyomtat.owndialog(Nyomtat.nyomtat_eredmenylap_versenysorozat_misz(combo_versenysorozat.Text, Convert.ToInt32(combo_versenyekszama.Text)));
                return;
            }
        }

        private void erlap_checkbox_klikk(object sender, EventArgs e)
        {
            CheckBox aktív = sender as CheckBox;
            if (aktív != teljes && aktív != null)
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

        private void vs_checkbox_klikk(object sender, EventArgs e)
        {
            CheckBox aktív = sender as CheckBox;
            if (aktív != verseny && aktív != null)
            {
                combo_versenyek.Enabled = false;
                combo_versenysorozat.Enabled = true;
                verseny.Checked = false;
                combo_versenyekszama.Enabled = true;
                versenysor.Checked = true;
            }
            else
            {
                verseny.Checked = true;
                versenysor.Checked = false;
                combo_versenyek.Enabled = true;
                combo_versenysorozat.Enabled = false;
                combo_versenyekszama.Enabled = false;

            }

        }

        public void verseny_hozzáadás(Verseny _verseny)
        {
            combo_versenyek.Items.Add(_verseny.azonosító);
        }

        public void verseny_törlés(string _verseny)
        {
            combo_versenyek.Items.Remove(_verseny);
        }

        public void verseny_módosítás(string _azonosító, Verseny _verseny)
        {
            if (_azonosító != _verseny.azonosító)
            {
                for (int current = 0; current < combo_versenyek.Items.Count; ++current)
                {
                    if (_azonosító == combo_versenyek.Items[current].ToString())
                    {
                        combo_versenyek.Items[current] = _verseny.azonosító;
                        break;
                    }
                }
            }
        }

        public void versenysorozat_hozzáadás(Versenysorozat _versenysorozat)
        {
            combo_versenysorozat.Items.Add(_versenysorozat.azonosító);
        }

        public void versenysorozat_törlés(string _versenysorozat)
        {
            combo_versenysorozat.Items.Remove(_versenysorozat);
        }

        public void versenysorozat_módosítás(string _azonosító, Versenysorozat _versenysorozat)
        {
            if (_azonosító != _versenysorozat.azonosító)
            {
                for (int current = 0; current < combo_versenysorozat.Items.Count; ++current)
                {
                    if (_azonosító == combo_versenysorozat.Items[current].ToString())
                    {
                        combo_versenysorozat.Items[current] = _versenysorozat.azonosító;
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
