using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

internal class FontManager
{
    SpriteFont fontSimple;
    SpriteFont fontTitle;
    SpriteFont fontUI;

    public enum fontStyle
    {
        simple,
        title,
        UI
    }

    public FontManager(ContentManager pContent)
    {
        fontSimple = pContent.Load<SpriteFont>("fontSimple");
        fontTitle = pContent.Load<SpriteFont>("fontTitre");
        fontUI = pContent.Load<SpriteFont>("fontUI");
    }

    public SpriteFont getFont(fontStyle pStyle)
    {
        switch (pStyle)
        {
            case fontStyle.simple:
                return fontSimple;
            case fontStyle.title:
                return fontTitle;
            case fontStyle.UI:
                return fontUI;
            default:
                return null;
        }
    }
}
