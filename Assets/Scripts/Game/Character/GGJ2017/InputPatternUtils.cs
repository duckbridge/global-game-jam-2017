using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPatternUtils {
	
	public static InputPattern FindUnfinishedInputPatternByFrameCount(List<InputPattern> source, int frameCount, bool useStartWithoutRange) {
		
		if (source == null) {
			return null;
		}

		double seconds = InputPatternUtils.ToSeconds (frameCount);

		return FindUnfinishedInputPatternByElapsedTime (source, seconds, useStartWithoutRange);
	}

	public static InputPattern FindUnfinishedInputPatternByElapsedTime(List<InputPattern> source, double seconds, bool useStartWithoutRange) {

		if (source == null) {
			return null;
		}


		foreach (InputPattern inputPattern in source) {
			if (useStartWithoutRange) {
				if (seconds >= inputPattern.start && seconds <= (inputPattern.start + inputPattern.range) && 
					!inputPattern.IsFinished()) {
					return inputPattern;
				}
			} else {
				if (seconds >= (inputPattern.start - inputPattern.range) && seconds <= (inputPattern.start + inputPattern.range) && 
					!inputPattern.IsFinished()) {
					return inputPattern;
				}
			}
		}

		return null;
	}

	public static double ToSeconds(int frames) {
		return ((double)frames) * 0.02d;
	}
}
