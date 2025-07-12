using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModJamJul2025.Tiles;

namespace ModJamJul2025.Items
{
    public class GalactaKnightRelic : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeable";
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<GalactaKnightRelicTile>());

            Item.width = 30;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.buyPrice(0, 5);
        }
    }
}
