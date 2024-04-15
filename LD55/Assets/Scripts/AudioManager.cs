using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> m_fishKillAudioClips = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> m_pickupScoreAudioClips = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> m_pickupHealthAudioClips = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> m_pickupCoinsAudioClips = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> m_playerDamagedAudioClips = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> m_slimeAttackAudioClips = new List<AudioClip>();

    [SerializeField]
    private AudioClip m_musicClip;
    [SerializeField]
    private AudioClip m_winClip;
    [SerializeField]
    private AudioClip m_loseClip;
    [SerializeField]
    private AudioClip m_activeSuccessClip;
    [SerializeField]
    private AudioClip m_activeFailClip;

    [SerializeField]
    private AudioSource m_ostSource;
    [SerializeField]
    private AudioSource m_playerDamageSource;
    [SerializeField]
    private AudioSource m_gameOverSource;
    [SerializeField]
    private AudioSource m_activeAbilitySource;

    [SerializeField]
    private List<AudioSource> m_pickupSources;
    [SerializeField]
    private List<AudioSource> m_slimeAttackSources;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = 0.5f;

        m_ostSource.loop = true;
        m_ostSource.clip = m_musicClip;
        m_ostSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayerDamage()
    {
        m_playerDamageSource.clip = m_playerDamagedAudioClips[Random.Range(0, m_playerDamagedAudioClips.Count)];

        m_playerDamageSource.Stop();
        m_playerDamageSource.Play();
    }

    public void LoseGame()
    {
        StopAudio();

        m_gameOverSource.clip = m_loseClip;
        m_gameOverSource.Play();
    }

    public void WinGame()
    {
        StopAudio();

        m_gameOverSource.clip = m_winClip;
        m_gameOverSource.Play();
    }

    public void SlimeKill()
    {
        int index = Random.Range(0, m_fishKillAudioClips.Count);
        PlayFromFreeSource(m_slimeAttackSources, m_fishKillAudioClips[index]);
    }

    public void SlimeAttack()
    {
        int index = Random.Range(0, m_slimeAttackAudioClips.Count);
        PlayFromFreeSource(m_slimeAttackSources, m_slimeAttackAudioClips[index]);
    }

    public void ActiveAbilitySuccess()
    {
        m_activeAbilitySource.clip = m_activeSuccessClip;
        m_activeAbilitySource.Stop();   
        m_activeAbilitySource.Play();
    }

    public void ActiveAbilityFail()
    {
        m_activeAbilitySource.clip = m_activeFailClip;
        m_activeAbilitySource.Stop();
        m_activeAbilitySource.Play();
    }

    public void PickupScore()
    {
        int index = Random.Range(0, m_pickupScoreAudioClips.Count);
        PlayFromFreeSource(m_pickupSources, m_pickupScoreAudioClips[index]);
    }

    public void PickupHealth()
    {
        int index = Random.Range(0, m_pickupHealthAudioClips.Count);
        PlayFromFreeSource(m_pickupSources, m_pickupHealthAudioClips[index]);
    }

    public void PickupCoin()
    {
        int index = Random.Range(0, m_pickupCoinsAudioClips.Count);
        PlayFromFreeSource(m_pickupSources, m_pickupCoinsAudioClips[index]);
    }


    private void StopAudio()
    {
        m_ostSource.Stop();
        m_gameOverSource.Stop();
    }

    private void PlayFromFreeSource(List<AudioSource> _sources, AudioClip _clip)
    {
        foreach(var source in _sources)
        {
            if(!source.isPlaying)
            {
                source.clip = _clip;
                source.Play();
            }
        }
    }

}
