using LD52;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

internal class SceneGameplay : Scene
{
    Sprite80 spr80;
    Spr8x8 sprTruck;

    const int SCREENW = 800 / 4;
    const int SCREENH = 480 / 4;
    const int MAPW = (int)(SCREENW / 8);
    const int MAPH = (int)(SCREENH / 8);

    private int[,] Map;

    private int _objective;
    private int _harvested;

    private double timerWin;

    private ParticleManager _particleManager;

    public SceneGameplay()
    {
        Debug.WriteLine(MAPW + "," + MAPH);

        Map = new int[MAPH, MAPW];

        _particleManager = new ParticleManager();
    }

    public override void Load()
    {
        spr80 = new Sprite80();
        sprTruck = new Spr8x8(17, 0, 0);

        _controlManager.SetMethodKey("right", Microsoft.Xna.Framework.Input.Keys.Right);
        _controlManager.SetMethodKey("left", Microsoft.Xna.Framework.Input.Keys.Left);
        _controlManager.SetMethodKey("up", Microsoft.Xna.Framework.Input.Keys.Up);
        _controlManager.SetMethodKey("down", Microsoft.Xna.Framework.Input.Keys.Down);

        ResetGame();

        base.Load();
    }

    private void ResetGame()
    {
        timerWin = 0;
        sprTruck.SetMapPosition(0, 0);
        sprTruck.GoTo(1, 0);

        _particleManager.Reset();

        for (int l = 0 + 1; l < MAPH - 1; l++)
        {
            for (int c = 0 + 1; c < MAPW - 1; c++)
            {
                Map[l, c] = 1;
            }
        }

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
            sprTruck.SetNum(17);
        }
        if (_controlManager.Down("left") && sprTruck.col > 0)
        {
            sprTruck.GoTo(sprTruck.row, sprTruck.col - 1);
            sprTruck.SetNum(18);
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

    public override void Update(GameTime gameTime)
    {
        if (!sprTruck.moving && timerWin == 0)
        {
            _Controls();
        }

        if (sprTruck.moving)
        {
            if (Utils.GetInt(1, 10) > 8)
            {
                int np = Utils.GetInt(36, 39);
                string dir = sprTruck.getDirection();
                float vx = 0;
                float vy = 0;
                switch (dir)
                {
                    case "left":
                        vx = (float)Utils.GetInt(1, 2) / 50;
                        vy = (float)Utils.GetInt(-1, 1) / 50;
                        break;
                    case "right":
                        vx = (float)Utils.GetInt(-2, -1) / 50;
                        vy = (float)Utils.GetInt(-1, 1) / 50;
                        break;
                    case "up":
                        vx = (float)Utils.GetInt(-1, 1) / 50;
                        vy = (float)Utils.GetInt(-2, 1) / 50;
                        break;
                    case "down":
                        vx = (float)Utils.GetInt(-1, 1) / 50;
                        vy = (float)Utils.GetInt(1, 2) / 50;
                        break;
                    default:
                        break;
                }
                double life = (double)Utils.GetInt(5, 10) / 20;
                _particleManager.AddParticule(np, (sprTruck.Position.X) + 4, (sprTruck.Position.Y) + 4, vx, vy, life);
            }
        }
        sprTruck.Update();

        if (sprTruck.justArrived && Map[sprTruck.row, sprTruck.col] != 0)
        {
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
                ResetGame();
                _sceneService.ChangeScene(SceneManager.sceneType.Win);
            }
        }

        if (_harvested == 10 && timerWin == 0)//_objective)
        {
            timerWin = 2;
            for (int i = 0; i < 100; i++)
            {
                float vx = (float)Utils.GetInt(-50, 50) / 20;
                float vy = (float)Utils.GetInt(-50, 50) / 20;
                float norm = (float)Math.Sqrt(vx * vx + vy * vy);
                //                vx /= norm / 2;
                //                vy /= norm / 2;
                double life = 10;
                _particleManager.AddParticule(40, (sprTruck.Position.X) + 4 + Utils.GetInt(-2, 2), (sprTruck.Position.Y) + 4 + Utils.GetInt(-2, 2), vx, vy, life);
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
                spr80.Spr(Map[l, c], c * 8, l * 8);
            }
        }

        _particleManager.Draw();

        sprTruck.Draw();

        int col = MAPW - 8;
        spr80.Spr(81, 8 * col++, 0);
        spr80.Spr(82, 8 * col++, 0);
        spr80.Spr(83, 8 * col++, 0);
        spr80.Spr(84, 8 * col, 0);
        spr80.Spr(75, 8 * col + (3 * 8), 0);

        col = MAPW - 4;
        float ratio = (float)_harvested / (float)_objective;
        int Percent = (int)(ratio * 100);

        if (Percent < 100)
        {
            if (Percent < 10)
                spr80.Spr(65, 8 * col, 0);

            string sPercent = Percent.ToString("00");
            int n1 = Percent / 10;
            spr80.Spr(65 + n1, 8 * col, 0);
            int n2 = int.Parse(sPercent[1].ToString());
            spr80.Spr(65 + n2, 8 * (col + 1), 0);
        }
        else
        {
            spr80.Spr(85, 8 * col, 0);
            spr80.Spr(86, 8 * (col + 1), 0);
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
