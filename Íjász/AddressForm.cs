using System;
using System.Net;
using System.Windows.Forms;

namespace Íjász
{
    public sealed class AddressForm : Form
    {
        public bool selected = false;
        public int selected_port;
        public IPAddress selected_address = IPAddress.None;

        private ListBox address_list;

        public AddressForm()
        {
            InitializeForm();
            InitializeContent();
        }

        private void InitializeForm()
        {
            Text = "Szerver IP-Cím választás";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new System.Drawing.Size(256 + 128, 128);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximumSize = Size;
        }

        private void InitializeContent()
        {
            Button button_specific = new Button();
            button_specific.Text = "Saját cím";
            button_specific.Size = new System.Drawing.Size(96, 24);
            button_specific.Location = new System.Drawing.Point(ClientSize.Width - 96 - 16, ClientSize.Height - 3 * 40);
            button_specific.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_specific.Click += button_specific_Click;

            Button button_nvm = new Button();
            button_nvm.Text = "Hálózat nélkül";
            button_nvm.Size = new System.Drawing.Size(96, 24);
            button_nvm.Location = new System.Drawing.Point(ClientSize.Width - 96 - 16, ClientSize.Height - 2 * 40);
            button_nvm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_nvm.Click += button_nvm_Click;
            
            address_list = new ListBox();
            address_list.Size = new System.Drawing.Size(256, 0);
            address_list.Dock = DockStyle.Left;
            address_list.Items.AddRange(Program.network.GetAddresses().ToArray());
            if (address_list.Items.Count <= 0) { MessageBox.Show("Nincs egyetlen megfelelő hálózati cím sem!", "Végzetes hiba!", MessageBoxButtons.OK, MessageBoxIcon.Stop); Application.Exit(); }
            address_list.SelectedIndexChanged += address_list_SelectedIndexChanged;
            address_list.DoubleClick += button_accept_Click;

            Controls.Add(button_specific);
            Controls.Add(button_nvm);
            Controls.Add(address_list);
        }

        #region EventHandlers
        private void address_list_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            selected_port = 0;
            selected_address = IPAddress.Parse(address_list.SelectedItem.ToString());
            selected = true;
        }


        private class SpecificAddressForm : Form
        {
            public bool accepted = false;
            public int port;
            public IPAddress address;

            private TextBox port_box;
            private TextBox address_box;

            public SpecificAddressForm()
            {
                InitializeForm();
                InitializeContent();
            }

            private void InitializeForm()
            {
                Text = "Saját hálózati cím megadása";
                ClientSize = new System.Drawing.Size(256 + 128, 80);
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = FormBorderStyle.FixedSingle;
                MaximumSize = Size;
            }

            private void InitializeContent()
            {
                Label address_label = new Label();
                address_label.Text = "Cím:";
                address_label.Size = new System.Drawing.Size(48, 24);
                address_label.Location = new System.Drawing.Point(16, 16 + 0 * 24);

                Label port_label = new Label();
                port_label.Text = "Port:";
                port_label.Size = new System.Drawing.Size(48, 24);
                port_label.Location = new System.Drawing.Point(16, 16 + 1 * 24);

                address_box = new TextBox();
                address_box.Size = new System.Drawing.Size(128, 24);
                address_box.Location = new System.Drawing.Point(address_label.Location.X + address_label.Size.Width + 16, address_label.Location.Y);

                port_box = new TextBox();
                port_box.Size = new System.Drawing.Size(128, 24);
                port_box.Location = new System.Drawing.Point(port_label.Location.X + port_label.Size.Width + 16, port_label.Location.Y);

                Button accept_button = new Button();
                accept_button.Text = "Rendben";
                accept_button.Size = new System.Drawing.Size(96, 24);
                accept_button.Location = new System.Drawing.Point(address_box.Location.X + address_box.Size.Width + 16, address_box.Location.Y);
                accept_button.Click += accept_button_Click;

                Controls.Add(address_label);
                Controls.Add(port_label);
                Controls.Add(address_box);
                Controls.Add(port_box);
                Controls.Add(accept_button);
            }

            private void accept_button_Click(object _sender, EventArgs _event)
            {
                try { address = IPAddress.Parse(address_box.Text); }
                catch { MessageBox.Show("Nem megfelelő az IP-cím!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                try { port = Convert.ToInt32(port_box.Text); }
                catch { MessageBox.Show("Nem megfelelő a port!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                accepted = true;

                Close();
            }
        }
        private void button_specific_Click(object _sender, EventArgs _event)
        {
            SpecificAddressForm specificaddress = new SpecificAddressForm();
            specificaddress.ShowDialog();

            if (specificaddress.accepted)
            {
                selected_port = specificaddress.port;
                selected_address = specificaddress.address;
                selected = true;
                Close();
            }
        }

        private void button_nvm_Click(object _sender, EventArgs _event)
        {
            selected_port = 0;
            selected_address = IPAddress.Any;
            selected = true;
            Close();
        }

        private void button_accept_Click(object _sender, EventArgs _event)
        {
            if (selected) Close();
        }
        #endregion
    }
}
