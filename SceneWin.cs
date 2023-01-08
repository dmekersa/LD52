using LD52;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

internal class SceneWin : Scene
{
    private SceneService _sceneService;

    public override void Load()
    {
        _sceneService = ServiceLocator.GetService<SceneService>();
        _controlManager.SetMethodKey("play", Keys.Space);

        base.Load();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_controlManager.Pressed("play"))
        {
            _sceneService.ChangeScene(SceneManager.sceneType.Menu);
        }
    }

    public override void Draw()
    {
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        FontManager fontManager = ServiceLocator.GetService<FontManager>();
        SpriteFont font = fontManager.getFont(FontManager.fontStyle.title);

        Vector2 size;

        string text;
        int y = 20;

        text = "Your time: " + GameState.lastTime.ToString() + "s";
        size = font.MeasureString(text);
        spriteBatch.DrawString(font, text, new Vector2((200 - size.X) / 2, y), Color.White);

        y += 20;
        text = "Your harvest: " + GameState.lastPercent.ToString() + "%";
        size = font.MeasureString(text);
        spriteBatch.DrawString(font, text, new Vector2((200 - size.X) / 2, y), Color.White);

        y += 20;
        text = "Dead chickens: " + GameState.lastChicken.ToString() + "";
        size = font.MeasureString(text);
        spriteBatch.DrawString(font, text, new Vector2((200 - size.X) / 2, y), Color.White);

        y += 20;
        text = "Best time 100%: " + GameState.bestTime.ToString() + "";
        size = font.MeasureString(text);
        spriteBatch.DrawString(font, text, new Vector2((200 - size.X) / 2, y), Color.White);

    }

    public override void DrawUI()
    {

    }
}