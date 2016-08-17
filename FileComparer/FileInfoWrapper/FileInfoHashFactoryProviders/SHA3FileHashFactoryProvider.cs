namespace FileComparer
{
    using System;
    using System.IO;

    using Org.BouncyCastle.Crypto.Digests;

    public class SHA3FileHashFactoryProvider : IFileHashFactoryProvider
    {
        public Func<FileInfo, byte[]> HashFactory { get; } = fileInfo =>
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    BinaryReader br = new BinaryReader(fileStream);
                    var length = (int)fileStream.Length;
                    var d = new Sha3Digest(224);
                    byte[] output = new byte[d.GetDigestSize()];

                    byte[] m = br.ReadBytes(length);
                    d.BlockUpdate(m, 0, m.Length);
                    d.DoFinal(output, 0);
                    return output;
                }
            };
    }
}