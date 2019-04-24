using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class SaveUtils {

	public static char DATA_SPLITTER = ':';
	public static string TMP_PREFIX = "tmp_";

	public static bool HasMapSaveFile(int slotNumber) {

		int savedSlotNumber = GameSettings.CHOSEN_SAVE_SLOT;

		GameSettings.CHOSEN_SAVE_SLOT = slotNumber;

		bool hasMapSaveFile = System.IO.File.Exists(GameSettings.GetMapSaveName());

		GameSettings.CHOSEN_SAVE_SLOT = savedSlotNumber;

		return hasMapSaveFile;

	}

	public static void DeleteMapSaveFile(int slotNumber) {
		int savedSlotNumber = GameSettings.CHOSEN_SAVE_SLOT;
		
		GameSettings.CHOSEN_SAVE_SLOT = slotNumber;

		if(System.IO.File.Exists(GameSettings.GetMapSaveName())) {
			System.IO.File.Delete(GameSettings.GetMapSaveName());
		}
		
		GameSettings.CHOSEN_SAVE_SLOT = savedSlotNumber;
		
	}
	
	public static void DeleteTPFile(int slotNumber) {
		int savedSlotNumber = GameSettings.CHOSEN_SAVE_SLOT;
		
		GameSettings.CHOSEN_SAVE_SLOT = slotNumber;

		if(System.IO.File.Exists(GameSettings.GetPlayerDataSaveName())) {
			System.IO.File.Delete(GameSettings.GetPlayerDataSaveName());
		}
		
		GameSettings.CHOSEN_SAVE_SLOT = savedSlotNumber;
		
	}

	public static void CreateMapSaveFolderIfNotExist() {

		bool hasMapSaveFolder = System.IO.File.Exists(GameSettings.GetMapSaveFolder());

		if(!hasMapSaveFolder) {
			System.IO.Directory.CreateDirectory(GameSettings.GetMapSaveFolder());
		}
	}

	public static SerializablePlayerDataSummary FindMostRecentSaveFile(List<SerializablePlayerDataSummary> allSaveFiles) {
		
		SerializablePlayerDataSummary mostRecentSaveFile = new SerializablePlayerDataSummary();
		mostRecentSaveFile.isCorrupt = true;
		
		DateTime mostRecentTime = new DateTime(1900, 1, 1);
		
		foreach(SerializablePlayerDataSummary foundDataSummary in allSaveFiles) {
			if(foundDataSummary.lastSaveDate > mostRecentTime) {
				mostRecentSaveFile = foundDataSummary;
				mostRecentTime = foundDataSummary.lastSaveDate;
			}
		}
		
		return mostRecentSaveFile;

	}

	public static SerializablePlayerDataSummary LoadSaveFileForSlot(XmlSerializer serializer, int slot) {
		if(File.Exists(GameSettings.GetPlayerDataSaveNameForSlot(slot))) {

			FileStream myFileStream = 
				new FileStream(GameSettings.GetPlayerDataSaveNameForSlot(slot), FileMode.Open);

			SerializablePlayerDataSummary serializablePlayerDataSummary = new SerializablePlayerDataSummary();

			serializablePlayerDataSummary = (SerializablePlayerDataSummary) serializer.Deserialize(myFileStream);
			serializablePlayerDataSummary.isCorrupt = false;

			myFileStream.Close();

			return serializablePlayerDataSummary;
		}

		return null;
	}

	public static List<SerializablePlayerDataSummary> LoadAllSaveFiles() {
		
		List<SerializablePlayerDataSummary> allSaveFiles = new List<SerializablePlayerDataSummary>();
		XmlSerializer serializer = new XmlSerializer(typeof(SerializablePlayerDataSummary));
		
		for(int i = 0 ; i < GameSettings.MAX_SAVE_SLOTS; i++) {
			SerializablePlayerDataSummary savedFile = LoadSaveFileForSlot (serializer, i);
			if (savedFile != null) {
				allSaveFiles.Add (savedFile);
			}
		}
		
		return allSaveFiles;
	}
}
