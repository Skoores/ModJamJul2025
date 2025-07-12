using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ModJamJul2025
{
	public class ModJamJul2025 : Mod
	{
        internal static ModJamJul2025 Instance;
        internal static Mod mod;

        internal Mod structureHelper = null;

        public override void Load()
        {
            Instance = this;

            structureHelper = null;
            ModLoader.TryGetMod("StructureHelper", out structureHelper);
        }

        public override void Unload()
        {
            structureHelper = null;
        }
    }
}
