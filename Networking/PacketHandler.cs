using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Networking
{
	internal abstract class PacketHandler
	{
		internal byte HandlerType { get; set; }

		public abstract void HandlePacket(BinaryReader reader, int fromWho);

		protected PacketHandler(byte handlerType)
		{
			HandlerType = handlerType;
		}

		protected ModPacket GetPacket(byte packetType, int fromWho)
		{
			ModPacket packet = JoJoStands.Instance.GetPacket();
			packet.Write(HandlerType);
			packet.Write(packetType);
			/*if (Main.netMode == NetmodeID.Server)
			{
				packet.Write((byte)fromWho);
			}*/
			return packet;
		}
	}
}