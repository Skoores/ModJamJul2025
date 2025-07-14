using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KirbyMod.Tiles;

namespace KirbyMod.Items
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
