using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle
{
    public class StateMachine<Indexer> : Component where Indexer : struct
    {
        public delegate Indexer StateCallback();

        private Indexer state;
        private Dictionary<Indexer, Action> enterStates;
        private Dictionary<Indexer, StateCallback> updates;
        private Dictionary<Indexer, Action> leaveStates;

        private Indexer switchTo;
        private int switchCounter;

        public StateMachine(Indexer initialState)
            : base(true, false)
        {
            state = initialState;

            enterStates = new Dictionary<Indexer, Action>();
            updates = new Dictionary<Indexer, StateCallback>();
            leaveStates = new Dictionary<Indexer, Action>();
        }

        public StateMachine()
            : this(default(Indexer))
        {

        }

        public Indexer State
        {
            get { return state; }
            set
            {
#if DEBUG
                if (!updates.ContainsKey(value))
                    throw new Exception("StateMachine has entered a state for which callbacks have not been set (you can set them to null for no actions)");
#endif 
                if (!state.Equals(value))
                {
                    switchCounter = 0;
                    if (leaveStates[state] != null)
                        leaveStates[state]();
                    state = value;
                    if (enterStates[state] != null)
                        enterStates[state]();
                }
            }
        }

        public void SetCallbacks(Indexer state, StateCallback onUpdate, Action onEnterState = null, Action onLeaveState = null)
        {
            updates[state] = onUpdate;
            enterStates[state] = onEnterState;
            leaveStates[state] = onLeaveState;
        }

        public override void Update()
        {
            if (switchCounter > 0)
            {
                switchCounter--;
                if (switchCounter == 0)
                    State = switchTo;
            }

            if (updates[state] != null)
                State = updates[state]();
        }

        static public implicit operator Indexer(StateMachine<Indexer> s)
        {
            return s.state;
        }

        /*
         *  Switch to the specified state in the specified amount of frames.
         *  This switch is cancelled if the state is otherwise changed before it is finished.
         */
        public void DelayedStateChange(Indexer changeTo, int frameDelay)
        {
#if DEBUG
            if (frameDelay <= 0)
                throw new Exception("Frame delay must be larger than zero");
#endif
            switchTo = changeTo;
            switchCounter = frameDelay;
        }
    }
}
