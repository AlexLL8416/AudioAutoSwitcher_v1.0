// Detecta el archivo config.json o guarda la configuración en el menú configuración

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Forms;
using AudioAutoSwitcher.Models;

namespace AudioAutoSwitcher.Configuration
{
    public static class ConfigSaveLoad
    {
        private static string GetConfigFilePath()
        {
            // Busco la carpeta AppData/Roaming del usuario actual
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
            // Creo una subcarpeta
            string appFolder = Path.Combine(appData, "AudioAutoSwitcher");
            
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }
            // Devuelvo ruta completa del .json
            return Path.Combine(appFolder, "audio_config.json");
        }

        private static readonly string FilePath = GetConfigFilePath(); 

        public static void SaveConfig(List<AudioProfile> devices)
        {
            try
            {
                // Opción para que se vea bien
                var options = new JsonSerializerOptions{WriteIndented=true};
                
                // Guardo en el archivo del FilePath
                string jsongString = JsonSerializer.Serialize(devices,options);
                File.WriteAllText(FilePath,jsongString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la configuración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static List<AudioProfile> LoadConfig()
        {
            // Verifico si existe el archivo
            if (!File.Exists(FilePath))
            {
                return new List<AudioProfile>();
            }

            try
            {
                // Leo texto del archivo
                string jsonString = File.ReadAllText(FilePath);

                // convierto el texto JSON a objetos C#
                var profiles = JsonSerializer.Deserialize<List<AudioProfile>>(jsonString);

                // Si devuelve un null, doy una lista vacía
                return profiles ?? new List<AudioProfile>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer la configuración: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new List<AudioProfile>();
            }
        }
    }
}