using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

internal class SceneMenu : Scene
{
    private double timerChangeScene = 0;
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
            _sceneService.ChangeScene(SceneManager.sceneType.Gameplay);
        }
    }

    public override void Draw()
    {
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        FontManager fontManager = ServiceLocator.GetService<FontManager>();

        if (spriteBatch != null && fontManager != null)
        {
            spriteBatch.DrawString(fontManager.getFont(FontManager.fontStyle.UI), "LD52", new Vector2(0, 0), Color.Yellow);
        }
    }

}
