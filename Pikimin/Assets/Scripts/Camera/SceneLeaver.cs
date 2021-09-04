using UnityEngine;
using UnityEngine.SceneManagement;
using static Audio.AudioManager;

namespace Camera
{
    public class SceneLeaver : MonoBehaviour
    {
        void Update()
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
