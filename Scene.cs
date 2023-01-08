using Microsoft.Xna.Framework;

public class Scene
{
    protected SceneService _sceneService;
    protected Gamecodeur.GCControlManager _controlManager;
    public bool loaded { get; private set; }

    public Scene()
    {
        _controlManager = ServiceLocator.GetService<Gamecodeur.GCControlManager>();
    }

    public virtual void Focus()
    {

    }

    public virtual void Load()
    {
        loaded = true;
        _sceneService = ServiceLocator.GetService<SceneService>();
    }

    public virtual void Update(GameTime gameTime)
    {
        _controlManager.Update();
    }

    public virtual void DrawUI()
    {

    }

    public virtual void Draw()
    {

    }
}