#Spritz.NET

C# implementation of Spritz, a spongy RC4-like stream cipher and hash function

## Quickstart
```
using SpritzDotNet;
```

### Encrypt
```
byte[] plaintext = Encoding.ASCII.GetBytes("hello world");
byte[] key = Encoding.ASCII.GetBytes("secret key");
byte[] ciphertext = Spritz.Encrypt(plaintext, key);
```

### Decrypt
```
byte[] cleartext = Spritz.Decrypt(ciphertext, key);
```

### Hash
```
byte[] hash = Spritz.Hash(plaintext, 32);
```
