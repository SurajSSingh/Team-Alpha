using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MusicArea))]
public class MusicAreaEditor : Editor {
	
	public override void OnInspectorGUI(){
		MusicArea ma = target as MusicArea;
		DrawDefaultInspector();
		if (GUILayout.Button("Play")){
			ma.playAllLoops();
		}
		if (GUILayout.Button("Stop")){
			ma.stopAllLoops();
		}
	}
}