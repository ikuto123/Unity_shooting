using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameScene;

public class AreaControl : MonoBehaviour
{
    private float _timeToWin;
    public float TimeToWin => _timeToWin;

    public event Action<float, float> OnProgressUpdated; 
    public event Action<Team?> OnGameEnd; 

    private float _teamAProgress = 0f;
    private float _teamBProgress = 0f;

    private List<CharactorTeam> _charactersInAreas = new List<CharactorTeam>();
    private bool _isGameFinished = false;
    
    private float _progressTimer = 0f;
    private const float UPDATE_INTERVAL = 3.0f; 

    private void Awake()
    {
        if (GameManager.Instance != null) { _timeToWin = GameManager.Instance.AreaTimeToWin; }
        else { Debug.LogError("GameManagerが見つかりません"); }
    }

    //エリアに入ったキャラクターをリストに追加
    private void OnTriggerEnter(Collider other)
    {
        CharactorTeam character = other.GetComponent<CharactorTeam>();
        if (character != null && !_charactersInAreas.Contains(character))
        {
            _charactersInAreas.Add(character);
        }
    }
    
　　//エリアから出たキャラクターをリストから削除
    private void OnTriggerExit(Collider other)
    {
        CharactorTeam character = other.GetComponent<CharactorTeam>();
        if (character != null)
        {
            _charactersInAreas.Remove(character);
        }
    }

    void Update()
    {
        if (_isGameFinished) return;
        UpdateCaptureProgress();
        CheckForWin();
    }

    private void UpdateCaptureProgress()
    {
        int teamACount = _charactersInAreas.Count(c => c.Team == Team.A);
        int teamBCount = _charactersInAreas.Count(c => c.Team == Team.B);

        bool isProgressChanged = false;
        
        _progressTimer += Time.deltaTime;
        
        //一定時間が立った際にチームが多い方のカウントを増やす
        if (_progressTimer >= UPDATE_INTERVAL)
        {
            _progressTimer -= UPDATE_INTERVAL;

            if (teamACount > teamBCount) 
            {
                _teamAProgress = Mathf.Min(_teamAProgress + 1, _timeToWin);
                
                isProgressChanged = true;
            }
            else if (teamBCount > teamACount) 
            {
                _teamBProgress = Mathf.Min(_teamBProgress + 1, _timeToWin);
                
                isProgressChanged = true;
            }
        }

        //ログレスに変更があった場合のみイベントを発行
        if (isProgressChanged) { OnProgressUpdated?.Invoke(_teamAProgress, _teamBProgress); }
    }

    //規定値に達したかどうか
    private void CheckForWin()
    {
        if (_teamAProgress >= _timeToWin)
        {
            AnnounceWinner(Team.A);
        }
        else if (_teamBProgress >= _timeToWin)
        {
            AnnounceWinner(Team.B);
        }
      
    }

    //勝利チームの発表    
    private void AnnounceWinner(Team winningTeam)
    {
        if (_isGameFinished) return; 
        _isGameFinished = true;
        OnGameEnd?.Invoke(winningTeam);
    }
    
    //引き分けの処理
    public Team? GetLeadingTeamOrDraw()
    {
        //進捗が大きい方を返す
        if (_teamAProgress > _teamBProgress) return Team.A;
        if (_teamBProgress > _teamAProgress) return Team.B;
        return null;
    }

    //時間切れによる強制終了
    public void ForceEndByTimeUp()
    {
        if (_isGameFinished) return; 
        _isGameFinished = true;

        Team? winner = GetLeadingTeamOrDraw();
        OnGameEnd?.Invoke(winner);  
    }
}