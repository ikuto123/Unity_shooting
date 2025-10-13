using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleMnager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return)){
            SceneManager.LoadScene("SampleScene");
        }
    }
}
