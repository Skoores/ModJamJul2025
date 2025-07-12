using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ModJamJul2025
{
    public class ArenaGen : ModSystem
    {
        // Generate biome after vanilla generation has completed
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new ArenaPass("Generating Arena", 100f));
            }
        }
    }

    public class ArenaPass : GenPass
    {
        public ArenaPass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            // Progress message    
            progress.Message = "Generating Arena";

            Mod mod = ModJamJul2025.Instance;
            StructureHelper.API.Generator.GenerateStructure("Structures/GalacticArena", new Point16(Main.spawnTileX - 100, 100), mod);
        }
    }
}
