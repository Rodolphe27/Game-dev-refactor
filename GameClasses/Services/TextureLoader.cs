using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public enum TextureListID
{
    // ATTACKS
    SwordBerrie, FireballVanko, GunshotRoman,

    // PLAYER CHARACTERS
    BerrieDefault, BerrieMoving, BerrieJumping, BerrieAttacking,
    RomanDefault, RomanMoving, RomanJumping, RomanAttacking,
    VankoDefault, VankoMoving, VankoJumping, VankoAttacking,

    // PLAYER OBJECTS
    Scope,
    
    // ENEMIES
    SpiderDefault, SpiderMoving, GhostDefault, GhostMoving,
    WitchDefault,
    WitchMoving,
}
public enum TextureID
{
    // TRAP
    Trap,
    // GUI
    StatsBackground, HeadBerrie, HeadRoman, HeadVanko,

    // ITEMS
    HealthPotion, LevelUp, Wings, KeyGolden, KeySilver, Chest,

    // TILES
    EmptyTile, Portal1, Portal2, belohnungsPortal, SpiderWeb,

    // LEVEL 2 TILES

    /* '#' */ 
    BrickTile1, BrickTileMoss1,

    /* '_' */
    BrickTileMG1, BrickTileMossMG1,

    /* ' ' */
    BrickTileBG1, BrickTileBG2, BrickTileBG3, BrickTileBG4,
    BrickTileBG5, BrickTileBG6, BrickTileBG7,

    /* '+' */
    IronBarsTile1,
}
public static class TextureLoader
{
    public static Dictionary<TextureID, Texture2D> Texture = [];
    public static Dictionary<TextureListID, List<Texture2D>> TextureList = [];

    public static Texture2D Pixel { get; internal set; }



    public static void LoadAll(ContentManager content, GraphicsDevice graphicsDevice)
    {

        Pixel = new Texture2D(graphicsDevice, 1, 1);

        // Paint it white
        Pixel.SetData([Color.White]);


        Texture[TextureID.Trap] = content.Load<Texture2D>("Trap/Trap");

        #region GUI

        Texture[TextureID.StatsBackground] = content.Load<Texture2D>("GUI/Stats_Background");
        Texture[TextureID.HeadBerrie] = content.Load<Texture2D>("GUI/HeadBerry");
        Texture[TextureID.HeadVanko] = content.Load<Texture2D>("GUI/HeadVanko");
        Texture[TextureID.HeadRoman] = content.Load<Texture2D>("GUI/HeadRoman");

        #endregion

        #region Items

        Texture[TextureID.HealthPotion] = content.Load<Texture2D>("Items/HealthPotion");
        Texture[TextureID.LevelUp] = content.Load<Texture2D>("Items/LevelUp");
        Texture[TextureID.KeyGolden] = content.Load<Texture2D>("Items/KeyGolden");
        Texture[TextureID.KeySilver] = content.Load<Texture2D>("Items/KeySilver");
        Texture[TextureID.Wings] = content.Load<Texture2D>("Items/Wings");
        Texture[TextureID.Chest] = content.Load<Texture2D>("Items/Chest");

        #endregion

        #region Tiles
        Texture[TextureID.Portal1] = content.Load<Texture2D>("Tiles/Portal1");
        Texture[TextureID.Portal2] = content.Load<Texture2D>("Tiles/Portal2");
        Texture[TextureID.belohnungsPortal] = content.Load<Texture2D>("Tiles/belohnungsPortal");
        Texture[TextureID.EmptyTile] = content.Load<Texture2D>("Tiles/EmptyTile");
        Texture[TextureID.SpiderWeb] = content.Load<Texture2D>("Tiles/SpiderWeb");

        // LEVEL 2
        Texture[TextureID.BrickTile1]     = content.Load<Texture2D>("Tiles/BrickTile1");
        Texture[TextureID.BrickTileMoss1] = content.Load<Texture2D>("Tiles/BrickTileMoss1");

        Texture[TextureID.BrickTileMG1]     = content.Load<Texture2D>("Tiles/BrickTileMG1");
        Texture[TextureID.BrickTileMossMG1] = content.Load<Texture2D>("Tiles/BrickTileMossMG1");

        Texture[TextureID.BrickTileBG1] = content.Load<Texture2D>("Tiles/BrickTileBG1");
        Texture[TextureID.BrickTileBG2] = content.Load<Texture2D>("Tiles/BrickTileBG2");
        Texture[TextureID.BrickTileBG3] = content.Load<Texture2D>("Tiles/BrickTileBG3");
        Texture[TextureID.BrickTileBG4] = content.Load<Texture2D>("Tiles/BrickTileBG4");
        Texture[TextureID.BrickTileBG5] = content.Load<Texture2D>("Tiles/BrickTileBG5");
        Texture[TextureID.BrickTileBG6] = content.Load<Texture2D>("Tiles/BrickTileBG6");
        Texture[TextureID.BrickTileBG7] = content.Load<Texture2D>("Tiles/BrickTileBG7");

        Texture[TextureID.IronBarsTile1] = content.Load<Texture2D>("Tiles/IronBarsTile1");

        #endregion


        #region Attacks

        // SWORD BERRIE ATTACK
        TextureList[TextureListID.SwordBerrie] = [
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack1"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack2"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack3"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack4"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack5"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack6"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack7"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack8"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack9"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack10"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack11"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack12"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack13"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack14"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack15"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack16"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack17"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack18"),
            content.Load<Texture2D>("Attacks/SwordBerrie/Attack19"),
        ];

