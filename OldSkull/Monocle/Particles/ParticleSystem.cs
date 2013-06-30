using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Monocle
{
    public class ParticleSystem : Entity
    {
        private Particle[] particles;

        public ParticleSystem(int layerIndex, int depth, int maxParticles)
            : base(layerIndex)
        {
            particles = new Particle[maxParticles];
            Depth = depth;
        }

        public override void Update()
        {
            for (int i = 0; i < particles.Length; i++)
                if (particles[i].Active)
                    particles[i].Update();
        }

        public override void Render()
        {
            foreach (var p in particles)
                if (p.Active)
                    p.Render();
        }

        private int GetParticleSlot()
        {
            int at;
            for (at = 0; at < particles.Length; at++)
            {
                if (!particles[at].Active)
                    break;
            }

#if DEBUG
            if (at >= particles.Length || particles[at].Active)
                throw new Exception("Particle Overflow at depth: " + Depth);
#endif

            return at;
        }

        public void Emit(ParticleType type, Vector2 position)
        {        
            particles[GetParticleSlot()] = type.Create(position);
        }

        public void Emit(ParticleType type, int amount, Vector2 position, Vector2 positionRange)
        {
            for (int i = 0; i < amount; i++)
                Emit(type, Calc.Random.Range(position - positionRange, positionRange * 2));
        }

        public void Emit(ParticleType type, Vector2 position, float direction)
        {
            particles[GetParticleSlot()] = type.Create(position, direction);
        }

        public void Emit(ParticleType type, int amount, Vector2 position, Vector2 positionRange, float direction)
        {
            for (int i = 0; i < amount; i++)
                Emit(type, Calc.Random.Range(position - positionRange, positionRange * 2), direction);
        }
    }
}
