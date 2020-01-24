using UnityEngine;
using System.Collections;

public class MusicTrigger : MonoBehaviour {

	public MusicArea musicArea0;
	public MusicArea musicArea1;

	static int musicTriggerNum = 0;

	static public void createMusicTrigger(GameObject go){
		MusicTrigger mt = go.AddComponent<MusicTrigger>();
		mt.name = "Music Trigger " + musicTriggerNum++;
	}

	public void initializeTrigger(){
		createMusicTriggerCollider(0);
		createMusicTriggerCollider(1);
	}

	void createMusicTriggerCollider(int musicArea){
		GameObject go = new GameObject();
		go.transform.parent = transform;
		go.name = "TriggerSide " + musicArea;
		MusicTriggerSide.createMusicTriggerSide(go, musicArea, this);
	}

	//	For children colliders
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	[HideInInspector]
	public bool inTrigger0;
	[HideInInspector]
	public bool inTrigger1;

	public void inTrigger(int colliderNum){
		if (colliderNum == 0) inTrigger0 = true;
		if (colliderNum == 1) inTrigger1 = true;
	}

	public void outOfTrigger(int colliderNum){
		if (colliderNum == 0) inTrigger0 = false;
		if (colliderNum == 1) inTrigger1 = false;
		if (colliderNum == 0 && !inTrigger1){
			//	If we're not in trigger 1 and we're leaving trigger 0
			//	we're in the area for trigger 0
			musicArea0.activate();
		}
		if (colliderNum == 1 && !inTrigger0){
			//	Vice versa
			musicArea1.activate();
		}
	}


}
