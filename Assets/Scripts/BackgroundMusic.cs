using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
	public AudioSource WorldMusic;
	public AudioSource FightMusic;
	private void Awake()
	{
		AppController.Music = this;
	}

	private void OnEnable()
	{
		WorldMusic.Play();
	}

	public void SwitchToFightMusic()
	{
		WorldMusic.Pause();
		FightMusic.Play();
	}

	public void SwitchToWorldMusic()
	{
		FightMusic.Pause();
		WorldMusic.Play();
	}
}
