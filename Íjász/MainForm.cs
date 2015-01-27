using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Íjász
{
    public sealed class MainForm : Form
    {
        public Panel_Versenysorozat versenysorozat_panel;
        public Panel_Verseny verseny_panel;
        public Panel_Korosztályok korosztályok_panel;
        public Panel_Íjtípusok íjtípusok_panel;
        public Panel_Indulók indulók_panel;
        public Panel_Eredmények eredmények_panel;
        public Panel_Kapcsolatok kapcsolatok_panel;
        public Panel_Startlista startlista_panel;
        public Panel_Eredménylap eredménylap_panel;


        public TabControl menu;
        private StatusStrip status;

        public MainForm()
        {
            InitializeForm();
            InitializeContent();
            InitializeEvents();
        }

        private void InitializeForm()
        {
            Text = "Íjász szerver címe: " + Program.network.GetEndPoint().ToString();
            ClientSize = new System.Drawing.Size(1024, 768);
            MinimumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeContent()
        {
            #region Status
            ToolStripStatusLabel Tulajdonos = new ToolStripStatusLabel(Program.Tulajdonos_Megnevezés);
            ToolStripStatusLabel Készítők = new ToolStripStatusLabel("Belinyák Nándor és Társai. \u00A9 2014");
            ToolStripStatusLabel Verzió = new ToolStripStatusLabel("Verzió: 0.9.3-2014.10.18");
            Készítők.BorderSides = ToolStripStatusLabelBorderSides.Left;
            Verzió.BorderSides = ToolStripStatusLabelBorderSides.Left;

            status = new StatusStrip();
            status.Items.Add(Tulajdonos);
            status.Items.Add(Készítők);
            status.Items.Add(Verzió);
            #endregion

            #region Menü
            TabPage Versenysorozat = new TabPage("Versenysorozatok");
            versenysorozat_panel = new Panel_Versenysorozat();
            versenysorozat_panel.Dock = DockStyle.Fill;
            Versenysorozat.Controls.Add(versenysorozat_panel);

            TabPage Verseny = new TabPage("Versenyek");
            verseny_panel = new Panel_Verseny();
            verseny_panel.Dock = DockStyle.Fill;
            Verseny.Controls.Add(verseny_panel);

            TabPage Korosztályok = new TabPage("Korosztályok");
            korosztályok_panel = new Panel_Korosztályok();
            korosztályok_panel.Dock = DockStyle.Fill;
            Korosztályok.Controls.Add(korosztályok_panel);

            TabPage Íjtípusok = new TabPage("Íjtípusok");
            íjtípusok_panel = new Panel_Íjtípusok();
            íjtípusok_panel.Dock = DockStyle.Fill;
            Íjtípusok.Controls.Add(íjtípusok_panel);

            TabPage Indulók = new TabPage("Indulók");
            indulók_panel = new Panel_Indulók();
            indulók_panel.Dock = DockStyle.Fill;
            Indulók.Controls.Add(indulók_panel);

            TabPage Eredmények = new TabPage("Eredmények");
            eredmények_panel = new Panel_Eredmények();
            eredmények_panel.Dock = DockStyle.Fill;
            Eredmények.Controls.Add(eredmények_panel);

            TabPage Kapcsolatok = new TabPage("Kapcsolatok");
            kapcsolatok_panel = new Panel_Kapcsolatok();
            kapcsolatok_panel.Dock = DockStyle.Fill;
            Kapcsolatok.Controls.Add(kapcsolatok_panel);

            TabPage Startlista = new TabPage("Startlisták");
            startlista_panel = new Panel_Startlista();
            startlista_panel.Dock = DockStyle.Fill;
            Startlista.Controls.Add(startlista_panel);

            TabPage Eredménylap = new TabPage("Eredménylapok");
            eredménylap_panel = new Panel_Eredménylap();
            eredménylap_panel.Dock = DockStyle.Fill;
            Eredménylap.Controls.Add(eredménylap_panel);


            //

            menu = new TabControl();
            menu.TabPages.Add(Versenysorozat);
            menu.TabPages.Add(Verseny);
            menu.TabPages.Add(Korosztályok);
            menu.TabPages.Add(Íjtípusok);
            menu.TabPages.Add(Indulók);
            menu.TabPages.Add(Eredmények);
            menu.TabPages.Add(Startlista);
            menu.TabPages.Add(Eredménylap);
            menu.TabPages.Add(Kapcsolatok);

            menu.DrawItem += menu_DrawItem;
            menu.DrawMode = TabDrawMode.OwnerDrawFixed;
            menu.Padding = new Point(18, 5);

            menu.Dock = DockStyle.Fill;
            #endregion

            Button BackupButton = new Button();
            BackupButton.Text = "Biztonsági mentés";
            BackupButton.Location = new Point(ClientSize.Width - 128, 0);
            BackupButton.Size = new System.Drawing.Size(128, 24);
            BackupButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            BackupButton.Click += BackupButton_Click;

            Controls.Add(status);
            Controls.Add(menu);
            menu.BringToFront();
            Controls.Add(BackupButton);
            BackupButton.BringToFront();
        }

        void menu_DrawItem(object sender, DrawItemEventArgs e)
        {

            TabPage currentab = menu.TabPages[e.Index];
            SolidBrush textbrush = new SolidBrush(Color.Black);
            Rectangle itemrect = menu.GetTabRect(e.Index);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            if (Convert.ToBoolean(e.State & DrawItemState.Selected))
            {
                Font f = new Font(menu.Font.Name, menu.Font.Size + 1, FontStyle.Bold);
                e.Graphics.DrawString(currentab.Text, f, textbrush, itemrect, sf);
            }
            else e.Graphics.DrawString(currentab.Text, e.Font, textbrush, itemrect, sf);
            textbrush.Dispose();

        }

        private void BackupButton_Click(object _sender, EventArgs _event)
        {
            //Program.database.CreateBackup("adat_biztonsági_" + DateTime.Now.ToString().Trim(new Char[] { '-' }).Replace(' ', '_').Replace('.', '-').Replace(':', '-'));
            Backup_Form mentés = new Backup_Form();
            mentés.ShowDialog();
        }
        private class Backup_Form : Form
        {
            private TextBox név_box;
            public Backup_Form()
            {
                InitializeForm();
                InitializeContent();
            }

            private void InitializeForm()
            {
                Text = "Biztonsági mentés";
                ClientSize = new System.Drawing.Size(256 + 64, 96);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
            }

            private void InitializeContent()
            {
                Label név = new Label();
                név.Text = "Mentés neve:";
                név.Location = new Point(16, 16);
                név.Size = new Size(48, 16);

                név_box = new TextBox();
                név_box.Location = new Point(név.Location.X + név.Size.Width + 16, név.Location.Y);
                név_box.Size = new Size(128, 24);

                Button mentés = new Button();
                mentés.Text = "Mentés";
                mentés.Location = new Point(név_box.Location.X + név_box.Size.Width + 16, név_box.Location.Y + név_box.Size.Height + 16);
                mentés.Size = new Size(64, 32);
                mentés.Click += mentés_Click;

                //

                Controls.Add(név);
                Controls.Add(név_box);
                Controls.Add(mentés);
            }

            #region EventHandlers
            private void mentés_Click(object _sender, EventArgs _event)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9_ ]");
                név_box.Text = rgx.Replace(név_box.Text, "");

                if (név_box.Text.Length == 0 || 30 <= név_box.Text.Length) { MessageBox.Show("A mentés nevének hossza nem megfelelő!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                Program.database.CreateBackup(név_box.Text);

                Close();
            }
            #endregion
        }

        private void InitializeEvents()
        {
            versenysorozat_panel.versenysorozat_hozzáadva += verseny_panel.verseny_form.versenysorozat_hozzáadás;
            versenysorozat_panel.versenysorozat_módosítva += verseny_panel.verseny_form.versenysorozat_módosítás;
            versenysorozat_panel.versenysorozat_törölve += verseny_panel.verseny_form.versenysorozat_törlés;

            versenysorozat_panel.versenysorozat_hozzáadva += eredménylap_panel.VersenysorozatHozzáadás;
            versenysorozat_panel.versenysorozat_módosítva += eredménylap_panel.VersenysorozatModositas;
            versenysorozat_panel.versenysorozat_törölve += eredménylap_panel.VersenysorozatTorles;

            //

            verseny_panel.verseny_hozzáadva += eredmények_panel.verseny_hozzáadás;
            verseny_panel.verseny_módosítva += eredmények_panel.verseny_módosítás;
            verseny_panel.verseny_törölve += eredmények_panel.verseny_törlés;
            verseny_panel.verseny_lezárva += eredmények_panel.verseny_lezárás;
            verseny_panel.verseny_megnyitva += eredmények_panel.verseny_megnyitás;

            verseny_panel.verseny_hozzáadva += korosztályok_panel.verseny_hozzáadás;
            verseny_panel.verseny_módosítva += korosztályok_panel.verseny_módosítás;
            verseny_panel.verseny_törölve += korosztályok_panel.verseny_törlés;

            verseny_panel.verseny_hozzáadva += eredménylap_panel.VersenyHozzaadas;
            verseny_panel.verseny_törölve += eredménylap_panel.VersenyTorles;
            verseny_panel.verseny_módosítva += eredménylap_panel.VersenyModositas;

            verseny_panel.verseny_hozzáadva += startlista_panel.VersenyHozzaadas;
            verseny_panel.verseny_törölve += startlista_panel.VersenyTorles;
            verseny_panel.verseny_módosítva += startlista_panel.VersenyModositas;

            verseny_panel.verseny_hozzáadva += Program.network.verseny_hozzáadás;
            verseny_panel.verseny_módosítva += Program.network.verseny_módosítás;
            verseny_panel.verseny_törölve += Program.network.verseny_törlés;
            verseny_panel.verseny_lezárva += Program.network.verseny_lezárás;
            verseny_panel.verseny_megnyitva += Program.network.verseny_megnyitás;

            //

            íjtípusok_panel.íjtípus_hozzáadva += Program.network.íjtípus_hozzáadás;
            íjtípusok_panel.íjtípus_módosítva += Program.network.íjtípus_módosítás;
            íjtípusok_panel.íjtípus_törölve += Program.network.íjtípus_törlés;

            //

            indulók_panel.induló_átnevezve += eredmények_panel.induló_átnevezés;

            indulók_panel.induló_hozzáadva += Program.network.induló_hozzáadás;
            indulók_panel.induló_módosítva += Program.network.induló_módosítás;
            indulók_panel.induló_átnevezve += Program.network.induló_átnevezés;
            indulók_panel.induló_törölve += Program.network.induló_törlés;

            //

            eredmények_panel.eredmény_beírva += verseny_panel.eredmény_beírás;
            eredmények_panel.eredmény_törölve += verseny_panel.eredmény_törlés;

            eredmények_panel.eredmény_beírva += verseny_panel.verseny_form.eredmény_beírás;
            eredmények_panel.eredmény_törölve += verseny_panel.verseny_form.eredmény_törlés;

            eredmények_panel.eredmény_beírva += indulók_panel.eredmények_beírás;
            eredmények_panel.eredmény_törölve += indulók_panel.eredmények_törlés;

            eredmények_panel.eredmény_beírva += íjtípusok_panel.eredmények_beírás;
            eredmények_panel.eredmény_törölve += íjtípusok_panel.eredmények_törlés;

            eredmények_panel.eredmény_beírva += Program.network.eredmény_beírás;
            eredmények_panel.eredmény_módosítva += Program.network.eredmény_módosítás;
            eredmények_panel.eredmény_törölve += Program.network.eredmény_törlés;
        }
    }
}
