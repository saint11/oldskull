using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class ParticleType
    {
        public Subtexture Source;
        public Color Color;
        public Color Color2;
        public int ColorSwitch;
        public bool ColorSwitchLoop;
        public float Speed;
        public float SpeedRange;
        public float SpeedMultiplier;
        public Vector2 Acceleration;
        public float Direction;
        public float DirectionRange;
        public int Life;
        public int LifeRange;
        public int Size;
        public int SizeRange;

        public ParticleType()
        {
            Color = Color2 = Color.White;
            ColorSwitch = 1;
            ColorSwitchLoop = true;
            Speed = SpeedRange = 0;
            SpeedMultiplier = 1;
            Acceleration = Vector2.Zero;
            Direction = DirectionRange = 0;
            Life = LifeRange = 0;
            Size = 2;
            SizeRange = 0;
        }

        public ParticleType(ParticleType copy)
        {
            Source = copy.Source;
            Color = copy.Color;
            Color2 = copy.Color2;
            ColorSwitch = copy.ColorSwitch;
            ColorSwitchLoop = copy.ColorSwitchLoop;
            Speed = copy.Speed;
            SpeedRange = copy.SpeedRange;
            SpeedMultiplier = copy.SpeedMultiplier;
            Acceleration = copy.Acceleration;
            Direction = copy.Direction;
            DirectionRange = copy.DirectionRange;
            Life = copy.Life;
            LifeRange = copy.LifeRange;
            Size = copy.Size;
            SizeRange = copy.SizeRange;
        }

        public Particle Create(Vector2 position)
        {
            return Create(position, Direction);
        }

        public Particle Create(Vector2 position, float direction)
        {
            Particle particle = new Particle();
            particle.Type = this;
            particle.Active = true;
            particle.Position = position;
            particle.Size = Calc.Random.Range(Size, SizeRange);
            particle.Color = Color;
            particle.Speed = Calc.AngleToVector(direction - DirectionRange / 2 + Calc.Random.NextFloat() * DirectionRange, Calc.Random.Range(Speed, SpeedRange));
            particle.Life = Calc.Random.Range(Life, LifeRange);
            particle.ColorSwitch = ColorSwitch;

            return particle;
        }
    }
}
