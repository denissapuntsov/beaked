using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteStateMachine : FiniteStateMachine
{
        public LevelCompleteStateMachine() : base()
        {
                
        }

        public override void Enter()
        {
                Debug.Log("Level Complete");
                SceneManager.LoadScene("Scenes/TransitionScreen");
        }
}
