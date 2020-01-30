using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Player_Sound
{
public class Player_Sound
{
    // Running Sounds
    public AudioClip runSound;
    public AudioClip mudRunSound;

    // Jump Sounds
    public AudioClip groundJumpSound;
    public AudioClip wallJumpSound;

    // Enemies
    public AudioClip knockbackSound;
    
    // Sound of abilities
    public AudioClip dashSound;
    public AudioClip doubleJumpSound;

    public void RunningSound(GameObject player)
    {
        AudioSource.PlayClipAtPoint(runSound,player.transform.position,2.0f);
    }

    public void JumpSound(GameObject player)
    {
        AudioSource.PlayClipAtPoint(groundJumpSound,player.transform.position,2.0f);
    }

    public void MudRunSound(GameObject player)
    {
        AudioSource.PlayClipAtPoint(mudRunSound,player.transform.position,2.0f);
    }

    public void WallJumpSound(GameObject player)
    {
        AudioSource.PlayClipAtPoint(wallJumpSound,player.transform.position,2.0f);
    }

    public void KnockbackSound(GameObject player)
    {
        AudioSource.PlayClipAtPoint(knockbackSound,player.transform.position,2.0f);
    }

    public void DashSound(GameObject player)
    {
        AudioSource.PlayClipAtPoint(dashSound,player.transform.position,2.0f);
    }

    public void DoubleJumpSound(GameObject player)
    {
        AudioSource.PlayClipAtPoint(runSound,player.transform.position,2.0f);
    }
}
}