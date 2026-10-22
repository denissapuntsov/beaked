using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
        public int chapter, stage;

        public static LevelManager instance;

        private Sign _sign;

        public bool IsWin { get; protected set; } = false;

        private void Awake()
        {
                if (instance == null)
                {
                        instance = this;
                }
                else if (instance != this)
                {
                        Destroy(gameObject);
                }
                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded += ProcessNewScene;
        }

        public void LoadLevel()
        {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void ProcessNewScene(Scene scene, LoadSceneMode mode)
        {
                IsWin = false;
                if (_sign)
                {
                        _sign.onPecked.RemoveListener(OnSignPecked);
                }
                _sign = FindAnyObjectByType<Sign>();
                _sign?.onPecked.AddListener(OnSignPecked);
        }

        private void OnSignPecked()
        {
                IsWin = true;
        }
}
