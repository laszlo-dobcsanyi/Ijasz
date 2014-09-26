using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
    public struct Eredmény
    {
        public string név;
        public Int64? sorszám;
        public string íjtípus;
        public int csapat;
        public int találat_10;
        public int találat_08;
        public int találat_05;
        public int mellé;
        public int? összpont;
        public int? százalék;
        public bool megjelent;

        public Eredmény(string _név, Int64? _sorszám, string _íjtípus, int _csapat, int _találat_10, int _találat_08, int _találat_05, int _mellé, int? _összpont, int? _százalék, bool _megjelent)
        {
            név = _név;
            sorszám = _sorszám;
            íjtípus = _íjtípus;
            csapat = _csapat;
            találat_10 = _találat_10;
            találat_08 = _találat_08;
            találat_05 = _találat_05;
            mellé = _mellé;
            összpont = _összpont;
            százalék = _százalék;
            megjelent = _megjelent;
        }
    }

    public delegate void Eredmény_Beírva(string _azonosító, Database.BeírásEredmény _beírás);
    public delegate void Eredmény_Módosítva(string _azonosító, Eredmény _eredeti, Eredmény _eredmény);
    public delegate void Eredmény_Törölve(string _azonosító, Eredmény _eredmény);

    public class Panel_Eredmények : Control
    {
        public Eredmény_Beírva eredmény_beírva;
        public Eredmény_Módosítva eredmény_módosítva;
        public Eredmény_Törölve eredmény_törölve;

        public DataTable data;
        public DataGridView table;

        private bool lezárva;
        private int verseny_összpont;
        private ComboBox combo_versenyek;

        private TextBox keresés;

        public Panel_Eredmények()
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
            table.Width = 683;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += módosítás_Click;

            ///

            Label verseny = new Label();
            verseny.Text = "Verseny:";
            verseny.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, 16 + 0 * 32);

            combo_versenyek = new ComboBox();
            combo_versenyek.Size = new System.Drawing.Size(128, 24);
            combo_versenyek.Location = new System.Drawing.Point(verseny.Location.X + verseny.Size.Width + 16, verseny.Location.Y);
            combo_versenyek.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_versenyek.SelectedIndexChanged += combo_versenyek_SelectedIndexChanged;

            ///

            Button törlés = new Button();
            törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            törlés.Text = "Törlés";
            törlés.Size = new System.Drawing.Size(96, 32);
            törlés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 8, ClientRectangle.Height - 32 - 16);
            törlés.Click += törlés_Click;

            Button lezárás = new Button();
            lezárás.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            lezárás.Text = "Lezárás";
            lezárás.Size = new System.Drawing.Size(96, 32);
            lezárás.Location = new System.Drawing.Point(törlés.Location.X + törlés.Size.Width + 16, ClientRectangle.Height - 32 - 16);
            lezárás.Click += lezárás_Click;

            Button megnyitás = new Button();
            megnyitás.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            megnyitás.Text = "Megnyitás";
            megnyitás.Size = new System.Drawing.Size(96, 32);
            megnyitás.Location = new System.Drawing.Point(törlés.Location.X + törlés.Size.Width + lezárás.Size.Width + 32, ClientRectangle.Height - 32 - 16);
            megnyitás.Click += megnyitás_Click;


            ///

            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny current in versenyek)
                combo_versenyek.Items.Add(current.azonosító);

            if (0 < combo_versenyek.Items.Count) combo_versenyek.SelectedIndex = 0;

            ///

            Label label_keresés = new Label();
            label_keresés.Text = "Sorszám keresés:";
            label_keresés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, 16 + 1 * 32);

            keresés = new TextBox();
            keresés.Location = new System.Drawing.Point(combo_versenyek.Location.X, 16 + 1 * 32);
            keresés.TextChanged += keresés_TextChanged;
            keresés.MaxLength = 30;
            keresés.Width = 150;
            keresés.KeyPress += keresés_KeyPress;

            Controls.Add(label_keresés);
            Controls.Add(keresés);

            Controls.Add(table);

            Controls.Add(verseny);
            Controls.Add(combo_versenyek);

            Controls.Add(törlés);
            Controls.Add(lezárás);
            Controls.Add(megnyitás);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Név", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Sorszám", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Íjtípus", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Csapat", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("10", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("8", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("5", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Mellé", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Összes", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("%", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Megjelent", System.Type.GetType("System.Boolean")));

            return data;
        }

        #region Accessors
        private delegate void Eredmény_Beírás_Callback(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent);
        public void Eredmény_Beírás(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent)
        {
            if (InvokeRequired)
            {
                Eredmény_Beírás_Callback callback = new Eredmény_Beírás_Callback(Eredmény_Beírás);
                Invoke(callback, new object[] { _név, _verseny, _íjtípus, _csapat, _megjelent });
            }
            else
            {
                Database.BeírásEredmény beírás = Program.database.EredményBeírás(_név, _verseny, _íjtípus, _csapat, _megjelent);
                if (beírás.eredmény == null) { MessageBox.Show("Adatbázis hiba a beírás során!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (_verseny == combo_versenyek.Text)
                {
                    if (beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
                    {
                        DataRow row = data.NewRow();
                            row[0] = beírás.eredmény.Value.név;
                            row[1] = beírás.eredmény.Value.sorszám;
                            row[2] = beírás.eredmény.Value.íjtípus;
                            row[3] = beírás.eredmény.Value.csapat;
                            row[4] = beírás.eredmény.Value.találat_10;
                            row[5] = beírás.eredmény.Value.találat_08;
                            row[6] = beírás.eredmény.Value.találat_05;
                            row[7] = beírás.eredmény.Value.mellé;
                            row[8] = beírás.eredmény.Value.összpont;
                            row[9] = beírás.eredmény.Value.százalék;
                            row[10] = beírás.eredmény.Value.megjelent;
                        data.Rows.Add(row);
                    }
                    else
                    {
                        foreach (DataRow current in data.Rows)
                        {
                            if (beírás.eredmény.Value.név == (string)current[0])
                            {
                                current[0] = beírás.eredmény.Value.név;
                                //current[1] = beírás.eredmény.sorszám;
                                current[2] = beírás.eredmény.Value.íjtípus;
                                current[3] = beírás.eredmény.Value.csapat;
                                current[10] = beírás.eredmény.Value.megjelent;
                                break;
                            }
                        }
                    }
                }

                if (eredmény_beírva != null) eredmény_beírva(_verseny, beírás);
            }

            //rendezés
            table.Sort(table.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }

        private delegate void Eredmény_Módosítás_Callback(string _azonosító, Eredmény _eredeti, Eredmény _eredmény);
        public void Eredmény_Módosítás(string _azonosító, Eredmény _eredeti, Eredmény _eredmény)
        {
            if (InvokeRequired)
            {
                Eredmény_Módosítás_Callback callback = new Eredmény_Módosítás_Callback(Eredmény_Módosítás);
                Invoke(callback, new object[] { _azonosító, _eredeti, _eredmény });
            }
            else
            {
                if (Program.database.EredményMódosítás(_azonosító, _eredeti, _eredmény) == -666) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (_azonosító == combo_versenyek.Text)
                {
                    foreach (DataRow current in data.Rows)
                    {
                        if (_eredeti.név == current[0].ToString())
                        {
                            //current[0] = _eredmény.név;
                            //current[1] = _eredmény.sorszám;
                            //current[2] = _eredmény.íjtípus;
                            //current[3] = _eredmény.csapat;
                            current[4] = _eredmény.találat_10;
                            current[5] = _eredmény.találat_08;
                            current[6] = _eredmény.találat_05;
                            current[7] = _eredmény.mellé;
                            current[8] = _eredmény.összpont;
                            current[9] = _eredmény.százalék;
                            //current[10] = _eredmény.megjelent;
                            break;
                        }
                    }
                }

                if (eredmény_módosítva != null) eredmény_módosítva(_azonosító, _eredeti, _eredmény);
            }
        }

        private delegate void Eredmény_Törlés_Callback(string _azonosító, Eredmény _eredmény);
        public void Eredmény_Törlés(string _azonosító, Eredmény _eredmény)
        {
            if (InvokeRequired)
            {
                Eredmény_Törlés_Callback callback = new Eredmény_Törlés_Callback(Eredmény_Törlés);
                Invoke(callback, new object[] { _azonosító, _eredmény });
            }
            else
            {
                if (!Program.database.EredményTörlés(_azonosító, _eredmény)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (_azonosító == combo_versenyek.Text)
                {
                    foreach (DataRow current in data.Rows)
                    {
                        if (_eredmény.név == current[0].ToString())
                        {
                            data.Rows.Remove(current);
                            break;
                        }
                    }
                }

                if (eredmény_törölve != null) eredmény_törölve(_azonosító, _eredmény);
            }
        }

        ///

        private delegate void Eredmény_Beírás_Hálózat_Callback(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _nyomtat, Connection _connection);
        public void Eredmény_Beírás_Hálózat(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _nyomtat, Connection _connection)
        {
            if (InvokeRequired)
            {
                Eredmény_Beírás_Hálózat_Callback callback = new Eredmény_Beírás_Hálózat_Callback(Eredmény_Beírás_Hálózat);
                Invoke(callback, new object[] { _név, _verseny, _íjtípus, _csapat, _megjelent, _nyomtat, _connection });
            }
            else
            {
                Database.BeírásEredmény beírás = Program.database.EredményBeírás_Ellenőrzött(_név, _verseny, _íjtípus, _csapat, _megjelent);
                if (beírás.eredmény == null)
                {
                    _connection.Send(ServerCommand.ERROR, "Hiba az eredmény beírásakor!");
                    return;
                }
                else
                {
                    if (_verseny == combo_versenyek.Text)
                    {
                        if (beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
                        {
                            DataRow row = data.NewRow();
                            row[0] = beírás.eredmény.Value.név;
                            row[1] = beírás.eredmény.Value.sorszám;
                            row[2] = beírás.eredmény.Value.íjtípus;
                            row[3] = beírás.eredmény.Value.csapat;
                            row[4] = beírás.eredmény.Value.találat_10;
                            row[5] = beírás.eredmény.Value.találat_08;
                            row[6] = beírás.eredmény.Value.találat_05;
                            row[7] = beírás.eredmény.Value.mellé;
                            row[8] = beírás.eredmény.Value.összpont;
                            row[9] = beírás.eredmény.Value.százalék;
                            row[10] = beírás.eredmény.Value.megjelent;
                            data.Rows.Add(row);
                        }
                        else
                        {
                            foreach (DataRow current in data.Rows)
                            {
                                if (beírás.eredmény.Value.név == (string)current[0])
                                {
                                    current[0] = beírás.eredmény.Value.név;
                                    //current[1] = _eredmény.sorszám;
                                    current[2] = beírás.eredmény.Value.íjtípus;
                                    current[3] = beírás.eredmény.Value.csapat;
                                    //current[4] = beírás.eredmény.Value.találat_10;
                                    //current[5] = beírás.eredmény.Value.találat_08;
                                    //current[6] = beírás.eredmény.Value.találat_05;
                                    //current[7] = beírás.eredmény.Value.mellé;
                                    //current[8] = beírás.eredmény.Value.összpont;
                                    //current[9] = beírás.eredmény.Value.százalék;
                                    current[10] = beírás.eredmény.Value.megjelent;
                                    break;
                                }
                            }
                        }
                    }

                    if (eredmény_beírva != null) eredmény_beírva(_verseny, beírás);

                    Verseny? verseny = Program.database.Verseny(_verseny);

                    if (_nyomtat)
                    {
                        if (verseny.Value.duplabeirlap)
                        {
                            Nyomtat.print(Nyomtat.nyomtat_beirlap(_verseny, beírás.eredmény.Value));
                            Nyomtat.print(Nyomtat.nyomtat_beirlap(_verseny, beírás.eredmény.Value));
                        }
                        else
                        {
                            Nyomtat.print(Nyomtat.nyomtat_beirlap(_verseny, beírás.eredmény.Value));
                        }
                    }
                }
            }
        }

        private delegate void Eredmény_Módosítás_Hálózat_Callback(string _azonosító, Eredmény _eredeti, Eredmény _eredmény, Connection _connection);
        public void Eredmény_Módosítás_Hálózat(string _azonosító, Eredmény _eredeti, Eredmény _eredmény, Connection _connection)
        {
            if (InvokeRequired)
            {
                Eredmény_Módosítás_Hálózat_Callback callback = new Eredmény_Módosítás_Hálózat_Callback(Eredmény_Módosítás_Hálózat);
                Invoke(callback, new object[] { _azonosító, _eredeti, _eredmény, _connection });
            }
            else
            {
                Eredmény? eredmény = Program.database.EredményMódosítás_Ellenőrzött(_azonosító, _eredeti, _eredmény);
                if (eredmény == null)
                {
                    _connection.Send(ServerCommand.ERROR, "Hiba az eredmény módosításakor!");
                    return;
                }
                else
                {
                    if (_azonosító == combo_versenyek.Text)
                    {
                        foreach (DataRow current in data.Rows)
                        {
                            if (_eredeti.név == current[0].ToString())
                            {
                                current[0] = eredmény.Value.név;
                                current[1] = eredmény.Value.sorszám;
                                current[2] = eredmény.Value.íjtípus;
                                current[3] = eredmény.Value.csapat;
                                current[4] = eredmény.Value.találat_10;
                                current[5] = eredmény.Value.találat_08;
                                current[6] = eredmény.Value.találat_05;
                                current[7] = eredmény.Value.mellé;
                                current[8] = eredmény.Value.összpont;
                                current[9] = eredmény.Value.százalék;
                                current[10] = eredmény.Value.megjelent;
                                break;
                            }
                        }
                    }

                    if (eredmény_módosítva != null) eredmény_módosítva(_azonosító, _eredeti, eredmény.Value);
                }
            }
        }
        #endregion

        #region EventHandlers
        public void verseny_hozzáadás(Verseny _verseny)
        {
            combo_versenyek.Items.Add(_verseny.azonosító);
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

        public void verseny_törlés(string _azonosító)
        {
            combo_versenyek.Items.Remove(_azonosító);
        }

        public void verseny_lezárás(string _azonosító)
        {
            if (_azonosító == combo_versenyek.Text)
            {
                lezárva = true;
            }
        }

        public void verseny_megnyitás(string _azonosító)
        {
            if (_azonosító == combo_versenyek.Text)
            {
                lezárva = false;
            }
        }

        ///

        public void induló_átnevezés(string _eredeti_név, string _új_név)
        {
            foreach (DataRow current in data.Rows)
            {
                if (_eredeti_név == current[0].ToString())
                {
                    current[0] = _új_név;
                    break;
                }
            }
        }

        ///

        private void table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 200;
            table.Columns[1].Width = 50;
            table.Columns[2].Width = 80;
            table.Columns[3].Width = 45;
            table.Columns[4].Width = 35;
            table.Columns[5].Width = 35;
            table.Columns[6].Width = 35;
            table.Columns[7].Width = 35;
            table.Columns[8].Width = 45;
            table.Columns[9].Width = 45;
            table.Columns[10].Width = 58;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;

            //rendezés
            table.Sort(table.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void combo_versenyek_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            List<Eredmény> eredmények = Program.database.Eredmények(combo_versenyek.Text);

            data.Rows.Clear();

            foreach (Eredmény current in eredmények)
            {
                DataRow row = data.NewRow();
                row[0] = current.név;
                row[1] = current.sorszám;
                row[2] = current.íjtípus;
                row[3] = current.csapat;
                row[4] = current.találat_10;
                row[5] = current.találat_08;
                row[6] = current.találat_05;
                row[7] = current.mellé;
                row[8] = current.összpont;
                row[9] = current.százalék;
                row[10] = current.megjelent;

                data.Rows.Add(row);
            }

            lezárva = Program.database.Verseny_Lezárva(combo_versenyek.SelectedItem.ToString());
            verseny_összpont = Program.database.Verseny_Összespont(combo_versenyek.SelectedItem.ToString());
        }

        private void módosítás_Click(object _sender, EventArgs _event)
        {
            if (combo_versenyek.Items.Count == 0) { MessageBox.Show("Nincsen még egy verseny sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (table.SelectedRows.Count == 0) { MessageBox.Show("Nem megfelelő a kiválasztott eredmények száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

            Form_Eredmény eredmény_form = new Form_Eredmény(combo_versenyek.Text, verseny_összpont, new Eredmény(
                table.SelectedRows[0].Cells[0].Value.ToString(),
                Convert.ToInt32(table.SelectedRows[0].Cells[1].Value),
                table.SelectedRows[0].Cells[2].Value.ToString(),
                Convert.ToInt32(table.SelectedRows[0].Cells[3].Value),
                Convert.ToInt32(table.SelectedRows[0].Cells[4].Value),
                Convert.ToInt32(table.SelectedRows[0].Cells[5].Value),
                Convert.ToInt32(table.SelectedRows[0].Cells[6].Value),
                Convert.ToInt32(table.SelectedRows[0].Cells[7].Value),
                Convert.ToInt32(table.SelectedRows[0].Cells[8].Value),
                Convert.ToInt32(table.SelectedRows[0].Cells[9].Value),
                Convert.ToBoolean(table.SelectedRows[0].Cells[10].Value)
                ));
            eredmény_form.ShowDialog();
        }

        private void törlés_Click(object _sender, EventArgs _event)
        {
            if (combo_versenyek.SelectedItem == null) return;
            if (!(table.SelectedRows.Count != 0 && table.SelectedRows[0].Index < data.Rows.Count && table.SelectedRows[0].Selected)) return;
            if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

            if (MessageBox.Show("Biztosan törli ezt az eredményt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (DataGridViewRow current in table.Rows)
            {
                if (table.SelectedRows[0] == current)
                {
                    Eredmény_Törlés(combo_versenyek.Text, new Eredmény(

                        current.Cells[0].Value.ToString(),
                        Convert.ToInt32(current.Cells[1].Value),
                        current.Cells[2].Value.ToString(),
                        Convert.ToInt32(current.Cells[3].Value),
                        Convert.ToInt32(current.Cells[4].Value),
                        Convert.ToInt32(current.Cells[5].Value),
                        Convert.ToInt32(current.Cells[6].Value),
                        Convert.ToInt32(current.Cells[7].Value),
                        Convert.ToInt32(current.Cells[8].Value),
                        Convert.ToInt32(current.Cells[9].Value),
                        Convert.ToBoolean(current.Cells[10].Value)
                        ));
                }
            }
        }

        private void lezárás_Click(object _sender, EventArgs _event)
        {
            if (combo_versenyek.SelectedItem == null) return;
            if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };
            if (MessageBox.Show("Biztosan lezárja a versenyt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            foreach (DataRow current in data.Rows)
            {
                if (Convert.ToInt32(current[4]) + Convert.ToInt32(current[5]) + Convert.ToInt32(current[6]) + Convert.ToInt32(current[7]) != (verseny_összpont) && current[10].ToString() != "False")
                { MessageBox.Show("Nem megfelelő a pontozás a következő indulónál: " + current[0].ToString() + "!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }

            Program.mainform.verseny_panel.Verseny_Lezárás(combo_versenyek.Text);
            Program.mainform.korosztályok_panel.Verseny_Számolás(combo_versenyek.Text);
        }

        void megnyitás_Click(object sender, EventArgs e)
        {
            if (combo_versenyek.SelectedItem == null) return;
            if (!lezárva) { return; };
            if (MessageBox.Show("Biztosan megnyitja a versenyt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            //Program.mainform.verseny_panel.verse
            Program.mainform.verseny_panel.Verseny_Megnyitás(combo_versenyek.Text);
        }


        void keresés_TextChanged(object sender, EventArgs e)
        {
            if (keresés.Text.ToString().Length == 0)
                return;

            foreach (DataGridViewRow item in table.Rows)
            {
                bool találat = true;
                for (int i = 0; i < keresés.Text.Length; i++)
                {
                    if ((item.Cells[1].Value.ToString()) != keresés.Text )
                    {
                        találat = false;
                    }

                }
                if (találat == true)
                {
                    table.Rows[item.Index].Selected = true;
                    table.FirstDisplayedScrollingRowIndex = item.Index;
                    return;
                }

            }
        }


        void keresés_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (keresés.Focus() == true && keresés.Text.Length != 0)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (combo_versenyek.Items.Count == 0) { MessageBox.Show("Nincsen még egy verseny sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (table.SelectedRows.Count == 0) { MessageBox.Show("Nem megfelelő a kiválasztott eredmények száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };


                    Form_Eredmény eredmény_form = new Form_Eredmény(combo_versenyek.Text, verseny_összpont, new Eredmény(
                        table.SelectedRows[0].Cells[0].Value.ToString(),
                        Convert.ToInt32(table.SelectedRows[0].Cells[1].Value),
                        table.SelectedRows[0].Cells[2].Value.ToString(),
                        Convert.ToInt32(table.SelectedRows[0].Cells[3].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[4].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[5].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[6].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[7].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[8].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[9].Value),
                        Convert.ToBoolean(table.SelectedRows[0].Cells[10].Value)
                        ));
                    eredmény_form.ShowDialog();
                }
            }
        }     
        #endregion

        public sealed class Form_Eredmény : Form
        {
            public string verseny_azonosító;
            private Eredmény eredeti;
            private int összespont;

            private ComboBox combo_név;
            private ComboBox combo_íjtípus;
            private ComboBox combo_csapat;
            private TextBox box_találat_10;
            private TextBox box_találat_8;
            private TextBox box_találat_5;
            private TextBox box_mellé;
            private Label label_összes;
            private Label label_százalék;
            private CheckBox box_megjelent;

            public Form_Eredmény(string _verseny, int _összespont, Eredmény _eredmény)
            {
                eredeti = _eredmény;
                összespont = _összespont;
                verseny_azonosító = _verseny;

                InitializeForm();
                InitializeContent();
                InitializeData(_eredmény);

                AddAcessorHooks();
                FormClosing += RemoveAccessorHooks;
            }

            private void InitializeForm()
            {
                Text = "Eredmény";
                ClientSize = new System.Drawing.Size(464 -64, 320 + 32);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            }

            private void InitializeContent()
            {
                Label név = new Label();
                név.Text = "Név:";
                név.Location = new System.Drawing.Point(32, 16 + 0 * 32);

                Label íjtípus = new Label();
                íjtípus.Text = "Íjtípus:";
                íjtípus.Location = new System.Drawing.Point(név.Location.X, 16 + 1 * 32);

                Label csapat = new Label();
                csapat.Text = "Csapatszám:";
                csapat.Location = new System.Drawing.Point(név.Location.X, 16 + 2 * 32);

                Label találat_10 = new Label();
                találat_10.Text = "Tíz találat:";
                találat_10.Location = new System.Drawing.Point(név.Location.X, 16 + 3 * 32);

                Label találat_8 = new Label();
                találat_8.Text = "Nyolc találat:";
                találat_8.Location = new System.Drawing.Point(név.Location.X, 16 + 4 * 32);

                Label találat_5 = new Label();
                találat_5.Text = "Öt találat:";
                találat_5.Location = new System.Drawing.Point(név.Location.X, 16 + 5 * 32);

                Label mellé = new Label();
                mellé.Text = "Mellé találat:";
                mellé.Location = new System.Drawing.Point(név.Location.X, 16 + 6 * 32);

                Label összes = new Label();
                összes.Text = "Összes találat:";
                összes.Location = new System.Drawing.Point(név.Location.X, 16 + 7 * 32);

                Label százalék = new Label();
                százalék.Text = "Eredmény százalék:";
                százalék.Location = new System.Drawing.Point(név.Location.X, 16 + 8 * 32);
                százalék.AutoSize = true;

                Label megjelent = new Label();
                megjelent.Text = "Megjelent:";
                megjelent.Location = new System.Drawing.Point(név.Location.X, 16 + 9 * 32);

                ///

                combo_név = new ComboBox();
                combo_név.Location = new System.Drawing.Point(név.Location.X + név.Size.Width + 16, név.Location.Y);
                combo_név.Size = new System.Drawing.Size(128 + 64, 24);
                combo_név.DropDownStyle = ComboBoxStyle.DropDownList;

                combo_íjtípus = new ComboBox();
                combo_íjtípus.Location = new System.Drawing.Point(íjtípus.Location.X + íjtípus.Size.Width + 16, íjtípus.Location.Y);
                combo_íjtípus.Size = combo_név.Size;
                combo_íjtípus.DropDownStyle = ComboBoxStyle.DropDownList;

                combo_csapat = new ComboBox();
                combo_csapat.Location = new System.Drawing.Point(csapat.Location.X + csapat.Size.Width + 16, csapat.Location.Y);
                combo_csapat.Size = combo_név.Size;
                combo_csapat.DropDownStyle = ComboBoxStyle.DropDownList;

                box_találat_10 = new TextBox();
                box_találat_10.Location = new System.Drawing.Point(találat_10.Location.X + találat_10.Size.Width + 16, találat_10.Location.Y);
                box_találat_10.Size = new System.Drawing.Size(64, 24);

                box_találat_8 = new TextBox();
                box_találat_8.Location = new System.Drawing.Point(találat_8.Location.X + találat_8.Size.Width + 16, találat_8.Location.Y);
                box_találat_8.Size = box_találat_10.Size;

                box_találat_5 = new TextBox();
                box_találat_5.Location = new System.Drawing.Point(találat_5.Location.X + találat_5.Size.Width + 16, találat_5.Location.Y);
                box_találat_5.Size = box_találat_10.Size;

                box_mellé = new TextBox();
                box_mellé.Location = new System.Drawing.Point(mellé.Location.X + mellé.Size.Width + 16, mellé.Location.Y);
                box_mellé.Size = box_találat_10.Size;

                box_találat_10.TextChanged += eredmény_számolás;
                box_találat_8.TextChanged += eredmény_számolás;
                box_találat_5.TextChanged += eredmény_számolás;
                box_mellé.TextChanged += eredmény_számolás;

                label_összes = new Label();
                label_összes.Location = new System.Drawing.Point(összes.Location.X + összes.Size.Width + 16, összes.Location.Y);

                label_százalék = new Label();
                label_százalék.Location = new System.Drawing.Point(százalék.Location.X + százalék.Size.Width + 16, százalék.Location.Y);

                //

                box_megjelent = new CheckBox();
                box_megjelent.Checked = false;
                box_megjelent.Location = new System.Drawing.Point(megjelent.Location.X + megjelent.Size.Width + 16, megjelent.Location.Y - 4);

                Button rendben = new Button();
                rendben.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                rendben.Text = "Rendben";
                rendben.Size = new System.Drawing.Size(96, 32);
                rendben.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
                rendben.Click += rendben_Click;

                ///

                List<Induló> indulók = Program.database.Indulók();
                foreach (Induló current in indulók)
                    combo_név.Items.Add(current.név);

                List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
                foreach (Íjtípus current in íjtípusok)
                    combo_íjtípus.Items.Add(current.azonosító);

                for (int i = 0; i < 35; i++) combo_csapat.Items.Add(i + 1);

                ///

                Controls.Add(név);
                Controls.Add(íjtípus);
                Controls.Add(csapat);
                Controls.Add(találat_10);
                Controls.Add(találat_8);
                Controls.Add(találat_5);
                Controls.Add(mellé);
                Controls.Add(összes);
                Controls.Add(százalék);
                Controls.Add(megjelent);

                Controls.Add(combo_név);
                Controls.Add(combo_íjtípus);
                Controls.Add(combo_csapat);
                Controls.Add(box_találat_10);
                Controls.Add(box_találat_8);
                Controls.Add(box_találat_5);
                Controls.Add(box_mellé);
                Controls.Add(label_összes);
                Controls.Add(label_százalék);
                Controls.Add(box_megjelent);
                Controls.Add(rendben);
            }


            private void InitializeData(Eredmény _eredmény)
            {
                combo_név.Text = _eredmény.név;
                combo_név.Enabled = false;
                combo_íjtípus.Text = _eredmény.íjtípus;
                combo_íjtípus.Enabled = false;
                combo_csapat.SelectedItem = Convert.ToInt32(_eredmény.csapat);
                combo_csapat.Enabled = false;

                box_találat_10.Text = _eredmény.találat_10.ToString();
                box_találat_8.Text = _eredmény.találat_08.ToString();
                box_találat_5.Text = _eredmény.találat_05.ToString();
                box_mellé.Text = _eredmény.mellé.ToString();
                label_összes.Text = _eredmény.összpont.ToString();
                label_százalék.Text = _eredmény.százalék.ToString() + "%";
                box_megjelent.Checked = _eredmény.megjelent;
                box_megjelent.Enabled = false;
            }

            private void AddAcessorHooks()
            {
                Program.mainform.íjtípusok_panel.íjtípus_hozzáadva += íjtípus_hozzáadás;
                Program.mainform.íjtípusok_panel.íjtípus_módosítva += íjtípus_módosítás;
                Program.mainform.íjtípusok_panel.íjtípus_törölve += íjtípus_törlés;

                Program.mainform.indulók_panel.induló_hozzáadva += induló_hozzáadás;
                Program.mainform.indulók_panel.induló_módosítva += induló_módosítás;
                Program.mainform.indulók_panel.induló_törölve += induló_törlés;
            }

            private void RemoveAccessorHooks(object _sender, EventArgs _event)
            {
                Program.mainform.íjtípusok_panel.íjtípus_hozzáadva -= íjtípus_hozzáadás;
                Program.mainform.íjtípusok_panel.íjtípus_módosítva -= íjtípus_módosítás;
                Program.mainform.íjtípusok_panel.íjtípus_törölve -= íjtípus_törlés;

                Program.mainform.indulók_panel.induló_hozzáadva -= induló_hozzáadás;
                Program.mainform.indulók_panel.induló_módosítva -= induló_módosítás;
                Program.mainform.indulók_panel.induló_törölve -= induló_törlés;
            }

            #region EventHandlers
            public void íjtípus_hozzáadás(Íjtípus _íjtípus)
            {
                combo_íjtípus.Items.Add(_íjtípus.azonosító);
            }

            public void íjtípus_módosítás(string _azonosító, Íjtípus _íjtípus)
            {
                if (_azonosító != _íjtípus.azonosító)
                {
                    for (int current = 0; current < combo_íjtípus.Items.Count; ++current)
                    {
                        if (_azonosító == combo_íjtípus.Items[current].ToString())
                        {
                            combo_íjtípus.Items[current] = _íjtípus.azonosító;
                            break;
                        }
                    }
                }
            }

            public void íjtípus_törlés(string _azonosító)
            {
                combo_íjtípus.Items.Remove(_azonosító);
            }

            public void induló_hozzáadás(Induló _induló)
            {
                combo_név.Items.Add(_induló.név);
            }

            public void induló_módosítás(string _név, Induló _induló)
            {
                if (_név != _induló.név)
                {
                    for (int current = 0; current < combo_név.Items.Count; ++current)
                    {
                        if (_név == combo_név.Items[current].ToString())
                        {
                            combo_név.Items[current] = _induló.név;
                            break;
                        }
                    }
                }
            }

            public void induló_törlés(string _név)
            {
                combo_név.Items.Remove(_név);
            }

            private void rendben_Click(object _sender, EventArgs _event)
            {
                if (combo_név.SelectedItem == null) { MessageBox.Show("Nincs kiválasztva név!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (combo_íjtípus.SelectedItem == null) { MessageBox.Show("Nincs kiválasztva íjtípus!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int találat_10;
                try { találat_10 = Convert.ToInt32(box_találat_10.Text); if (találat_10 < 0) { MessageBox.Show("Nem megfelelő a 10 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; } }
                catch { MessageBox.Show("Nem megfelelő a 10 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int találat_8;
                try { találat_8 = Convert.ToInt32(box_találat_8.Text); if (találat_8 < 0) { MessageBox.Show("Nem megfelelő a 8 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; } }
                catch { MessageBox.Show("Nem megfelelő a 8 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int találat_5;
                try { találat_5 = Convert.ToInt32(box_találat_5.Text); if (találat_5 < 0) { MessageBox.Show("Nem megfelelő az 5 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; } }
                catch { MessageBox.Show("Nem megfelelő az 5 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int találat_mellé;
                try { találat_mellé = Convert.ToInt32(box_mellé.Text); if (találat_mellé < 0) { MessageBox.Show("Nem megfelelő a mellé találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; } }
                catch { MessageBox.Show("Nem megfelelő a mellé találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                int összes = találat_10 * 10 + találat_8 * 8 + találat_5 * 5;
                int százalék = (int)(((double)összes / (összespont * 10)) * 100);

                if (!((találat_10 == 0 && találat_8 == 0 && találat_5 == 0 && találat_mellé == 0) || (összespont == találat_10 + találat_8 + találat_5 + találat_mellé))) { MessageBox.Show("Nem megfelelő a lövések darabszáma!\n" + "Lövések darabszáma: " + (összespont).ToString() + "\nBeírt lövések: " + (találat_10 + találat_8 + találat_5 + találat_mellé).ToString(), "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                Program.mainform.eredmények_panel.Eredmény_Módosítás(verseny_azonosító, eredeti,
                    new Eredmény(eredeti.név, eredeti.sorszám, eredeti.íjtípus, eredeti.csapat, Convert.ToInt32(box_találat_10.Text),
                        Convert.ToInt32(box_találat_8.Text), Convert.ToInt32(box_találat_5.Text), Convert.ToInt32(box_mellé.Text), összes, százalék, box_megjelent.Checked));

                Close();
            }

            private void eredmény_számolás(object _sender, EventArgs _event)
            {
                int találat_10;
                try { találat_10 = Convert.ToInt32(box_találat_10.Text); if (találat_10 < 0) { return; } }
                catch { return; }
                int találat_8;
                try { találat_8 = Convert.ToInt32(box_találat_8.Text); if (találat_8 < 0) { return; } }
                catch { return; }
                int találat_5;
                try { találat_5 = Convert.ToInt32(box_találat_5.Text); if (találat_5 < 0) { return; } }
                catch { return; }
                int találat_mellé;
                try { találat_mellé = Convert.ToInt32(box_mellé.Text); if (találat_mellé < 0) { return; } }
                catch { return; }

                label_összes.Text = (Convert.ToInt32(box_találat_10.Text) * 10 + Convert.ToInt32(box_találat_8.Text) * 8 + Convert.ToInt32(box_találat_5.Text) * 5).ToString();
                List<Verseny> versenyek = Program.database.Versenyek();
                foreach (Verseny item in versenyek)
                {
                    if (item.azonosító == verseny_azonosító) { label_százalék.Text = ((int)(((double)Convert.ToInt32(label_összes.Text) / (item.összes * 10)) * 100)).ToString() + "%"; return; }
                }
            }
            #endregion
        }

    }
}
