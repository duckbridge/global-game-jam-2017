using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

public class SettingsSaveComponent : MonoBehaviour {

    private float fxVolume = GameSettings.DEFAULT_FX_VOLUME;
    private float bgVolume = GameSettings.DEFAULT_MUSIC_VOLUME;
    private bool hasCameraShakeEnabled = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SaveSettingsData() {
        XmlSerializer serializer = new 
            XmlSerializer(typeof(SerializableSettingsDataSummary));

        SerializableSettingsDataSummary serializableSettingsDataSummary = new SerializableSettingsDataSummary();
        
        serializableSettingsDataSummary.bgVolume = System.Convert.ToInt32(bgVolume * 100);
        serializableSettingsDataSummary.fxVolume = System.Convert.ToInt32(fxVolume * 100);

        serializableSettingsDataSummary.hasCameraShakeEnabled = hasCameraShakeEnabled;

        StreamWriter myWriter = new StreamWriter(GameSettings.GetSettingsDataSaveName());
        serializer.Serialize(myWriter, serializableSettingsDataSummary);
        
        myWriter.Close();
        
        Logger.Log ("done saving settings data");
    }


    public SerializableSettingsDataSummary LoadSettingsData() {
        
        XmlSerializer serializer = new XmlSerializer(typeof(SerializableSettingsDataSummary));
        
        SerializableSettingsDataSummary serializableSettingsDataSummary = new SerializableSettingsDataSummary();

        serializableSettingsDataSummary.isCorrupt = true;

        if(File.Exists(GameSettings.GetSettingsDataSaveName())) {
            
            FileStream myFileStream = 
                new FileStream(GameSettings.GetSettingsDataSaveName(), FileMode.Open);
            
            serializableSettingsDataSummary = (SerializableSettingsDataSummary) serializer.Deserialize(myFileStream);
            serializableSettingsDataSummary.isCorrupt = false;
            
            bgVolume = ((float)serializableSettingsDataSummary.bgVolume / 100);
            fxVolume = ((float)serializableSettingsDataSummary.fxVolume / 100);

            SoundUtils.SetBGVolume(bgVolume);
            SoundUtils.SetFXVolume(fxVolume);

            hasCameraShakeEnabled = serializableSettingsDataSummary.hasCameraShakeEnabled;

            myFileStream.Close();
        }

        return serializableSettingsDataSummary;

    }

    public void SetBgVolume(float bgVolume) {
        this.bgVolume = bgVolume;
        SoundUtils.SetBGVolume(bgVolume);
    }

    public void SetFxVolume(float fxVolume) {
        this.fxVolume = fxVolume;
        SoundUtils.SetFXVolume(fxVolume);
    }
    
    public bool HasCameraShakeEnabled() {
        return hasCameraShakeEnabled;
    }

    public void ToggleCameraShake(bool isToggledOn) {
        this.hasCameraShakeEnabled = isToggledOn;
    }
}
