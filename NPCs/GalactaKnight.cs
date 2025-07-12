using ModJamJul2025.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ModJamJul2025.NPCs
{
    [AutoloadBossHead]
    public class GalactaKnight : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 76;
            NPC.damage = 12;
            NPC.defense = 100;
            NPC.lifeMax = 150000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;

            NPC.aiStyle = -1;

            //boss bar goes here once we make it

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Music/GalactaKnightTheme");
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //no bitches?
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.resealedGalactaKnight, -1);
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void AI()
        {
            
        }
    }

}
