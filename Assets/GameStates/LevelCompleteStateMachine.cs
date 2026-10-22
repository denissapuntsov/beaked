using LevelStructure;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteStateMachine : FiniteStateMachine
{
        private TileGridController _controller;
        private MenuCanvas _menuCanvas;
        
        public LevelCompleteStateMachine(TileGridController controller, MenuCanvas menuCanvas) : base()
        {
                _controller = controller;
                _menuCanvas = menuCanvas;
        }

        public override void Enter()
        {
                Debug.Log("Level Complete");
                _controller.AnimateWorldDown(LoadNextLevel);
        }

        private void LoadNextLevel()
        {
                LevelManager.instance.LoadLevel();
                Complete();
        }
}
