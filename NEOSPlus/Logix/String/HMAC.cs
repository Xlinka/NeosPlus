using System;
using System.Security.Cryptography;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

public enum HashFunction
{
    MD5,
    SHA1,
    SHA256,
    SHA384,
    SHA512
}

[Category("LogiX/NeosPlus/String")]
[NodeName("HMAC")]
public class HMACNode : LogixNode
{
    public readonly Input<string> Message;
    public readonly Input<string> Key;
    public readonly Input<HashFunction> HashAlgorithm;

    public readonly Output<string> Result;

    protected override void OnEvaluate()
    {
        string message = Message.Evaluate();
        string key = Key.Evaluate();
        HashFunction hashFunction = HashAlgorithm.Evaluate();

        if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(key))
        {
            Result.Value = "";
            return;
        }

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);

        using (HMAC hmac = GetHMAC(hashFunction, keyBytes))
        {
            byte[] hash = hmac.ComputeHash(messageBytes);
            Result.Value = BitConverter.ToString(hash).Replace("-", "");
        }
    }

    private HMAC GetHMAC(HashFunction hashFunction, byte[] key)
    {
        switch (hashFunction)
        {
            case HashFunction.MD5:
                return new HMACMD5(key);
            case HashFunction.SHA1:
                return new HMACSHA1(key);
            case HashFunction.SHA256:
                return new HMACSHA256(key);
            case HashFunction.SHA384:
                return new HMACSHA384(key);
            case HashFunction.SHA512:
                return new HMACSHA512(key);
            default:
                throw new ArgumentException($"Unsupported hash function {hashFunction}");
        }
    }
}
