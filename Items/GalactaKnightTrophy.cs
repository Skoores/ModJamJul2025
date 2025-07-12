using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModJamJul2025.Tiles;

namespace ModJamJul2025.Items
{
    public class GalactaKnightTrophy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeable";
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<GalactaKnightTrophyTile>());

            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1);
        }
    }
}
