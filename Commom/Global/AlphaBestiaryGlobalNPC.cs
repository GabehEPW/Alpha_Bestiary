using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Common.Global
{
    public class AlphaBestiaryGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int lastHitItemType = 0;
        private int lastHitPlayerWhoAmI = -1;

        private bool IsValidModWeapon(Item item)
        {
            return item != null &&
                   !item.IsAir &&
                   item.ModItem != null &&
                   item.ModItem.Mod == Mod;
        }

        private bool CanCountForBestiaryProgress(NPC npc)
        {
            if (npc == null || !npc.active)
                return false;

            if (npc.lifeMax <= 5)
                return false;

            if (npc.friendly)
                return false;

            if (npc.townNPC)
                return false;

            if (npc.type <= NPCID.None)
                return false;

            // Descomente se quiser impedir farm com estátua
            // if (npc.SpawnedFromStatue)
            //     return false;

            return true;
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByItem(npc, player, item, hit, damageDone);

            if (!CanCountForBestiaryProgress(npc))
                return;

            if (player == null || !player.active)
                return;

            if (IsValidModWeapon(item))
            {
                lastHitItemType = item.type;
                lastHitPlayerWhoAmI = player.whoAmI;
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByProjectile(npc, projectile, hit, damageDone);

            if (!CanCountForBestiaryProgress(npc))
                return;

            if (projectile == null || !projectile.active)
                return;

            var globalProj = projectile.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            if (globalProj.sourcePlayerWhoAmI < 0 || globalProj.sourcePlayerWhoAmI >= Main.maxPlayers)
                return;

            if (globalProj.sourceItemType == 0)
                return;

            Player player = Main.player[globalProj.sourcePlayerWhoAmI];
            if (player == null || !player.active)
                return;

            lastHitItemType = globalProj.sourceItemType;
            lastHitPlayerWhoAmI = globalProj.sourcePlayerWhoAmI;
        }

        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            if (!CanCountForBestiaryProgress(npc))
                return;

            if (lastHitPlayerWhoAmI < 0 || lastHitPlayerWhoAmI >= Main.maxPlayers)
                return;

            if (lastHitItemType == 0)
                return;

            Player player = Main.player[lastHitPlayerWhoAmI];
            if (player == null || !player.active)
                return;

            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            bool wasNewKill = modPlayer.RegisterKill(lastHitItemType, npc.type);

            if (wasNewKill && Main.myPlayer == player.whoAmI)
            {
                int newLevel = modPlayer.GetWeaponLevel(lastHitItemType);
                Main.NewText($"Nova criatura registrada! Nível da arma: {newLevel}");
            }

            lastHitItemType = 0;
            lastHitPlayerWhoAmI = -1;
        }
    }
}