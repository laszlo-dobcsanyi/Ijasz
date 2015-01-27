using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace Íjász
{
    public struct Induló
    {
        public string név;
        public string nem;
        public string születés;
        public string engedély;
        public string egyesület;
        public int eredmények;

        public Induló(string _név, string _nem, string _születés, string _engedély, string _egyesület, int _eredmények)
        {
            név = _név;
            nem = _nem;
            születés = _születés;
            engedély = _engedély;
            egyesület = _egyesület;
            eredmények = _eredmények;
        }
    }
   
    public delegate void Induló_Hozzáadva(Induló _induló);
    public delegate void Induló_Módosítva(string _név, Induló _induló);
    public delegate void Induló_Átnevezve(string _eredeti_név, string _új_név);
    public delegate void Induló_Törölve(string _név);

    public sealed class Panel_Indulók : Control
    {
        public Form_Induló_Teszt eredmény_form;

        public Induló_Hozzáadva induló_hozzáadva;
        public Induló_Módosítva induló_módosítva;
        public Induló_Átnevezve induló_átnevezve;
        public Induló_Törölve induló_törölve;

        private DataTable data;
        private DataGridView table;
        private TextBox keresés;

        public static int lastindex;

        public Panel_Indulók()
        {
            InitializeContent();

        }

        private void InitializeContent()
        {
            table = new DataGridView();
            table.DataSource = CreateSource();
            table.Dock = DockStyle.Left;
            table.RowHeadersVisible = false;
            table.AllowUserToResizeRows = false;
            table.AllowUserToResizeColumns = false;
            table.AllowUserToAddRows = false;
            table.Width = 703;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += módosítás_Click;

            ///

            Button hozzáadás = new Button();
            hozzáadás.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            hozzáadás.Text = "Hozzáadás";
            hozzáadás.Size = new System.Drawing.Size(96, 32);
            hozzáadás.Location = new System.Drawing.Point(ClientRectangle.Width - 96, ClientRectangle.Height - 32 - 16);
            hozzáadás.Click += hozzáadás_Click;

            Button beírás = new Button();
            beírás.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            beírás.Text = "Beírás";
            beírás.Size = new System.Drawing.Size(96, 32);
            beírás.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 8 - hozzáadás.Width, ClientRectangle.Height - 32 - 16);
            beírás.Click += beírás_Click;

            Button törlés = new Button();
            törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            törlés.Text = "Törlés";
            törlés.Size = new System.Drawing.Size(96, 32);
            törlés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16);
            törlés.Click += törlés_Click;


            Label label_keresés = new Label();
            label_keresés.Text = "Név keresés:";
            label_keresés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, 16 + 0 * 32);

            keresés = new TextBox();
            keresés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + label_keresés.Width + 16, 16 + 0 * 32);
            keresés.TextChanged += keresés_TextChanged;
            keresés.MaxLength = 30;
            keresés.Width = 150;

            Controls.Add(label_keresés);
            Controls.Add(keresés);

            Controls.Add(table);

            Controls.Add(hozzáadás);
            Controls.Add(beírás);
            Controls.Add(törlés);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Név", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Nem", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Születési idő", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Engedély", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Egyesület név", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("#", System.Type.GetType("System.Int32")));

            List<Induló> indulók = Program.database.Indulók();

            foreach (Induló current in indulók)
            {
                DataRow row = data.NewRow();
                row[0] = current.név;
                row[1] = current.nem;
                row[2] = current.születés;
                row[3] = current.engedély;
                row[4] = current.egyesület;
                row[5] = current.eredmények;

                data.Rows.Add(row);
            }

            return data;
        }

        #region Accessors
        private delegate void Induló_Hozzáadás_Callback(Induló _induló);
        public void Induló_Hozzáadás(Induló _induló)
        {
            if (InvokeRequired)
            {
                Induló_Hozzáadás_Callback callback = new Induló_Hozzáadás_Callback(Induló_Hozzáadás);
                Invoke(callback, new object[] { _induló });
            }
            else
            {
                if (!Program.database.ÚjInduló(_induló)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                DataRow row = data.NewRow();
                row[0] = _induló.név;
                row[1] = _induló.nem;
                row[2] = _induló.születés;
                row[3] = _induló.engedély;
                row[4] = _induló.egyesület;
                row[5] = 0; //

                data.Rows.Add(row);

                if (induló_hozzáadva != null) induló_hozzáadva(_induló);
            }
        }

        private delegate void Induló_Módosítás_Callback(string _név, Induló _induló);
        public void Induló_Módosítás(string _név, Induló _induló)
        {
            if (InvokeRequired)
            {
                Induló_Módosítás_Callback callback = new Induló_Módosítás_Callback(Induló_Módosítás);
                Invoke(callback, new object[] { _név, _induló });
            }
            else
            {
                if (!Program.database.IndulóMódosítás(_név, _induló)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_név == current[0].ToString())
                    {
                        current[0] = _induló.név;
                        current[1] = _induló.nem;
                        current[2] = _induló.születés;
                        current[3] = _induló.engedély;
                        current[4] = _induló.egyesület;

                        // Jól legyen broadcastolva a módosítás!
                        _induló.eredmények = (int)current[5];
                        break;
                    }
                }

                if (induló_módosítva != null) induló_módosítva(_név, _induló);


                if (_név != _induló.név && 0 < _induló.eredmények)
                {
                    if (!Program.database.Induló_EredményekÁtnevezése(_név, _induló.név)) { MessageBox.Show("Adatbázis hiba!\nLEHETETLEN!!!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (induló_átnevezve != null) induló_átnevezve(_név, _induló.név);
                }

            }
        }

        private delegate void Induló_Törlés_Callback(string _név);
        public void Induló_Törlés(string _név)
        {
            if (InvokeRequired)
            {
                Induló_Törlés_Callback callback = new Induló_Törlés_Callback(Induló_Törlés);
                Invoke(callback, new object[] { _név });
            }
            else
            {
                if (!Program.database.IndulóTörlés(_név)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_név == current[0].ToString())
                    {
                        data.Rows.Remove(current);
                        break;
                    }
                }

                if (induló_törölve != null) induló_törölve(_név);
            }
        }
        #endregion

        #region EventHandlers
        public void eredmények_beírás(string _azonosító, Database.BeírásEredmény _beírás)
        {
            if (_beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
            {
                foreach (DataRow current in data.Rows)
                {
                    if (_beírás.eredmény.Value.név == (string)current[0])
                    {
                        current[5] = ((int)current[5]) + 1;
                        break;
                    }
                }
            }
        }

        public void eredmények_törlés(string _azonosító, Eredmény _eredmény)
        {
            foreach (DataRow current in data.Rows)
            {
                if (_eredmény.név == (string)current[0])
                {
                    current[5] = ((int)current[5]) - 1;
                    break;
                }
            }
        
        }
        private void beírás_Click(object sender, EventArgs e)
        {
            List<Verseny> versenyek = Program.database.Versenyek();
            if (versenyek.Count == 0) { MessageBox.Show("Nincsen még egy verseny sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
            if (íjtípusok.Count == 0) { MessageBox.Show("Nincsen még egy íjtípus sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (table.SelectedRows.Count != 1) { MessageBox.Show("Nem megfelelő a kiválasztott indulók száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            eredmény_form = new Form_Induló_Teszt(table.SelectedRows[0].Cells[0].Value.ToString());
            eredmény_form.ShowDialog();
        }


        private void table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 250;
            table.Columns[1].Width = 50;
            table.Columns[2].Width = 79;
            table.Columns[3].Width = 54;
            table.Columns[4].Width = 200;
            table.Columns[5].Width = 50;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;

            //rendezés
            table.Sort(table.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void hozzáadás_Click(object _sender, EventArgs _event)
        {
            Form_Induló induló = new Form_Induló();
            induló.ShowDialog();
        }

        private void módosítás_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) { MessageBox.Show("Nincs kiválasztva induló!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

            foreach (DataGridViewRow current in table.Rows)
            {
                if (table.SelectedRows[0] == current)
                {
                    Form_Induló induló = new Form_Induló(new Induló(current.Cells[0].Value.ToString(),
                        current.Cells[1].Value.ToString(),
                        current.Cells[2].Value.ToString(),
                        current.Cells[3].Value.ToString(), 
                        current.Cells[4].Value.ToString(), 
                        Convert.ToInt32(current.Cells[5].Value) ) ) ;
                    induló.ShowDialog();
                    return;
                }

            }
        }

        private void törlés_Click(object _sender, EventArgs _event)
        {
            if (!(table.SelectedRows.Count != 0 && table.SelectedRows[0].Index < data.Rows.Count && table.SelectedRows[0].Selected)) return;
            if (Convert.ToInt32(table.SelectedRows[0].Cells[5].Value) != 0) { MessageBox.Show("Ez az induló nem törölhető, mivel szerepel versenyen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (MessageBox.Show("Biztosan törli ezt az indulót?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (DataGridViewRow current in table.Rows)
            {
                if (table.SelectedRows[0] == current)
                {

                    Induló_Törlés(current.Cells[0].Value.ToString());

                }
            }
        }

        private void keresés_TextChanged(object _sender, EventArgs _event)
        {
            foreach (DataGridViewRow row in table.Rows)
            {
                int talált = 0;
                for (int i = 0; i < keresés.Text.Length; i++)
                {
                    if (row.Cells[0].Value.ToString().Length < keresés.Text.Length)
                    {
                        break;
                    }
                    if (row.Cells[0].Value.ToString()[i] == keresés.Text[i] || row.Cells[0].Value.ToString()[i] == Char.ToUpper(keresés.Text[i]))
                    {
                        talált++;
                    }
                }
                if (talált == keresés.Text.Length)
                {
                    table.Rows[row.Index].Selected = true;
                    table.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }
        }

        
        #endregion

        public sealed class Form_Induló : Form
        {
            private string eredeti_név = null;

            private TextBox box_név;
            private TextBox box_nem;
            private DateTimePicker date_születés;
            private TextBox box_engedély;
            private TextBox box_egyesület;
            private Label eredmények_száma;

            public Form_Induló()
            {
                InitializeForm();
                InitializeContent();
                InitializeData();
            }

            public Form_Induló(Induló _induló)
            {
                eredeti_név = _induló.név;

                InitializeForm();
                InitializeContent();
                InitializeData(_induló);
            }

            private void InitializeForm()
            {
                Text = "Induló";
                ClientSize = new System.Drawing.Size(400 - 32, 232);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent()
            {
                Label név = new Label();
                név.Text = "Név:";
                név.Location = new System.Drawing.Point(16, 16 + 0 * 32);

                Label nem = new Label();
                nem.Text = "Nem:";
                nem.Location = new System.Drawing.Point(név.Location.X, 16 + 1 * 32);

                Label születés = new Label();
                születés.Text = "Születési idő:";
                születés.Location = new System.Drawing.Point(név.Location.X, 16 + 2 * 32);

                Label engedély = new Label();
                engedély.Text = "Engedélyszám:";
                engedély.Location = new System.Drawing.Point(név.Location.X, 16 + 3 * 32);

                Label egyesület = new Label();
                egyesület.Text = "Egyesület név:";
                egyesület.Location = new System.Drawing.Point(név.Location.X, 16 + 4 * 32);

                Label eredmények = new Label();
                eredmények.Text = "Eredmények:";
                eredmények.Location = new System.Drawing.Point(név.Location.X, 16 + 5 * 32);

                ///

                box_név = new TextBox();
                box_név.Location = new System.Drawing.Point(név.Location.X + név.Size.Width + 16, név.Location.Y);
                box_név.Size = new System.Drawing.Size(128 + 64, 24);
                box_név.MaxLength = 30;

                box_nem = new TextBox();
                box_nem.Location = new System.Drawing.Point(nem.Location.X + nem.Size.Width + 16, nem.Location.Y);
                box_nem.Size = new System.Drawing.Size(64, 24);
                box_nem.MaxLength = 10;

                date_születés = new DateTimePicker();
                date_születés.Location = new System.Drawing.Point(születés.Location.X + születés.Size.Width + 16, születés.Location.Y);
                date_születés.Size = box_név.Size;
                date_születés.Value = DateTime.Now;

                box_engedély = new TextBox();
                box_engedély.Location = new System.Drawing.Point(engedély.Location.X + engedély.Size.Width + 16, engedély.Location.Y);
                box_engedély.Size = box_név.Size;
                box_engedély.MaxLength = 30;

                box_egyesület = new TextBox();
                box_egyesület.Location = new System.Drawing.Point(egyesület.Location.X + egyesület.Size.Width + 16, egyesület.Location.Y);
                box_egyesület.Size = box_név.Size;
                box_egyesület.MaxLength = 30;

                eredmények_száma = new Label();
                eredmények_száma.Location = new System.Drawing.Point(eredmények.Location.X + eredmények.Size.Width + 16, eredmények.Location.Y);

                Button rendben = new Button();
                rendben.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                rendben.Text = "Rendben";
                rendben.Size = new System.Drawing.Size(96, 32);
                rendben.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
                rendben.Click += rendben_Click;

                ///

                Controls.Add(név);
                Controls.Add(nem);
                Controls.Add(születés);
                Controls.Add(engedély);
                Controls.Add(egyesület);
                Controls.Add(eredmények);

                Controls.Add(box_név);
                Controls.Add(box_nem);
                Controls.Add(date_születés);
                Controls.Add(box_engedély);
                Controls.Add(box_egyesület);
                Controls.Add(eredmények_száma);
                Controls.Add(rendben);
            }

            private void InitializeData()
            {
                box_név.Text = "";
                box_név.Enabled = true;
                box_nem.Text = "";
                date_születés.Value = DateTime.Now;
                box_engedély.Text = "";
                box_egyesület.Text = "";
                eredmények_száma.Text = "0";
            }

            private void InitializeData(Induló _induló)
            {
                box_név.Text = _induló.név;
                box_nem.Text = _induló.nem == "N" ? "Nő" : "Férfi";
                box_nem.Enabled = (_induló.eredmények > 0 ? false : true);
                date_születés.Value = DateTime.Parse(_induló.születés);
                date_születés.Enabled = (_induló.eredmények > 0 ? false : true);
                box_engedély.Text = _induló.engedély;
                box_egyesület.Text = _induló.egyesület;
                eredmények_száma.Text = _induló.eredmények.ToString();
            }

            private void rendben_Click(object _sender, EventArgs _event)
            {
                if (date_születés.Value.Year == DateTime.Now.Year && date_születés.Value.Month == DateTime.Now.Month && date_születés.Value.Day == DateTime.Now.Day) { MessageBox.Show("A születési dátum nem lehet a mai nap!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!(0 < box_név.Text.Length && box_név.Text.Length <= 30)) { MessageBox.Show("Nem megfelelő a név hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(box_név.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!(0 < box_nem.Text.Length && box_nem.Text.Length <= 10)) { MessageBox.Show("Nem megfelelő a nem hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                bool nő = false;
                if (box_nem.Text.ToLower() == "n" || box_nem.Text.ToLower() == "nő") nő = true;
                else if (!(box_nem.Text.ToLower() == "f" || box_nem.Text.ToLower() == "férfi")) { MessageBox.Show("Nem megfelelő nem!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!(box_engedély.Text.Length <= 30)) { MessageBox.Show("Nem megfelelő az engedély hossza (0 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(box_engedély.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!(box_egyesület.Text.Length <= 30)) { MessageBox.Show("Nem megfelelő az egyesület hossza (0 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(box_egyesület.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eredeti_név != null)
                    Program.mainform.indulók_panel.Induló_Módosítás(eredeti_név, new Induló(box_név.Text, (nő ? "N" : "F"), date_születés.Value.ToShortDateString(), box_engedély.Text, box_egyesület.Text, Convert.ToInt32(eredmények_száma.Text)));
                else
                    Program.mainform.indulók_panel.Induló_Hozzáadás(new Induló(box_név.Text, (nő ? "N" : "F"), date_születés.Value.ToShortDateString(), box_engedély.Text, box_egyesület.Text, 0));

                Close();
            }
        }

        public sealed class Form_Induló_Teszt : Form
        {
            public Form_Csapatlista csapatlista_form;

            public ComboBox combo_verseny;
            private Label label_név;
            private ComboBox combo_íjtípus;
            private ComboBox combo_csapat;
            private CheckBox check_megjelent;

            public Form_Induló_Teszt( string _név )
            {
                label_név = new Label();
                label_név.Text = _név;

                InitializeForm();
                InitializeContent();
                InitializeData();
            }


            private void InitializeForm()
            {
                Text = "Beírás";
                ClientSize = new System.Drawing.Size(464 - 64, 200);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent()
            {
                Label név = new Label();
                név.Text = "Név:";
                név.Location = new System.Drawing.Point(32, 16 + 0 * 32);

                Label verseny = new Label();
                verseny.Text = "Verseny:";
                verseny.Location = new System.Drawing.Point(név.Location.X, 16 + 1 * 32);
               
                Label íjtípus = new Label();
                íjtípus.Text = "Íjtípus:";
                íjtípus.Location = new System.Drawing.Point(név.Location.X, 16 + 2 * 32);

                Label csapat = new Label();
                csapat.Text = "Csapatszám:";
                csapat.Location = new System.Drawing.Point(név.Location.X, 16 + 3 * 32);

                Label megjelent = new Label();
                megjelent.Text = "Megjelent:";
                megjelent.Location = new System.Drawing.Point(név.Location.X, 16 + 4 * 32);


                ///

                label_név.Location = new System.Drawing.Point(név.Location.X + név.Size.Width + 16, név.Location.Y);
                label_név.Size = new System.Drawing.Size(128 + 64, 24);

                combo_verseny = new ComboBox();
                combo_verseny.Location = new System.Drawing.Point(verseny.Location.X + verseny.Size.Width + 16, verseny.Location.Y);
                combo_verseny.Size = label_név.Size;
                combo_verseny.DropDownStyle = ComboBoxStyle.DropDownList;
                combo_verseny.SelectedIndexChanged += combo_verseny_SelectedIndexChanged;

                combo_íjtípus = new ComboBox();
                combo_íjtípus.Location = new System.Drawing.Point(íjtípus.Location.X + íjtípus.Size.Width + 16, íjtípus.Location.Y);
                combo_íjtípus.Size = label_név.Size;
                combo_íjtípus.DropDownStyle = ComboBoxStyle.DropDownList;

                combo_csapat = new ComboBox();
                combo_csapat.Location = new System.Drawing.Point(csapat.Location.X + csapat.Size.Width + 16, csapat.Location.Y);
                combo_csapat.Size = label_név.Size;
                combo_csapat.DropDownStyle = ComboBoxStyle.DropDownList;

                for (int i = 0; i < 45; i++) combo_csapat.Items.Add(i + 1);
                combo_csapat.SelectedItem = combo_csapat.Items[0];

                check_megjelent = new CheckBox();
                check_megjelent.Location = new System.Drawing.Point(combo_csapat.Location.X, megjelent.Location.Y);

                Button rendben = new Button();
                rendben.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                rendben.Text = "Rendben";
                rendben.Size = new System.Drawing.Size(96, 32);
                rendben.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
                rendben.Click += rendben_Click;

                Button csapatok_megtekint = new Button();
                csapatok_megtekint.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                csapatok_megtekint.Text = "Csapatok megtekintése";
                csapatok_megtekint.Size = new System.Drawing.Size(96, 32);
                csapatok_megtekint.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - rendben.Width - 32, ClientRectangle.Height - 32 - 16);
                csapatok_megtekint.Click += csapatok_megtekint_Click;

                ///

                List<Verseny> versenyek = Program.database.Versenyek();
                foreach (Verseny current in versenyek)
                    combo_verseny.Items.Add(current.azonosító);
               
                List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
                foreach (Íjtípus current in íjtípusok)
                    combo_íjtípus.Items.Add(current.azonosító); 

                ///

                Controls.Add(combo_verseny);

                Controls.Add(név);
                Controls.Add(íjtípus);
                Controls.Add(csapat);
                Controls.Add(megjelent);

                Controls.Add(verseny);
                Controls.Add(label_név);
                Controls.Add(combo_íjtípus);
                Controls.Add(combo_csapat);
                Controls.Add(rendben);
                Controls.Add(csapatok_megtekint);
                Controls.Add(check_megjelent);
            }

            private void InitializeData()
            {
                // Lefut a combo_verseny_SelectedIndexChanged, majd az beállít mindent!
                combo_verseny.SelectedIndex = lastindex;
            }

            #region EventHandlers
            private void combo_verseny_SelectedIndexChanged(object _sender, EventArgs _event)
            {
                Eredmény? eredmény = Program.database.Eredmény(combo_verseny.Text, label_név.Text);
                if (eredmény != null)
                {
                    combo_íjtípus.SelectedItem = eredmény.Value.íjtípus;
                    combo_csapat.SelectedItem = eredmény.Value.csapat;
                    check_megjelent.Checked = eredmény.Value.megjelent;
                }
                else
                {
                    combo_íjtípus.SelectedIndex = 0;
                    combo_csapat.SelectedIndex = 0;
                    check_megjelent.Checked = false;
                }
            }

            private void rendben_Click(object _sender, EventArgs _event)
            {
                if (combo_verseny.SelectedItem == null) { MessageBox.Show("Nincs kiválasztva verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (combo_íjtípus.SelectedItem == null) { MessageBox.Show("Nincs kiválasztva íjtípus!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(combo_csapat.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (Program.database.Verseny_Lezárva(combo_verseny.Text)) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                Program.mainform.eredmények_panel.Eredmény_Beírás(label_név.Text, combo_verseny.Text, combo_íjtípus.Text, combo_csapat.SelectedIndex + 1, check_megjelent.Checked);

                if (MessageBox.Show("Nyomtassak beírólapot ennek a versenyzőnek: " + label_név.Text + "?", "Nyomtatás", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Eredmény? eredmény = Program.database.Eredmény(combo_verseny.Text, label_név.Text);
                    
                    foreach(DataRow item in Program.mainform.verseny_panel.data.Rows)
                    {
                        Verseny? verseny = Program.database.Verseny(combo_verseny.Text);
                        if ((string)item[0] == combo_verseny.Text && verseny.Value.dupla_beirlap==false )
                        {
                                Nyomtat.print(Nyomtat.nyomtat_beirlap(combo_verseny.Text, eredmény.Value));
                        }
                        else if ((string)item[0] == combo_verseny.Text && verseny.Value.dupla_beirlap == true)
                        {
                            Nyomtat.print(Nyomtat.nyomtat_beirlap(combo_verseny.Text, eredmény.Value));
                            Nyomtat.print(Nyomtat.nyomtat_beirlap(combo_verseny.Text, eredmény.Value));
                        }
                    }
                    
                }

                lastindex = combo_verseny.SelectedIndex;
                Close();
            }

            private void csapatok_megtekint_Click(object sender, EventArgs e)
            {
                if (combo_verseny.SelectedItem == null)
                {
                    return;
                }
                csapatlista_form = new Form_Csapatlista(combo_verseny.Text);
                csapatlista_form.ShowDialog();
                return;
            }

            #endregion
        }

        public sealed class Form_Csapatlista : Form
        {
            private SplitContainer splitContainer1;
            private DataGridView dataGridView1;
            private DataGridView dataGridView2;
            private DataTable data;
            string azonosító;
        
            public Form_Csapatlista( string _azonosító)
            {
                azonosító = _azonosító;
                InitializeForm();
                InitializeContent();
            }

            private void InitializeForm()
            {
                Text = " Csapatok (" + azonosító + ")";
                ClientSize = new System.Drawing.Size(788 - 64 + 8, 700);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent()
            {

                splitContainer1 = new SplitContainer();
                dataGridView1 = new DataGridView();
                dataGridView2 = new DataGridView();
                splitContainer1.Dock = DockStyle.Fill;


                splitContainer1.Location = new System.Drawing.Point(0, 0);
                splitContainer1.Name = "splitContainer1";
                splitContainer1.Panel1.Controls.Add(dataGridView1);
                splitContainer1.Panel2.Controls.Add(dataGridView2);
                splitContainer1.Size = ClientSize;

                dataGridView1.Dock = DockStyle.Fill;
                splitContainer1.SplitterDistance = dataGridView1.Width-24;
                splitContainer1.IsSplitterFixed = true;
                dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
                //dataGridView2.Size = new System.Drawing.Size(splitContainer1.Width / 2 - 10, splitContainer1.Height - 5);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AllowUserToResizeColumns = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;

                dataGridView2.RowHeadersVisible = false;
                dataGridView2.AllowUserToResizeColumns = false;
                dataGridView2.AllowUserToResizeRows = false;
                dataGridView2.AllowUserToAddRows = false;
                dataGridView2.MultiSelect = false;
                dataGridView2.ReadOnly = true;


                Controls.Add(splitContainer1);
                dataGridView1.DataSource = CreateSource();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                

            }

            void dataGridView1_SelectionChanged(object sender, EventArgs e)
            {
                if (dataGridView1.Rows.Count == 0 || dataGridView1.SelectedRows.Count == 0)
                {
                    return;
                }
                if ( dataGridView1.SelectedRows[0].Cells[1].Value.ToString() == "0")
                {
                    dataGridView2.DataSource = null;
                    dataGridView2.DataSource = CreateEmptySource();
                    return;
                }

                List<Eredmény> eredmények = Program.database.Eredmények(azonosító);
                foreach (Eredmény item in eredmények)
                {
                    if (item.csapat == (int)dataGridView1.SelectedRows[0].Cells[0].Value)
                    {
                        dataGridView2.DataSource = CreateSource2();
                    }
                }
            }

            private DataTable CreateSource()
            {
                const int CsapatokSzama = 45; 

                data = new DataTable();
                data.Columns.Add(new DataColumn("Csapat", System.Type.GetType("System.Int32")));
                data.Columns.Add(new DataColumn("Indulók száma", System.Type.GetType("System.Int32")));

                for (int i = 0; i < CsapatokSzama; i++)
                {
                    DataRow row = data.NewRow();
                    row[0] = i + 1;
                    row[1] = 0;
                    data.Rows.Add(row);
                }

                List<Eredmény> eredmények = Program.database.Eredmények(azonosító);
                foreach (Eredmény item in eredmények)
                {
                    int a = Convert.ToInt32(data.Rows[Convert.ToInt32(item.csapat) - 1][1]);
                    data.Rows[Convert.ToInt32(item.csapat) - 1][1] = a + 1;
                }
                return data;
            }

            private DataTable CreateSource2()
            {
                data = new DataTable();
                data.Columns.Add(new DataColumn("Szám", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("Név", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("Íjtípus", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("Kor", System.Type.GetType("System.Int32")));
                data.Columns.Add(new DataColumn("Egyesület", System.Type.GetType("System.String")));
                if (dataGridView2.Columns.Count != 0)
                {
                    dataGridView2.Columns[0].Width = 40;
                    dataGridView2.Columns[1].Width = 160;
                    dataGridView2.Columns[2].Width = 100;
                    dataGridView2.Columns[3].Width = 40;
                    dataGridView2.Columns[4].Width = 160;
                }


                List<Eredmény> eredmények = Program.database.Eredmények(azonosító);
                List<Induló> indulók = Program.database.Indulók();
                int seged = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                foreach (Eredmény item in eredmények)
                {
                    if (item.csapat == seged)
                    {
                        DataRow row = data.NewRow();
                        foreach (Induló inner in indulók)
                        {
                            if (inner.név == item.név)
                            {
                                row[3] = (new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(inner.születés))).Year - 1;
                                row[4] = inner.egyesület;

                            }
                        }
                        row[0] = item.sorszám;
                        row[1] = item.név + (item.megjelent ? "(megjelent)" : "(nem jelent meg)");
                        row[2] = item.íjtípus;
                        data.Rows.Add(row);
                    }
                }

                return data;
            }

            private DataTable CreateEmptySource()
            {
                data = new DataTable();
                data.Columns.Add(new DataColumn("Szám", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("Név", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("Íjtípus", System.Type.GetType("System.String")));
                data.Columns.Add(new DataColumn("Kor", System.Type.GetType("System.Int32")));
                data.Columns.Add(new DataColumn("Egyesület", System.Type.GetType("System.String")));
                return data;
            }
        }
    }
}
