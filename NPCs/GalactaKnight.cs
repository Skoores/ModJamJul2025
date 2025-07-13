using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModJamJul2025.Systems;
using ReLogic.Content;
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
    [AutoloadBossHead]
    public class GalactaKnight : ModNPC
    {
        //public override string Texture => "ModJamJul2025/NPCs/GalactaKnightSide";

        public static Asset<Texture2D> frontWings, sideWings, currentWings;

        public static readonly SoundStyle hitSound = new("ModJamJul2025/SFX/GalactaKnightHit");
        public static readonly SoundStyle deathSound = new("ModJamJul2025/SFX/GalactaKnightDeath");

        public int currentWingFrame, currentWingFrameY;

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
            Main.npcFrameCount[Type] = 12;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            if (!Main.dedServ)
            {
                sideWings = ModContent.Request<Texture2D>("ModJamJul2025/NPCs/GalactaKnightSide_Wings", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 12;
            NPC.defense = 100;
            NPC.lifeMax = 150000;
            NPC.HitSound = hitSound;
            NPC.DeathSound = deathSound;
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
            int startFrame = 0;
            int endFrame = 0;

            //if (AIState == 1)
            //{
            //    return;
            //}

            //if (AIState == 2f)
            //{
            //    startFrame = 2;
            //    endFrame = 4;

            //    //currentWings = sideWings;
            //}

            if (NPC.frame.Y < startFrame * frameHeight)
            {
                NPC.frame.Y = startFrame * frameHeight;
            }

            int frameSpeed = 3;

            NPC.frameCounter += 1;
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y > endFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }

            // wing animation
            int wingFrameHeight = 164;

            int wingStartFrame = 0;
            int wingEndFrame = 1;

            if (currentWingFrameY < startFrame * frameHeight)
            {
                currentWingFrameY = startFrame * frameHeight;
            }

            currentWingFrame += 1;
            if (currentWingFrame > frameSpeed)
            {
                currentWingFrame = 0;
                currentWingFrameY += wingFrameHeight;

                if (currentWingFrameY > wingEndFrame * wingFrameHeight)
                {
                    currentWingFrameY = wingStartFrame * wingFrameHeight;
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

            //if (AIState == 1f)
            //{
            //    DrawWings(1);
            //}
            //else
            //{
            //    DrawWings(2);
            //}
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int frameHeight = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            int frameWidth = TextureAssets.Npc[NPC.type].Value.Width;

            Rectangle wingFrame = new Rectangle(0, currentWingFrameY, frameWidth, frameHeight);
            Vector2 origin = NPC.Size * 0.5f;
            Vector2 center = NPC.Center - screenPos;

            Vector2 halfSizeTexture = new Vector2((TextureAssets.Npc[NPC.type].Value.Width / 2) + 10, (frameHeight / 2) + 4);

            Texture2D bossTexture = TextureAssets.Npc[NPC.type].Value;

            spriteBatch.Draw(sideWings.Value, center, wingFrame, drawColor, NPC.rotation, halfSizeTexture, NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(bossTexture, center, NPC.frame, drawColor, NPC.rotation, halfSizeTexture, NPC.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
