using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModJamJul2025.Tiles
{
    public abstract class GalacticTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
            Main.tileMergeDirt[Type] = true;

            MinPick = 1000;
            MineResist = 1f;
            HitSound = SoundID.Tink;
            DustType = DustID.GemSapphire;

            Main.tileMerge[ModContent.TileType<GalacticRockTile>()][Type] = true;
            Main.tileMerge[ModContent.TileType<GalacticCrystalTile>()][Type] = true;

            AddMapEntry(new Color(49, 88, 232));
        }
    }
    public class GalacticRockTile : GalacticTile
    {
    }

    public class GalacticCrystalTile : GalacticTile
    {
    }
}
