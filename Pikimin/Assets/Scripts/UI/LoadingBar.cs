using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField] private Slider loadingBar;
        [SerializeField] private float loadingSpeed;
        [SerializeField] private float waitToStartLoadBar;

        [SerializeField] private bool initializedLoad;
        // Start is called before the first frame update
        void Start()
        {
            loadingBar.minValue = 0;
            loadingBar.maxValue = 1;
            loadingBar.value = 0;
            waitToStartLoadBar = .5f;
            initializedLoad = false;
        }

        // Update is called once per frame
        void Update()
        {
            waitToStartLoadBar -= Time.deltaTime;
            if (waitToStartLoadBar < 0)
            {
                loadingBar.value += loadingSpeed * Time.deltaTime;
                if (Math.Abs(loadingBar.value - 1) < .09f && !initializedLoad)
                {
                    initializedLoad = true;
                    StartCoroutine( LoadSceneAsync());
                }
            }
        }

        private IEnumerator LoadSceneAsync()
        {
            AsyncOperation asyncLoad;
            if (GameManager.Instance.continueGame)
            {
                asyncLoad  =  SceneManager.LoadSceneAsync(2);
            }
            else
            {
                asyncLoad =  SceneManager.LoadSceneAsync(1);
            }
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
