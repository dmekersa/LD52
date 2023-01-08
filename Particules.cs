using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD52
{
    class ParticleManager
    {
        class Particule : Sprite80
        {
            public float vx;
            public float vy;
            public double life;

            public void Update(GameTime gameTime)
            {
                life -= gameTime.ElapsedGameTime.TotalSeconds;
                Move(vx, vy);
                base.Update(gameTime);
            }
        }

        private List<Particule> lstParticules;

        public ParticleManager()
        {
            lstParticules = new List<Particule>();
        }

        public void Reset()
        {
            lstParticules.Clear();
        }

        public void AddParticule(int pSpr, float px, float py, float pVX, float pVY, double pLife)
        {
            Particule p = new Particule();
            p.vx = pVX;
            p.vy = pVY;
            p.life = pLife;
            p.Centered = true;
            p.SetPosition(px, py);
            p.SetNum(pSpr);
            lstParticules.Add(p);
        }

        public void Update(GameTime gameTime)
        {
            for (int n = lstParticules.Count - 1; n >= 0; n--)
            {
                Particule p = lstParticules[n];
                p.Update(gameTime);
                if (p.life <= 0)
                {
                    lstParticules.Remove(p);
                }
            }
        }

        public void Draw()
        {
            foreach (Particule p in lstParticules)
            {
                p.Draw();
            }
        }

    }
}
