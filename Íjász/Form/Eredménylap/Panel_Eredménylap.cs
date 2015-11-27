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
        ComboBox cboVersenyAzonosito;
        ComboBox cboVersenysorozatAzonosito;
        CheckBox chkTeljes;
        CheckBox chkMisz;
        CheckBox chkEgyesulet;
        CheckBox chkReszletes;

        Label lblVersenyNyomtat;
        Label lblVersenysorozatNyomtat;
        CheckBox chkVerseny;
        CheckBox chkVersenysorozat;

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

            Button btnNyomtat = new iButton("Nyomtat",
                                            new Point(cWidth - 96 - 150,cHeight - 32 - 16),
                                            new Size(96, 32),
                                            btnNyomtat_Click,
                                            this);

            lblVersenysorozatAzonosito = new iLabel("Versenysorozat azonosító:",
                                                    new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 36 * 16),
                                                    this);

            lblVersenyAzonosito = new iLabel("Verseny azonosító:",
                                            new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 33 * 16),
                                            this);

            lblVersenyNyomtat = new iLabel("Versenyt nyomtat:",
                                            new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 41 * 16),
                                            this);

            lblVersenysorozatNyomtat = new iLabel("Versenysorozatot nyomtat:",
                                                    new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 39 * 16),
                                                    this);

            Label lblEredmeny = new iLabel("Eredménylap típus:",
                                            new Point(cWidth - 96 - 52 * 16,cHeight - 32 - 30 * 16),
                                            this);

            cboVersenyAzonosito = new iComboBox(new Point(cWidth - 96 - 42 * 16,cHeight - 32 - 33 * 16),
                                                new Size(128, 24),
                                                cboVersenyek_SelectedIndexChanged,
                                                this);

            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny current in versenyek)
                cboVersenyAzonosito.Items.Add(current.Azonosito);
            if (cboVersenyAzonosito.Items.Count != 0) cboVersenyAzonosito.SelectedIndex = 0;

            cboVersenysorozatAzonosito = new iComboBox( new Point(cWidth - 96 - 42 * 16,cHeight - 32 - 36 * 16),
                                                        new Size(128, 24),
                                                        cboVersenysorozatAzonosito_SelectedIndexChanged,
                                                        this);

            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
            foreach (Versenysorozat current in versenysorozatok)
                cboVersenysorozatAzonosito.Items.Add(current.azonosító);
            if (cboVersenysorozatAzonosito.Items.Count != 0) cboVersenysorozatAzonosito.SelectedIndex = 0;

            lblVersenyMegnevezes = new iLabel("Verseny megnevezés: ",
                                                     new Point(cWidth - 96 - 32 * 16,cHeight - 32 - 33 * 16),
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
                                      new Point(cWidth - 96 - 45 * 16,cHeight - 32 - 30 * 16 - 8),
                                      Eredmenylap_Click,
                                      this);

            chkMisz = new iCheckBox("MÍSZ",
                                new Point(cWidth - 96 - 41 * 16,cHeight - 32 - 30 * 16 - 8),
                                Eredmenylap_Click,
                                this);

            chkEgyesulet = new iCheckBox("Egyesület",
                                         new Point( cWidth - 96 - 36 * 16,cHeight - 32 - 30 * 16 - 8 ),
                                         Eredmenylap_Click,
                                         this);

            chkReszletes= new iCheckBox("Részletes",
                                         new Point(cWidth - 96 - 31 * 16, cHeight - 32 - 30 * 16 - 8),
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
                if (cboVersenyAzonosito.Text == current.Azonosito)
                {
                    lblVersenyMegnevezes2.Text = current.Megnevezes;
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

        }

        private void 
        cboVersenyek_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            List<Verseny> Versenyek = Program.database.Versenyek();
            foreach (Verseny current in Versenyek)
            {
                if (cboVersenyAzonosito.Text == current.Azonosito)
                {
                    lblVersenyMegnevezes2.Text = current.Megnevezes;
                }
            }
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

            if ( chkVerseny.CheckState == CheckState.Checked && chkTeljes.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog( Nyomtat.NyomtatEredmenylapVersenyTeljes( cboVersenyAzonosito.Text ) );
                return;
            }

            if (chkVerseny.CheckState == CheckState.Checked && chkMisz.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog( Nyomtat.NyomtatEredmenylapVersenyMisz( cboVersenyAzonosito.Text ) );
                return;
            }

            if (chkVersenysorozat.CheckState == CheckState.Checked && chkTeljes.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog( Nyomtat.NyomtatEredmenylapVersenysorozatTeljes(
                                    cboVersenysorozatAzonosito.Text));
                return;
            }

            if (chkVersenysorozat.CheckState == CheckState.Checked && chkMisz.CheckState == CheckState.Checked)
            {

                Nyomtat.Dialog( Nyomtat.NyomtatEredmenylapVersenysorozatMisz(
                                    cboVersenysorozatAzonosito.Text));
                return;
            }

            if( chkVerseny.CheckState == CheckState.Checked && chkEgyesulet.CheckState == CheckState.Checked  )
            {
                Nyomtat.Dialog(Nyomtat.NyomtatEredmenylapVersenyEgyesulet( cboVersenyAzonosito.Text ));

            }

            if( chkVersenysorozat.CheckState == CheckState.Checked && chkEgyesulet.CheckState == CheckState.Checked  )
            {
                Nyomtat.Dialog( Nyomtat.NyomtatEredmenylapVersenysorozatEgyesulet( cboVersenysorozatAzonosito.Text ) );
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
                chkVersenysorozat.Checked = true;
                chkReszletes.Enabled = true;
            }
            else
            {
                chkVerseny.Checked = true;
                chkVersenysorozat.Checked = false;
                cboVersenyAzonosito.Enabled = true;
                cboVersenysorozatAzonosito.Enabled = false;
                chkReszletes.Enabled = false;
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
        VersenyModositas(string _azonosito, Verseny _verseny)
        {
            if (_azonosito != _verseny.Azonosito)
            {
                for (int current = 0; current < cboVersenyAzonosito.Items.Count; ++current)
                {
                    if (_azonosito == cboVersenyAzonosito.Items[current].ToString())
                    {
                        cboVersenyAzonosito.Items[current] = _verseny.Azonosito;
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
