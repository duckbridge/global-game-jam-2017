using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITweenBuilder {

	private Hashtable hashTable = new Hashtable();

	public ITweenBuilder SetName(string name) {
		hashTable.Add ("name", name);
		return this;
	}

	public ITweenBuilder SetScale(Vector3 scale) {
		hashTable.Add ("scale", scale);
		return this;
	}

	public ITweenBuilder SetOnComplete(string onComplete) {
		hashTable.Add ("oncomplete", onComplete);
		return this;
	}

	public ITweenBuilder SetOnCompleteTarget(GameObject target) {
		hashTable.Add ("onCompleteTarget", target);
		return this;
	}

	public ITweenBuilder SetEaseType(iTween.EaseType easeType) {
		hashTable.Add ("easetype", easeType);
		return this;
	}

	public ITweenBuilder SetOnUpdate(string onUpdateCallBack) {
		hashTable.Add ("onupdate", onUpdateCallBack);
		return this;
	}

	public ITweenBuilder SetAmount(Vector3 amount) {
		hashTable.Add ("amount", amount);

		return this;
	}

	public ITweenBuilder SetFromAndTo(float from, float to) {
		hashTable.Add ("from", from);
		hashTable.Add("to", to);

		return this;
	}

	public ITweenBuilder SetFromAndTo(Color from, Color to) {
		hashTable.Add ("from", from);
		hashTable.Add("to", to);
		
		return this;
	}

	public ITweenBuilder SetTime(float time) {
		hashTable.Add ("time", time);
		return this;
	}

	public ITweenBuilder SetRotation(Vector3 rotation) {
		hashTable.Add ("rotation", rotation);
		return this;
	}

	public ITweenBuilder SetSpeed(float speed) {
		hashTable.Add ("speed", speed);
		return this;
	}

	public ITweenBuilder SetLocal(bool isLocal = true) {
		hashTable.Add("isLocal", isLocal);
		return this;
	}

	public ITweenBuilder SetPosition(Vector3 position) {
		hashTable.Add ("position", position);
		return this;
	}

	public Hashtable Build() {
		return hashTable;
	}
}
