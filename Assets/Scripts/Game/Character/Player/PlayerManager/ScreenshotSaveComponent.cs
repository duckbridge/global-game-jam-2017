using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

public class ScreenshotSaveComponent : MonoBehaviour {

    private List<ScreenshotSummary> screenshotSummaries = new List<ScreenshotSummary>();

	// Use this for initialization
	void Start () {
	    InitializeScreenshotFolders();
        LoadScreenshotData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void InitializeScreenshotFolders() {

        if(!File.Exists(GameSettings.SCREENSHOTS)) {
            System.IO.Directory.CreateDirectory(GameSettings.SCREENSHOTS);
        }

        for(int i = 0; i < GameSettings.MAX_SAVE_SLOTS; i++) {

            string screenShotSubFolder = GameSettings.GetScreenShotFolderForSlot(i + 1);

            if(!File.Exists(screenShotSubFolder)) {    
                System.IO.Directory.CreateDirectory(screenShotSubFolder);
            }
        }
    }

    public void SaveScreenShot(string name, string description) {

        string fullScreenshotPath = GameSettings.GetScreenShotFolderForCurrentSlot() + name + ".png";
        
        Application.CaptureScreenshot(fullScreenshotPath);

        XmlSerializer serializer = new 
            XmlSerializer(typeof(SerializableScreenShotDataSummary));

        SerializableScreenShotDataSummary serializableScreenshotDataSummary = new SerializableScreenShotDataSummary();
        
        string fullSaveData = name + SaveUtils.DATA_SPLITTER + description + SaveUtils.DATA_SPLITTER + fullScreenshotPath;
        if(!serializableScreenshotDataSummary.screenShots.Contains(fullSaveData)) {
            serializableScreenshotDataSummary.screenShots.Add(fullSaveData);
            screenshotSummaries.Add(new ScreenshotSummary().SetName(name).SetDescription(description).Build());
        }

        StreamWriter myWriter = new StreamWriter(GameSettings.GetScreenShotDataName());
        serializer.Serialize(myWriter, serializableScreenshotDataSummary);
        
        myWriter.Close();
        
        Logger.Log ("done saving screenshot data");
    }


    public List<ScreenshotSummary> LoadScreenshotData() {
        
        screenshotSummaries = new List<ScreenshotSummary>();

        XmlSerializer serializer = new 
            XmlSerializer(typeof(SerializableScreenShotDataSummary));

        SerializableScreenShotDataSummary serializableScreenshotDataSummary = new SerializableScreenShotDataSummary();
      
        serializableScreenshotDataSummary.isCorrupt = true;

        if(File.Exists(GameSettings.GetScreenShotDataName())) {
            
            FileStream myFileStream = 
                new FileStream(GameSettings.GetScreenShotDataName(), FileMode.Open);
            
            serializableScreenshotDataSummary = (SerializableScreenShotDataSummary) serializer.Deserialize(myFileStream);
            serializableScreenshotDataSummary.isCorrupt = false;
            
            foreach(string ssData in serializableScreenshotDataSummary.screenShots) {

                string[] splitData = ssData.Split(SaveUtils.DATA_SPLITTER);
                screenshotSummaries.Add(new ScreenshotSummary().SetName(splitData[0]).SetDescription(splitData[1]).SetUrl(splitData[2]).Build());
            
            }

            myFileStream.Close();
        }

        return screenshotSummaries;

    }

    public List<ScreenshotSummary> GetScreenshotSummaries() {
        return screenshotSummaries;
    }
}
