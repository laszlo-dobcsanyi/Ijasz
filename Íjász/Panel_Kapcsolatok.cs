using System;
using System.Data;
using System.Windows.Forms;

namespace Íjász
{
    public sealed class Panel_Kapcsolatok : Control
    {
        private delegate void KapcsolatHozzáadás_Callback(string _adat);
        private delegate void NévHozzáadás_Callback(string _adat, string _név);
        private delegate void KapcsolatTörlés_Callback(string _adat);

        private DataTable data;
        private DataGridView kapcsolatok;

        public Panel_Kapcsolatok()
        {
            InitializeContent();
        }

        private void InitializeContent()
        {
            kapcsolatok = new DataGridView();
            kapcsolatok.DataSource = CreateSource();
            kapcsolatok.Dock = DockStyle.Fill;
            kapcsolatok.RowHeadersVisible = false;
            kapcsolatok.AllowUserToResizeRows = false;
            kapcsolatok.AllowUserToResizeColumns = false;
            kapcsolatok.AllowUserToAddRows = false;
            kapcsolatok.Width = 203;
            kapcsolatok.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            kapcsolatok.MultiSelect = false;
            kapcsolatok.ReadOnly = true;
            kapcsolatok.DataBindingComplete += kapcsolatok_DataBindingComplete;

            Controls.Add(kapcsolatok);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Név", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("IP-cím", System.Type.GetType("System.String")));

            return data;
        }

        #region Accessors
        public void KapcsolatHozzáadás(string _adat)
        {
            if (InvokeRequired)
            {
                KapcsolatHozzáadás_Callback callback = new KapcsolatHozzáadás_Callback(KapcsolatHozzáadás);
                Invoke(callback, new object[] { _adat });
            }
            else
            {
                DataRow row = data.NewRow();
                row[0] = "";
                row[1] = _adat;
                data.Rows.Add(row);
            }
        }

        public void NévHozzáadás(string _adat, string _név)
        {
            if (InvokeRequired)
            {
                NévHozzáadás_Callback callback = new NévHozzáadás_Callback(NévHozzáadás);
                Invoke(callback, new object[] { _adat, _név });
            }
            else
            {
                for (int current = 0; current < data.Rows.Count; ++current)
                {
                    if (data.Rows[current].ItemArray[1].Equals(_adat))
                    {
                        data.Rows[current].SetField(0, _név);
                        return;
                    }
                }
            }
        }

        public void KapcsolatTörlés(string _adat)
        {
            if (InvokeRequired)
            {
                KapcsolatTörlés_Callback callback = new KapcsolatTörlés_Callback(KapcsolatTörlés);
                Invoke(callback, new object[] { _adat });
            }
            else
            {
                for (int current = 0; current < data.Rows.Count; ++current)
                {
                    if (data.Rows[current].ItemArray[1].Equals(_adat))
                    {
                        data.Rows.RemoveAt(current);
                        return;
                    }
                }
            }
        }
        #endregion

        #region EventHandlers
        void kapcsolatok_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            kapcsolatok.DataBindingComplete -= kapcsolatok_DataBindingComplete;

            kapcsolatok.Columns[1].Width = 200;

            foreach (DataGridViewColumn column in kapcsolatok.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        #endregion
    }
}
