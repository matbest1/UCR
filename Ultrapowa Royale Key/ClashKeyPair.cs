using Sodium;
using System;

namespace UCK
{
    // This class is disposable
    public class ClashKeyPair : IDisposable
    {
        // This is the key length ( constant )
        public const int KeyLength = 32;

        // This is the nonce length ( constant )
        public const int NonceLength = 24;

        // Disposed or no, who know ?
        private bool _disposed;

        // Storing keypair
        private readonly KeyPair _keyPair;

        public ClashKeyPair(byte[] publicKey, byte[] privateKey)
        {
            if (publicKey == null)
                // If the public key is empty, something wrong
                throw new ArgumentNullException(nameof(publicKey));
            if (publicKey.Length != PublicKeyBox.PublicKeyBytes)
                // If the public key length is not 32 bytes length, something wrong
                throw new ArgumentOutOfRangeException(nameof(publicKey), "publicKey must be 32 bytes in length.");

            if (privateKey == null)
                // If private key is empty, something wrong
                throw new ArgumentNullException(nameof(privateKey));
            if (privateKey.Length != PublicKeyBox.SecretKeyBytes)
                // If private key length is not 32 bytes, something wrong
                throw new ArgumentOutOfRangeException(nameof(privateKey), "publicKey must be 32 bytes in length.");

            // We return a keypair
            _keyPair = new KeyPair(publicKey, privateKey);
        }

        // The private key of server
        public byte[] PrivateKey
        {
            get
            {
                if (_disposed)
                    // If the function is already disposed, we can't access to it
                    throw new ObjectDisposedException(null, "Cannot access CoCKeyPair object because it was disposed.");
                // We return the private key of the generated keypair
                return _keyPair.PrivateKey;
            }
        }

        // The public key of server
        public byte[] PublicKey
        {
            get
            {
                if (_disposed)
                    // If the function is already dispoed, we can't access to the key
                    throw new ObjectDisposedException(null, "Cannot access CoCKeyPair object because it was disposed.");

                // We return the public key from the generated keypair
                return _keyPair.PublicKey;
            }
        }

        // Function for dispose the class
        public void Dispose()
        {
            if (_disposed)
                // If function already disposed, no need to do it again
                return;

            _keyPair.Dispose();
            // We dispose the keypair ( We suppress it from memory )
            _disposed = true;
            // We set the boolean var to true
            GC.SuppressFinalize(this);
            // Garbage Collector is collecting all useless data dropped
        }
    }
}