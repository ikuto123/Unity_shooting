using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaControl : MonoBehaviour
{
    private float _timeToWin;
    public float TimeToWin => _timeToWin;

    public event Action<float, float> OnProgressUpdated; 
    public event Action<Team?> OnGameEnd; 

    private float _teamACaptureProgress = 0f;
    private float _teamBCaptureProgress = 0f;

    private List<CharactorTeam> _charactersInArea = new List<CharactorTeam>();
    private bool _isGameFinished = false;
    
    private float _progressUpdateTimer = 0f;
    private const float UPDATE_INTERVAL = 3.0f; 

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            _timeToWin = GameManager.Instance.AreaTimeToWin;
        }
        else
        {
            Debug.LogError("GameManagerが見つかりません！デフォルトの勝利時間を使用します。");
            _timeToWin = 10.0f; // フォールバック値
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CharactorTeam character = other.GetComponent<CharactorTeam>();
        if (character != null && !_charactersInArea.Contains(character))
        {
            _charactersInArea.Add(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharactorTeam character = other.GetComponent<CharactorTeam>();
        if (character != null)
        {
            _charactersInArea.Remove(character);
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
        int teamACount = _charactersInArea.Count(c => c.Team == Team.A);
        int teamBCount = _charactersInArea.Count(c => c.Team == Team.B);

        bool isProgressChanged = false;

      
        _progressUpdateTimer += Time.deltaTime;

       
        if (_progressUpdateTimer >= UPDATE_INTERVAL)
        {
            _progressUpdateTimer -= UPDATE_INTERVAL;

            if (teamACount > teamBCount) 
            {
                _teamACaptureProgress = Mathf.Min(_teamACaptureProgress + 1, _timeToWin);
                
                isProgressChanged = true;
            }
            else if (teamBCount > teamACount) 
            {
                _teamBCaptureProgress = Mathf.Min(_teamBCaptureProgress + 1, _timeToWin);
                
                isProgressChanged = true;
            }
        }

        // プログレスに変更があった場合のみイベントを発行
        if (isProgressChanged)
        {
            OnProgressUpdated?.Invoke(_teamACaptureProgress, _teamBCaptureProgress);
            // デバッグログ更新
            Debug.Log($"エリア状況: Team A [{_teamACaptureProgress}/{_timeToWin}] vs Team B [{_teamBCaptureProgress}/{_timeToWin}]");
        }
    }

    private void CheckForWin()
    {
        if (_teamACaptureProgress >= _timeToWin)
        {
            AnnounceWinner(Team.A);
        }
        else if (_teamBCaptureProgress >= _timeToWin)
        {
            AnnounceWinner(Team.B);
        }
      
    }

    private void AnnounceWinner(Team winningTeam)
    {
        if (_isGameFinished) return; // 既に終了していたら何もしない
        _isGameFinished = true;
        OnGameEnd?.Invoke(winningTeam);
        Debug.Log($"<color=yellow>決着！ {winningTeam} の勝利！</color>");
    }
    
    public Team? GetLeadingTeamOrDraw()
    {
        // 進捗が大きい方を返す。同点なら null（引き分け）
        if (_teamACaptureProgress > _teamBCaptureProgress) return Team.A;
        if (_teamBCaptureProgress > _teamACaptureProgress) return Team.B;
        return null;
    }

// 時間切れによる強制終了（勝者は現在進捗から判定）
    public void ForceEndByTimeUp()
    {
        if (_isGameFinished) return; // 二重終了防止
        _isGameFinished = true;

        Team? winner = GetLeadingTeamOrDraw();
        OnGameEnd?.Invoke(winner);   // Presenter/UIへ通知
        Debug.Log(winner == null 
            ? "<color=yellow>時間切れ：引き分け</color>"
            : $"<color=yellow>時間切れ：{winner} の勝利！</color>");
    }
}