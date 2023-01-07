﻿using Microsoft.Xna.Framework;
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

            public void Update(double pTime)
            {
                life -= pTime;
                Move(vx, vy);
                base.Update();
            }
        }

        private List<Particule> lstParticules;

        public ParticleManager()
        {
            lstParticules = new List<Particule>();
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
                p.Update(gameTime.ElapsedGameTime.TotalSeconds);
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