        // FIREBALL VANKO ATTACK
        TextureList[TextureListID.FireballVanko] = [
            content.Load<Texture2D>("Attacks/FireballVanko/Attack11"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack11"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack9"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack7"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack4"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack1"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack2"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack3"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack4"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack5"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack6"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack7"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack8"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack9"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack10"),
            content.Load<Texture2D>("Attacks/FireballVanko/Attack11"),
        ];

        // GUNSHOT ROMAN ATTACK
        TextureList[TextureListID.GunshotRoman] = [
            content.Load<Texture2D>("Attacks/GunshotRoman/Attack1"),
            content.Load<Texture2D>("Attacks/GunshotRoman/Attack2"),
            content.Load<Texture2D>("Attacks/GunshotRoman/Attack3"),
        ];

        //Witch Attack
        TextureList[TextureListID.WitchDefault] = [
            content.Load<Texture2D>("Attacks/Thunderbolt/Thunderbolt")
        ];
        #endregion

        #region Players

        // BERRIE
        TextureList[TextureListID.BerrieDefault] = [
            content.Load<Texture2D>("Berrie/Stand1")
        ];
        TextureList[TextureListID.BerrieMoving] = [
            content.Load<Texture2D>("Berrie/Run1"),
            content.Load<Texture2D>("Berrie/Run2"),
            content.Load<Texture2D>("Berrie/Run3"),
            content.Load<Texture2D>("Berrie/Run4"),
            content.Load<Texture2D>("Berrie/Run5"),
            content.Load<Texture2D>("Berrie/Run6"),
            content.Load<Texture2D>("Berrie/Run7"),
            content.Load<Texture2D>("Berrie/Run8"),
            content.Load<Texture2D>("Berrie/Run9"),
            content.Load<Texture2D>("Berrie/Run10"),
            content.Load<Texture2D>("Berrie/Run11"),
            content.Load<Texture2D>("Berrie/Run12"),
        ];
        TextureList[TextureListID.BerrieJumping] = [
            content.Load<Texture2D>("Berrie/Jump1"),
        ];
        TextureList[TextureListID.BerrieAttacking] = [
            content.Load<Texture2D>("Berrie/Attack1"),
            content.Load<Texture2D>("Berrie/Attack2"),
            content.Load<Texture2D>("Berrie/Attack3"),
            content.Load<Texture2D>("Berrie/Attack4"),
            content.Load<Texture2D>("Berrie/Attack5"),
            content.Load<Texture2D>("Berrie/Attack6"),
            content.Load<Texture2D>("Berrie/Attack7"),
            content.Load<Texture2D>("Berrie/Attack8"),
            content.Load<Texture2D>("Berrie/Attack9"),
            content.Load<Texture2D>("Berrie/Attack10"),
            content.Load<Texture2D>("Berrie/Attack11"),
            content.Load<Texture2D>("Berrie/Attack12"),
            content.Load<Texture2D>("Berrie/Attack13"),
            content.Load<Texture2D>("Berrie/Attack14"),
            content.Load<Texture2D>("Berrie/Attack15"),
            content.Load<Texture2D>("Berrie/Attack16"),
            content.Load<Texture2D>("Berrie/Attack17"),
            content.Load<Texture2D>("Berrie/Attack18"),
            content.Load<Texture2D>("Berrie/Attack19"),
        ];

