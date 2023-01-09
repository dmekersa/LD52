using LD52;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

internal class SceneMenu : Scene
{
    const string version = "1.0b";

    List<Spr8x8> lstChickens;
    float sin = 0;

    public SceneMenu()
    {
        lstChickens = new List<Spr8x8>();
    }

    public override void Focus()
    {
        lstChickens.Clear();
        for (int n = 0; n < 15 + 1; n++)
        {
            Spr8x8 c = new Spr8x8(6, n, -1);
            c.Alpha = 0.5f;
            c.speed = (float)Utils.GetInt(10, 20) / 50;
            c.GoTo(c.row, c.col + 1);
            lstChickens.Add(c);
            c = new Spr8x8(6, n, 25);
            c.Alpha = 0.5f;
            c.speed = (float)Utils.GetInt(10, 20) / 50;
            c.GoTo(c.row, c.col - 1);
            lstChickens.Add(c);
        }

        base.Focus();
    }

    public override void Load()
    {
        _controlManager.SetMethodKey("play", Keys.Space);
        base.Load();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        sin += .03f;

        if (_controlManager.Pressed("play"))
        {
            _sceneService.ChangeScene(SceneManager.sceneType.Gameplay);
        }

        foreach (Spr8x8 spr in lstChickens)
        {
            spr.Update(gameTime);
            if (spr.AtDestination())
            {
                if (spr.col >= 27)
                {
                    spr.SetMapPosition(spr.row, -1);
                }
                else if (spr.col < -2)
                {
                    spr.SetMapPosition(spr.row, 26);
                }
                else
                {
                    if (spr.getDirection() == "right")
                        spr.GoTo(spr.row, spr.col + 1);
                    else
                        spr.GoTo(spr.row, spr.col - 1);
                }
            }
        }
    }

    public override void Draw()
    {
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        FontManager fontManager = ServiceLocator.GetService<FontManager>();
        SpriteFont font = fontManager.getFont(FontManager.fontStyle.title);
        string title = "THE GLUTEN FARM";

        Vector2 size = font.MeasureString(title);

        if (spriteBatch != null && fontManager != null)
        {
            spriteBatch.DrawString(font, title, new Vector2((200 - size.X) / 2, ((50 - size.Y) / 2) + (int)(10 * Math.Sin(sin))), Color.White);
        }

        font = fontManager.getFont(FontManager.fontStyle.simple);


        title = "Harvest weat, produce Gluten";
        size = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2((200 - size.X) / 2, 50), Color.White);
        title = "Avoid bundles and chickens";
        size = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2((200 - size.X) / 2, 60), Color.LightYellow);
        title = "Spacebar to play";
        size = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2((200 - size.X) / 2, 80), Color.Yellow);


        title = "Best time: " + GameState.bestTime.ToString();
        size = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2((200 - size.X) / 2, 100), Color.GreenYellow);

        foreach (Spr8x8 spr in lstChickens)
        {
            spr.Draw();
        }

    }

    public override void DrawUI()
    {
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        FontManager fontManager = ServiceLocator.GetService<FontManager>();
        SpriteFont font = fontManager.getFont(FontManager.fontStyle.simple);
        string title = version + " - " + "Coded by David Mekersa for LD52";

        Vector2 size = font.MeasureString(title);

        if (spriteBatch != null && fontManager != null)
        {
            spriteBatch.DrawString(font, title, new Vector2(0, 0), Color.White);
        }

    }
}