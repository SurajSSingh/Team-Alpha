using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LoopMGR))]
public class LoopMGREditor : Editor {
	

	public override void OnInspectorGUI(){
		LoopMGR loopMGR = target as LoopMGR;
		DrawDefaultInspector();

		if (loopMGR.inGameplay) return;
		if (GUILayout.Button("Play")){
			loopMGR.playAllLoops();
		}
		if (GUILayout.Button("Stop")){
			loopMGR.stopAllLoops();
		}
		if (GUILayout.Button("Create Music Area")){
			loopMGR.createMusicArea();
		}
		if (GUILayout.Button("Create Music Trigger")){
			loopMGR.createTrigger();
		}
	}
}
