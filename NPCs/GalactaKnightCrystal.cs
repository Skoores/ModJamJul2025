using Microsoft.Xna.Framework;
using KirbyMod.Systems;
using KirbyMod.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KirbyMod.NPCs
{
    //[AutoloadBossHead]
    public class GalactaKnightCrystal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 150;
            NPC.defense = 100;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.SpawnWithHigherTime(30);

            NPC.aiStyle = 94;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public static bool CanSave()
        {
            return true;
        }

        public override void OnKill()
        {
            int type = ModContent.NPCType<GalactaKnight>();
            int spawnX = (int)NPC.position.X + (TextureAssets.Npc[NPC.type].Value.Width / 2);
            int spawnY = (int)NPC.position.Y + (TextureAssets.Npc[NPC.type].Value.Height / 2);

            NPC.NewNPC(NPC.GetSource_FromAI(), spawnX, spawnY, type);
        }
    }
}
