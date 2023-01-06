using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TileSets
{
    private Texture2D _maTexture;

    public TileSets()
    {
        _maTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("tiles");
    }

    public Texture2D GetTileSet()
    {
        return _maTexture;
    }
}
