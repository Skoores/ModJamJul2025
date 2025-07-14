using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyMod.Systems;
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

namespace KirbyMod.NPCs
{
    [AutoloadBossHead]
    public class GalactaKnight : ModNPC
    {
        //public override string Texture => "KirbyMod/NPCs/GalactaKnightSide";

        public static Asset<Texture2D> frontWings, sideWings, currentWings;

        public static readonly SoundStyle hitSound = new("KirbyMod/SFX/GalactaKnightHit");
        public static readonly SoundStyle deathSound = new("KirbyMod/SFX/GalactaKnightDeath");

        public int currentWingFrame, currentWingFrameY;

        public bool shouldFacePlayer;

        private float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        private float AITimerMax
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
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
                sideWings = ModContent.Request<Texture2D>("KirbyMod/NPCs/GalactaKnightSide_Wings", AssetRequestMode.AsyncLoad);
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

            AIState = 2;

            if (AIState == 1)
            {
                return;
            }

            if (AIState == 2)
            {
                startFrame = 2;
                endFrame = 4;

                //currentWings = sideWings;
            }

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

            if (currentWingFrameY < wingStartFrame * frameHeight)
            {
                currentWingFrameY = wingStartFrame * frameHeight;
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

            shouldFacePlayer = true;

            // Face player
            if (shouldFacePlayer)
            {
                int facePlayerDirection = Math.Sign(player.Center.X - NPC.Center.X);
                if (facePlayerDirection != 0)
                    NPC.direction = NPC.spriteDirection = facePlayerDirection;
            }
            

            //phasePick = [1f, 1f, 1f, 1f, 1f, 1f, 5f, 5f, 5f, 9f, 9f, 11f];

            /*if (Math.Abs(NPC.Center.X - player.Center.X) < 20)
            {
                phasePick = [9f];
                if (NPC.Center.Y - player.Center.Y > 10)
                {
                    phasePick.Add(7f);
                    phasePick.Add(7f);
                    phasePick.Add(7f);
                }
                else
                {
                    phasePick.Add(2f);
                    phasePick.Add(2f);
                    phasePick.Add(2f);
                }
            }

            AIState = phasePick[rnd.Next(phasePick.Count)];*/

            //if (NPC.lifeMax / 2 > NPC.life)
            //{
            //}

            AIState = 1f;
            
            AITimer = 0;
            switch (AIState)
            {
                case 2f:
                    
                    break;
                case 5f:
                    break;
                case 7f:
                    LanceSlash(player);
                    break;
                case 11f:
                    break;
                default:
                    FlyForward(player);
                    break;
            }
        }

        private void FlyForward(Player player)
        {
            float baseMovementSpeed = 10f;
            float accelaration = 0.2f;

            FlyToTarget(player, baseMovementSpeed, accelaration, out float distancetoPlayer);
            return;
        }

        private void FlyToTarget(Player player, float baseMovementSpeed, float accelaration, out float distancetoPlayer)
        {
            distancetoPlayer = Vector2.Distance(NPC.Center, player.Center);

            float movementSpeed = baseMovementSpeed / distancetoPlayer;

            float targetVelocityX = (player.Center.X - NPC.Center.X) * movementSpeed;
            float targetVelocityY = (player.Center.Y - NPC.Center.Y) * movementSpeed;


            if (NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X += accelaration;
            }
            else if (NPC.velocity.X > targetVelocityX)
            {
                NPC.velocity.X -= accelaration;
            }
            if (NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y += accelaration;
            }
            else if (NPC.velocity.Y > targetVelocityY)
            {
                NPC.velocity.Y -= accelaration;
            }
            AITimer += 1;

            if (Math.Abs(player.Center.X - NPC.Center.X) < 100 && Math.Abs(player.Center.Y - NPC.Center.Y) < 20f)
            {
                NPC.netUpdate = true;
                LanceSlash(player);
                AITimer = 0;
                NPC.netUpdate = true;
            }

            AITimerMax += rnd.Next(7000, 13000);
            if (AITimer >= AITimerMax)
            {
                NPC.netUpdate = true;
                LanceSlash(player);
                AITimer = 0;
                NPC.netUpdate = true;
                return;
            }
        }

        private void LanceSlash(Player player)
        {
            float baseSpeed = 8f;
            if (Main.expertMode)
            {
                baseSpeed = 10;
            }

            NPC.velocity *= 1.3f;


            NPC.netUpdate = true;
            if (NPC.netSpam > 10)
            {
                NPC.netSpam = 10;
            }

            NPC.velocity *= 0.8f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int frameWidth = TextureAssets.Npc[NPC.type].Value.Width;
            int frameHeight = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];

            Rectangle wingFrame = new Rectangle(0, currentWingFrameY, frameWidth, frameHeight);
            Vector2 origin = NPC.Size * 0.5f;
            Vector2 center = NPC.Center - screenPos;

            Vector2 halfSizeTextureLeft = new Vector2((TextureAssets.Npc[NPC.type].Value.Width / 2) + 10, (frameHeight / 2) + 4);
            Vector2 halfSizeTextureRight = new Vector2((TextureAssets.Npc[NPC.type].Value.Width / 2) - 10, (frameHeight / 2) + 4);
            Vector2 chosenOrigin;

            Texture2D bossTexture = TextureAssets.Npc[NPC.type].Value;

            SpriteEffects spriteDirection;
            
            if (NPC.spriteDirection == 1) // Facing right
            {
                spriteDirection = SpriteEffects.FlipHorizontally;
                chosenOrigin = halfSizeTextureRight;
            }
            else // Facing left
            {
                spriteDirection = SpriteEffects.None;
                chosenOrigin = halfSizeTextureLeft;
            }

            spriteBatch.Draw(sideWings.Value, center, wingFrame, drawColor, NPC.rotation, chosenOrigin, NPC.scale, spriteDirection, 0f);
            spriteBatch.Draw(bossTexture, center, NPC.frame, drawColor, NPC.rotation, chosenOrigin, NPC.scale, spriteDirection, 0f);
            return false;
        }
    }
}
