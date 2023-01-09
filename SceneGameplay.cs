using LD52;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

internal class SceneGameplay : Scene
{
    SoundEffect sndHarvest;
    SoundEffect sndBallot;
    SoundEffect sndChicken;
    SoundEffect sndExplode;
    SoundEffect sndExplodeMax;
    SoundEffect sndExplodeChicken;
    SoundEffect sndGameover;
    SoundEffect sndWin;

    Sprite80 spr80;

    Spr8x8 sprTruck;
    int HP;

    Spr8x8 sprFarm;
    List<Spr8x8> lstBallot;
    List<Spr8x8> lstChickens;
    private double _timerNextChicken;

    const int SCREENW = 800 / 4;
    const int SCREENH = 480 / 4;
    const int MAPW = (int)(SCREENW / 8);
    const int MAPH = (int)(SCREENH / 8);

    private int[,] Map;

    private int _objective;
    private int _harvested;

    private double timerWin;

    private ParticleManager _particleManager;

    private int BallotLimit = 0;

    private double time;
    private int deadChickens;

    public SceneGameplay()
    {
        Debug.WriteLine(MAPW + "," + MAPH);

        Map = new int[MAPH, MAPW];
        lstBallot = new List<Spr8x8>();
        lstChickens = new List<Spr8x8>();

        _particleManager = new ParticleManager();
    }

    public override void Load()
    {
        spr80 = new Sprite80();
        sprTruck = new Spr8x8(4, 0, 0);
        sprTruck.Wide = 2;

        sprFarm = new Spr8x8(21, 0, (MAPW - 4));
        sprFarm.Wide = 3;

        _controlManager.SetMethodKey("right", Microsoft.Xna.Framework.Input.Keys.Right);
        _controlManager.SetMethodKey("left", Microsoft.Xna.Framework.Input.Keys.Left);
        _controlManager.SetMethodKey("up", Microsoft.Xna.Framework.Input.Keys.Up);
        _controlManager.SetMethodKey("down", Microsoft.Xna.Framework.Input.Keys.Down);

        ContentManager cm = ServiceLocator.GetService<ContentManager>();

        sndHarvest = cm.Load<SoundEffect>("recolte 2");
        sndBallot = cm.Load<SoundEffect>("paille spawn");
        sndChicken = cm.Load<SoundEffect>("poulet spawn");
        sndExplode = cm.Load<SoundEffect>("paille hit 1");
        sndExplodeMax = cm.Load<SoundEffect>("explosion poule 2");
        sndExplodeChicken = cm.Load<SoundEffect>("poule spawn 2");
        sndGameover = cm.Load<SoundEffect>("defaite longue");
        sndWin = cm.Load<SoundEffect>("win 2");

        base.Load();
    }

    public override void Focus()
    {
        ResetGame();
    }

    private void ResetTruck()
    {
        sprTruck.SetMapPosition(0, 0);
        sprTruck.GoTo(1, 0);
        BallotLimit = 0;
        sprTruck.FadeIn(0.1f);
    }

    private void Explosion(int pSprMin, int pSprMax, int pQty)
    {
        for (int i = 0; i < pQty; i++)
        {
            int s = Utils.GetInt(pSprMin, pSprMax);
            float vx = (float)Utils.GetInt(-50, 50) / 20;
            float vy = (float)Utils.GetInt(-50, 50) / 20;
            float norm = (float)Math.Sqrt(vx * vx + vy * vy);
            //                vx /= norm / 2;
            //                vy /= norm / 2;
            double life = 4;
            _particleManager.AddParticule(s, (sprTruck.Position.X) + 4 + Utils.GetInt(-2, 2), (sprTruck.Position.Y) + 4 + Utils.GetInt(-2, 2), vx, vy, life);
        }
    }

    private void ChickenGo(Spr8x8 pSprite)
    {
        int newRow = pSprite.row;
        int newCol = pSprite.col;
        if (Utils.GetInt(1, 6) > 3)
        {
            // vertical move
            newCol += Utils.GetInt(-1, 1);
            if (newCol >= 1 && newCol <= MAPW - 2 && Map[newRow, newCol] != 2)
                pSprite.GoTo(newRow, newCol);
        }
        else
        {
            // horizontal move
            newRow += Utils.GetInt(-1, 1);
            if (newRow >= 1 && newRow <= MAPH - 2 && Map[newRow, newCol] != 2)
                pSprite.GoTo(newRow, newCol);
        }
    }

    private void NextChicken()
    {
        sndChicken.Play();
        Spr8x8 sprChicken = new Spr8x8(6, 0, MAPW - 3);
        //ChickenGo(sprChicken);
        sprChicken.GoTo(sprChicken.row + 1, sprChicken.col);
        sprChicken.FadeIn(0.01f);
        lstChickens.Add(sprChicken);

        _timerNextChicken = (double)Utils.GetInt(10, 20);
    }

