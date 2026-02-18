// Detecta el archivo config.json o guarda la configuración en el menú configuración

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using AudioAutoSwitcher.Models;

namespace AudioAutoSwitcher.Configuration
{
    public static class ConfigSaveLoad
    {
        private readonly static string FilePath = "audio_config.json";

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