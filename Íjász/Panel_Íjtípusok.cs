using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
    public struct Íjtípus
    {
        public string Azonosito;
        public string Megnevezes;
        public int Sorszam;
        public int Eredmenyek;

        public Íjtípus( string _Azonosito,
                       string _Megnevezes,
                       int _Sorszam,
                       int _Eredmenyek )
        {
            Azonosito = _Azonosito;
            Megnevezes = _Megnevezes;
            Sorszam = _Sorszam;
            Eredmenyek = _Eredmenyek;
        }
    }

    public delegate void Íjtípus_Hozzáadva(Íjtípus _íjtípus);
    public delegate void Íjtípus_Módosítva(string _azonosító, Íjtípus _íjtípus);
    public delegate void Íjtípus_Törölve(string _azonosító);

    public sealed class Panel_Íjtípusok : Control
    {
        public Íjtípus_Hozzáadva íjtípus_hozzáadva;
        public Íjtípus_Módosítva íjtípus_módosítva;
        public Íjtípus_Törölve íjtípus_törölve;

        private DataTable data;
        private DataGridView table;

        public Panel_Íjtípusok()
        {
            InitializeContent();
        }

        private void InitializeContent()
        {

            table = new DataGridView();
            table.Dock = DockStyle.Left;
            table.RowHeadersVisible = false;
            table.AllowUserToResizeRows = false;
            table.AllowUserToResizeColumns = false;
            table.AllowUserToAddRows = false;
            table.Width = 303;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataSource = CreateSource();
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
            data.Columns.Add(new DataColumn("Listázási sorszám", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Eredmények", System.Type.GetType("System.Int32")));

            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();

            foreach (Íjtípus current in íjtípusok)
            {
                DataRow row = data.NewRow();
                row[0] = current.Azonosito;
                row[1] = current.Megnevezes;
                row[2] = current.Sorszam;
                row[3] = current.Eredmenyek;

                data.Rows.Add(row);
            }           
            return data;
        }

        #region Accessors
        private delegate void Íjtípus_Hozzáadás_Callback(Íjtípus _íjtípus);
        public void Íjtípus_Hozzáadás(Íjtípus _íjtípus)
        {
            if (InvokeRequired)
            {
                Íjtípus_Hozzáadás_Callback callback = new Íjtípus_Hozzáadás_Callback(Íjtípus_Hozzáadás);
                Invoke(callback, new object[] { _íjtípus });
            }
            else
            {
                if (!Program.database.ÚjÍjtípus(_íjtípus)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                DataRow row = data.NewRow();
                row[0] = _íjtípus.Azonosito;
                row[1] = _íjtípus.Megnevezes;
                row[2] = _íjtípus.Sorszam;
                row[3] = _íjtípus.Eredmenyek;

                data.Rows.Add(row);

                if (íjtípus_hozzáadva != null) íjtípus_hozzáadva(_íjtípus);
            }
        }

        private delegate void Íjtípus_Módosítás_Callback(string _azonosító ,Íjtípus _íjtípus);
        public void Íjtípus_Módosítás(string _azonosító, Íjtípus _íjtípus)
        {
            if (InvokeRequired)
            {
                Íjtípus_Módosítás_Callback callback = new Íjtípus_Módosítás_Callback(Íjtípus_Módosítás);
                Invoke(callback, new object[] { _azonosító, _íjtípus });
            }
            else
            {
                if (!Program.database.ÍjtípusMódosítás(_azonosító, _íjtípus)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[0] = _íjtípus.Azonosito;
                        current[1] = _íjtípus.Megnevezes;
                        current[2] = _íjtípus.Sorszam;
                        break;
                    }
                }

                if (íjtípus_módosítva != null) íjtípus_módosítva(_azonosító, _íjtípus);
            }
        }

        private delegate void Íjtípus_Törlés_Callback(string _azonosító);
        public void Íjtípus_Törlés(string _azonosító)
        {
            if (InvokeRequired)
            {
                Íjtípus_Törlés_Callback callback = new Íjtípus_Törlés_Callback(Íjtípus_Törlés);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.ÍjtípusTörlés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        data.Rows.Remove(current);
                        break;
                    }

                }

                if (íjtípus_törölve != null) íjtípus_törölve(_azonosító);
            }
        }

        //Nincs invokeolva, csak belső hívásra!!
        public void Íjtípus_EredményekNövelés(string _azonosító)
        {
            if (!Program.database.Íjtípus_EredményekNövelés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[3] = ((int)current[3]) + 1;
                    break;
                }
            }
        }

        //Nincs invokeolva, csak belső hívásra!!
        public void Íjtípus_EredményekCsökkentés(string _azonosító)
        {
            if (!Program.database.Íjtípus_EredményekCsökkentés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[3] = ((int)current[3]) - 1;
                    break;
                }
            }
        }
        #endregion

        #region EventHandlers
        public void eredmények_beírás(string _azonosító, Database.BeírásEredmény _beírás)
        {
            if (_beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
            {
                Íjtípus_EredményekNövelés(_beírás.eredmény.Value.Ijtipus);
            }

            if (_beírás.flag == Database.BeírásEredmény.Flag.MÓDOSÍTOTT)
            {
                Íjtípus_EredményekNövelés(_beírás.eredmény.Value.Ijtipus);
                Íjtípus_EredményekCsökkentés(_beírás.eredeti.Value.Ijtipus);
            }
        }


        public void eredmények_törlés(string _azonosító, Eredmény _eredmény)
        {
            Íjtípus_EredményekCsökkentés(_eredmény.Ijtipus);
        }

        ///

        void table_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
            table.Columns[3].Visible = false;
        }

        private void hozzáadás_Click(object _sender, EventArgs _event)
        {
            Form_Íjtípus íjtípus = new Form_Íjtípus();
            íjtípus.ShowDialog();
        }

        private void módosítás_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;

            Form_Íjtípus íjtípus = new Form_Íjtípus(new Íjtípus(data.Rows[table.SelectedRows[0].Index][0].ToString(),
                data.Rows[table.SelectedRows[0].Index][1].ToString(), Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][2]), Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][3])));
            íjtípus.ShowDialog();
        }

        private void törlés_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;
            if ((int)data.Rows[table.SelectedRows[0].Index][3] != 0) { MessageBox.Show("Ez az íjtípus nem törölhető, mivel szerepel versenyen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (MessageBox.Show("Biztosan törli ezt az íjtípust?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Íjtípus_Törlés(data.Rows[table.SelectedRows[0].Index][0].ToString());
        }
        #endregion

        public sealed class Form_Íjtípus : Form
        {
            private string eredeti_azonosító = null;

            private TextBox text_íjazon;
            private TextBox text_megn;
            private TextBox text_listsorszam;
            private Label label_eredmények;

            public Form_Íjtípus()
            {
                InitializeForm();
                InitializeContent();
                InitializeData();
            }

            public Form_Íjtípus(Íjtípus _íjtípus)
            {
                InitializeForm();
                InitializeContent();
                InitializeData(_íjtípus);
            }

            private void InitializeForm()
            {
                Text = "Íjtípus";
                ClientSize = new System.Drawing.Size(400 - 64, 128 + 64);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent()
            {
                Label label_íjazon = new Label();
                label_íjazon.Text = "Azonosító:";
                label_íjazon.AutoSize = true;
                label_íjazon.Location = new System.Drawing.Point(16, 16 + 0 * 32);

                Label label_megnevezés = new Label();
                label_megnevezés.Text = "Megnevezés:";
                label_megnevezés.AutoSize = true;
                label_megnevezés.Location = new System.Drawing.Point(label_íjazon.Location.X, 16 + 1 * 32);

                Label label_listsorszam = new Label();
                label_listsorszam.Text = "Listázási sorszám:";
                label_listsorszam.Location = new System.Drawing.Point(label_íjazon.Location.X, 16 + 2 * 32);

                Label eredmények = new Label();
                eredmények.Text = "Eredmények:";
                eredmények.Location = new System.Drawing.Point(label_íjazon.Location.X, 16 + 3 * 32);

                ///

                text_íjazon = new TextBox();
                text_íjazon.Location = new System.Drawing.Point(label_íjazon.Location.X + label_íjazon.Size.Width + 16, 16 + 0 * 32);
                text_íjazon.MaxLength = 10;

                text_megn = new TextBox();
                text_megn.Location = new System.Drawing.Point(text_íjazon.Location.X, 16 + 1 * 32);
                text_megn.MaxLength = 30;

                text_listsorszam = new TextBox();
                text_listsorszam.Location = new System.Drawing.Point(text_íjazon.Location.X, 16 + 2 * 32);

                label_eredmények = new Label();
                label_eredmények.Location = new System.Drawing.Point(eredmények.Location.X + eredmények.Size.Width + 16, eredmények.Location.Y);

                Button rendben = new Button();
                rendben.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                rendben.Text = "Rendben";
                rendben.Size = new System.Drawing.Size(96, 32);
                rendben.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
                rendben.Click += rendben_Click;

                ///

                Controls.Add(label_íjazon);
                Controls.Add(label_megnevezés);
                Controls.Add(label_listsorszam);
                //Controls.Add(eredmények);

                Controls.Add(text_íjazon);
                Controls.Add(text_megn);
                Controls.Add(text_listsorszam);
                //Controls.Add(label_eredmények);
                Controls.Add(rendben);
            }

            private void InitializeData()
            {
                text_íjazon.Text = "";
                text_íjazon.Enabled = true;
                text_listsorszam.Text = "";
                text_megn.Text = "";
                label_eredmények.Text = "0";
            }

            private void InitializeData(Íjtípus _íjtípus)
            {
                eredeti_azonosító = _íjtípus.Azonosito;

                text_íjazon.Text = _íjtípus.Azonosito;
                text_íjazon.Enabled = (_íjtípus.Eredmenyek > 0 ? false : true);
                text_listsorszam.Text = _íjtípus.Sorszam.ToString();
                text_megn.Text = _íjtípus.Megnevezes;
                label_eredmények.Text = _íjtípus.Eredmenyek.ToString();
            }

            #region EventHandlers
            private void rendben_Click(object _sender, EventArgs _event)
            {
                if (text_íjazon.Text.Length == 0 || text_íjazon.Text.Length > 10) { MessageBox.Show("Íjtípusazonosító hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(text_íjazon.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (text_megn.Text.Length == 0 || text_megn.Text.Length > 30) { MessageBox.Show("Íjtípus megnevezésének hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(text_megn.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                try { Convert.ToInt32(text_listsorszam.Text); }
                catch { MessageBox.Show("Nem megfelelő a sorszám formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eredeti_azonosító != null)
                    Program.mainform.íjtípusok_panel.Íjtípus_Módosítás(eredeti_azonosító, new Íjtípus(text_íjazon.Text, text_megn.Text, Convert.ToInt32(text_listsorszam.Text), -666));
                else
                    Program.mainform.íjtípusok_panel.Íjtípus_Hozzáadás(new Íjtípus(text_íjazon.Text, text_megn.Text, Convert.ToInt32(text_listsorszam.Text), 0));

                Close();
            }
            #endregion
        }
    }
}