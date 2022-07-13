
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

using DivergencyMod.Items.Armors;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace DivergencyMod.Items.Accs.Forest
{
	public class PortableTree : ModItem
	{

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("mogus");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
            


		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed -= 0.15f;
			player.GetModPlayer<TreePlayer>().treeNear = 300;
            player.GetModPlayer<DrawTreeBackPlayer>().drawtree = true;
		}



	}
    public class DrawTreeBackPlayer : ModPlayer
    {
        public bool drawtree;
        public override void ResetEffects()
        {
            drawtree = false;
        }
    }
    public class DrawTreeBack : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(PortableTree), EquipType.Back);
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {

            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.GetModPlayer<DrawTreeBackPlayer>().drawtree)
            {

                Color color = drawPlayer.GetImmuneAlphaPure(drawInfo.colorArmorBody, drawInfo.shadow);

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DivergencyMod/Items/Accs/Forest/PortableTree_Back").Value;
                Vector2 Position = drawInfo.Position;
                Vector2 origin = new(texture.Width * 0.5f, texture.Height * 0.5f);
                Vector2 drawPos = new Vector2((int)(Position.X - drawPlayer.bodyFrame.Width / 2 + drawPlayer.width / 2), (int)(Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 20f)) + drawPlayer.bodyPosition + new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2);
                drawPos.X += drawPlayer.direction == 1 ? -3 : 3;
                drawPos.Y -= 70 * drawPlayer.gravDir;
                DrawData drawData = new(texture, drawPos + (Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height] * drawPlayer.gravDir) - Main.screenPosition, new Rectangle?(), color, drawInfo.drawPlayer.headRotation, origin, 1, drawInfo.playerEffect, 0)
                {
                    shader = drawInfo.cHead
                };
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

	
}