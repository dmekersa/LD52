using Microsoft.Xna.Framework;

public class Scene
{
    protected Gamecodeur.GCControlManager _controlManager;
    public bool loaded { get; private set; }

    public Scene()
    {
        _controlManager = ServiceLocator.GetService<Gamecodeur.GCControlManager>();
    }

    public virtual void Load()
    {
        loaded = true;
    }

    public virtual void Update(GameTime gameTime)
    {
        _controlManager.Update();
    }

    public virtual void Draw()
    {

    }
}