        // ROMAN
        TextureList[TextureListID.RomanDefault] = [
            content.Load<Texture2D>("Roman/Stand1")
        ];
        TextureList[TextureListID.RomanMoving] = [
            content.Load<Texture2D>("Roman/Run1"),
            content.Load<Texture2D>("Roman/Run2"),
            content.Load<Texture2D>("Roman/Run3"),
            content.Load<Texture2D>("Roman/Run4"),
            content.Load<Texture2D>("Roman/Run5"),
            content.Load<Texture2D>("Roman/Run6"),
            content.Load<Texture2D>("Roman/Run7"),
            content.Load<Texture2D>("Roman/Run8"),
            content.Load<Texture2D>("Roman/Run9"),
            content.Load<Texture2D>("Roman/Run10"),
        ];
        TextureList[TextureListID.RomanJumping] = [
            content.Load<Texture2D>("Roman/Jump1"),
        ];
        TextureList[TextureListID.RomanAttacking] = [
            content.Load<Texture2D>("Roman/Attack1"),
            content.Load<Texture2D>("Roman/Attack2"),
            content.Load<Texture2D>("Roman/Attack3"),
            content.Load<Texture2D>("Roman/Attack4"),
            content.Load<Texture2D>("Roman/Attack5"),
            content.Load<Texture2D>("Roman/Attack6"),
        ];

        // VANKO
        TextureList[TextureListID.VankoDefault] = [
            content.Load<Texture2D>("Vanko/Stand1"),
        ];
        TextureList[TextureListID.VankoMoving] = [
            content.Load<Texture2D>("Vanko/Run1"),
            content.Load<Texture2D>("Vanko/Run2"),
            content.Load<Texture2D>("Vanko/Run3"),
            content.Load<Texture2D>("Vanko/Run4"),
            content.Load<Texture2D>("Vanko/Run5"),
            content.Load<Texture2D>("Vanko/Run6"),
            content.Load<Texture2D>("Vanko/Run7"),
            content.Load<Texture2D>("Vanko/Run8"),
            content.Load<Texture2D>("Vanko/Run9"),
            content.Load<Texture2D>("Vanko/Run10"),
            content.Load<Texture2D>("Vanko/Run11"),
            content.Load<Texture2D>("Vanko/Run12"),
        ];
        TextureList[TextureListID.VankoJumping] = [
            content.Load<Texture2D>("Vanko/Jump1"),
        ];
        TextureList[TextureListID.VankoAttacking] = [
            content.Load<Texture2D>("Vanko/Attack1"),
            content.Load<Texture2D>("Vanko/Attack2"),
            content.Load<Texture2D>("Vanko/Attack3"),
            content.Load<Texture2D>("Vanko/Attack4"),
        ];
        #endregion

        #region PlayerObjects
        TextureList[TextureListID.Scope] = [
            content.Load<Texture2D>("PlayerObjects/Scope/Scope"),
        ];
        #endregion


        #region Enemies

        // SPIDER
        TextureList[TextureListID.SpiderDefault] = [
             content.Load<Texture2D>("Spider/Move1"),
        ];
        TextureList[TextureListID.SpiderMoving] = [
            content.Load<Texture2D>("Spider/Move1"),
            content.Load<Texture2D>("Spider/Move2"),
            content.Load<Texture2D>("Spider/Move3"),
            content.Load<Texture2D>("Spider/Move4"),
            content.Load<Texture2D>("Spider/Move3"),
            content.Load<Texture2D>("Spider/Move2"),
        ];

        // GHOST
        TextureList[TextureListID.GhostDefault] = [
             content.Load<Texture2D>("Ghost/Move1"),
        ];
        TextureList[TextureListID.GhostMoving] = [
            content.Load<Texture2D>("Ghost/Move1"),
            content.Load<Texture2D>("Ghost/Move2"),
            content.Load<Texture2D>("Ghost/Move3"),
            content.Load<Texture2D>("Ghost/Move4"),
            content.Load<Texture2D>("Ghost/Move5"),
            content.Load<Texture2D>("Ghost/Move6"),
            content.Load<Texture2D>("Ghost/Move7"),
            content.Load<Texture2D>("Ghost/Move8"),
            content.Load<Texture2D>("Ghost/Move9"),
        ];

        // WITCH
        TextureList[TextureListID.WitchDefault] = [
             content.Load<Texture2D>("Witch/WM"),
        ];
        TextureList[TextureListID.WitchMoving] = [
            content.Load<Texture2D>("Witch/WM"),
        ];
        #endregion
    }
}