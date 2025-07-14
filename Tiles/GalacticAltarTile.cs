using Microsoft.Xna.Framework;
using ModJamJul2025.NPCs;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace ModJamJul2025.Tiles
{
    public class GalacticAltarTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileHammeringIfOnTopOfIt[Type] = true;
            TileID.Sets.AvoidedByMeteorLanding[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.addTile(Type);

            MinPick = 1000;
            HitSound = SoundID.Tink;
            DustType = DustID.GemSapphire;

            AddMapEntry(new Color(49, 88, 232));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // When the tile is removed, we need to remove the Tile Entity as well.
            ModContent.GetInstance<AltarTileEntity>().Kill(i, j);
        }
    }

    public class AltarTileEntity : ModTileEntity
    {
        // IsTileValidForEntity is required, usually you can use this code directly.
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];

            return tile.HasTile && tile.TileType == ModContent.TileType<GalacticAltarTile>();
        }

        public override void Update()
        {
            base.Update();

            int crystal = ModContent.NPCType<GalactaKnightCrystal>();

            // Check if there's already one in the world
            if (NPC.AnyNPCs(crystal) || NPC.AnyNPCs(ModContent.NPCType<GalactaKnight>()))
                return;

            Vector2 altarPosition = Position.ToWorldCoordinates();

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                // Should only respawn if offscreen from all players (second line is to account for absent player coords still being stored out of bounds)
                if (Vector2.Distance(Main.player[i].Center, Position.ToWorldCoordinates()) > Main.screenWidth / 2 &&
                    Vector2.Distance(Main.player[i].Center, Position.ToWorldCoordinates()) < Main.screenWidth * 10)
                {
                    NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)altarPosition.X, (int)altarPosition.Y, crystal);
                    return;
                }
            }

            
        }
    }
}
