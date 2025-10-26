using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverText : MonoBehaviour
{
    private Outline _outline;
    
    [SerializeField] private SoundData _mouseOverSE;
    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnPointerEnter()
    {
        Debug.Log("OnPointerEnter");
        SoundManager.Instance.PlaySE_2D(_mouseOverSE);
        _outline.enabled = true;
    }
    
    public void OnPointerExit()
    {
        _outline.enabled = false;
    }
    
}
