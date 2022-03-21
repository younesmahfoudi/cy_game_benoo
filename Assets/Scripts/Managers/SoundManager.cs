using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public enum SoundPriority { COMMON, NORMAL, IMPORTANT };
	public List<AudioSource> _effectsSourcesCommon;
	public List<AudioSource> _effectsSourcesNormal;
	public List<AudioSource> _effectsSourcesImportant;
	public AudioSource MusicSource;
	private float MusicVolume = 1;
	private float EffectsVolume = 1;

	// Singleton instance.
	public static SoundManager Instance = null;

	// Initialize the singleton instance.
	private void Awake()
	{
		if (Instance == null)
		{
			MusicVolume = 1;
			EffectsVolume = 1;
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	public void Play(AudioClip clip, SoundPriority sp)
	{
		switch(sp)
        {
			case SoundPriority.COMMON:
				PlaySoundInList(clip, _effectsSourcesCommon);
				break;
			case SoundPriority.NORMAL:
				PlaySoundInList(clip, _effectsSourcesNormal);
				break;
			case SoundPriority.IMPORTANT:
				PlaySoundInList(clip, _effectsSourcesImportant);
				break;
		}
	}

	private void PlaySoundInList(AudioClip clip, List<AudioSource> asList)
    {
		foreach (AudioSource audiosource in asList)
		{
			if (!audiosource.isPlaying)
			{
				audiosource.clip = clip;
				audiosource.Play();
				return;
			}
		}
		AudioSource lastSoundInDate = asList[0];
		asList.RemoveAt(0);
		asList.Insert(asList.Count, lastSoundInDate);
		asList[asList.Count - 1].Play();
	}

	public void PlayMusic(AudioClip clip)
	{
		MusicSource.clip = clip;
		MusicSource.Play();
	}

	public void UpdateMusicVolume(float changeNbr)
    {
		MusicVolume += changeNbr;
		if (MusicVolume < 0)
			MusicVolume = 0;
		if (MusicVolume > 1)
			MusicVolume = 1;
		MusicSource.volume = MusicVolume;
    }
	public void UpdateEffectsVolume(float changeNbr)
	{
		
		EffectsVolume += changeNbr;
		if (EffectsVolume < 0)
			EffectsVolume = 0;
		if (EffectsVolume > 1)
			EffectsVolume = 1;
		UpdateEffectsVolumeInList(EffectsVolume, _effectsSourcesCommon);
		UpdateEffectsVolumeInList(EffectsVolume, _effectsSourcesNormal);
		UpdateEffectsVolumeInList(EffectsVolume, _effectsSourcesImportant);
	}

	private void UpdateEffectsVolumeInList(float volume, List<AudioSource> asList)
    {
		foreach (AudioSource audiosource in asList)
			audiosource.volume = volume;
    }

	public int getMusicSoundPercent()
    {
        return Mathf.RoundToInt(MusicVolume * 100);
    }

	public int getEffectsSoundPercent()
	{
		return Mathf.RoundToInt(EffectsVolume * 100);
	}
}