// Detectar salidas de audio, darlos en una lista y cambiar la salida de audio

using System;
using System.Collections.Generic;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AudioAutoSwitcher.Services{
    public class AudioManager
    {
        private readonly CoreAudioController controller;

        public AudioManager()
        // Constructor
        {
            controller = new CoreAudioController();
        }

        public CoreAudioDevice GetCurrentDevice()
        {
            return controller.GetDefaultDevice(DeviceType.Playback,Role.Multimedia);
        }

        public IEnumerable<CoreAudioDevice> EnumOutput()
        /*
        Output: Devuelve un IEnumerable<CoreAudioDevice>, es decir un iterable de los dispositivos de audio conectados
        */
        {
            return controller.GetPlaybackDevices(DeviceState.Active);
        }

        public void ChangeOutput(CoreAudioDevice device)
        /*
        Input: Dispositivo al que quiero cambiar

        Output: Cambio el dispositivo de salida de audio
        */
        {
            if (device!=null)
            {
                device.SetAsDefault(); //Cambio la salida de audio al del dispotivo
                device.SetAsDefaultCommunications(); //Cambio también en Discord
            }
        }

        public CoreAudioDevice GetById(Guid id)
        /*
        Input: La ID del dispositivo al que quiero cambiar en tipo Guid (Identificador global único)

        Output: Dispositivo de audio activo con dicho ID
        */
        {
            return controller.GetDevice(id,DeviceState.Active);
        }
    }
}