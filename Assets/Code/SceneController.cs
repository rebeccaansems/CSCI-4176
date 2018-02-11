using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//logic for changing scences on button press

public class SceneController : MonoBehaviour
{
    public void SceneLoad(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}