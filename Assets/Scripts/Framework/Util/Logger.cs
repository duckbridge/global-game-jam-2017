using UnityEngine;
using System.Collections;
using System;

public class Logger : MonoBehaviour {
	
	public static void Log(object message) {
		Log (message, LogType.Log);
	}

	public static void Log(object[] messageAsArray) {
		for (int i = 0; i < messageAsArray.Length; i++) {
			Log ( messageAsArray[i], LogType.Log);
		}
	}

	public static void Log(object message, LogType logType) {
		Debug.Log (DateTime.Now.ToString("HH:mm:ss tt") + "\t [" + logType.ToString() + "]" + " \t " + message.ToString());
	}
}
