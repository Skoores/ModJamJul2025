using KirbyMod.NPCs;
using KirbyMod.Tiles;
using System;
using System.Collections;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

namespace KirbyMod.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool resealedGalactaKnight = false;
        public override void ClearWorld()
        {
            resealedGalactaKnight = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (resealedGalactaKnight)
            {
                tag["resealedGalactaKnight"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            resealedGalactaKnight = tag.ContainsKey("resealedGalactaKnight");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(resealedGalactaKnight);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out resealedGalactaKnight);
        }
    }

    public class CrystalSpawnSystem : ModSystem
    {
        public override void PreUpdateWorld()
        {
            base.PreUpdateWorld();

            //int crystal = ModContent.NPCType<GalactaKnightCrystal>();

            //// Check if there's already one in the world
            //if (NPC.AnyNPCs(crystal) || NPC.AnyNPCs(ModContent.NPCType<GalactaKnight>()))
            //    return;

            //Vector2 altarPosition = ModContent.GetInstance<AltarTileEntity>().Position.ToWorldCoordinates();

            //NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)altarPosition.X, (int)altarPosition.Y, crystal);

            //for (int i = 0; i < Main.maxTilesX; i++)
            //{
            //    for (int j = 0; j < Main.maxTilesY; j++)
            //    {
            //        int spawnX = (i + 1) * 16;
            //        int spawnY = j * 16;

            //            if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == ModContent.TileType<GalacticAltarTile>()) 
            //                //&& (spawnX < Main.screenPosition.X || spawnX > Main.screenPosition.X + Main.screenWidth)
            //                //&& (spawnY < Main.screenPosition.Y || spawnY > Main.screenPosition.Y + Main.screenHeight))
            //        {
            //            NPC.NewNPC(NPC.GetSource_NaturalSpawn(), spawnX, spawnY, crystal);
            //            return;
            //        }
            //    }
            //}
        }
    }
}