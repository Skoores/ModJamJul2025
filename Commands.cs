using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using KirbyMod.Tiles;

namespace KirbyMod
{
    public class ChangeToMurkstone : ModCommand
    {
        // CommandType.Chat means that command can be used in Chat in SP and MP
        public override CommandType Type
            => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command
            => "changetogalactic";

        // A short description of this command
        public override string Description
            => "Replace designated blocks with Galactic blocks";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            // Execute
            for (int x = 0; x <= Main.maxTilesX; x++)
            {
                for (int y = 0; y <= Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    Tile wall = Main.tile[x, y];
                    if (tile.TileType == TileID.Waterfall)
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<GalacticRockTile>();
                    if (tile.TileType == TileID.Lead)
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<GalacticCrystalTile>();
                }
            }
        }
    }

    public class FillWillNullTiles : ModCommand
    {
        // CommandType.Chat means that command can be used in Chat in SP and MP
        public override CommandType Type
            => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command
            => "fillnull";

        // A short description of this command
        public override string Description
            => "Fills the world with null tiles";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Mod structureHelper = KirbyMod.Instance.structureHelper;
            structureHelper.TryFind("NullBlock", out ModTile nullBlock);
            structureHelper.TryFind("NullWall", out ModWall nullWall);

            // Execute
            for (int x = 0; x <= Main.maxTilesX; x++)
            {
                for (int y = 0; y <= Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    Tile wall = Main.tile[x, y];

                    if (tile.TileType == TileID.RedStucco && (wall.WallType == 0 || wall.WallType == WallID.RedStucco))
                    {
                        Main.tile[x, y].TileType = nullBlock.Type;
                        Main.tile[x, y].WallType = nullWall.Type;
                    }

                    if ((wall.WallType == 0 && tile.TileType >= 1) || wall.WallType == WallID.RedStucco)
                    {
                        Main.tile[x, y].WallType = nullWall.Type;
                    }
                }
            }
        }
    }

    public class ClearNullTiles : ModCommand
    {
        // CommandType.Chat means that command can be used in Chat in SP and MP
        public override CommandType Type
            => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command
            => "clearnull";

        // A short description of this command
        public override string Description
            => "Clears the world of null tiles";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Mod structureHelper = KirbyMod.Instance.structureHelper;
            structureHelper.TryFind("NullBlock", out ModTile nullBlock);
            structureHelper.TryFind("NullWall", out ModWall nullWall);

            // Execute
            for (int x = 0; x <= Main.maxTilesX; x++)
            {
                for (int y = 0; y <= Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    Tile wall = Main.tile[x, y];

                    if (tile.TileType == nullBlock.Type || wall.WallType == nullWall.Type)
                        Main.tile[x, y].ClearEverything();
                    /*
                    if (wall.WallType == nullWall.Type)
                        WorldGen.KillWall(x, y);
                    */
                }
            }
        }
    }
}