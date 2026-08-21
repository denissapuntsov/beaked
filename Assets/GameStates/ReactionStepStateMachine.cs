using System;
using UnityEngine;

public class ReactionStepStateMachine : FiniteStateMachine
{
        public ExitState EntryState;
        
        public SpeakReactionStateMachine SpeakReactionState { get; protected set; }
        public InteractReactionStateMachine InteractReactionState { get; protected set; }
        
        public ReactionStepStateMachine() : base()
        {
                SpeakReactionState = new SpeakReactionStateMachine()
                {
                        OnComplete = HandleSpeakReactionStepComplete
                };

                InteractReactionState = new InteractReactionStateMachine()
                {
                        OnComplete = HandleInteractReactionStateComplete
                };
        }
        
        public override void Enter()
        {
                base.Enter();
                switch (EntryState)
                {
                        case ExitState.Interacting:
                                ChangeState(InteractReactionState);
                                return;
                        case ExitState.Speaking:
                                ChangeState(SpeakReactionState);
                                return;
                        case ExitState.Listening:
                                return;
                        case ExitState.Walking:
                                return;
                }
        }
        
        private void HandleSpeakReactionStepComplete()
        {
                Complete();
        }
        
        private void HandleInteractReactionStateComplete()
        {
                Complete();
        }

        public override void Exit()
        {
                base.Exit();
                EntryState = ExitState.None;
        }
}
