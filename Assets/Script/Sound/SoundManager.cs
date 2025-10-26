using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [Header("2DAudioSource")]
    [SerializeField] private AudioSource _Source_2D;
    
    private Queue<AudioSource> _audioSourcePool = new Queue<AudioSource>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_Source_2D == null)
        {
            _Source_2D = gameObject.AddComponent<AudioSource>();
            _Source_2D.playOnAwake = false;
        }

        
        InitializePool();
    }
    
    private void InitializePool()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            go.SetActive(false); 
            AudioSource source = go.GetComponent<AudioSource>();
            _audioSourcePool.Enqueue(source); 
        }
    }
    
    //どこからでも同じように聞こえるサウンド
    public void PlaySE_2D(SoundData soundData)
    {
        if (soundData == null || soundData.clip == null) return;
        
        _Source_2D.PlayOneShot(soundData.clip, soundData.volume);
    }
    
    //位置によって聞こえ方が変わるサウンド
    public void PlaySE_3D(SoundData soundData, Vector3 position, float pitchRange = 0.05f)
    {
        if (soundData == null || soundData.clip == null) return;

        //プールからオーディオソースを取得
        AudioSource source = GetAudioSource();
        if (source == null)
        { 
            Debug.LogWarning("3Dオーディオソースのプールが枯渇しました。PoolSizeを大きくしてください。");
            return;
        }
        
        Play3DSound(source, soundData, position, pitchRange).Forget();
    }
    private AudioSource GetAudioSource()
    {
        if (_audioSourcePool.Count > 0)
        {
            return _audioSourcePool.Dequeue(); 
        }
        return null;
    }
    
    //サウンドの再生と終了処理
    private async UniTask Play3DSound(AudioSource source, SoundData soundData, Vector3 position, float pitchRange)
    {
        source.transform.position = position;
        source.pitch = soundData.pitch + Random.Range(-pitchRange, pitchRange);
        source.clip = soundData.clip;
        source.volume = soundData.volume;
        source.spatialBlend = 1.0f; 
        source.minDistance = 3f;  
        source.maxDistance = 300f;  
        source.dopplerLevel = 0f; 
        source.gameObject.SetActive(true);
        source.Play();

        await UniTask.Delay((int)(soundData.clip.length * 1000));

        source.Stop();
        source.gameObject.SetActive(false);
        _audioSourcePool.Enqueue(source); 
    }
}
