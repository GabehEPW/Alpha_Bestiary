using Terraria.ModLoader;

namespace BestiaryAlpha.Common.Global
{
    public class AlphaBestiaryGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public int sourceItemType = 0;
        public int sourcePlayerWhoAmI = -1;
    }
}