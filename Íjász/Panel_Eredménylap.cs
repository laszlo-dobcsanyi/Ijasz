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
        CheckBox chkTeljes;
        CheckBox chkMisz;
        CheckBox chkEgyesulet;
        CheckBox chkReszletes;

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
            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            Button btnNyomtat = new iButton("Nyomtat,",
                                            new Point(cWidth - 96 - 150,cHeight - 32 - 16),
                                            new Size(96, 32),
                                            btnNyomtat_Click,
                                            this);
 

            lblVersenysorozatAzonosito = new iLabel("Versenysorozat azonosító:",
                                                    new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 36 * 16),
                                                    this);
  
            lblVersenyekSzama = new iLabel("Versenyek száma:",
                                            new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 33 * 16),
                                            this);

            lblVersenyAzonosito = new iLabel("Verseny azonosító:",
                                            new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 30 * 16),
                                            this);

            lblVersenyNyomtat = new iLabel("Versenyt nyomtat:",
                                            new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 41 * 16),
                                            this);

            lblVersenysorozatNyomtat = new iLabel("Versenysorozatot nyomtat:",
                                                    new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 39 * 16),
                                                    this);

            Label lblEredmeny = new iLabel("Eredménylap típus:",
                                            new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 27 * 16),
                                            this);

            cboVersenyAzonosito = new iComboBox(new Point(cWidth - 96 - 42 * 16,cHeight - 32 - 30 * 16),
                                                new Size(128, 24),
                                                cboVersenyek_SelectedIndexChanged,
                                                this);

            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny current in versenyek)
                cboVersenyAzonosito.Items.Add(current.azonosító);
            if (cboVersenyAzonosito.Items.Count != 0) cboVersenyAzonosito.SelectedIndex = 0;

            cboVersenyekSzama = new iComboBox( new Point(cWidth - 96 - 42 * 16,cHeight - 32 - 33 * 16),
                                                new Size(128, 24),
                                                cboVersenyekSzama_SelectedIndexChanged,
                                                this);

            cboVersenysorozatAzonosito = new iComboBox( new Point(cWidth - 96 - 42 * 16,cHeight - 32 - 36 * 16),
                                                        new Size(128, 24),
                                                        cboVersenysorozatAzonosito_SelectedIndexChanged,
                                                        this);

            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
            foreach (Versenysorozat current in versenysorozatok)
                cboVersenysorozatAzonosito.Items.Add(current.azonosító);
            if (cboVersenysorozatAzonosito.Items.Count != 0) cboVersenysorozatAzonosito.SelectedIndex = 0;

            lblVersenyMegnevezes = new iLabel("Verseny megnevezés: ",
                                                     new Point(cWidth - 96 - 32 * 16,cHeight - 32 - 30 * 16),
                                                     this);

            lblVersenyMegnevezes2 = new iLabel( "",
                                                new Point(lblVersenyMegnevezes.Location.X + lblVersenyMegnevezes.Width + 18,lblVersenyMegnevezes.Location.Y),
                                                this);
            lblVersenyMegnevezes2.Font = new Font(lblVersenyMegnevezes2.Font, FontStyle.Underline);

            lblVersenysorozatMegnevezes = new iLabel("Versenysorozat megnevezés: ",
                                                            new Point(cWidth - 96 - 32 * 16,cHeight - 32 - 36 * 16),
                                                            this);

            lblVersenysorozatMegnevezes2 = new iLabel("",
                                                      new Point(lblVersenysorozatMegnevezes.Location.X + lblVersenysorozatMegnevezes.Width + 56,lblVersenysorozatMegnevezes.Location.Y),
                                                      this);

            chkTeljes = new iCheckBox("Teljes",
                                      new Point(cWidth - 96 - 45 * 16,cHeight - 32 - 27 * 16 - 8),
                                      Eredmenylap_Click,
                                      this);

            chkMisz = new iCheckBox("MÍSZ",
                                new Point(cWidth - 96 - 41 * 16,cHeight - 32 - 27 * 16 - 8),
                                Eredmenylap_Click,
                                this);

            chkEgyesulet = new iCheckBox("Egyesület",
                                         new Point( cWidth - 96 - 36 * 16,cHeight - 32 - 27 * 16 - 8 ),
                                         Eredmenylap_Click,
                                         this);

            chkReszletes= new iCheckBox("Részletes",
                                         new Point(cWidth - 96 - 31 * 16, cHeight - 32 - 27 * 16 - 8),
                                         Eredmenylap_Click,
                                         this);


            chkVerseny = new iCheckBox("",
                                        new Point(cWidth - 96 - 48 * 16,cHeight - 32 - 41 * 16 - 12),
                                        Versenysorozat_Click,
                                        this);

            chkVersenysorozat = new iCheckBox( "",
                                               new Point(cWidth - 96 - 48 * 16,cHeight - 32 - 39 * 16 - 12),
                                               Versenysorozat_Click,
                                               this);
                                                

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
        btnNyomtat_Click(object _sender, EventArgs _event)
        {
                //nincs verseny versenysorozat pipa
            if (chkVerseny.Checked == false && chkVersenysorozat.Checked == false) return;

                //van pipa verseny de nincs kiválasztva
            if(chkVerseny.Checked && cboVersenyAzonosito.SelectedItem==null) return;

                //van pipa versenysorozat de nincs kiválasztva
            if(chkVersenysorozat.Checked && cboVersenysorozatAzonosito.SelectedItem == null) return;
            
                //nincs eredménylap
            if (chkTeljes.Checked == false && chkMisz.Checked == false && chkEgyesulet.Checked == false && chkReszletes.Checked == false) return;

                // nincs verseny a vs-ban
            if (chkVersenysorozat.Checked && VersenyekSzama == 0) return;

            if ( chkVerseny.CheckState == CheckState.Checked && chkTeljes.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog(Nyomtat.nyomtat_eredmenylap_verseny_teljes(cboVersenyAzonosito.Text));
                return;
            }

            if (chkVerseny.CheckState == CheckState.Checked && chkMisz.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog(Nyomtat.nyomtat_eredmenylap_verseny_misz(cboVersenyAzonosito.Text));
                return;
            }

            if (chkVersenysorozat.CheckState == CheckState.Checked && chkTeljes.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog(Nyomtat.nyomtat_eredmenylap_versenysorozat_teljes(
                                    cboVersenysorozatAzonosito.Text, Convert.ToInt32(cboVersenyekSzama.Text)));
                return;
            }

            if (chkVersenysorozat.CheckState == CheckState.Checked && chkMisz.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog(Nyomtat.nyomtat_eredmenylap_versenysorozat_misz(
                                    cboVersenysorozatAzonosito.Text, Convert.ToInt32(cboVersenyekSzama.Text)));
                return;
            }

            if( chkVerseny.CheckState == CheckState.Checked && chkEgyesulet.CheckState == CheckState.Checked  )
            {
                Nyomtat.Dialog(Nyomtat.NyomtatEredmenylapVersenyEgyesulet( cboVersenyAzonosito.Text ));

            }

            if( chkVersenysorozat.CheckState == CheckState.Checked && chkEgyesulet.CheckState == CheckState.Checked  )
            {
                Nyomtat.Dialog(Nyomtat.NyomtatEredmenylapVersenySorozatEgyesulet(cboVersenysorozatAzonosito.Text));
            }

            if( chkVersenysorozat.CheckState == CheckState.Checked && chkReszletes.CheckState == CheckState.Checked  )
            {
                Nyomtat.Dialog(Nyomtat.NyomtatEredmenylapVersenySorozatReszletes(cboVersenysorozatAzonosito.Text));

            }
        }

        private void 
        Eredmenylap_Click(object _sender, EventArgs _event)
        {
            CheckBox Aktiv = _sender as CheckBox;
            if(Aktiv==null) return ;

            if(Aktiv == chkTeljes)
            {
                chkTeljes.Checked = true;
                chkMisz.Checked = false;
                chkEgyesulet.Checked = false;
                chkReszletes.Checked = false;
            }
            else if (Aktiv == chkMisz)
            {
                chkMisz.Checked = true;
                chkTeljes.Checked = false;
                chkEgyesulet.Checked = false;
                chkReszletes.Checked = false;
            }
            else if (Aktiv == chkEgyesulet)
            {
                chkEgyesulet.Checked = true;
                chkTeljes.Checked = false;
                chkMisz.Checked = false;
                chkReszletes.Checked = false;
            }
            else if (Aktiv == chkReszletes )
            {
                chkReszletes.Checked = true;
                chkEgyesulet.Checked = false;
                chkTeljes.Checked = false;
                chkMisz.Checked = false;
            }

        }

        private void 
        Versenysorozat_Click(object _sender, EventArgs _event)
        {
            CheckBox aktív = _sender as CheckBox;
            if (aktív != chkVerseny &&aktív != null)
            {
                cboVersenyAzonosito.Enabled = false;
                chkVerseny.Checked = false;
                cboVersenysorozatAzonosito.Enabled = true;
                cboVersenyekSzama.Enabled = true;
                chkVersenysorozat.Checked = true;
                chkReszletes.Enabled = true;
            }
            else
            {
                chkVerseny.Checked = true;
                chkVersenysorozat.Checked = false;
                cboVersenyAzonosito.Enabled = true;
                cboVersenysorozatAzonosito.Enabled = false;
                cboVersenyekSzama.Enabled = false;
                chkReszletes.Enabled = false;
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
