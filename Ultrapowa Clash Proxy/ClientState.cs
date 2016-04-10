using Sodium;

namespace UCP
{
    public class ClientState : State
    {
        public KeyPair clientKey;
        public byte[] serverKey, nonce;
        public ServerState serverState;
    }
}