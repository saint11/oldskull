using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle;
using OldSkull.GameLevel;

namespace OldSkull.GameLevel
{
    public class PlatformLevelEntity : Entity
    {

        public List<PlatformerLevel.GameState> UpdateOnState;
        private PlatformerLevel Level { get { return (PlatformerLevel)Scene; } }


        public PlatformLevelEntity(int layerIndex, PlatformerLevel.GameState UpdateState = PlatformerLevel.GameState.Game)
            :base(layerIndex)
        {
            UpdateOnState = new List<PlatformerLevel.GameState>();
            UpdateOnState.Add(UpdateState);
        }

        public override void Update()
        {
            if (HasState(Level.CurrentState))
            {
                base.Update();
                Step();
            }
        }

        public virtual void Step()
        {
            
        }

        public bool HasState(PlatformerLevel.GameState GameState)
        {
            foreach (PlatformerLevel.GameState s in UpdateOnState)
            {
                if (s == GameState) return true;
            }
            return false;
        }

        public void UpdateOnlyOnState(PlatformerLevel.GameState GameState)
        {
            UpdateOnState = new List<PlatformerLevel.GameState>();
            UpdateOnState.Add(GameState);
        }


        public void ClearUpdateStates(PlatformerLevel.GameState GameState)
        {
            UpdateOnState = new List<PlatformerLevel.GameState>();
        }

        public void AddUpdateState(PlatformerLevel.GameState GameState)
        {
            if (UpdateOnState==null) UpdateOnState = new List<PlatformerLevel.GameState>();
            UpdateOnState.Add(GameState);
        }

        public void AddUpdateState(List<PlatformerLevel.GameState> GameStates)
        {
            if (UpdateOnState == null) UpdateOnState = new List<PlatformerLevel.GameState>();
            UpdateOnState.AddRange(GameStates);
        }
    }
}
