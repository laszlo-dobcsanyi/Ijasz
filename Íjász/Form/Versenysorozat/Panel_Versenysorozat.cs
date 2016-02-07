using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Íjász {
    public struct Versenysorozat {
        public string azonosító;
        public string megnevezés;
        public int versenyek;

        public Versenysorozat(string _azonosító, string _megnevezés, int _versenyek) {
            azonosító = _azonosító;
            megnevezés = _megnevezés;
            versenyek = _versenyek;
        }

        public override string ToString( ) {
            return "Versenysorozat" + Environment.NewLine + 
                    "{" + Environment.NewLine +
                    "   azonosító='" + azonosító + '\'' + Environment.NewLine +
                    "   megnevezés='" + megnevezés + '\'' + Environment.NewLine +
                    "   versenyek=" + versenyek + Environment.NewLine +
                    '}';
        }
    };

    public delegate void Versenysorozat_Hozzáadva(Versenysorozat _versenysorozat);
    public delegate void Versenysorozat_Módosítva(string _azonosító, Versenysorozat _versenysorozat);
    public delegate void Versenysorozat_Törölve(string _azonosító);

    public sealed class Panel_Versenysorozat : Control {
        public Versenysorozat_Hozzáadva versenysorozat_hozzáadva;
        public Versenysorozat_Módosítva versenysorozat_módosítva;
        public Versenysorozat_Törölve versenysorozat_törölve;

        private DataTable data;
        private DataGridView table;

        public Panel_Versenysorozat() {
            InitializeContent();
        }

        private void InitializeContent() {

            table = new DataGridView() {
                DataSource = CreateSource(),
                Dock = DockStyle.Left,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                Width = 403,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
            };
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += Modositas_Click;
            Controls.Add(table);

            Button btnTorles = new Button {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "Törlés",
                Location = new Point(Width - 96 - 96 - 32, ClientRectangle.Height - 32 - 16),
                Size = new Size(96, 32)
            };
            btnTorles.Click += btnTorles_Click;
            this.Controls.Add(btnTorles);

            Button btnHozzaadas = new Button {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "Hozzáadás",
                Location = new Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16),
                Size = new Size(96, 32),
            };
            btnHozzaadas.Click += btnHozzaadas_Click;
            this.Controls.Add(btnHozzaadas);
        }

        private DataTable CreateSource() {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Megnevezés", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Versenyek száma", System.Type.GetType("System.Int32")));

            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Versenysorozat current in versenysorozatok) {
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
        public void VersenySorozatHozzaadas(Versenysorozat _versenysorozat) {
            if (InvokeRequired) {
                Versenysorozat_Hozzáadás_Callback callback = new Versenysorozat_Hozzáadás_Callback(VersenySorozatHozzaadas);
                Invoke(callback, new object[] { _versenysorozat });
            }
            else {
                if (!Program.database.ÚjVersenysorozat(_versenysorozat)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                DataRow row = data.NewRow();
                row[0] = _versenysorozat.azonosító;
                row[1] = _versenysorozat.megnevezés;
                row[2] = _versenysorozat.versenyek;

                data.Rows.Add(row);

                if (versenysorozat_hozzáadva != null)
                    versenysorozat_hozzáadva(_versenysorozat);
            }
        }

        private delegate void Versenysorozat_Módosítás_Callback(string _azonosító, Versenysorozat _versenysorozat);
        public void VersenySorozatModositas(string _azonosító, Versenysorozat _versenysorozat) {
            if (InvokeRequired) {
                Versenysorozat_Módosítás_Callback callback = new Versenysorozat_Módosítás_Callback(VersenySorozatModositas);
                Invoke(callback, new object[] { _azonosító, _versenysorozat });
            }
            else {
                if (!Program.database.VersenysorozatMódosítás(_azonosító, _versenysorozat)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows) {
                    if (_azonosító == current[0].ToString()) {
                        current[0] = _versenysorozat.azonosító;
                        current[1] = _versenysorozat.megnevezés;
                        current[2] = _versenysorozat.versenyek;
                        break;
                    }
                }

                if (versenysorozat_módosítva != null)
                    versenysorozat_módosítva(_azonosító, _versenysorozat);
            }
        }

        private delegate void Versenysorozat_Törlés_Callback(string _azonosító);
        public void VersenySorozatTorles(string _azonosító) {
            if (InvokeRequired) {
                Versenysorozat_Törlés_Callback callback = new Versenysorozat_Törlés_Callback(VersenySorozatTorles);
                Invoke(callback, new object[] { _azonosító });
            }
            else {
                if (!Program.database.VersenysorozatTörlés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows) {
                    if (_azonosító == current[0].ToString()) {
                        data.Rows.Remove(current);
                        break;
                    }
                }

                if (versenysorozat_törölve != null)
                    versenysorozat_törölve(_azonosító);
            }
        }

        ///

        //Nincs invokeolva, csak belső hívásra!!
        public void Versenysorozat_VersenyNövelés(string _azonosító) {
            if (!Program.database.Versenysorozat_VersenyekNövel(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows) {
                if (_azonosító == current[0].ToString()) {
                    current[2] = ((int)current[2]) + 1;
                    break;
                }
            }
        }

        //Nincs invokeolva, csak belső hívásra!!
        public void Versenysorozat_VersenyCsökkentés(string _azonosító) {
            if (!Program.database.Versenysorozat_VersenyekCsökkent(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows) {
                if (_azonosító == current[0].ToString()) {
                    current[2] = ((int)current[2]) - 1;
                    break;
                }
            }
        }
        #endregion

        #region EventHandlers
        private void table_DataBindingComplete(object _sender, EventArgs _event) {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 80;
            table.Columns[1].Width = 200;
            table.Columns[2].Width = 120;

            foreach (DataGridViewColumn column in table.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void btnHozzaadas_Click(object _sender, EventArgs _event) {
            Form_Versenysorozat versenysorozat = new Form_Versenysorozat();
            versenysorozat.ShowDialog();
        }

        private void Modositas_Click(object _sender, EventArgs _event) {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count))
                return;

            Form_Versenysorozat versenysorozat = new Form_Versenysorozat(new Versenysorozat(data.Rows[table.SelectedRows[0].Index][0].ToString(),
                   data.Rows[table.SelectedRows[0].Index][1].ToString(), Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][2])));
            versenysorozat.ShowDialog();
        }

        private void btnTorles_Click(object _sender, EventArgs _event) {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count))
                return;
            if ((int)data.Rows[table.SelectedRows[0].Index][2] != 0) { MessageBox.Show("Ez a versenysorozat nem törölhető, mivel van hozzárendelve verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (MessageBox.Show("Biztosan törli ezt a versenysorozatot?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            VersenySorozatTorles(data.Rows[table.SelectedRows[0].Index][0].ToString());
        }
        #endregion

    }
}
