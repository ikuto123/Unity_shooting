using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverText : MonoBehaviour
{
    private Outline _outline;
    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnPointerEnter()
    {
        Debug.Log("OnPointerEnter");
        _outline.enabled = true;
    }
    
    public void OnPointerExit()
    {
        _outline.enabled = false;
    }
    
}
