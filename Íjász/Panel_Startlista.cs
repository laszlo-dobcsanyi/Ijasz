using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
    public sealed class Panel_Startlista : Control
    {
        private ComboBox combo_versenyek;
        Label azon;
        Label megn = new Label();
        Label megn2 = new Label();
        CheckBox csapat;
        CheckBox nevez;
        CheckBox nemmegjelent;
        public Panel_Startlista()
        {
            InitializeContent();
        }
        public void InitializeContent()
        {
            
            Button nyomtat = new Button();
            nyomtat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            nyomtat.Text = "Nyomtat";
            nyomtat.Size = new System.Drawing.Size(96, 32);
            nyomtat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 150, ClientRectangle.Height - 32 - 16);
            nyomtat.Click += nyomtat_Click;

            azon = new Label();
            azon.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            azon.Text = "Verseny azonosító:";
            azon.AutoSize = true;
            azon.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 50 * 16, ClientRectangle.Height - 32 - 40 * 16);

            Label start = new Label();
            start.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            start.Text = "Startlista típus:";
            start.AutoSize = true;
            start.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 50 * 16, ClientRectangle.Height - 32 - 37 * 16 - 7);

            combo_versenyek = new ComboBox();
            combo_versenyek.Size = new System.Drawing.Size(128, 24);
            combo_versenyek.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            combo_versenyek.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 42 * 16, ClientRectangle.Height - 32 - 40 * 16);
            combo_versenyek.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_versenyek.SelectedIndexChanged += combo_versenyek_SelectedIndexChanged;

            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny current in versenyek)
                combo_versenyek.Items.Add(current.azonosító);

            if (combo_versenyek.Items.Count != 0) combo_versenyek.SelectedIndex = 0;

            megn.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            megn.Text = "Verseny megnevezés: ";
            megn.AutoSize = true;
            megn.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 32 * 16, ClientRectangle.Height - 32 - 40 * 16);

            megn2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            megn2.AutoSize = true;
            megn2.Location = new System.Drawing.Point(megn.Location.X + megn.Size.Width + 18, megn.Location.Y);
            megn2.Font = new System.Drawing.Font(megn2.Font, FontStyle.Underline);

            foreach (Verseny current in versenyek)
            {
                if (combo_versenyek.Text == current.azonosító)
                {
                    megn2.Text = current.megnevezés;
                }
            }
            nevez = new CheckBox();
            nevez.Text = "Nevezési";
            nevez.AutoSize = true;
            nevez.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            nevez.FlatStyle = FlatStyle.Flat;
            nevez.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 44 * 16, ClientRectangle.Height - 32 - 38 * 16);

            csapat = new CheckBox();
            csapat.Text = "Csapat";
            csapat.AutoSize = true;
            csapat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            csapat.FlatStyle = FlatStyle.Flat;
            csapat.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 40 * 16, ClientRectangle.Height - 32 - 38 * 16);

            nemmegjelent = new CheckBox();
            nemmegjelent.Text = "Nem Megjelent";
            nemmegjelent.AutoSize = true;
            nemmegjelent.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            nemmegjelent.FlatStyle = FlatStyle.Flat;
            nemmegjelent.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 33 * 16, ClientRectangle.Height - 32 - 38 * 16);

            nevez.Click += checkbox_klikk;
            csapat.Click += checkbox_klikk;
            nemmegjelent.Click += checkbox_klikk;

            Controls.Add(nevez);
            Controls.Add(csapat);
            Controls.Add(nemmegjelent);
            Controls.Add(megn);
            Controls.Add(megn2);
            Controls.Add(combo_versenyek);
            Controls.Add(azon);
            Controls.Add(start);
            Controls.Add(nyomtat);
        }

        #region EventHandlers

        private void combo_versenyek_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny current in versenyek)
            {
                if (combo_versenyek.Text == current.azonosító)
                {
                    megn2.Text = current.megnevezés;
                }
            }
        }

        private void nyomtat_Click(object _sender, EventArgs _event)
        {
            if (combo_versenyek.Text == "") return;

            if (csapat.Checked == false && nevez.Checked == false && nemmegjelent.Checked == false) return;

            if (csapat.Checked == true)
            {
                Nyomtat.owndialog(Nyomtat.nyomtat_csapatlista(combo_versenyek.Text));

                return;
                // Nyomtat.print("NEVEZLISTA.docx");
            }
            if (nevez.Checked == true)
            {
                Nyomtat.owndialog(Nyomtat.nyomtat_nevezesilista(combo_versenyek.Text, false));
                return;
                // Nyomtat.print("NEVEZLISTA.docx");
            }
            if (nemmegjelent.Checked == true)
            {
                Nyomtat.owndialog(Nyomtat.nyomtat_nevezesilista(combo_versenyek.Text, true));
                return;
                // Nyomtat.print("CSAPATLISTA.docx");
            }

            Nyomtat.owndialog(combo_versenyek.Text);




            /* DialogResult temp = MessageBox.Show("Biztosan nyomtatni akar?", "Nyomtatás", MessageBoxButtons.YesNo);
             if (temp == DialogResult.Yes)
             {

                 if (csapat.Checked == true)
                 {
                        
                     Nyomtat.nyomtat_csapatlista(combo_versenyek.Text);
                     return;
                     // Nyomtat.print("NEVEZLISTA.docx");
                 }
                 if (nevez.Checked == true)
                 {
                     Nyomtat.nyomtat_nevezesilista(combo_versenyek.Text, false);
                     return;
                     // Nyomtat.print("NEVEZLISTA.docx");
                 }
                 if (nemmegjelent.Checked == true)
                 {
                     Nyomtat.nyomtat_nevezesilista(combo_versenyek.Text, true);
                     return;
                     // Nyomtat.print("CSAPATLISTA.docx");
                 }

             }
             else
             {
                 return;
             }
             * */
        }

        private void checkbox_klikk(object sender, EventArgs e)
        {
            CheckBox aktív = sender as CheckBox;
            if (aktív == csapat )
            {
                csapat.Checked = true;
                nevez.Checked = false;
                nemmegjelent.Checked = false;

            }
            else if(aktív == nevez )
            {
                nevez.Checked = true;
                csapat.Checked = false;
                nemmegjelent.Checked = false;

            }
            else if( aktív == nemmegjelent )
            {
                nemmegjelent.Checked = true;
                nevez.Checked = false;
                csapat.Checked = false;


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

        #endregion
    }
}
