using TMPro;
using UnityEngine; 
using UnityEngine.UI;

public class ResultPresenter
{
    private readonly AreaControl _areaControl;
    private readonly Image _playerWinPanel; 
    private readonly Image _playerLosePanel; 
    private readonly Team _playerTeam;     
    private readonly TextMeshProUGUI _drawText;
    
    public ResultPresenter(AreaControl areaControl, Image playerWinPanel, Image playerLosePanel, Team playerTeam , TextMeshProUGUI drawText)
    {
        _areaControl = areaControl;
        _playerWinPanel = playerWinPanel;
        _playerLosePanel = playerLosePanel;
        _playerTeam = playerTeam;
    }

    public void Enable()
    {
        
        _areaControl.OnGameEnd += ShowResult;
        _playerWinPanel.gameObject.SetActive(false);
        _playerLosePanel.gameObject.SetActive(false);
    }

    public void Disable()
    {
        _areaControl.OnGameEnd -= ShowResult;
    }

   
    private void ShowResult(Team? winningTeam) 
    {
        // まず全て消す
        if (_playerWinPanel) _playerWinPanel.gameObject.SetActive(false);
        if (_playerLosePanel) _playerLosePanel.gameObject.SetActive(false);
        if (_drawText) _drawText.gameObject.SetActive(false);

        if (winningTeam == null)
        {
            // ★ 引き分け
            if (_drawText)
            {
                _drawText.text = "引き分け";
                _drawText.gameObject.SetActive(true);
            }
            Debug.Log("Game ended in a Draw!");
            return;
        }

        // 勝敗
        if (winningTeam == _playerTeam)
        {
            if (_playerWinPanel) _playerWinPanel.gameObject.SetActive(true);
            Debug.Log("Player Wins!");
        }
        else
        {
            if (_playerLosePanel) _playerLosePanel.gameObject.SetActive(true);
            Debug.Log("Player Loses!");
        }
    }
}