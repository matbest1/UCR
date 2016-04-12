using Sodium;
using System;
using System.Linq;
using System.Net.Sockets;
using UCP;

namespace UCP
{
    public class ServerCrypto : Protocol
    {
        protected static KeyPair serverKey =
            PublicKeyBox.GenerateKeyPair(
                Utilities.HexToBinary("1891D401FADB51D25D3A9174D472A9F691A45B974285D47729C45C6538070D85"));

        public static void DecryptPacket(Socket socket, ServerState state, byte[] packet)
        {
            var messageId = BitConverter.ToInt32(new byte[2].Concat(packet.Take(2)).Reverse().ToArray(), 0);
            var payloadLength = BitConverter.ToInt32(new byte[1].Concat(packet.Skip(2).Take(3)).Reverse().ToArray(), 0);
            var unknown = BitConverter.ToInt32(new byte[2].Concat(packet.Skip(2).Skip(3).Take(2)).Reverse().ToArray(), 0);
            var cipherText = packet.Skip(2).Skip(3).Skip(2).ToArray();
            byte[] plainText;

            if (messageId == 10100)
            {
                plainText = cipherText;
            }
            else if (messageId == 10101)
            {
                state.clientKey = cipherText.Take(32).ToArray();
                var nonce = GenericHash.Hash(state.clientKey.Concat(state.serverKey.PublicKey).ToArray(), null, 24);
                cipherText = cipherText.Skip(32).ToArray();
                plainText = PublicKeyBox.Open(cipherText, nonce, state.serverKey.PrivateKey, state.clientKey);
                state.sessionKey = plainText.Take(24).ToArray();
                state.clientState.nonce = plainText.Skip(24).Take(24).ToArray();
                plainText = plainText.Skip(24).Skip(24).ToArray();
            }
            else
            {
                state.clientState.nonce = Utilities.Increment(Utilities.Increment(state.clientState.nonce));
                plainText = SecretBox.Open(new byte[16].Concat(cipherText).ToArray(), state.clientState.nonce,
                    state.sharedKey);
            }
            Console.WriteLine("[UCR]    {0}" + Environment.NewLine + "{1}", PacketInfos.GetPacketName(messageId),
                Utilities.BinaryToHex(packet.Take(7).ToArray()) + Utilities.BinaryToHex(plainText));
            ClientCrypto.EncryptPacket(state.clientState.socket, state.clientState, messageId, unknown, plainText);
        }

        public static void EncryptPacket(Socket socket, ServerState state, int messageId, int unknown, byte[] plainText)
        {
            byte[] cipherText;
            if (messageId == 20100)
            {
                cipherText = plainText;
            }
            else if (messageId == 20104)
            {
                var nonce =
                    GenericHash.Hash(
                        state.clientState.nonce.Concat(state.clientKey).Concat(state.serverKey.PublicKey).ToArray(),
                        null, 24);
                plainText = state.nonce.Concat(state.sharedKey).Concat(plainText).ToArray();
                cipherText = PublicKeyBox.Create(plainText, nonce, state.serverKey.PrivateKey, state.clientKey);
            }
            else
            {
                cipherText = SecretBox.Create(plainText, state.nonce, state.sharedKey).Skip(16).ToArray();
            }
            var packet =
                BitConverter.GetBytes(messageId)
                    .Reverse()
                    .Skip(2)
                    .Concat(BitConverter.GetBytes(cipherText.Length).Reverse().Skip(1))
                    .Concat(BitConverter.GetBytes(unknown).Reverse().Skip(2))
                    .Concat(cipherText)
                    .ToArray();
            socket.BeginSend(packet, 0, packet.Length, 0, SendCallback, state);
        }
    }
}