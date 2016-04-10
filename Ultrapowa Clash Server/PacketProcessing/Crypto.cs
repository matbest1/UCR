using Sodium;
using System;

namespace UCS.PacketProcessing
{
    // This class is disposable
    public class Crypto : IDisposable
    {
        // This is the key length ( constant )
        public const int KeyLength = 32;

        // This is the nonce length ( constant )
        public const int NonceLength = 24;

        // Disposed or no, who know ?
        private bool _disposed;

        // Storing keypair
        private readonly KeyPair _keyPair;

        public Crypto(byte[] publicKey, byte[] privateKey)
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
    public static class Key
    {
        // We store the standard private key in a byte array
        private static readonly byte[] _standardPrivateKey =
        {
            0x18, 0x91, 0xD4, 0x01, 0xFA, 0xDB, 0x51, 0xD2, 0x5D, 0x3A, 0x91, 0x74,
            0xD4, 0x72, 0xA9, 0xF6, 0x91, 0xA4, 0x5B, 0x97, 0x42, 0x85, 0xD4, 0x77,
            0x29, 0xC4, 0x5C, 0x65, 0x38, 0x07, 0x0D, 0x85
        };

        // We store the standard public key in a byte array
        private static readonly byte[] _standardPublicKey =
        {
            0x72, 0xF1, 0xA4, 0xA4, 0xC4, 0x8E, 0x44, 0xDA, 0x0C, 0x42, 0x31, 0x0F,
            0x80, 0x0E, 0x96, 0x62, 0x4E, 0x6D, 0xC6, 0xA6, 0x41, 0xA9, 0xD4, 0x1C,
            0x3B, 0x50, 0x39, 0xD8, 0xDF, 0xAD, 0xC2, 0x7E
        };

        public static Crypto Crypto
        {
            // We return a keypair , public key + private key
            get { return new Crypto((byte[])_standardPublicKey.Clone(), (byte[])_standardPrivateKey.Clone()); }
        }
    }
}