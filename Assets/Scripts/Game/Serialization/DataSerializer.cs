using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class DataSerializer : MonoBehaviour {

	public List<TileBlock> tileBlocks { get; set;}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Serialize(List<TileBlock> allTileBlocks) {

		XmlSerializer serializer = new 
			XmlSerializer(typeof(SerializableTileBlockContainer));

		SerializableTileBlockContainer serializableTileBlockContainer = new SerializableTileBlockContainer();

		List<SerializableTileBlock> serializableTileBlocks = new List<SerializableTileBlock>();

		for(int i = 0 ; i < allTileBlocks.Count; i++) {
			serializableTileBlocks.Add (SerializationHelper.SerializeTileBlock(allTileBlocks[i]));
		}

		serializableTileBlockContainer.serializableTileBlocks = serializableTileBlocks;

		StreamWriter myWriter = new StreamWriter(GameSettings.GetMapSaveName());
		serializer.Serialize(myWriter, serializableTileBlockContainer);

		myWriter.Close();

		Logger.Log ("done serializing");
	}

	public SerializableTileBlockContainer DeSerialize() {
		
		XmlSerializer serializer = new XmlSerializer(typeof(SerializableTileBlockContainer));
		
		SerializableTileBlockContainer serializableTileBlockContainer;
		
		List<SerializableTileBlock> serializableTileBlocks = new List<SerializableTileBlock>();

		FileStream myFileStream = 
			new FileStream(GameSettings.GetMapSaveName(), FileMode.Open);

		serializableTileBlockContainer = (SerializableTileBlockContainer) serializer.Deserialize(myFileStream);
		
		myFileStream.Close();
		
		Logger.Log ("done deserializing");

		return serializableTileBlockContainer;
	}
}
