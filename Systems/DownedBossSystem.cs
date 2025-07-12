using System.Collections;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
namespace ModJamJul2025.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool resealedGalactaKnight = false;
        public override void ClearWorld()
        {
            resealedGalactaKnight = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (resealedGalactaKnight)
            {
                tag["resealedGalactaKnight"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag) {
			resealedGalactaKnight = tag.ContainsKey("resealedGalactaKnight");
		}

        public override void NetSend(BinaryWriter writer) {
			writer.WriteFlags(resealedGalactaKnight);
			}

		public override void NetReceive(BinaryReader reader) {
			reader.ReadFlags(out resealedGalactaKnight);
		}
    }
}