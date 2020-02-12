using UnityEngine;
using System.Collections;

public class MusicTriggerSide : MonoBehaviour {

	[HideInInspector]
	public int sideNum;
	public MusicTrigger parent;
	static float colliderWidth = 1f;
	static float colliderHeight = 1000f;
	static float colliderDepth = 1000f;

	
	//	This public function is used by 
	static public void createMusicTriggerSide(GameObject obj, int sidenum, MusicTrigger parent){
		MusicTriggerSide mts = obj.AddComponent<MusicTriggerSide>();
		obj.layer = LayerMask.NameToLayer("Music");
		obj.transform.localScale = new Vector3(colliderWidth, colliderHeight, colliderDepth); 
		mts.sideNum = sidenum;
		mts.parent = parent;

		Vector3 pos = parent.transform.position;
		pos.x += (mts.sideNum * colliderWidth);
		obj.transform.position = pos;

		BoxCollider2D bc = obj.AddComponent<BoxCollider2D>();
		bc.isTrigger = true;
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log(other.gameObject);
		parent.inTrigger(sideNum);
	}

	void OnTriggerExit2D(Collider2D other){
		Debug.Log(other.gameObject);
		parent.outOfTrigger(sideNum);
	}

	//	Awake and Update are defined to ensure that the 
	//	triggers do not move even if the LoopMGR is set
	//	to follow an object
	Vector3 initialPos;
	void Awake(){
		initialPos = transform.position;
	}

	void Update(){
		transform.position = initialPos;
	}

}
