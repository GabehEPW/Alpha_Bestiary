using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;
using System.Collections.Generic;

public class VitalCrystalWorld : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int index = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

        if (index != -1)
        {
            tasks.Insert(index + 1, new PassLegacy("Vital Crystal", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Spawning Vital Crystals";

                for (int i = 0; i < Main.maxTilesX * Main.maxTilesY * 0.00012; i++)
                {
                    WorldGen.TileRunner(
                        WorldGen.genRand.Next(Main.maxTilesX),
                        WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, (int)Main.rockLayer),
                        WorldGen.genRand.Next(3, 6),
                        WorldGen.genRand.Next(3, 6),
                        ModContent.TileType<VitalCrystalTile>()
                    );
                }
            }));
        }
    }
}