using Microsoft.Xna.Framework;
using static SceneManager;

interface SceneService
{
    public void ChangeScene(sceneType pType);
}

internal class SceneManager : SceneService
{
    private Scene _currentScene;
    private SceneMenu _sceneMenu;
    private SceneGameplay _sceneGameplay;

    public enum sceneType
    {
        Menu,
        Gameplay,
        Gameover,
        Test
    }

    public SceneManager()
    {
        _currentScene = null;
        _sceneMenu = new SceneMenu();
        _sceneGameplay = new SceneGameplay();
    }

    public void ChangeScene(sceneType pType)
    {
        switch (pType)
        {
            case sceneType.Menu:
                _currentScene = _sceneMenu;
                break;
            case sceneType.Gameplay:
                _currentScene = _sceneGameplay;
                break;
            case sceneType.Gameover:
                break;
            default:
                break;
        }

        if (_currentScene.loaded == false)
            _currentScene.Load();
    }

    public void Update(GameTime gameTime)
    {
        if (_currentScene != null)
            _currentScene.Update(gameTime);
    }

    public void Draw()
    {
        if (_currentScene != null)
            _currentScene.Draw();
    }
}
