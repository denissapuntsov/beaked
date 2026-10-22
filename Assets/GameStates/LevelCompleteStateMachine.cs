using LevelStructure;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteStateMachine : FiniteStateMachine
{
        private TileGridController _controller;
        
        public LevelCompleteStateMachine(TileGridController controller) : base()
        {
                _controller = controller;
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
