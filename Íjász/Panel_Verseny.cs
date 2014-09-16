using System;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Íjász
{
    public struct Verseny
    {
        public string azonosító;
        public string megnevezés;
        public string dátum;
        public string versenysorozat;
        public int összes;
        public int állomások;
        public int indulók;
        public bool lezárva;

        public Verseny(string _azonosító, string _megnevezés, string _dátum, string _versenysorozat, int _összes, int _állomások, int _indulók, bool _lezárva)
        {
            azonosító = _azonosító;
            megnevezés = _megnevezés;
            dátum = _dátum;
            versenysorozat = _versenysorozat;
            összes = _összes;
            állomások = _állomások;
            indulók = _indulók;
            lezárva = _lezárva;
        }
    }

    public delegate void Verseny_Hozzáadva(Verseny _verseny);
    public delegate void Verseny_Módosítva(string _azonosító, Verseny _verseny);
    public delegate void Verseny_Törölve(string _azonosító);
    public delegate void Verseny_Lezárva(string _azonosító);

    public sealed class Panel_Verseny : Control
    {
        public Verseny_Hozzáadva verseny_hozzáadva;
        public Verseny_Módosítva verseny_módosítva;
        public Verseny_Törölve verseny_törölve;
        public Verseny_Lezárva verseny_lezárva;
        public Verseny_Lezárva verseny_megnyitva;

        private DataTable data;
        private DataGridView table;

        public Form_Verseny verseny_form;

        public Panel_Verseny()
        {
            InitializeContent();

            verseny_form = new Form_Verseny();
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
            table.Width = 743;
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
            hozzáadás.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
            hozzáadás.Click += hozzáadás_Click;

            Button törlés = new Button();
            törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            törlés.Text = "Törlés";
            törlés.Size = new System.Drawing.Size(96, 32);
            törlés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16);
            törlés.Click += törlés_Click;

            ///

            Controls.Add(table);

            Controls.Add(hozzáadás);
            Controls.Add(törlés);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Megnevezés", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Dátum", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Versenysorozat", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Lövések", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Állomások", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Indulók száma", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Lezárva", System.Type.GetType("System.Boolean")));

            List<Verseny> versenysorozatok = Program.database.Versenyek();

            foreach (Verseny current in versenysorozatok)
            {
                DataRow row = data.NewRow();
                row[0] = current.azonosító;
                row[1] = current.megnevezés;
                row[2] = current.dátum;
                row[3] = current.versenysorozat;
                row[4] = current.összes;
                row[5] = current.állomások;
                row[6] = current.indulók;
                row[7] = current.lezárva;

                data.Rows.Add(row);
            }

            return data;
        }

        #region Accessors
        private delegate void Verseny_Hozzáadás_Callback(Verseny _verseny);
        public void Verseny_Hozzáadás(Verseny _verseny)
        {
            if (InvokeRequired)
            {
                Verseny_Hozzáadás_Callback callback = new Verseny_Hozzáadás_Callback(Verseny_Hozzáadás);
                Invoke(callback, new object[] { _verseny });
            }
            else
            {
                if (_verseny.azonosító.Contains(" ")) { MessageBox.Show("A versenyazonosító nem tartalmazhat szóközt!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Program.database.ÚjVerseny(_verseny)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                DataRow row = data.NewRow();
                row[0] = _verseny.azonosító;
                row[1] = _verseny.megnevezés;
                row[2] = _verseny.dátum;
                row[3] = _verseny.versenysorozat;
                row[4] = _verseny.összes;
                row[5] = _verseny.állomások;
                row[6] = _verseny.indulók;
                row[7] = _verseny.lezárva;
                data.Rows.Add(row);

                if (verseny_hozzáadva != null) verseny_hozzáadva(_verseny);
            }
        }

        private delegate void Verseny_Módosítva_Callback(string _azonosító, Verseny _verseny);
        public void Verseny_Módosítás(string _azonosító, Verseny _verseny)
        {
            if (InvokeRequired)
            {
                Verseny_Módosítva_Callback callback = new Verseny_Módosítva_Callback(Verseny_Módosítás);
                Invoke(callback, new object[] { _azonosító, _verseny });
            }
            else
            {
                if (!Program.database.VersenyMódosítás(_azonosító, _verseny)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[0] = _verseny.azonosító;
                        current[1] = _verseny.megnevezés;
                        current[2] = _verseny.dátum;
                        current[3] = _verseny.versenysorozat;
                        current[4] = _verseny.összes;
                        current[5] = _verseny.állomások;
                        current[6] = _verseny.indulók;
                        current[7] = _verseny.lezárva;
                        break;
                    }
                }

                if (verseny_módosítva != null) verseny_módosítva(_azonosító, _verseny);
            }
        }

        private delegate void Verseny_Törölve_Callback(string _azonosító);
        public void Verseny_Törlés(string _azonosító)
        {
            if (InvokeRequired)
            {
                Verseny_Törölve_Callback callback = new Verseny_Törölve_Callback(Verseny_Törlés);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.VersenyTörlés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        data.Rows.RemoveAt(table.SelectedRows[0].Index);
                        break;
                    }
                }

                if (verseny_törölve != null) verseny_törölve(_azonosító);
            }
        }

        private delegate void Verseny_Lezárás_Callback(string _azonosító);
        public void Verseny_Lezárás(string _azonosító)
        {
            if (InvokeRequired)
            {
                Verseny_Lezárás_Callback callback = new Verseny_Lezárás_Callback(Verseny_Lezárás);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.Verseny_Lezárás(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[6] = true;
                        break;
                    }
                }

                if (verseny_lezárva != null) verseny_lezárva(_azonosító);
            }
        }

        private delegate void Verseny_Megnyitás_Callback(string _azonosító);
        public void Verseny_Megnyitás(string _azonosító)
        {
            if (InvokeRequired)
            {
                Verseny_Megnyitás_Callback callback = new Verseny_Megnyitás_Callback(Verseny_Lezárás);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.Verseny_Megnyitás(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[6] = false;
                        break;
                    }
                }

                if (verseny_megnyitva != null) verseny_megnyitva(_azonosító);
            }
        }
        #endregion

        #region EventHandlers
        public void eredmény_beírás(string _azonosító, Database.BeírásEredmény _beírás)
        {
            if (_beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
            {
                if (!Program.database.Verseny_IndulókNövelés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[5] = ((int)current[5]) + 1;
                        return;
                    }
                }

                MessageBox.Show("Nem taláható a verseny?!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void eredmény_törlés(string _azonosító, Eredmény _eredmény)
        {
            if (!Program.database.Verseny_IndulókCsökkentés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[5] = ((int)current[5]) - 1;
                    return;
                }
            }

            MessageBox.Show("Nem taláható a verseny?!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        ///

        void table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 80;
            table.Columns[1].Width = 200;
            table.Columns[2].Width = 90;
            table.Columns[3].Width = 80;
            table.Columns[4].Width = 60;
            table.Columns[5].Width = 60;
            table.Columns[6].Width = 120;
            table.Columns[7].Width = 50;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void hozzáadás_Click(object _sender, EventArgs _event)
        {
            verseny_form = new Form_Verseny();
            verseny_form.ShowDialog();
        }

        private void módosítás_Click(object _sender, EventArgs _event)
        {
            if (table.SelectedRows.Count != 1) return;

            verseny_form = new Form_Verseny(new Verseny(data.Rows[table.SelectedRows[0].Index][0].ToString(),
                data.Rows[table.SelectedRows[0].Index][1].ToString(), data.Rows[table.SelectedRows[0].Index][2].ToString(), data.Rows[table.SelectedRows[0].Index][3].ToString(),
                Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][4]), Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][5]), Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][6]),
                Convert.ToBoolean(data.Rows[table.SelectedRows[0].Index][7])));
            verseny_form.ShowDialog();
        }

        private void törlés_Click(object _sender, EventArgs _event)
        {
            if (table.SelectedRows.Count != 1) return;
            if (0 < (int)(data.Rows[table.SelectedRows[0].Index][5])) { MessageBox.Show("Ez a verseny nem törölhető, mivel van hozzárendelve eredmény!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (MessageBox.Show("Biztosan törli ezt a versenyt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // TODO ezt egyben kellene!
            Program.mainform.versenysorozat_panel.Versenysorozat_VersenyCsökkentés(data.Rows[table.SelectedRows[0].Index][3].ToString());
            Verseny_Törlés(data.Rows[table.SelectedRows[0].Index][0].ToString());
        }
        #endregion

        public sealed class Form_Verseny : Form
        {
            private string eredeti_azonosító = null;
            private string eredeti_versenysorozat = null;

            private TextBox box_azonosító;
            private TextBox box_megnevezés;
            private DateTimePicker dátumválasztó;
            private ComboBox combo_versenysorozat;
            private TextBox box_összes;
            private TextBox box_állomások;
            private Label label_indulók;
            private Label label_lezárva;

            public Form_Verseny()
            {
                InitializeForm();
                InitializeContent();
                InitializeData();
            }

            public Form_Verseny(Verseny _verseny)
            {
                eredeti_azonosító = _verseny.azonosító;
                eredeti_versenysorozat = _verseny.versenysorozat;

                InitializeForm();
                InitializeContent();
                InitializeData(_verseny);
            }

            private void InitializeForm()
            {
                Text = "Verseny";
                ClientSize = new System.Drawing.Size(400 - 64, 308);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent()
            {
                Label azonosító = new Label();
                azonosító.Text = "Azonosító:";
                azonosító.Location = new System.Drawing.Point(16, 16 + 0 * 32);
                //azonosító.Font = new System.Drawing.Font("Arial Black", 10);

                Label megnevezés = new Label();
                megnevezés.Text = "Megnevezés:";
                megnevezés.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 1 * 32);
                //megnevezés.Font = new System.Drawing.Font("Arial Black", 10);

                Label dátum = new Label();
                dátum.Text = "Dátum:";
                dátum.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 2 * 32);

                Label versenysorozat = new Label();
                versenysorozat.Text = "Versenysorozat:";
                versenysorozat.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 3 * 32);

                Label összes = new Label();
                összes.Text = "Lövések száma:";
                összes.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 4 * 32);

                Label állomások = new Label();
                állomások.Text = "Állomások száma:";
                állomások.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 5 * 32);

                Label indulók = new Label();
                indulók.Text = "Indulók száma:";
                indulók.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 6 * 32);
                //szám.Font = new System.Drawing.Font("Arial Black", 10);

                Label lezárva = new Label();
                lezárva.Text = "Lezárva:";
                lezárva.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 7 * 32);
                
                ///

                box_azonosító = new TextBox();
                box_azonosító.Location = new System.Drawing.Point(azonosító.Location.X + azonosító.Size.Width + 16, azonosító.Location.Y);
                box_azonosító.Size = new System.Drawing.Size(128 + 64, 24);
                box_azonosító.MaxLength = 10;

                box_megnevezés = new TextBox();
                box_megnevezés.Location = new System.Drawing.Point(megnevezés.Location.X + megnevezés.Size.Width + 16, megnevezés.Location.Y);
                box_megnevezés.Size = box_azonosító.Size;
                box_megnevezés.MaxLength = 30;

                dátumválasztó = new DateTimePicker();
                dátumválasztó.Location = new System.Drawing.Point(dátum.Location.X + dátum.Size.Width + 16, dátum.Location.Y);
                dátumválasztó.Size = box_azonosító.Size;
                dátumválasztó.Value = DateTime.Now;

                combo_versenysorozat = new ComboBox();
                combo_versenysorozat.Location = new System.Drawing.Point(versenysorozat.Location.X + versenysorozat.Size.Width + 16, versenysorozat.Location.Y);
                combo_versenysorozat.Size = box_azonosító.Size;
                combo_versenysorozat.DropDownStyle = ComboBoxStyle.DropDownList;

                box_összes = new TextBox();
                box_összes.Location = new System.Drawing.Point(összes.Location.X + összes.Size.Width + 16, összes.Location.Y);
                box_összes.Size = box_azonosító.Size;

                box_állomások = new TextBox();
                box_állomások.Location = new System.Drawing.Point(állomások.Location.X + állomások.Size.Width + 16, állomások.Location.Y);
                box_állomások.Size = box_azonosító.Size;

                label_indulók = new Label();
                label_indulók.Location = new System.Drawing.Point(indulók.Location.X + indulók.Size.Width + 16, indulók.Location.Y);
                label_indulók.Size = box_azonosító.Size;

                label_lezárva = new Label();
                label_lezárva.Location = new System.Drawing.Point(lezárva.Location.X + lezárva.Size.Width + 16, lezárva.Location.Y);
                label_lezárva.Size = box_azonosító.Size;
                
                Button rendben = new Button();
                rendben.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                rendben.Text = "Rendben";
                rendben.Size = new System.Drawing.Size(96, 32);
                rendben.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
                rendben.Click += rendben_Click;

                ///

                combo_versenysorozat.Items.Add("");
                List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
                foreach (Versenysorozat current in versenysorozatok)
                    combo_versenysorozat.Items.Add(current.azonosító);

                ///

                Controls.Add(azonosító);
                Controls.Add(megnevezés);
                Controls.Add(dátum);
                Controls.Add(versenysorozat);
                Controls.Add(összes);
                Controls.Add(állomások);
                Controls.Add(indulók);
                Controls.Add(lezárva);

                Controls.Add(box_azonosító);
                Controls.Add(box_megnevezés);
                Controls.Add(dátumválasztó);
                Controls.Add(combo_versenysorozat);
                Controls.Add(box_összes);
                Controls.Add(box_állomások);
                Controls.Add(label_indulók);
                Controls.Add(label_lezárva);

                Controls.Add(rendben);
            }

            private void InitializeData()
            {
                box_azonosító.Text = "";
                box_azonosító.Enabled = true;
                box_megnevezés.Text = "";
                dátumválasztó.Value = DateTime.Now;
                combo_versenysorozat.Text = "";
                box_összes.Text = "0";
                box_állomások.Text = "0";
                label_indulók.Text = "0";
                label_lezárva.Text = "Hamis";
            }

            private void InitializeData(Verseny _verseny)
            {
                box_azonosító.Text = _verseny.azonosító;
                box_azonosító.Enabled = (_verseny.indulók == 0) ? true : false;
                box_megnevezés.Text = _verseny.megnevezés;
                dátumválasztó.Value = DateTime.Parse(_verseny.dátum);
                combo_versenysorozat.Text = _verseny.versenysorozat;
                box_összes.Text = (_verseny.összes).ToString();
                box_összes.Enabled = (_verseny.indulók == 0) ? true : false;
                box_állomások.Text = (_verseny.állomások).ToString();
                box_állomások.Enabled = (_verseny.indulók == 0) ? true : false;
                label_indulók.Text = _verseny.indulók.ToString();
                label_lezárva.Text = _verseny.lezárva ? "Igen" : "Nem";
            }

            #region EventHandlers
            public void versenysorozat_hozzáadás(Versenysorozat _versenysorozat)
            {
                combo_versenysorozat.Items.Add(_versenysorozat.megnevezés + " (" + _versenysorozat.azonosító + ")");
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
                            return;
                        }
                    }
                }
            }

            public void versenysorozat_törlés(string _azonosító)
            {
                combo_versenysorozat.Items.Remove(_azonosító);
            }

            ///

            public void eredmény_beírás(string _azonosító, Database.BeírásEredmény _beírás)
            {
                if (_beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
                {
                    if (_azonosító == eredeti_azonosító) label_indulók.Text = ((Convert.ToInt32(label_indulók.Text)) + 1).ToString();
                }
            }

            public void eredmény_törlés(string _azonosító, Eredmény _eredmény)
            {
                if (_azonosító == eredeti_azonosító)
                {
                    label_indulók.Text = ((Convert.ToInt32(label_indulók.Text)) - 1).ToString();
                }
            }


            ///

            private void rendben_Click(object _sender, EventArgs _event)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                box_azonosító.Text = rgx.Replace(box_azonosító.Text, "");

                if (!(0 < box_azonosító.Text.Length && box_azonosító.Text.Length <= 10)) { MessageBox.Show("Nem megfelelő az azonosító hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!(0 < box_megnevezés.Text.Length && box_megnevezés.Text.Length <= 30)) { MessageBox.Show("Nem megfelelő a megnevezés hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(box_megnevezés.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int összes; try { összes = Convert.ToInt32(box_összes.Text); } catch { MessageBox.Show("Nem szám található a lövéseknél!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (összes < 1) { MessageBox.Show("Túl kevés a lövések száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int állomások; try { állomások = Convert.ToInt32(box_állomások.Text); } catch { MessageBox.Show("Nem szám található az állomásoknál!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if ((állomások < 1) || (30 < állomások)) { MessageBox.Show("Nem megfelelő az állomások száma (1-30)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eredeti_azonosító != null)
                {
                    if ((0 < Convert.ToInt32(label_indulók.Text)) && (eredeti_azonosító != box_azonosító.Text))
                    { MessageBox.Show("Ez a verseny nem átnevezhető!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

                    // TODO ezt sem kéne külön csinálni!
                    if (combo_versenysorozat.Text != eredeti_versenysorozat)
                    {
                        Program.mainform.versenysorozat_panel.Versenysorozat_VersenyCsökkentés(eredeti_versenysorozat);
                        Program.mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés(combo_versenysorozat.Text);
                    }

                    Program.mainform.verseny_panel.Verseny_Módosítás(eredeti_azonosító, new Verseny(box_azonosító.Text, box_megnevezés.Text, dátumválasztó.Value.ToShortDateString(),
                        combo_versenysorozat.Text, összes, állomások, Convert.ToInt32(label_indulók.Text), label_lezárva.Text == "Igen" ? true : false));
                }
                else
                {
                    // TODO ezt sem kéne külön csinálni!
                    Program.mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés(combo_versenysorozat.Text);
                    Program.mainform.verseny_panel.Verseny_Hozzáadás(new Verseny(box_azonosító.Text, box_megnevezés.Text, dátumválasztó.Value.ToShortDateString(), combo_versenysorozat.Text, összes, állomások, 0, false));
                }

                Close();
            }
            #endregion
        }
    }
}
