using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

/*
 * Sprite based on TIC-80 tilesheet
 * emulating Spr API
 */
public class Sprite80
{
    public int Num { get; private set; }
    private Texture2D _maTexture;
    public Rectangle CollideBox { get { return _collideBox; } }
    // C'est de la merde ce système de position !!!
    public Vector2 Position { get; private set; }
    private Rectangle _collideBox;
    public bool Centered;
    public float Alpha = 1;

    public Sprite80() : this(0, new Vector2(0, 0))
    {
        Centered = false;
    }

    public Sprite80(int num, Vector2 pPosition)
    {
        Alpha = 1;
        Num = num;
        _collideBox = new Rectangle((int)pPosition.X, (int)pPosition.Y, 8, 8);
        _maTexture = ServiceLocator.GetService<TileSets>().GetTileSet();
    }

    public void SetNum(int pNum)
    {
        int texW = _maTexture.Width / 8;
        int texH = _maTexture.Height / 8;
        int nb = texW * texH;
        if (Num <= nb)
            Num = pNum;
        Debug.Assert(Num <= nb, "Numéro de sprite hors bornes");
    }

    public Rectangle GetCollideBox()
    {
        return _collideBox;
    }

    private void UpdateCollideBox()
    {
        _collideBox = new Rectangle((int)Position.X, (int)Position.Y, 8, 8);
    }

    public void Move(float pox, float poy)
    {
        Position += new Vector2(pox, poy);
        _collideBox = new Rectangle((int)Position.X, (int)Position.Y, 8, 8);
    }

    public void SetPosition(float px, float py)
    {
        Position = new Vector2(px, py);
        UpdateCollideBox();
    }

    public virtual void Update()
    {
        if (Alpha > 1)
            Alpha = 1;
        if (Alpha < 0)
            Alpha = 0;
        UpdateCollideBox();
    }

    public void Draw()
    {
        Spr(Num, _collideBox.X, _collideBox.Y);
    }

    public bool CollideWith(Sprite80 pSprite)
    {
        return _collideBox.Intersects(pSprite.CollideBox);
    }

    public void Spr(int pnSprite, float pX, float pY)
    {
        int texW = _maTexture.Width / 8;
        int texH = _maTexture.Height / 8;
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();

        Rectangle rect = new Rectangle();

        int line = (int)Math.Floor((double)(pnSprite / texW));
        int col = pnSprite - (line * texW);

        rect.X = col * 8;
        rect.Y = line * 8;
        float xx = pX;
        float yy = pY;
        if (Centered)
        {
            xx -= 4;
            yy -= 4;
        }
        rect.Width = 8;
        rect.Height = 8;
        spriteBatch.Draw(_maTexture, new Vector2(xx, yy), rect, Color.White * Alpha);
    }
}