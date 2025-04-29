using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	[Header("Properties")]
	[SerializeField, Range(1, 100)] private int minimumPooledSFXSource = 10;
	[SerializeField, Range(1, 100)] private int minimumPooledInterfaceSource = 3;
	[SerializeField, Range(1, 100)] private int minimumPooledMusicSource = 1;

	[Header("Mixer Groups")]
	[SerializeField] AudioMixerGroup masterAMG;
	[SerializeField] AudioMixerGroup sfxAMG;
	[SerializeField] AudioMixerGroup interfaceAMG;
	[SerializeField] AudioMixerGroup musicAMG;

	[Header("Technical")]
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioSource globalAudioSource;
	[SerializeField] private GameObject sourceInstanceSFXPrefab;
	[SerializeField] private GameObject sourceInstanceInterfacePrefab;
	[SerializeField] private GameObject sourceInstanceMusicPrefab;

	// Source instances parent
	private Transform _sourceInstanceParent;

	// Active
	private List<AudioSourceInstance> _activePoolSFX = new();
	private List<AudioSourceInstance> _activePoolInterface = new();
	private List<AudioSourceInstance> _activePoolMusic = new();
	
	// Inactive
	private List<AudioSourceInstance> _inactivePoolSFX = new();
	private List<AudioSourceInstance> _inactivePoolInterface = new();
	private List<AudioSourceInstance> _inactivePoolMusic = new();




	public AudioEntry test;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		InitializePools();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			Play(test, new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f)));
			//PlayOneShot(test, 0.03f);
			MixerGroup m = MixerGroup.Master;
			Debug.Log("test :\n" + m.ToString() + "Volume");
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			SetVolume(MixerGroup.Master, 0.8f);
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			SetVolume(MixerGroup.Master, 0.3f);
		}
	}

	void InitializePools()
	{
		GameObject obj = new("AudioSourceInstancesParent");
		_sourceInstanceParent = obj.transform;
		_sourceInstanceParent.parent = transform;

		for (int i = 0; i < minimumPooledSFXSource; i++)
			AddSourceInstance(MixerGroup.SFX);

		for (int i = 0; i < minimumPooledInterfaceSource; i++)
			AddSourceInstance(MixerGroup.Interface);

		for (int i = 0; i < minimumPooledMusicSource; i++)
			AddSourceInstance(MixerGroup.Music);
	}

	void AddSourceInstance(MixerGroup group)
	{
		GameObject obj;
		AudioSourceInstance asi;

		switch (group)
		{
			case MixerGroup.Master:
				Debug.LogWarning("Master doesn't have pooled audio sources !");
				return;
			case MixerGroup.SFX:
				obj = Instantiate(sourceInstanceSFXPrefab, _sourceInstanceParent);
				asi = obj.GetComponent<AudioSourceInstance>();
				asi.SetMixerGroup(group);
				_inactivePoolSFX.Add(asi);
				asi.gameObject.SetActive(false);
				return;
			case MixerGroup.Interface:
				obj = Instantiate(sourceInstanceInterfacePrefab, _sourceInstanceParent);
				asi = obj.GetComponent<AudioSourceInstance>();
				asi.SetMixerGroup(group);
				_inactivePoolInterface.Add(asi);
				asi.gameObject.SetActive(false);
				return;
			case MixerGroup.Music:
				obj = Instantiate(sourceInstanceMusicPrefab, _sourceInstanceParent);
				asi = obj.GetComponent<AudioSourceInstance>();
				asi.SetMixerGroup(group);
				_inactivePoolMusic.Add(asi);
				asi.gameObject.SetActive(false);
				return;
		}
	}

	public AudioMixerGroup GetMixerGroup(MixerGroup group)
	{
		switch (group)
		{
			case MixerGroup.Master:
				return masterAMG;
			case MixerGroup.SFX:
				return sfxAMG;
			case MixerGroup.Interface:
				return interfaceAMG;
			case MixerGroup.Music:
				return musicAMG;
			default:
				return masterAMG;
		}
	}


	public void ReturnASIToPool(AudioSourceInstance asi)
	{
		switch (asi.MixerGroup)
		{
			case MixerGroup.Master:
				Debug.LogWarning("There aren't any object in the master pool !\nThis Object will be destroyed !");
				Destroy(asi.gameObject);
				return;
			case MixerGroup.SFX:
				asi.gameObject.SetActive(false);
				_inactivePoolSFX.Add(asi);
				_activePoolSFX.Remove(asi);
				return;
			case MixerGroup.Interface:
				asi.gameObject.SetActive(false);
				_inactivePoolInterface.Add(asi);
				_activePoolInterface.Remove(asi);
				return;
			case MixerGroup.Music:
				asi.gameObject.SetActive(false);
				_inactivePoolMusic.Add(asi);
				_activePoolMusic.Remove(asi);
				return;
			default:
				return;
		}
	}

	public void Play(AudioEntry entry)
	{
		Play(entry, Vector3.zero);
	}

	public void Play(AudioEntry entry, Vector3 position)
	{
		AudioSourceInstance asi;

		switch (entry.group)
		{
			case MixerGroup.Master:
				Debug.LogWarning("You cannot play audio directly on the master");
				return;
			case MixerGroup.SFX:
				if (_inactivePoolSFX.Count == 0)
					AddSourceInstance(MixerGroup.SFX);
				asi = _inactivePoolSFX[0];
				_inactivePoolSFX.Remove(asi);
				_activePoolSFX.Add(asi);
				asi.gameObject.SetActive(true);
				asi.PlayAudio(entry, position);
				return;
			case MixerGroup.Interface:
				if (_inactivePoolInterface.Count == 0)
					AddSourceInstance(MixerGroup.Interface);
				asi = _inactivePoolInterface[0];
				_inactivePoolInterface.Remove(asi);
				_activePoolInterface.Add(asi);
				asi.gameObject.SetActive(true);
				asi.PlayAudio(entry, position);
				return;
			case MixerGroup.Music:
				if (_inactivePoolMusic.Count == 0)
					AddSourceInstance(MixerGroup.Music);
				asi = _inactivePoolMusic[0];
				_inactivePoolMusic.Remove(asi);
				_activePoolMusic.Add(asi);
				asi.gameObject.SetActive(true);
				asi.PlayAudio(entry, position);
				return;
		}
	}




	void SetGlobalAudioSourceParams(AudioEntry entry)
	{
		globalAudioSource.pitch = entry.pitch;
		globalAudioSource.volume = entry.volume;
		globalAudioSource.bypassEffects = entry.bypassEffects;
		globalAudioSource.bypassEffects = entry.bypassListenerEffects;
		globalAudioSource.loop = entry.loop;
	}

	public void SetVolume(MixerGroup group, float volume)
	{
		audioMixer.SetFloat(group.ToString() + "Volume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
	}

	public void PlayOneShot(AudioEntry entry)
	{
		SetGlobalAudioSourceParams(entry);
		globalAudioSource.PlayOneShot(entry.audioClip);
	}

	public void PlayOneShot(AudioEntry entry, float randomPitchDelta)
	{
		SetGlobalAudioSourceParams(entry);
		globalAudioSource.pitch = entry.pitch + Random.Range(-randomPitchDelta, randomPitchDelta);
		globalAudioSource.PlayOneShot(entry.audioClip);
	}

	public enum MixerGroup
	{
		Master,
		SFX,
		Interface,
		Music,
	}
}
