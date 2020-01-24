using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoopMGR : MonoBehaviour {

	public GameObject follow;
	//public AudioClip baseLoop;
	public LoopTrack[] loopLayers;
	AudioSource loopBase;
	AudioSource[] waitingLayers;
	AudioSource[] playingLayers;
	//public AudioSource[] playingLayersArray;
	static bool created;

	[HideInInspector]
	public bool inGameplay = false;
	bool initializing;


	//	Public Function
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	//	setTracks is called by a MusicArea when the music area is activated
	//	calling set tracks queues the new tracks to be added in the next time loopbase reaches its end
	public void setTracks(LoopTrack[] area){
		for (int i  = 0; i < area.Length; ++i){
			waitingLayers[i] = createAudioSource(area[i], "AudioTrack from Set");
		}
	}

	//	Functions for the inspector
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	//	The following functions provide the functionality for
	//	the buttons that appear in the inspector when the 
	//	LoopMGREditor script is included in the Editor folder of the project

	//PLAY~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	[HideInInspector]
	public List<AudioSource> editorAudio = new List<AudioSource>();
	public void playAllLoops(){
		if (inGameplay) return;
		transform.position = follow.transform.position;
		if (editorAudio.Count != 0) return;
		initializeEditorAudio();
		foreach(AudioSource aud in editorAudio){
			if (aud) aud.Play();
		}
	}
	
	public void playAllLoops(LoopTrack[] loops){
		if (inGameplay) return;
		transform.position = follow.transform.position;
		if (editorAudio.Count != 0) return;
		initializeEditorAudio(loops);
		foreach(AudioSource aud in editorAudio){
			if (aud) aud.Play();
		}
	}
	
	void initializeEditorAudio(){
		editorAudio.Clear();
		foreach (LoopTrack track in loopLayers){
			AudioSource aud = createAudioSource(track, "Editor Audio");
			editorAudio.Add(aud);
		}
	}
	
	public void initializeEditorAudio(LoopTrack[] loops){
		editorAudio.Clear();
		foreach (LoopTrack track in loops){
			AudioSource aud = createAudioSource(track, "Editor Audio");
			print ("Adding " + track.clip.name);
			editorAudio.Add(aud);
		}
	}


	//STOP~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void stopAllLoops(){
		clearAllLoops();
	}
	
	public void clearAllLoops(){
		foreach(AudioSource aud in editorAudio){
			if (aud) DestroyImmediate(aud.gameObject);
		}
		editorAudio.Clear();
	}


	//CREATE MUSIC AREA~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void createMusicArea(){
		GameObject go = new GameObject();
		go.transform.parent = transform;
		MusicArea.CreateMusicArea(go, loopLayers);
		go.transform.position = transform.position;
		go.transform.rotation = Quaternion.Euler(0,0,0);
	}

	//CREATE MUSIC TRIGGER~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void createTrigger(){
		GameObject go = new GameObject();
		go.transform.parent = transform;
		MusicTrigger.createMusicTrigger(go);
	}


	//	Private stuff to manage loops
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


	void Awake(){
		inGameplay = true;
		clearAllLoops();
	}

	void Start(){
		setupLoopMGR();

		//	Play the base loop
		loopBase = createAudioSource(loopLayers[0]);
		waitingLayers[0] = loopBase;
		for (int i = 1; i < loopLayers.Length; ++i){
			waitingLayers[i] = createAudioSource(loopLayers[i]);
		}
	}

	void setupLoopMGR(){
		//~~~~~~~~~~~~~~~~~~~~~~~
		//	If you want the manager to be persistent across scene
		//	resets and loads, uncomment the following region:

//		if (created) Destroy(gameObject);
//		created = true;
//		DontDestroyOnLoad(gameObject);

		//~~~~~~~~~~~~~~~~~~~~~~~~~~

		
		//	Initialize the waiting and playing layer arrays
		waitingLayers = new AudioSource[loopLayers.Length];
		playingLayers = new AudioSource[loopLayers.Length];

		initializing = true;
	}


	void Update(){
		if (initializing) playWaitingLayers();
		initializing = false;
		if (follow) transform.position = follow.transform.position;
		if (shouldReplayTracks()) playWaitingLayers();

	}

	void toggleSource(int i){
		if (playingLayers[i] != null) { //	Turn off track
			removeSource(i);
			return;
		}
		if (waitingLayers[i] != null){ //	Remove track from waiting
			Destroy(waitingLayers[i].gameObject);
			waitingLayers[i] = null;
			return;
		}
		//	Otherwise, turn on track
		addSource(i);
	}

	void pitchTracks(){
		AudioSource[] playingClips = GetComponentsInChildren<AudioSource>();
		foreach (AudioSource clip in playingClips){
			clip.pitch = Time.timeScale;
		}
	}
	
	void addSource(int i){
		AudioSource aud = createAudioSource(loopLayers[i]);
		waitingLayers[i] = aud;
		if (!layersArePlaying()){
			loopBase = aud;
			playWaitingLayers();
		}
	}
	
	void removeSource(int i){
		if (loopBase == playingLayers[i]) setNewLoopBase();
		Destroy(playingLayers[i].gameObject);
		playingLayers[i] = null;
		if (waitingLayers.Length != 0){
			//	Play all of the waiting layers
			playWaitingLayers();
			return;
		}
		//Destroy(playingLayers[i].gameObject);
	}

	void setNewLoopBase(){
		for (int i = 0; i < playingLayers.Length; ++i){
			if (playingLayers[i] == loopBase) continue;
			if (playingLayers[i] == null) continue;
			loopBase = playingLayers[i];
			return;
		}
		loopBase = null;
	}
	
	bool layersArePlaying(){
		for (int i = 0; i < playingLayers.Length; ++i){
			if (playingLayers[i] != null) return true;
		}
		return false;
	}

	AudioSource createAudioSource(LoopTrack track){
		return createAudioSource(track, "Audio Track");
	}

	AudioSource createAudioSource(LoopTrack track, string name){
		GameObject go = new GameObject();
		go.transform.parent = transform;
		go.AddComponent<AudioSource>();
		go.transform.position = transform.position;
		go.transform.rotation = Quaternion.Euler(0,0,0);
		go.name = name;
		AudioSource aud = go.GetComponent<AudioSource>();
		aud.loop = true;
		aud.clip = track.clip;
		aud.volume = track.volume;
		return aud;
	}

	static bool waitingForLoopEnd;
	bool waitingForLoopEndIns;
	bool inFirstHalfIns;
	bool shouldReplayTracks(){

		if (!loopBase){
			print ("No loopbase; not playing Waiting");
			return false;
		}
		bool inFirstHalf = loopBase.timeSamples < (loopBase.clip.samples / 2);
		inFirstHalfIns = inFirstHalf;
		if (waitingForLoopEnd && inFirstHalf){
			waitingForLoopEnd = false;
			waitingForLoopEndIns = waitingForLoopEnd;
			return true;
		} 
		if (waitingForLoopEnd && !inFirstHalf) return false;
		if (!waitingForLoopEnd && !inFirstHalf){
			waitingForLoopEnd = true;
			waitingForLoopEndIns = waitingForLoopEnd;
			return false;
		}
		return false;
	}

	
	void playWaitingLayers(){
		print("playing Waiting");
		//if (loopBase == null) return;
		print ("Waiting layers length: " + waitingLayers.Length);
		for (int i = 0; i < waitingLayers.Length; ++i){
			//	if the waiting layer is null, move on
			AudioSource aud = waitingLayers[i];
			if (aud == null) continue;

			//	If it's not, and if there is a playing layer, destroy the playing layer
			print ("Playing layer at position " + i + " is " + playingLayers[i]);
			if (playingLayers[i]){
				if (playingLayers[i] == loopBase) loopBase = waitingLayers[i];
				Destroy(playingLayers[i].gameObject);

			}

			//	Play the layer and update lists
			aud.Play();
			playingLayers[i] = aud;
			waitingLayers[i] = null;
		}
	}






}
