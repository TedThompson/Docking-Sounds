//*******************************
// Title: DPSoundFX
// Author: Ted Thompson
// License: GNU GPL v3
// Created: June 19, 2015
//*******************************
using UnityEngine;
using System;

namespace DockingSounds
{
    class DPSoundFX : PartModule
    {
        [KSPField]
        public string sound_docking = "FP_DPSoundFX/Sounds/dock";
        [KSPField]
        public string sound_undocking = "FP_DPSoundFX/Sounds/undock";
        [KSPField]
        public bool internalSoundsOnly = false;
        public FXGroup DockSound = null;
        public FXGroup UndockSound = null;

        //Create FX group for sounds
        public bool CreateGroup(FXGroup group, string filename, bool loop)
        {
            if (name != string.Empty)
            {
                if (!GameDatabase.Instance.ExistsAudioClip(filename))
                {
                    PrintToLog("[DPSoundFX]ERROR - file " + filename + ".* not found!", 3);
                    return false;
                }
                group.audio = gameObject.AddComponent<AudioSource>();
                group.audio.volume = GameSettings.SHIP_VOLUME;
                group.audio.rolloffMode = AudioRolloffMode.Logarithmic;
                group.audio.dopplerLevel = 0f;
                //group.audio.panLevel = 1f; Depreciated so we add 'spatialBlend' below
                group.audio.spatialBlend = 1f;
                group.audio.clip = GameDatabase.Instance.GetAudioClip(filename);
                group.audio.loop = loop;
                group.audio.playOnAwake = false;
                return true;
            }
            return false;
        }

        // Play docking sound
        public void DPFXdock(GameEvents.FromToAction<Part, Part> partAction)
        {
            PrintToLog("[DPSoundFX] Docking\n               Docked FROM   : " +
                        partAction.from.vessel.vesselName +
                        "\n               Docked TO     : " +
                        partAction.to.vessel.vesselName +
                        "\n               Docked FROM ID: " +
                        partAction.from.vessel.id +
                        "\n               Docked TO ID  : " +
                        partAction.to.vessel.id, 1);

            if (internalSoundsOnly && CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.IVA)
            {
                if (FlightGlobals.getStaticPressure() <= 50.0)
                    return;
            }
            // Does the "caller" even have a DPSoundFX entry in it's config?  (KIS trigger workaround)
            if (!partAction.from.Modules.Contains("DPSoundFX"))
                return;

            // Is the "caller" the one that just docked?  If not go away, you're not welcome here!!
            if (Part.FromGO(gameObject).flightID != partAction.from.flightID)
                return;

            if (!DockSound.audio.isPlaying)
            {
                DockSound.audio.volume = GameSettings.SHIP_VOLUME;
                DockSound.audio.Play();
                PrintToLog("[DPSoundFX] Starting Playback, DOCKING", 1);
            }
        }

        public void DPFXundock(Part dockingPortPart)
        {
            if (internalSoundsOnly && CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.IVA)
            {
                if (FlightGlobals.getStaticPressure() < 50.0)
                    return;
            }

            // Does the "caller" even have a DPSoundFX entry in it's config?  (KIS didn't seem to trigger this, but it might avoid a false trigger from elsewhere)
            if (!dockingPortPart.Modules.Contains("DPSoundFX"))
                return;

            // Did the caller just undock?  If not go home caller, you're drunk.
            if (Part.FromGO(gameObject).flightID != dockingPortPart.flightID)
                return;

            if (!UndockSound.audio.isPlaying)
            {
                UndockSound.audio.volume = GameSettings.SHIP_VOLUME;
                UndockSound.audio.Play();
                PrintToLog("[DPSoundFX] Starting Playback, UNDOCKING", 1);
            }
        }

        public override void OnStart(PartModule.StartState state)
        {
            try
            {
                if (state == StartState.Editor || state == StartState.None) return;

                PrintToLog("[DPSoundFX] OnStart Called: State was " + state, 1);

                if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                    return;

                base.OnStart(state);

                CreateGroup(DockSound, sound_docking, false);
                CreateGroup(UndockSound, sound_undocking, false);

                GameEvents.onPartCouple.Add(DPFXdock);
                GameEvents.onPartUndock.Add(DPFXundock);
            }

            catch (Exception ex)
            {
                Debug.LogError("[DPSoundFX] OnStart: " + ex.Message);
            }
        }

        void OnDestroy()
        {
            PrintToLog("[DPSoundFX] OnDestroy()! (KaBOOM?)", 1);
            GameEvents.onPartCouple.Remove(DPFXdock);
            GameEvents.onPartUndock.Remove(DPFXundock);
        }

        void PrintToLog(string outText, int styleFlag)
        {
#if DEBUG
            switch (styleFlag)
            {
                case 1:
                    Debug.Log(outText);
                    break;
                case 2:
                    Debug.LogWarning(outText);
                    break;
                case 3:
                    Debug.LogError(outText);
                    break;
                default:
                    Debug.LogError("[DPSoundFX] Improper call to internal logger.");
                    break;
            }
#endif
        }
    }
}