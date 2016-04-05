using Sodium;
using System;
using System.Linq;
using System.Net.Sockets;

namespace UCP
{
    public class ClientCrypto : Protocol
    {
        protected static byte[] serverKey =
            Utilities.HexToBinary("150C52DB12BA1C9DD809B8934A535F428A91B7B61E15AB469E42B9614C76A325");

        protected KeyPair clientKey = PublicKeyBox.GenerateKeyPair();

        public static void DecryptPacket(Socket socket, ClientState state, byte[] packet)
        {
            var messageId = BitConverter.ToInt32(new byte[2].Concat(packet.Take(2)).Reverse().ToArray(), 0);
            var payloadLength = BitConverter.ToInt32(new byte[1].Concat(packet.Skip(2).Take(3)).Reverse().ToArray(), 0);
            var unknown = BitConverter.ToInt32(new byte[2].Concat(packet.Skip(2).Skip(3).Take(2)).Reverse().ToArray(), 0);
            var cipherText = packet.Skip(2).Skip(3).Skip(2).ToArray();
            byte[] plainText;

            if (messageId == 20100)
            {
                plainText = cipherText;
            }
            else if (messageId == 20104)
            {
                var nonce =
                    GenericHash.Hash(state.nonce.Concat(state.clientKey.PublicKey).Concat(state.serverKey).ToArray(),
                        null, 24);
                plainText = PublicKeyBox.Open(cipherText, nonce, state.clientKey.PrivateKey, state.serverKey);
                state.serverState.nonce = plainText.Take(24).ToArray();
                state.serverState.sharedKey = plainText.Skip(24).Take(32).ToArray();
                plainText = plainText.Skip(24).Skip(32).ToArray();
            }
            else
            {
                state.serverState.nonce = Utilities.Increment(Utilities.Increment(state.serverState.nonce));
                plainText = SecretBox.Open(new byte[16].Concat(cipherText).ToArray(), state.serverState.nonce,
                    state.serverState.sharedKey);
            }
            Console.WriteLine("[UCR]    {0} " + Environment.NewLine + "{1}", PacketInfos.GetPacketName(messageId),
                Utilities.BinaryToHex(packet.Take(7).ToArray()) + Utilities.BinaryToHex(plainText));
            ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
        }

        public static void EncryptPacket(Socket socket, ClientState state, int messageId, int unknown, byte[] plainText)
        {
            byte[] cipherText;
            if (messageId == 10100)
            {
                cipherText = plainText;
            }
            else if (messageId == 10101)
            {
                var nonce = GenericHash.Hash(state.clientKey.PublicKey.Concat(state.serverKey).ToArray(), null, 24);
                plainText = state.serverState.sessionKey.Concat(state.nonce).Concat(plainText).ToArray();
                cipherText = PublicKeyBox.Create(plainText, nonce, state.clientKey.PrivateKey, state.serverKey);
                cipherText = state.clientKey.PublicKey.Concat(cipherText).ToArray();
            }
            else
            {
                cipherText = SecretBox.Create(plainText, state.nonce, state.serverState.sharedKey).Skip(16).ToArray();
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