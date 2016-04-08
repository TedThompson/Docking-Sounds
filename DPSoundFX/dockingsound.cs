//*******************************
// Title: DPSoundFX
// Author: Ted Thompson
// License: GNU GPL v3
// Created: June 19, 2015
//*******************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
		public FXGroup dockSound = null;
		public FXGroup undockSound = null;
		
		//Create FX group for sounds
		public bool createGroup (FXGroup group, string name, bool loop)
		{
			if (name != string.Empty) {
				if (!GameDatabase.Instance.ExistsAudioClip (name)) {
					Debug.LogError ("[DPSoundFX]ERROR - file " + name + ".* not found!");
					return false;
				}
				group.audio = gameObject.AddComponent<AudioSource> ();
				group.audio.volume = GameSettings.SHIP_VOLUME;
				group.audio.rolloffMode = AudioRolloffMode.Logarithmic;
				group.audio.dopplerLevel = 0f;
				//group.audio.panLevel = 1f; Depreciated so we add 'spatialBlend' below
				group.audio.spatialBlend = 1f;
				group.audio.clip = GameDatabase.Instance.GetAudioClip (name);
				group.audio.loop = loop;
				group.audio.playOnAwake = false;             
				return true;
			}
			return false;
		}
		
		// Play docking sound
		public void DPFXdock (GameEvents.FromToAction<Part, Part> partAction)
		{
			//Debug.Log ("[DPSoundFX] Docking");
			//Debug.Log ("               Docked FROM   : " + partAction.from.vessel.vesselName);
			//Debug.Log ("               Docked TO     : " + partAction.to.vessel.vesselName);
			//Debug.Log ("               Docked FROM ID: " + partAction.from.vessel.id.ToString ());
			//Debug.Log ("               Docked TO ID  : " + partAction.to.vessel.id.ToString ());
			
			if (internalSoundsOnly && CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.IVA) {
				if (FlightGlobals.getStaticPressure() <= 50.0)
					return;
			}
			// Does the "caller" even have a DPSoundFX entry in it's config?  (KIS trigger workaround)
			if (!partAction.from.Modules.Contains ("DPSoundFX"))
				return;
			
			// Is the "caller" the one that just docked?  If not go away, you're not welcome here!!
			if (Part.FromGO (gameObject).flightID != partAction.from.flightID)
				return;
			
			if (!this.dockSound.audio.isPlaying) { 
				this.dockSound.audio.volume = GameSettings.SHIP_VOLUME;
				this.dockSound.audio.Play ();
			}
		}
		
		public void DPFXundock (Part part)
		{
			if (internalSoundsOnly && CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.IVA) {
				if (FlightGlobals.getStaticPressure() < 50.0)
					return;
			}
			
			// Does the "caller" even have a DPSoundFX entry in it's config?  (KIS didn't seem to trigger this, but it might avoid a false trigger from elsewhere)
			if (!part.Modules.Contains ("DPSoundFX"))
				return;
			
			// Did the caller just undock?  If not go home caller, you're drunk.
			if (Part.FromGO (gameObject).flightID != part.flightID)
				return;
			
			if (!this.dockSound.audio.isPlaying) {
				this.dockSound.audio.volume = GameSettings.SHIP_VOLUME;				
				this.undockSound.audio.Play ();
			}
		}
		
		public override void OnStart (PartModule.StartState state)
		{
			Debug.Log ("[DPSoundFX] OnStart Called: State is " + state);
			
			if (HighLogic.LoadedScene != GameScenes.FLIGHT)
				return;
			
			base.OnStart (state);
			
			createGroup (dockSound, sound_docking, false);
			createGroup (undockSound, sound_undocking, false);
			
			GameEvents.onPartCouple.Add (DPFXdock);
			GameEvents.onPartUndock.Add (DPFXundock);
			
			Debug.Log ("[DPSoundFX] OnStart Executed: State was " + state);
		}
		
		void OnDestroy ()
		{
			GameEvents.onPartUndock.Remove (DPFXundock);
			GameEvents.onPartCouple.Remove (DPFXdock);
		}
	}
}