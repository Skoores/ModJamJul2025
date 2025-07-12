using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModJamJul2025.Tiles;

namespace ModJamJul2025.Items
{
    public class GalacticAltar : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeable";
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<GalacticAltarTile>());
        }
    }
}
