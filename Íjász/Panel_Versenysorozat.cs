using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Íjász
{
    public struct Versenysorozat
    {
        public string azonosító;
        public string megnevezés;
        public int versenyek;

        public Versenysorozat(string _azonosító, string _megnevezés, int _versenyek)
        {
            azonosító = _azonosító;
            megnevezés = _megnevezés;
            versenyek = _versenyek;
        }
    };

    public delegate void Versenysorozat_Hozzáadva(Versenysorozat _versenysorozat);
    public delegate void Versenysorozat_Módosítva(string _azonosító, Versenysorozat _versenysorozat);
    public delegate void Versenysorozat_Törölve(string _azonosító);

    public sealed class Panel_Versenysorozat : Control
    {
        public Versenysorozat_Hozzáadva versenysorozat_hozzáadva;
        public Versenysorozat_Módosítva versenysorozat_módosítva;
        public Versenysorozat_Törölve   versenysorozat_törölve;

        private DataTable data;
        private DataGridView table;

        private Button törlés;

        public Panel_Versenysorozat()
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
            table.Width = 403;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += módosítás_Click;

            törlés = new Button();
            törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            törlés.Text = "Törlés";
            törlés.Size = new System.Drawing.Size(96, 32);
            törlés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16);
            törlés.Click += törlés_Click;

            Button hozzáadás = new Button();
            hozzáadás.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            hozzáadás.Text = "Hozzáadás";
            hozzáadás.Size = new System.Drawing.Size(96, 32);
            hozzáadás.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
            hozzáadás.Click += hozzáadás_Click;

            Controls.Add(table);

            Controls.Add(törlés);
            Controls.Add(hozzáadás);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();
            
            data.Columns.Add(new DataColumn("Azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Megnevezés", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Versenyek száma", System.Type.GetType("System.Int32")));

            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Versenysorozat current in versenysorozatok)
            {
                DataRow row = data.NewRow();
                row[0] = current.azonosító;
                row[1] = current.megnevezés;
                row[2] = current.versenyek;
               

                data.Rows.Add(row);
            }

            return data;
        }

        #region Accessors
        private delegate void Versenysorozat_Hozzáadás_Callback(Versenysorozat _versenysorozat);
        public void Versenysorozat_Hozzáadás(Versenysorozat _versenysorozat)
        {
            if (InvokeRequired)
            {
                Versenysorozat_Hozzáadás_Callback callback = new Versenysorozat_Hozzáadás_Callback(Versenysorozat_Hozzáadás);
                Invoke(callback, new object[] { _versenysorozat });
            }
            else
            {
                if (!Program.database.ÚjVersenysorozat(_versenysorozat)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                DataRow row = data.NewRow();
                row[0] = _versenysorozat.azonosító;
                row[1] = _versenysorozat.megnevezés;
                row[2] = _versenysorozat.versenyek;

                data.Rows.Add(row);

                if (versenysorozat_hozzáadva != null) versenysorozat_hozzáadva(_versenysorozat);
            }
        }

        private delegate void Versenysorozat_Módosítás_Callback(string _azonosító, Versenysorozat _versenysorozat);
        public void Versenysorozat_Módosítás(string _azonosító, Versenysorozat _versenysorozat)
        {
            if (InvokeRequired)
            {
                Versenysorozat_Módosítás_Callback callback = new Versenysorozat_Módosítás_Callback(Versenysorozat_Módosítás);
                Invoke(callback, new object[] { _azonosító, _versenysorozat });
            }
            else
            {
                if (!Program.database.VersenysorozatMódosítás(_azonosító, _versenysorozat)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[0] = _versenysorozat.azonosító;
                        current[1] = _versenysorozat.megnevezés;
                        current[2] = _versenysorozat.versenyek;
                        break;
                    }
                }

                if (versenysorozat_módosítva != null) versenysorozat_módosítva(_azonosító, _versenysorozat);
            }
        }

        private delegate void Versenysorozat_Törlés_Callback(string _azonosító);
        public void Versenysorozat_Törlés(string _azonosító)
        {
            if (InvokeRequired)
            {
                Versenysorozat_Törlés_Callback callback = new Versenysorozat_Törlés_Callback(Versenysorozat_Törlés);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.VersenysorozatTörlés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        data.Rows.Remove(current);
                        break;
                    }
                }

                if (versenysorozat_törölve != null) versenysorozat_törölve(_azonosító);
            }
        }

        ///

        //Nincs invokeolva, csak belső hívásra!!
        public void Versenysorozat_VersenyNövelés(string _azonosító)
        {
            if (!Program.database.Versenysorozat_VersenyekNövel(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[2] = ((int)current[2]) + 1;
                    break;
                }
            }
        }

        //Nincs invokeolva, csak belső hívásra!!
        public void Versenysorozat_VersenyCsökkentés(string _azonosító)
        {
            if (!Program.database.Versenysorozat_VersenyekCsökkent(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[2] = ((int)current[2]) - 1;
                    break;
                }
            }
        }
        #endregion

        #region EventHandlers
        private void table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 80;
            table.Columns[1].Width = 200;
            table.Columns[2].Width = 120;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void hozzáadás_Click(object _sender, EventArgs _event)
        {
            Form_Versenysorozat versenysorozat = new Form_Versenysorozat();
            versenysorozat.ShowDialog();
        }

        private void módosítás_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;

            Form_Versenysorozat versenysorozat = new Form_Versenysorozat(new Versenysorozat(data.Rows[table.SelectedRows[0].Index][0].ToString(),
                data.Rows[table.SelectedRows[0].Index][1].ToString(), Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][2])));
            versenysorozat.ShowDialog();
        }

        private void törlés_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;
            if ((int)data.Rows[table.SelectedRows[0].Index][2] != 0) { MessageBox.Show("Ez a versenysorozat nem törölhető, mivel van hozzárendelve verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (MessageBox.Show("Biztosan törli ezt a versenysorozatot?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Versenysorozat_Törlés(data.Rows[table.SelectedRows[0].Index][0].ToString());
        }
        #endregion

        public sealed class Form_Versenysorozat : Form
        {
            private string eredeti_azonosító = null;

            private TextBox box_azonosító;
            private TextBox box_megnevezés;
            private Label label_szám;

            public Form_Versenysorozat()
            {
                InitializeForm();
                InitializeContent();
                InitializeData();
            }

            public Form_Versenysorozat(Versenysorozat _versenysorozat)
            {
                eredeti_azonosító = _versenysorozat.azonosító;

                InitializeForm();
                InitializeContent();
                InitializeData(_versenysorozat);
            }

            private void InitializeForm()
            {
                Text = "Versenysorozat";
                ClientSize = new System.Drawing.Size(400 - 64, 128 + 32);
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

                Label szám = new Label();
                szám.Text = "Versenyek száma:";
                szám.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 2 * 32);
                //szám.Font = new System.Drawing.Font("Arial Black", 10);

                box_azonosító = new TextBox();
                box_azonosító.Location = new System.Drawing.Point(azonosító.Location.X + azonosító.Size.Width + 16, azonosító.Location.Y);
                box_azonosító.Size = new System.Drawing.Size(128 + 64, 24);
                box_azonosító.MaxLength = 10;

                box_megnevezés = new TextBox();
                box_megnevezés.Location = new System.Drawing.Point(megnevezés.Location.X + megnevezés.Size.Width + 16, megnevezés.Location.Y);
                box_megnevezés.Size = box_azonosító.Size;
                box_megnevezés.MaxLength = 30;

                label_szám = new Label();
                label_szám.Location = new System.Drawing.Point(szám.Location.X + szám.Size.Width + 16, szám.Location.Y);
                label_szám.Size = box_azonosító.Size;

                Button rendben = new Button();
                rendben.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                rendben.Text = "Rendben";
                rendben.Size = new System.Drawing.Size(96, 32);
                rendben.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
                rendben.Click += rendben_Click;

                ///

                Controls.Add(azonosító);
                Controls.Add(megnevezés);
                Controls.Add(szám);
                Controls.Add(box_azonosító);
                Controls.Add(box_megnevezés);
                Controls.Add(label_szám);

                Controls.Add(rendben);
            }

            private void InitializeData()
            {
                box_azonosító.Text = "";
                box_megnevezés.Text = "";
                label_szám.Text = "0";
            }

            private void InitializeData(Versenysorozat _versenysorozat)
            {
                box_azonosító.Text = _versenysorozat.azonosító;
                box_azonosító.Enabled = ((_versenysorozat.versenyek == 0) ? true : false);
                box_megnevezés.Text = _versenysorozat.megnevezés;
                label_szám.Text = _versenysorozat.versenyek.ToString();
            }

            private void rendben_Click(object _sender, EventArgs _event)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 ]");
                box_azonosító.Text = rgx.Replace(box_azonosító.Text, "");

                if (!(0 < box_azonosító.Text.Length && box_azonosító.Text.Length <= 10)) { MessageBox.Show("Nem megfelelő az azonosító hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(box_azonosító.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!(0 < box_megnevezés.Text.Length && box_megnevezés.Text.Length <= 30)) { MessageBox.Show("Nem megfelelő a megnevezés hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(box_megnevezés.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eredeti_azonosító != null)
                    Program.mainform.versenysorozat_panel.Versenysorozat_Módosítás(eredeti_azonosító, new Versenysorozat(box_azonosító.Text, box_megnevezés.Text, Convert.ToInt32(label_szám.Text)));
                else
                    Program.mainform.versenysorozat_panel.Versenysorozat_Hozzáadás(new Versenysorozat(box_azonosító.Text, box_megnevezés.Text, 0));

                Close();
            }
        }
    }
}
