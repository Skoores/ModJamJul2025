using Microsoft.Xna.Framework;
using ModJamJul2025.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ModJamJul2025.NPCs
{
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
            NPC.damage = 12;
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

        public override void OnKill()
        {
            int type = ModContent.NPCType<GalactaKnight>();
            int spawnX = (int)NPC.position.X + (TextureAssets.Npc[NPC.type].Value.Width / 2);
            int spawnY = (int)NPC.position.Y + (TextureAssets.Npc[NPC.type].Value.Height / 2);

            NPC.NewNPC(NPC.GetSource_FromAI(), spawnX, spawnY, type);
        }
    }

}
