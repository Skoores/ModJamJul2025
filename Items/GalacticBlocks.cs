using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KirbyMod.Tiles;

namespace KirbyMod.Items
{
    public abstract class GalacticBlock : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeable";
        public override void SetStaticDefaults()
        {
	        Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Type] = true;
        }
    }
    public class GalacticRock : GalacticBlock
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToPlaceableTile(ModContent.TileType<GalacticRockTile>());
        }
    }
    public class GalacticCrystal : GalacticBlock
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToPlaceableTile(ModContent.TileType<GalacticCrystalTile>());
        }
    }
}
