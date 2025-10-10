using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaControl : MonoBehaviour
{
    private float timeToWin = 10.0f;
    
    private float teamACaptureProgress = 0f;
    private float teamBCaptureProgress = 0f;
    
    private List<CharactorTeam> charactersInArea = new List<CharactorTeam>();
    
    private bool isGameFinished = false;
    
    private float progressUpdateTimer = 0f;
    
    private void OnTriggerEnter(Collider other)
    {
        CharactorTeam character = other.GetComponent<CharactorTeam>();
        if (character != null)
        {
            if (!charactersInArea.Contains(character))
            {
                charactersInArea.Add(character);
                Debug.Log($"{character.name} (Team: {character.Team}) がエリアに入りました。");
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        CharactorTeam character = other.GetComponent<CharactorTeam>();
        if (character != null)
        {
            charactersInArea.Remove(character);
            Debug.Log($"{character.name} (Team: {character.Team}) がエリアから出ました。");
        }
    }
    
    void Update()
    {
        if (isGameFinished) return;
        UpdateCaptureProgress();
        CheckForWin();
    }
    
    private void UpdateCaptureProgress()
    {
        //エリアにいるチームの数を数える
        int teamACount = charactersInArea.Count(c => c.Team == Team.A);
        int teamBCount = charactersInArea.Count(c => c.Team == Team.B);
        
        if (teamACount > teamBCount)
        {
            progressUpdateTimer += Time.deltaTime;
            
            if (progressUpdateTimer >= 1.0f)
            {
                progressUpdateTimer -= 1.0f; 
                teamACaptureProgress += 1;
                teamBCaptureProgress = 0;
                Debug.Log($"Team A 占領中... Progress: {teamACaptureProgress} / {timeToWin}");
            }
        }
        else if (teamBCount > teamACount)
        {
            progressUpdateTimer += Time.deltaTime;
            
            if (progressUpdateTimer >= 1.0f)
            {
                progressUpdateTimer -= 1.0f;
                teamBCaptureProgress += 1;
                teamACaptureProgress = 0;
                Debug.Log($"Team B 占領中... Progress: {teamBCaptureProgress} / {timeToWin}");
            }
        }
        else
        {
            teamACaptureProgress = 0;
            teamBCaptureProgress = 0;
            progressUpdateTimer = 0f;
        }
    }
    private void CheckForWin()
    {
        if (teamACaptureProgress >= timeToWin)
        {
            AnnounceWinner(Team.A);
        }
        else if (teamBCaptureProgress >= timeToWin)
        {
            AnnounceWinner(Team.B);
        }
    }

    //勝利した時に呼び出される
    private void AnnounceWinner(Team winningTeam)
    {
        isGameFinished = true;
        Debug.Log($"決着！ {winningTeam} の勝利！");
    }
}