    private void ResetGame()
    {
        time = 0;
        deadChickens = 0;

        _particleManager.Reset();
        lstBallot.Clear();
        lstChickens.Clear();

        ResetTruck();
        timerWin = 0;
        HP = 3;
        NextChicken();

        Debug.WriteLine("next chicken {0}", _timerNextChicken);

        for (int l = 0 + 1; l < MAPH - 1; l++)
        {
            for (int c = 0 + 1; c < MAPW - 1; c++)
            {
                Map[l, c] = 1;
            }
        }

        _objective = 0;
        _harvested = 0;
        for (int l = 0 + 1; l < MAPH - 1; l++)
        {
            for (int c = 0 + 1; c < MAPW - 1; c++)
            {
                if (Map[l, c] == 1)
                    _objective++;
            }
        }
    }

    private void _Controls()
    {
        if (_controlManager.Down("right") && sprTruck.col < MAPW - 1)
        {
            sprTruck.GoTo(sprTruck.row, sprTruck.col + 1);
        }
        if (_controlManager.Down("left") && sprTruck.col > 0)
        {
            sprTruck.GoTo(sprTruck.row, sprTruck.col - 1);
        }
        if (_controlManager.Down("up") && sprTruck.row > 0)
        {
            sprTruck.GoTo(sprTruck.row - 1, sprTruck.col);
        }
        if (_controlManager.Down("down") && sprTruck.row < MAPH - 1)
        {
            sprTruck.GoTo(sprTruck.row + 1, sprTruck.col);
        }
    }

    private void Life()
    {
        HP--;
        sprTruck.SetMapPosition(0, 0);
        sprTruck.GoTo(1, 0);

        if (HP == 0)
        {
            sndGameover.Play();
            UpdateGameState();
            _sceneService.ChangeScene(SceneManager.sceneType.Win);
        }
    }

    private void UpdateGameState()
    {
        float ratio = (float)_harvested / (float)_objective;
        int Percent = (int)(ratio * 100);

        GameState.lastPercent = Percent;
        GameState.lastTime = (int)time;
        GameState.lastChicken = deadChickens;
        if (Percent >= 100)
            if (GameState.bestTime > time || GameState.bestTime == 0)
                GameState.bestTime = (int)time;
    }

    public override void Update(GameTime gameTime)
    {
        time += gameTime.ElapsedGameTime.TotalSeconds;
        _timerNextChicken -= gameTime.ElapsedGameTime.TotalSeconds;
        if (_timerNextChicken <= 0)
        {
            NextChicken();
        }

        if (!sprTruck.moving && timerWin == 0)
        {
            _Controls();
        }

        // Smoke
        if (sprTruck.moving)
        {
            if (Utils.GetInt(1, 10) > 8)
            {
                int np = Utils.GetInt(36, 39);
                string dir = sprTruck.getDirection();
                float vx = 0;
                float vy = 0;
                vy = (float)Utils.GetInt(-10, -2) / 50;
                switch (dir)
                {
                    case "left":
                        vx = (float)Utils.GetInt(1, 2) / 50;
                        break;
                    case "right":
                        vx = (float)Utils.GetInt(-2, -1) / 50;
                        break;
                    case "up":
                        vx = (float)Utils.GetInt(-1, 1) / 50;
                        break;
                    case "down":
                        vx = (float)Utils.GetInt(-1, 1) / 50;
                        break;
                    default:
                        break;
                }
                double life = (double)Utils.GetInt(5, 10) / 20;
                _particleManager.AddParticule(np, (sprTruck.Position.X) + 4, (sprTruck.Position.Y) + 7, vx, vy, life);
            }
        }
        sprTruck.Update(gameTime);

        // Collecte
        if (sprTruck.justArrived && Map[sprTruck.row, sprTruck.col] == 1)
        {
            sndHarvest.Play();
            BallotLimit++;
            if (BallotLimit == 10)
            {
                sndBallot.Play();
                int bc = sprTruck.col;
                int br = sprTruck.row;
                switch (sprTruck.getDirection())
                {
                    case "left":
                        bc = sprTruck.col + 1;
                        break;
                    case "right":
                        bc = sprTruck.col - 1;
                        break;
                    case "up":
                        br = sprTruck.row + 1;
                        break;
                    case "down":
                        br = sprTruck.row - 1;
                        break;
                    default:
                        break;
                }
                Spr8x8 newBallot = new Spr8x8(20, br, bc);
                Map[br, bc] = 2;
                newBallot.FadeIn(0.1f);
                lstBallot.Add(newBallot);
                BallotLimit = 0;
            }
            Map[sprTruck.row, sprTruck.col] = 0;
            _harvested++;
            for (int i = 0; i < 2; i++)
            {
                int np = Utils.GetInt(33, 35);
                float vx = (float)Utils.GetInt(-20, 20) / 70;
                float vy = (float)Utils.GetInt(-20, 20) / 70;
                double life = (double)Utils.GetInt(5, 10) / 20;
                _particleManager.AddParticule(np, (sprTruck.col * 8) + 4, (sprTruck.row * 8) + 4, vx, vy, life);
            }
        }

        if (timerWin > 0)
        {
            timerWin -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timerWin <= 0)
            {
                _sceneService.ChangeScene(SceneManager.sceneType.Win);
            }
        }

