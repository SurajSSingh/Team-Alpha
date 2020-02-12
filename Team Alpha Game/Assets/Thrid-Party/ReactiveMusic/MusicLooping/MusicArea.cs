using UnityEngine;
using System.Collections;

public class MusicArea : MonoBehaviour {
	static int number = 0;
	public LoopTrack[] loops;

	static public void CreateMusicArea(GameObject obj, LoopTrack[] other){
		MusicArea ma = obj.AddComponent<MusicArea>();
		ma.loops = new LoopTrack[other.Length];
		obj.name = "Music Area " + number++;
		for (int i = 0; i < other.Length; ++i){
			ma.loops[i] = new LoopTrack(other[i]);
		}
	}

	MusicArea(LoopTrack[] other){
		loops = new LoopTrack[other.Length];
		for (int i = 0; i < other.Length; ++i){
			loops[i] = new LoopTrack(other[i]);
		}
	}

	public void activate(){
		GetComponentInParent<LoopMGR>().setTracks(loops);
	}

	public void playAllLoops(){
		GetComponentInParent<LoopMGR>().playAllLoops(loops);
	}

	public void stopAllLoops(){
		GetComponentInParent<LoopMGR>().stopAllLoops();
		
	}
}
