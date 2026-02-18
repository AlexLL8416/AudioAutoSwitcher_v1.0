// Archivo para crear menú de configuración
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using AudioAutoSwitcher.Models;
using AudioAutoSwitcher.Services;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AudioAutoSwitcher.Configuration
{
    public class ConfigMenu:Form{ //Hereda la forma de Form
        private AudioManager audioManager;
        private ComboBox cmbDevice1, cmbDevice2;
        private TextBox txtName1, txtName2;
        private ComboBox cmbIcon1, cmbIcon2;
        private Button btnSave;

        // Propiedad pública para recuperar los datos al cerrar
        public List<AudioProfile> PerfilesGuardados { get; private set; }

        public ConfigMenu()
        {
            this.Text = "Configuración de Audio";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            audioManager = new AudioManager();
            PerfilesGuardados = new List<AudioProfile>();

            StartComp();
            LoadData();
        }

        private void StartComp()
        {
            GroupBox group1 = CrearGrupo("Perfil 1 (Principal)", 20);
            
            // Crear controles dentro del grupo 1
            cmbDevice1 = CrearComboDispositivos(20, 30);
            txtName1 = CrearInputTexto(20, 70, "Nombre personalizado...");
            cmbIcon1 = CrearComboIconos(20, 110);
            
            group1.Controls.AddRange(new Control[] { new Label { Text="Dispositivo:", Top=15, Left=20 }, cmbDevice1, txtName1, cmbIcon1 });
            this.Controls.Add(group1);

            // Segundo dispositivo 
            GroupBox group2 = CrearGrupo("Perfil 2 (Secundario)", 160);
            
            // Crear controles dentro del grupo 2
            cmbDevice2 = CrearComboDispositivos(20, 30);
            txtName2 = CrearInputTexto(20, 70, "Nombre personalizado...");
            cmbIcon2 = CrearComboIconos(20, 110);

            group2.Controls.AddRange(new Control[] { new Label { Text = "Dispositivo:", Top = 15, Left = 20 }, cmbDevice2, txtName2, cmbIcon2 });
            this.Controls.Add(group2);

            // Botón guardar
            btnSave = new Button() { Text = "GUARDAR CONFIGURACIÓN", Location = new Point(100, 310), Size = new Size(240, 40), BackColor = Color.LightGreen };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);
        }

        private void LoadData()
        {
            try 
            {
                var dispositivos = audioManager.EnumOutput().ToList();

                foreach (var dev in dispositivos)
                {
                    var item = new DeviceComboItem(dev);
                    cmbDevice1.Items.Add(item);
                    cmbDevice2.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error detectando audio: " + ex.Message);
            }
        }

        // Lógica de guardado
        private void BtnSave_Click(object sender, EventArgs e)
        {
            PerfilesGuardados.Clear();

            // Procesar Perfil 1
            if (cmbDevice1.SelectedItem is DeviceComboItem item1)
            {
                PerfilesGuardados.Add(new AudioProfile
                {
                    DeviceId = item1.Device.Id.ToString(),
                    Name = string.IsNullOrWhiteSpace(txtName1.Text) ? item1.Device.FullName : txtName1.Text,
                    Icon = cmbIcon1.SelectedItem?.ToString() ?? "Default"
                });
            }

            // Procesar Perfil 2
            if (cmbDevice2.SelectedItem is DeviceComboItem item2)
            {
                PerfilesGuardados.Add(new AudioProfile
                {
                    DeviceId = item2.Device.Id.ToString(),
                    Name = string.IsNullOrWhiteSpace(txtName2.Text) ? item2.Device.FullName : txtName2.Text,
                    Icon = cmbIcon2.SelectedItem?.ToString() ?? "Default"
                });
            }

            if (PerfilesGuardados.Count == 0)
            {
                MessageBox.Show("Por favor, selecciona al menos un dispositivo.");
                return;
            }
            
            ConfigSaveLoad.SaveConfig(PerfilesGuardados);

            this.DialogResult = DialogResult.OK; 
            this.Close();
        }

        // Funciones auxiliares para crear botones...
        private GroupBox CrearGrupo(string texto, int top)
        {
            return new GroupBox() { Text = texto, Location = new Point(20, top), Size = new Size(400, 140) };
        }

        private ComboBox CrearComboDispositivos(int x, int y)
        {
            return new ComboBox() { Location = new Point(x, y), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
        }

        private TextBox CrearInputTexto(int x, int y, string placeholder)
        {
            return new TextBox() { Location = new Point(x, y), Width = 200, Text = "" };
        }

        private ComboBox CrearComboIconos(int x, int y)
        {
            var combo = new ComboBox() { Location = new Point(x + 210, y - 40), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(new object[] { "Altavoces", "Auriculares", "Bluetooth", "Pantalla" });
            combo.SelectedIndex = 0;
            return combo;
        }

        // Clase auxiliar pequeña para que el ComboBox muestre el nombre bonito y guarde el objeto real
        public class DeviceComboItem
        {
            public CoreAudioDevice Device { get; }
            public DeviceComboItem(CoreAudioDevice device) { Device = device; }
            public override string ToString() { return Device.FullName; }
        }
    };
}