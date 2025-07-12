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
        private float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        List<float> phasePick = new List<float>
        {
            1f, 1f, 1f, 2f
        };

        Random rnd = new Random();
        
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
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.GalactaKnightTrophy>(), 10));

            //LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            //notExpertRule.OnSuccess(npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.BrokenLance>())));

            //notExpertRule.OnSuccess(npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.BrokenLance>(), 9)));

            //npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MinionBossBag>()));

            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.GalactaKnightRelic>()));
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

        public override void FindFrame(int frameHeight)
        {
            int startFrame = 1;
            int endFrame = 1;

            if (AIState == 1)
            {
                return;
            }

            if (AIState == 2f)
            {
                startFrame = 2;
                endFrame = 4;
            }

            if (NPC.frame.Y < startFrame * frameHeight)
            {
                NPC.frame.Y = startFrame * frameHeight;
            }

            int frameSpeed = 3;

            NPC.frameCounter += 1f;
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y > endFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (player.dead)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }

            AIState = phasePick[rnd.Next(phasePick.Count)];

            if (AIState == 1f)
            {
                floatMul(1);
            }
            else
            {
                floatMul(2);
            }
        }
        private void floatMul(float f){
            //Main.EntitySpriteDraw(GalactaKnightSide_Wings);
            return;
        }
    }

}
