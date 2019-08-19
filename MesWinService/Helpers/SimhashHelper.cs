using System;
using System.Numerics;

namespace MesWinService.Helpers
{
    public class SimhashHelper
    {
        public class SimHash
        {
            private String tokens;
            private BigInteger strSimHash;
            private int hashbits = 128;

            public BigInteger StrSimHash
            {
                get
                {
                    return strSimHash;
                }
            }

            public SimHash(String tokens, int hashbits)
            {
                this.tokens = tokens;
                this.hashbits = hashbits;
                this.strSimHash = simHash();
            }
            public SimHash(String tokens)
            {
                this.tokens = tokens;
                this.strSimHash = simHash();
            }

            private BigInteger simHash()
            {
                int[] v = new int[this.hashbits];
                ChxTokenizer stringTokens = new ChxTokenizer(this.tokens);
                while (stringTokens.hasMoreTokens())
                {
                    String temp = stringTokens.nextToken();
                    BigInteger t = this.hash(temp);
                    //Console.WriteLine("temp = {0} ： {1}", temp, t);
                    for (int i = 0; i < this.hashbits; i++)
                    {
                        BigInteger bitmask = BigInteger.One << i;
                        if ((t & bitmask).Sign != 0)
                        {
                            v[i] += 1;
                        }
                        else
                        {
                            v[i] -= 1;
                        }
                    }
                }
                BigInteger fingerprint = BigInteger.Zero;
                for (int i = 0; i < this.hashbits; i++)
                {
                    if (v[i] >= 0)
                    {
                        fingerprint = fingerprint + (BigInteger.Parse("1") << i);
                    }
                }
                return fingerprint;
            }

            private BigInteger hash(string source)
            {
                if (source == null || source.Length == 0)
                {
                    return BigInteger.Zero;
                }
                else
                {
                    char[] sourceArray = source.ToCharArray();
                    long longsourceArray = (long)sourceArray[0];
                    //if (longsourceArray < 8)
                    //{
                    //    longsourceArray <<= 12;
                    //}
                    //if (longsourceArray < 16)
                    //{
                    //    longsourceArray <<= 11;
                    //}
                    //if (longsourceArray < 32)
                    //{
                    //    longsourceArray <<= 10;
                    //}
                    //else if (longsourceArray < 64)
                    //{
                    //    longsourceArray <<= 9;
                    //}
                    //else if (longsourceArray < 128)
                    //{
                    //    longsourceArray <<= 8;
                    //}
                    if (longsourceArray < 1024)
                    {
                        longsourceArray *= 10;
                        longsourceArray += 19200;
                    }
                    BigInteger x = new BigInteger(longsourceArray << 7);
                    //BigInteger x = new BigInteger(((long)sourceArray[0]) << 7);
                    BigInteger m = BigInteger.Parse("1000003");
                    BigInteger mask = BigInteger.Pow(new BigInteger(2), this.hashbits) - BigInteger.One;
                    foreach (char item in sourceArray)
                    {
                        BigInteger temp = new BigInteger((long)item);
                        x = ((x * m) ^ temp) & mask;
                    }
                    x = x ^ (new BigInteger(source.Length));
                    if (x.Equals(BigInteger.MinusOne))
                    {
                        x = new BigInteger(-2);
                    }
                    return x;
                }
            }

            public int HammingDistance(SimHash other)
            {
                BigInteger m = (BigInteger.One << this.hashbits) - BigInteger.One;
                BigInteger x = (this.strSimHash ^ other.strSimHash) & m;
                int tot = 0;
                while (x.Sign != 0)
                {
                    tot += 1;
                    x = x & (x - BigInteger.One);
                }
                return tot;
            }

        }

        //简单的分词法，直接将中文分成单个汉。可以用其他分词法代替
        public class ChxTokenizer
        {
            private string source;
            private int index;
            private int length;
            public ChxTokenizer(string source)
            {
                this.source = source;
                this.index = 0;
                this.length = (source ?? "").Length;
            }

            public bool hasMoreTokens()
            {
                return index < length;
            }

            public string nextToken()
            {
                String s = source.Substring(index, 1);
                index++;
                return s;
            }
        }
    }
}
