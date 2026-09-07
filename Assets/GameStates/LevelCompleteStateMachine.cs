using UnityEngine;

public class LevelCompleteStateMachine : FiniteStateMachine
{
        public LevelCompleteStateMachine() : base()
        {
                
        }

        public override void Enter()
        {
                Debug.Log("Level Complete");
        }
}
