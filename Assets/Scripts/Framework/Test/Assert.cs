using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Assert {

	public static void AssertTrue(bool actual) {
		StackFrame frame = new StackFrame(1);
		var method = frame.GetMethod();

		if(actual == true) {
			Logger.Log ("[PASSED] " + method.Name + " expected True and was " + actual, LogType.Assert);
		} else {
			Logger.Log ("[FAILED] " + method.Name + " expected True and was " + actual, LogType.Assert);
		}
	}

	public static void AssertFalse(bool actual) {
		StackFrame frame = new StackFrame(1);
		var method = frame.GetMethod();
		
		if(actual == false) {
			Logger.Log ("[PASSED] " + method.Name + " expected False and was " + actual, LogType.Assert);
		} else {
			Logger.Log ("[FAILED] " + method.Name + " expected False and was " + actual, LogType.Assert);
		}
	}

	public static void AssertEquals(object expected, object actual) {
		StackFrame frame = new StackFrame(1);
		var method = frame.GetMethod();

		if(expected == actual) {
			Logger.Log ("[PASSED] " + method.Name + " expected " + expected + " and was " + actual, LogType.Assert);
		} else {
			Logger.Log ("[FAILED] " + method.Name + " expected " + expected + " but was " + actual, LogType.Assert);
		}
	}

	public static void AssertEquals(int expected, int actual) {
		StackFrame frame = new StackFrame(1);
		var method = frame.GetMethod();
		
		if(expected.Equals(actual)) {
			Logger.Log ("[PASSED] " + method.Name + " expected " + expected + " and was " + actual, LogType.Assert);
		} else {
			Logger.Log ("[FAILED] " + method.Name + " expected " + expected + " but was " + actual, LogType.Assert);
		}
	}

	public static void AssertNotEquals(object expected, object actual) {
		StackFrame frame = new StackFrame(1);
		var method = frame.GetMethod();
		
		if(expected != actual) {
			Logger.Log ("[PASSED] " + method.Name + " expected " + expected + " and was " + actual, LogType.Assert);
		} else {
			Logger.Log ("[FAILED] " + method.Name + " expected " + expected + " but was " + actual, LogType.Assert);
		}
	}
	
	public static void AssertNotEquals(int expected, int actual) {
		StackFrame frame = new StackFrame(1);
		var method = frame.GetMethod();
		
		if(!expected.Equals(actual)) {
			Logger.Log ("[PASSED] " + method.Name + " expected " + expected + " and was " + actual, LogType.Assert);
		} else {
			Logger.Log ("[FAILED] " + method.Name + " expected " + expected + " but was " + actual, LogType.Assert);
		}
	}
}
