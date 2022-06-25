using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

	public AudioClip WorldsMusic;
	public AudioClip FightMusic;
	public AudioSource Source;
	private void Awake()
	{
		AppController.Music = this;
	}

	private void OnEnable()
	{
		Source.clip = WorldsMusic;
	}

	public void SwitchToFightMusic()
	{
		Source.clip = FightMusic;
	}

	public void SwitchToWorldMusic()
	{
		Source.clip = WorldsMusic;
	}
}
