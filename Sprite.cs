using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

/*
 * Sprite based on TIC-80 tilesheet
 * emulating Spr API
 */
public class Sprite
{
    public int Num { get; private set; }
    private Texture2D _maTexture;
    public Rectangle CollideBox { get { return _collideBox; } }
    public Vector2 Position { get { return new Vector2(_collideBox.X, _collideBox.Y); } }
    private Rectangle _collideBox;

    public Sprite(int num, Vector2 pPosition)
    {
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

    public void Move(int pX, int pY)
    {
        _collideBox.X += pX;
        _collideBox.Y += pY;
    }

    public void Draw()
    {
        Spr(Num, _collideBox.X, _collideBox.Y);
    }

    public bool CollideWith(Sprite pSprite)
    {
        return _collideBox.Intersects(pSprite.CollideBox);
    }

    private void Spr(int pnSprite, float pX, float pY)
    {
        int texW = _maTexture.Width / 8;
        int texH = _maTexture.Height / 8;
        SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();

        Rectangle rect = new Rectangle();

        int line = (int)Math.Floor((double)(pnSprite / texW));
        int col = pnSprite - (line * texW);

        rect.X = col * 8;
        rect.Y = line * 8;
        rect.Width = 8;
        rect.Height = 8;
        spriteBatch.Draw(_maTexture, new Vector2(pX, pY), rect, Color.White);
        spriteBatch.Draw(_maTexture, new Vector2(pX, pY), rect, Color.White);
    }
}