using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
    public struct Korosztály
    {
        public string verseny;
        public string azonosító;
        public string megnevezés;
        public int alsó_határ;
        public int felső_határ;
        public bool nők;
        public bool férfiak;
        public int indulók_nők;
        public int indulók_férfiak;

        public Korosztály(string _verseny, string _azonosító, string _megnevezés, int _alsó, int _felső, bool _nők, bool _férfiak, int _indulók_nők, int _indulók_férfiak)
        {
            verseny = _verseny;
            azonosító = _azonosító;
            megnevezés = _megnevezés;
            alsó_határ = _alsó;
            felső_határ = _felső;
            nők = _nők;
            férfiak = _férfiak;
            indulók_nők = _indulók_nők;
            indulók_férfiak = _indulók_férfiak;
        }
    }

    public delegate void Korosztály_Hozzáadva(Korosztály _korosztály);
    public delegate void Korosztály_Módosítva(string _azonosító, Korosztály _korosztály);
    public delegate void Korosztály_Törölve( string _azonosító);

    public sealed class Panel_Korosztályok : Control
    {
        public Korosztály_Hozzáadva korosztály_hozzáadva;
        public Korosztály_Módosítva korosztály_módosítva;
        public Korosztály_Törölve korosztály_törölve;

        private DataTable data;
        private DataGridView table;

        private ComboBox box_vazon;


        public Panel_Korosztályok()
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
            table.Width = 603;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += módosítás_Click;

            ///

            Label label_vazon = new Label();
            label_vazon.Text = "Verseny azonosítója:";
            label_vazon.AutoSize = true;
            label_vazon.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, 16 + 0 * 32);


            box_vazon = new ComboBox();
            box_vazon.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16 + 4 * 32, 16 + 0 * 32);
            box_vazon.DropDownStyle = ComboBoxStyle.DropDownList;
            box_vazon.SelectedIndexChanged += box_vazon_SelectedIndexChanged;

            ///

            Button törlés = new Button();
            törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            törlés.Text = "Törlés";
            törlés.Size = new System.Drawing.Size(96, 32);
            törlés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16);
            törlés.Click += törlés_Click;
            
            Button számolás = new Button();
            számolás.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            számolás.Text = "Számolás";
            számolás.Size = new System.Drawing.Size(96, 32);
            számolás.Location = new System.Drawing.Point(törlés.Location.X + törlés.Size.Width + 16, törlés.Location.Y);
            számolás.Click += számolás_Click;

            Button hozzáadás = new Button();
            hozzáadás.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            hozzáadás.Text = "Hozzáadás";
            hozzáadás.Size = new System.Drawing.Size(96, 32);
            hozzáadás.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
            hozzáadás.Click += hozzáadás_Click;

            ///

            List<Verseny> versenyek = Program.database.Versenyek();

             foreach(Verseny current in versenyek)
                box_vazon.Items.Add(current.azonosító);

            if (0 < box_vazon.Items.Count) box_vazon.SelectedIndex = 0;

            ///

            Controls.Add(table);

            Controls.Add(label_vazon);

            Controls.Add(box_vazon);


            Controls.Add(törlés);
            Controls.Add(számolás);
            Controls.Add(hozzáadás);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();
            data.Columns.Add(new DataColumn("Verseny azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Megnevezés", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Alsó életkorhatár", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Felső életkorhatár", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Nők", System.Type.GetType("System.Boolean")));
            data.Columns.Add(new DataColumn("Férfiak", System.Type.GetType("System.Boolean")));
            data.Columns.Add(new DataColumn("# Nők", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("# Férfiak", System.Type.GetType("System.Int32")));

            return data;
        }

        #region Accessors
        private delegate void Korosztály_Hozzáadás_Callback(Korosztály _korosztály);
        public void Korosztály_Hozzáadás(Korosztály _korosztály)
        {
            if (InvokeRequired)
            {
                Korosztály_Hozzáadás_Callback callback = new Korosztály_Hozzáadás_Callback(Korosztály_Hozzáadás);
                Invoke(callback, new object[] { _korosztály });
            }
            else
            {
                if (!Program.database.ÚjKorosztály(_korosztály)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (_korosztály.verseny == box_vazon.SelectedItem.ToString())
                {
                    DataRow row = data.NewRow();
                    row[0] = _korosztály.verseny;
                    row[1] = _korosztály.azonosító;
                    row[2] = _korosztály.megnevezés;
                    row[3] = _korosztály.alsó_határ;
                    row[4] = _korosztály.felső_határ;
                    row[5] = _korosztály.nők;
                    row[6] = _korosztály.férfiak;
                    row[7] = _korosztály.indulók_nők;
                    row[8] = _korosztály.indulók_férfiak;

                    data.Rows.Add(row);
                }

                if (korosztály_hozzáadva != null) korosztály_hozzáadva(_korosztály);
            }
            //rendezés
            table.Sort( table.Columns[3],System.ComponentModel.ListSortDirection.Ascending );
        }

        private delegate void Korosztály_Módosítás_Callback(string _azonosító, Korosztály _korosztály);
        public void Korosztály_Módosítás(string _azonosító, Korosztály _korosztály)
        {
            if (InvokeRequired)
            {
                Korosztály_Módosítás_Callback callback = new Korosztály_Módosítás_Callback(Korosztály_Módosítás);
                Invoke(callback, new object[] { _azonosító, _korosztály });
            }
            else
            {
                if (!Program.database.KorosztályMódosítás(_azonosító, _korosztály)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (_korosztály.verseny == box_vazon.SelectedItem.ToString())
                {
                    foreach (DataRow current in data.Rows)
                    {
                        if (_azonosító == current[1].ToString())
                        {
                            current[0] = _korosztály.verseny;
                            current[1] = _korosztály.azonosító;
                            current[2] = _korosztály.megnevezés;
                            current[3] = _korosztály.alsó_határ;
                            current[4] = _korosztály.felső_határ;
                            current[5] = _korosztály.nők;
                            current[6] = _korosztály.férfiak;
                            current[7] = _korosztály.indulók_nők;
                            current[8] = _korosztály.indulók_férfiak;

                            break;
                        }
                    }
                }

                if (korosztály_módosítva != null) korosztály_módosítva(_azonosító, _korosztály);
            }
            //rendezés
            table.Sort(table.Columns[3], System.ComponentModel.ListSortDirection.Ascending);

        }

        private delegate void Korosztály_Törlés_Callback(string _verseny, string _azonosító);
        public void Korosztály_Törlés(string _verseny, string _azonosító)
        {
            if (InvokeRequired)
            {
                Korosztály_Törlés_Callback callback = new Korosztály_Törlés_Callback(Korosztály_Törlés);
                Invoke(callback, new object[] { _verseny, _azonosító });
            }
            else
            {
                if (!Program.database.KorosztályTörlés(_verseny, _azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[1].ToString())
                    {
                        data.Rows.Remove(current);
                        break;
                    }
                }

                if (korosztály_törölve != null) korosztály_törölve(_azonosító);
            }
        }

        //Csak belső használatra, nincs invokeolva!
        public void Verseny_Számolás(string _verseny)
        {
            if (!Program.database.KorosztálySzámolás(_verseny)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (_verseny == box_vazon.SelectedItem.ToString())
            {
                List<Korosztály> korosztályok = Program.database.Korosztályok(box_vazon.Text);

                data.Rows.Clear();

                foreach (Korosztály current in korosztályok)
                {
                    DataRow row = data.NewRow();
                    row[0] = current.verseny;
                    row[1] = current.azonosító;
                    row[2] = current.megnevezés;
                    row[3] = current.alsó_határ;
                    row[4] = current.felső_határ;
                    row[5] = current.nők;
                    row[6] = current.férfiak;
                    row[7] = current.indulók_nők;
                    row[8] = current.indulók_férfiak;

                    data.Rows.Add(row);
                }
            }
        }
        #endregion

        #region EventHandlers
        public void verseny_hozzáadás(Verseny _verseny)
        {
            box_vazon.Items.Add(_verseny.azonosító);
        }

        public void verseny_módosítás(string _azonosító, Verseny _verseny)
        {
            if (_azonosító != _verseny.azonosító)
            {
                for (int current = 0; current < box_vazon.Items.Count; ++current)
                {
                    if (_azonosító == box_vazon.Items[current].ToString())
                    {
                        box_vazon.Items[current] = _verseny.azonosító;
                        break;
                    }
                }
            }
        }

        public void verseny_törlés(string _azonosító)
        {
            if (box_vazon.SelectedItem != null && _azonosító == box_vazon.SelectedItem.ToString())
            {
                data.Rows.Clear();
            } 
            
            box_vazon.Items.Remove(_azonosító);
        }

        ///

        void table_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Visible = false;
            table.Columns[5].Width = 40;
            table.Columns[6].Width = 40;
            table.Columns[7].Width = 60;
            table.Columns[8].Width = 60;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
            //rendezés
            table.Sort(table.Columns[3], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void box_vazon_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            List<Korosztály> korosztályok = Program.database.Korosztályok(box_vazon.Text);

            data.Rows.Clear();

            foreach (Korosztály current in korosztályok)
            {
                DataRow row = data.NewRow();
                row[0] = current.verseny;
                row[1] = current.azonosító;
                row[2] = current.megnevezés;
                row[3] = current.alsó_határ;
                row[4] = current.felső_határ;
                row[5] = current.nők;
                row[6] = current.férfiak;
                row[7] = current.indulók_nők;
                row[8] = current.indulók_férfiak;

                data.Rows.Add(row);
            }
        }

        private void hozzáadás_Click(object _sender, EventArgs _event)
        {
            if (box_vazon.SelectedItem == null) { MessageBox.Show("Nincs kiválasztva verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            Form_Korosztály korosztály = new Form_Korosztály(box_vazon.Text);
            korosztály.ShowDialog();
        }

        private void módosítás_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;

            foreach (DataGridViewRow current in table.Rows)
            {
                if (table.SelectedRows[0] == current)
                {
                    Form_Korosztály korosztály =  new Form_Korosztály(box_vazon.Text, new Korosztály(current.Cells[0].Value.ToString(),
                        current.Cells[1].Value.ToString(), 
                        current.Cells[2].Value.ToString(),
                        Convert.ToInt32(current.Cells[3].Value),
                        Convert.ToInt32(current.Cells[4].Value),
                        (bool)current.Cells[5].Value,
                        (bool)current.Cells[6].Value,
                        Convert.ToInt32(current.Cells[7].Value),
                        Convert.ToInt32(current.Cells[8].Value)));

                    korosztály.ShowDialog();
                    break;
                }
            }
        }

        private void számolás_Click(object _sender, EventArgs _event)
        {
            if (box_vazon.SelectedItem == null) { MessageBox.Show("Nincs kiválasztva verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            Verseny_Számolás(box_vazon.SelectedItem.ToString());
        }

        private void törlés_Click(object _sender, EventArgs _event)
        {
            if (box_vazon.SelectedItem == null) return;
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;
            //if ((int)data.Rows[table.SelectedRows[0].Index][4] != 0) { MessageBox.Show("Ez a korosztály nem törölhető, mivel van hozzárendelve eredmény!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (MessageBox.Show("Biztosan törli ezt a korosztályt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (DataGridViewRow current in table.Rows)
            {
                if (table.SelectedRows[0] == current)
                {

                    Korosztály_Törlés(current.Cells[0].Value.ToString(), current.Cells[1].Value.ToString());

                }
            }
        }
        #endregion

        public sealed class Form_Korosztály : Form
        {
            private string eredeti_azonosító = null;
            private string eredeti_verseny = null;

            private TextBox text_kazon;
            private TextBox text_megn;
            private TextBox text_felso;
            private TextBox text_also;
            private CheckBox box_nők;
            private CheckBox box_férfiak;
            private Label text_indulo_nő;
            private Label text_indulo_férfi;

            public Form_Korosztály(string _verseny)
            {
                eredeti_verseny = _verseny;

                InitializeForm();
                InitializeContent();
                InitializeData();
            }

            public Form_Korosztály(string _verseny, Korosztály _korosztály)
            {
                eredeti_verseny = _verseny;

                InitializeForm();
                InitializeContent();
                InitializeData(_korosztály);
            }

            private void InitializeForm()
            {
                Text = "Korosztály";
                ClientSize = new System.Drawing.Size(400 - 64, 264);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent()
            {
                Label label_kazon = new Label();
                label_kazon.Text = "Azonosító:";
                label_kazon.Location = new System.Drawing.Point(16, 16 + 0 * 32);

                Label label_megnevezés = new Label();
                label_megnevezés.Text = "Megnevezés:";
                label_megnevezés.Location = new System.Drawing.Point(label_kazon.Location.X, 16 + 1 * 32);

                Label label_also = new Label();
                label_also.Text = "Alsó életkorhatár:";
                label_also.Location = new System.Drawing.Point(label_kazon.Location.X, 16 + 2 * 32);

                Label label_felso = new Label();
                label_felso.Text = "Felső életkorhatár:";
                label_felso.Location = new System.Drawing.Point(label_kazon.Location.X, 16 + 3 * 32);

                Label nők = new Label();
                nők.Text = "Nők:";
                nők.Location = new System.Drawing.Point(label_kazon.Location.X, 16 + 4 * 32);
                nők.Size = new System.Drawing.Size(64, 24);

                Label férfiak = new Label();
                férfiak.Text = "Férfiak:";
                férfiak.Location = new System.Drawing.Point(label_kazon.Location.X + 128, 16 + 4 * 32);
                férfiak.Size = new System.Drawing.Size(64, 24);

                Label label_indulok_nő = new Label();
                label_indulok_nő.Text = "Nő indulók:";
                label_indulok_nő.Location = new System.Drawing.Point(label_kazon.Location.X, 16 + 5 * 32);

                Label label_indulok_férfi = new Label();
                label_indulok_férfi.Text = "Férfi indulók:";
                label_indulok_férfi.Location = new System.Drawing.Point(label_kazon.Location.X, 16 + 6 * 32);

                ///

                text_kazon = new TextBox();
                text_kazon.Location = new System.Drawing.Point(label_kazon.Location.X + label_kazon.Width + 32, label_kazon.Location.Y);

                text_megn = new TextBox();
                text_megn.Location = new System.Drawing.Point(text_kazon.Location.X, label_megnevezés.Location.Y);

                text_also = new TextBox();
                text_also.Location = new System.Drawing.Point(text_kazon.Location.X, label_also.Location.Y);

                text_felso = new TextBox();
                text_felso.Location = new System.Drawing.Point(text_kazon.Location.X, label_felso.Location.Y);

                box_nők = new CheckBox();
                box_nők.Location = new System.Drawing.Point(nők.Location.X + nők.Size.Width + 16, nők.Location.Y);

                box_férfiak = new CheckBox();
                box_férfiak.Location = new System.Drawing.Point(férfiak.Location.X + férfiak.Size.Width + 16, férfiak.Location.Y);

                text_indulo_nő = new Label();
                text_indulo_nő.Location = new System.Drawing.Point(text_kazon.Location.X, label_indulok_nő.Location.Y);
                text_indulo_nő.Size = new System.Drawing.Size(64, 24);

                text_indulo_férfi = new Label();
                text_indulo_férfi.Location = new System.Drawing.Point(text_kazon.Location.X, label_indulok_férfi.Location.Y);
                text_indulo_férfi.Size = new System.Drawing.Size(64, 24);

                ///

                Button rendben = new Button();
                rendben.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                rendben.Text = "Rendben";
                rendben.Size = new System.Drawing.Size(96, 32);
                rendben.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
                rendben.Click += rendben_Click;

                ///
               
                Controls.Add(label_kazon);
                Controls.Add(label_megnevezés);
                Controls.Add(label_also);
                Controls.Add(label_felso);
                Controls.Add(nők);
                Controls.Add(férfiak);
                Controls.Add(label_indulok_nő);
                Controls.Add(label_indulok_férfi);

                Controls.Add(text_kazon);
                Controls.Add(text_megn);
                Controls.Add(text_also);
                Controls.Add(text_felso);
                Controls.Add(box_nők);
                Controls.Add(box_férfiak);
                Controls.Add(text_indulo_nő);
                Controls.Add(text_indulo_férfi);

                Controls.Add(rendben);
            }

            private void InitializeData()
            {
                text_megn.Text = "";
                text_also.Text = "";
                text_felso.Text = "";
                box_nők.Checked = true;
                box_férfiak.Checked = true;
                text_indulo_nő.Text = "";
                text_indulo_férfi.Text = "";
                text_kazon.Text = "";
            }

            private void InitializeData(Korosztály _korosztály)
            {
                eredeti_azonosító = _korosztály.azonosító;
                eredeti_verseny = _korosztály.verseny;

                text_kazon.Text = _korosztály.azonosító;
                text_megn.Text = _korosztály.megnevezés;
                text_also.Text = _korosztály.alsó_határ.ToString();
                text_felso.Text = _korosztály.felső_határ.ToString();
                box_nők.Checked = _korosztály.nők;
                box_férfiak.Checked = _korosztály.férfiak;
                text_indulo_nő.Text = _korosztály.indulók_nők.ToString();
                text_indulo_férfi.Text = _korosztály.indulók_férfiak.ToString();
            }

            #region EventHandlers
            private void rendben_Click(object _sender, EventArgs _event)
            {
                if (text_kazon.Text.Length == 0 || text_kazon.Text.Length > 10) { MessageBox.Show("Korosztályazonosító hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(text_kazon.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (text_megn.Text.Length == 0 || text_megn.Text.Length > 30) { MessageBox.Show("Korosztály megnevezése hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(text_megn.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                int alsó = 0, felső = 0;

                try { alsó = Convert.ToInt32(text_also.Text); }
                catch { MessageBox.Show("Nem megfelelő az alsó életkor formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                try { felső = Convert.ToInt32(text_felso.Text); }
                catch { MessageBox.Show("Nem megfelelő a felső életkor formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (alsó <= 0) { MessageBox.Show("Alsó korhatár túl kicsi!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (felső <= alsó) { MessageBox.Show("A felső korhatárnak nagyobbnak kell lenni, mint az alsónak", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (100 < felső) { if (MessageBox.Show("Felső korhatár túl magas, biztosan hagyjuk így?", "Korhatár", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return; }

                Database.CountPair indulók = Program.database.KorosztálySzámolás(eredeti_verseny, alsó, felső, box_nők.Checked, box_férfiak.Checked, false);
                if (eredeti_azonosító != null)
                    Program.mainform.korosztályok_panel.Korosztály_Módosítás(eredeti_azonosító, new Korosztály(eredeti_verseny, text_kazon.Text, text_megn.Text, alsó, felső, box_nők.Checked, box_férfiak.Checked, indulók.nők, indulók.férfiak));
                else
                    Program.mainform.korosztályok_panel.Korosztály_Hozzáadás(new Korosztály(eredeti_verseny, text_kazon.Text, text_megn.Text, alsó, felső, box_nők.Checked, box_férfiak.Checked, indulók.nők, indulók.férfiak));

                Close();
            }
            #endregion
        }
    }
}