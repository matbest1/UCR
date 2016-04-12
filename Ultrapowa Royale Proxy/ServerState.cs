using Sodium;

namespace UCP
{
    public class ServerState : State
    {
        public byte[] clientKey, nonce, sessionKey, sharedKey;
        public ClientState clientState;

        public KeyPair serverKey;
    }
}