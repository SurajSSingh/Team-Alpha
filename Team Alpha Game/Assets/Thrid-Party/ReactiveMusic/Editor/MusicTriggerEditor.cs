using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MusicTrigger))]
public class MusicTriggerEditor : Editor {
	
	
	public override void OnInspectorGUI(){
		MusicTrigger musicTrigger = target as MusicTrigger;
		DrawDefaultInspector();

		if (GUILayout.Button("Initialize Trigger")){
			musicTrigger.initializeTrigger();
		}

	}
}