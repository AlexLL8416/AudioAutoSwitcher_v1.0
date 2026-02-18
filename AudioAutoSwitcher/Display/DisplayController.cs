using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using AudioAutoSwitcher.Configuration;
using AudioAutoSwitcher.Models;
using AudioAutoSwitcher.Services;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AudioAutoSwitcher.Display{
    
    public class DisplaySwitch : ApplicationContext
    {
        private NotifyIcon displayIcon;
        private AudioManager audioManager;
        private List<AudioProfile> profiles;
        private ConfigMenu configWindow = null;
        public DisplaySwitch()
        {
            // Inicio los servicios
            audioManager = new AudioManager();
            profiles = new List<AudioProfile>();

            // Creo el icono
            displayIcon = new NotifyIcon();
            displayIcon.Visible = true;
            displayIcon.Text = "Audio Auto Switcher";

            // Click izquierdo
            displayIcon.MouseClick += OnIconClick;

            // Click derecho 
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Configuración",null,OpenConfig);
            menu.Items.Add("Salir", null, Exit);
            displayIcon.ContextMenuStrip = menu;

            LoadAndUpdate();
        }

        private void OnIconClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (profiles.Count == 0)
                {
                    OpenConfig(null,null);
                }
                else
                {
                    ChangeNextDevice();
                }
            }
        }

        private void ChangeNextDevice()
        {
            // Consigo la id del dispositivo que está sonando
            var currentDevice = audioManager.GetCurrentDevice();
            string idCurrent = currentDevice.Id.ToString();

            // Calculo su posición en los dispositivos configurados
            int indexCurrent = profiles.FindIndex(p => p.DeviceId == idCurrent);

            // Calculo siguiente posición
            int indexNext = (indexCurrent + 1) % profiles.Count;

            // Si no aparece en la configuración se toma el primer dispositivo
            if (indexCurrent == -1) indexNext = 0;

            // Tomo el siguiente dispositivo
            var profileNext = profiles[indexNext];

            Guid idGuid;
            if (Guid.TryParse(profileNext.DeviceId,out idGuid))
            {
                var realDevice = audioManager.GetById(idGuid);
                if (realDevice != null)
                {
                    audioManager.ChangeOutput(realDevice);
                }
            }

            LoadAndUpdate();
        }

        private void LoadAndUpdate()
        {
            profiles = ConfigSaveLoad.LoadConfig();

            // Si no hay configuración es un simbolo de información
            if(profiles == null || profiles.Count == 0)
            {
                displayIcon.Icon = SystemIcons.Information;
            }
            else
            {
                // Miro que dispositivo está sonando
                var currentDevice = audioManager.GetCurrentDevice();
                string idCurrent = currentDevice.Id.ToString();

                // Calculo su posición en los dispositivos configurados
                int indexCurrent = profiles.FindIndex(p => p.DeviceId == idCurrent);

                if (indexCurrent != -1)
                {
                    displayIcon.Icon = GetIconByName(profiles[indexCurrent].Icon);
                }
                else
                {
                    displayIcon.Icon = SystemIcons.Warning;
                }
            }
        }

        private Icon GetIconByName(string nameIcon)
        {
            string nombreArchivo = "";

            // Asignamos el nombre del archivo según el caso
            switch (nameIcon)
            {
                case "Altavoces": nombreArchivo = "Altavoz.ico"; break;
                case "Auriculares": nombreArchivo = "Auriculares.ico"; break;
                case "Bluetooth": nombreArchivo = "Bluetooth.ico"; break;
                case "Pantalla": nombreArchivo = "Pantalla.ico"; break;
                default: return SystemIcons.Application;
            }

            try
            {
                // Empiezo a buscar donde están los iconos
                string directorioActual = AppDomain.CurrentDomain.BaseDirectory;
                string rutaFinal = "";
                bool encontrado = false;

                // Busco el icono en todos los niveles
                for (int i = 0; i < 4; i++)
                {
                    // Pruebo si existe Assets\icons\archivo.ico en el nivel actual
                    string posibleRuta = Path.Combine(directorioActual, "Assets", "icons", nombreArchivo);
                    
                    if (File.Exists(posibleRuta))
                    {
                        rutaFinal = posibleRuta;
                        encontrado = true;
                        break;
                    }
                    // Subo si no lo he encontrado
                    var padre = Directory.GetParent(directorioActual);
                    if (padre == null) break; // Ya no podría subir más
                    directorioActual = padre.FullName;
                }

                if (encontrado)
                {
                    return new Icon(rutaFinal);
                }
                else
                {
                    return SystemIcons.Warning; 
                }
            }
            catch (Exception)
            {
                return SystemIcons.Error;
            }
        }

        void OpenConfig(object sender, EventArgs e)
        {
            if (configWindow==null||configWindow.IsDisposed)
            {
                configWindow = new ConfigMenu();
                // Si cierro la configuración al guardar, actualizo el icono
                configWindow.FormClosed += (s,args) =>
                {
                    if (configWindow.DialogResult == DialogResult.OK)
                    {
                        LoadAndUpdate();
                    }
                };
                configWindow.Show();
            }
            else
            {
                configWindow.BringToFront();
                configWindow.Focus();
            }
        }

        void Exit(object sender, EventArgs e)
        {
            displayIcon.Visible = false;
            Application.Exit();
        }
    }
}