        if (_harvested >= _objective && timerWin == 0)//_objective)
        {
            sndWin.Play();
            timerWin = 2;
            Explosion(40, 40, 100);

            UpdateGameState();
            _sceneService.ChangeScene(SceneManager.sceneType.Win);

        }

        // Collisions with ballots
        foreach (Spr8x8 b in lstBallot)
        {
            b.Update(gameTime);
            if (b.CollideBox.Intersects(sprTruck.CollideBox))
            {
                sndExplode.Play();
                Explosion(36, 40, 100);
                GameState.camShake = 0.4f;
                Life();
            }
        }

        if (timerWin == 0)
        {
            for (int n = lstChickens.Count - 1; n >= 0; n--)
            {
                Spr8x8 c = lstChickens[n];
                c.Update(gameTime);
                if (c.CollideBox.Intersects(sprTruck.CollideBox) && c.Alpha >= 1)
                {
                    sndExplodeChicken.Play();
                    sndExplodeMax.Play();
                    Explosion(36, 40, 100);
                    Explosion(49, 54, 100);
                    GameState.camShake = 0.4f;
                    Life();
                    lstChickens.Remove(c);
                    deadChickens++;
                }
                else if (c.AtDestination())
                {
                    ChickenGo(c);
                }
            }
        }

        _particleManager.Update(gameTime);

        base.Update(gameTime);
    }

    public override void Draw()
    {
        for (int l = 0; l < MAPH; l++)
        {
            for (int c = 0; c < MAPW; c++)
            {
                if (Map[l, c] == 1)
                {
                    spr80.Spr(Map[l, c], c * 8, l * 8);
                    if (l > 0)
                    {
                        if (Map[l - 1, c] == 0)
                        {
                            spr80.Spr(2, c * 8, (l - 1) * 8);
                        }
                    }
                    if (l < MAPH - 1)
                    {
                        if (Map[l + 1, c] == 0)
                        {
                            spr80.Spr(17, c * 8, (l + 1) * 8);
                        }
                    }
                }
            }
        }

        foreach (Spr8x8 sprBallot in lstBallot)
        {
            sprBallot.Draw();
        }

        foreach (Spr8x8 sprChicken in lstChickens)
        {
            sprChicken.Draw();
        }

        sprFarm.Draw();

        _particleManager.Draw();

        sprTruck.Draw();

        int colUI = MAPW - 8;
        int rowUI = 8 * (MAPH - 1);
        spr80.Spr(81, 8 * colUI++, rowUI);
        spr80.Spr(82, 8 * colUI++, rowUI);
        spr80.Spr(83, 8 * colUI++, rowUI);
        spr80.Spr(84, 8 * colUI, rowUI);
        spr80.Spr(75, 8 * colUI + (3 * 8), rowUI);

        colUI = MAPW - 4;
        float ratio = (float)_harvested / (float)_objective;
        int Percent = (int)(ratio * 100);

        if (timerWin == 0)
        {
            if (Percent < 10)
                spr80.Spr(65, 8 * colUI, rowUI);

            string sPercent = Percent.ToString("00");
            int n1 = Percent / 10;
            spr80.Spr(65 + n1, 8 * colUI, rowUI);
            int n2 = int.Parse(sPercent[1].ToString());
            spr80.Spr(65 + n2, 8 * (colUI + 1), rowUI);
        }
        else
        {
            spr80.Spr(85, 8 * colUI, rowUI);
            spr80.Spr(86, 8 * (colUI + 1), rowUI);
        }

        // HP
        for (int i = 0; i < HP; i++)
        {
            spr80.Spr(3, i * 8, 0);
        }
    }

    public override void DrawUI()
    {
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        FontManager fontManager = ServiceLocator.GetService<FontManager>();

        float ratio = (float)_harvested / (float)_objective;
        float Percent = ratio * 100f;
        //spriteBatch.DrawString(fontManager.getFont(FontManager.fontStyle.title), (int)Percent + "%", new Vector2(0, 0), Color.Yellow);

        base.DrawUI();
    }


}
