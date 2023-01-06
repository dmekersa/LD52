using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class SceneGameplay : Scene
{
    Sprite sprTest;

    public SceneGameplay()
    {
    }

    public override void Load()
    {
        sprTest = new Sprite(21, new Vector2(10, 10));

        base.Load();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw()
    {
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        FontManager fontManager = ServiceLocator.GetService<FontManager>();

        sprTest.Draw();
    }


}
