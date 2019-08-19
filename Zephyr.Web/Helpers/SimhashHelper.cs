using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sysnum = System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Areas.Helpers
{
    public class SimhashHelper
    {
        public class SimHash
        {
            private String tokens;
            private sysnum.BigInteger strSimHash;
            private int hashbits = 128;

            public sysnum.BigInteger StrSimHash
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

            private sysnum.BigInteger simHash()
            {
                int[] v = new int[this.hashbits];
                ChxTokenizer stringTokens = new ChxTokenizer(this.tokens);
                while (stringTokens.hasMoreTokens())
                {
                    String temp = stringTokens.nextToken();
                    sysnum.BigInteger t = this.hash(temp);
                    //Console.WriteLine("temp = {0} ： {1}", temp, t);
                    for (int i = 0; i < this.hashbits; i++)
                    {
                        sysnum.BigInteger bitmask = sysnum.BigInteger.One << i;
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
                sysnum.BigInteger fingerprint = sysnum.BigInteger.Zero;
                for (int i = 0; i < this.hashbits; i++)
                {
                    if (v[i] >= 0)
                    {
                        fingerprint = fingerprint + (sysnum.BigInteger.Parse("1") << i);
                    }
                }
                return fingerprint;
            }

            private sysnum.BigInteger hash(string source)
            {
                if (source == null || source.Length == 0)
                {
                    return sysnum.BigInteger.Zero;
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
                    sysnum.BigInteger x = new sysnum.BigInteger(longsourceArray << 7);
                    //sysnum.BigInteger x = new sysnum.BigInteger(((long)sourceArray[0]) << 7);
                    sysnum.BigInteger m = sysnum.BigInteger.Parse("1000003");
                    sysnum.BigInteger mask = sysnum.BigInteger.Pow(new sysnum.BigInteger(2), this.hashbits) - sysnum.BigInteger.One;
                    foreach (char item in sourceArray)
                    {
                        sysnum.BigInteger temp = new sysnum.BigInteger((long)item);
                        x = ((x * m) ^ temp) & mask;
                    }
                    x = x ^ (new sysnum.BigInteger(source.Length));
                    if (x.Equals(sysnum.BigInteger.MinusOne))
                    {
                        x = new sysnum.BigInteger(-2);
                    }
                    return x;
                }
            }

            public int HammingDistance(SimHash other)
            {
                sysnum.BigInteger m = (sysnum.BigInteger.One << this.hashbits) - sysnum.BigInteger.One;
                sysnum.BigInteger x = (this.strSimHash ^ other.strSimHash) & m;
                int tot = 0;
                while (x.Sign != 0)
                {
                    tot += 1;
                    x = x & (x - sysnum.BigInteger.One);
                }
                return tot;
            }

            public int HammingDistance(string other)
            {
                sysnum.BigInteger temp = sysnum.BigInteger.Parse(other);
                sysnum.BigInteger m = (sysnum.BigInteger.One << this.hashbits) - sysnum.BigInteger.One;
                sysnum.BigInteger x = (this.strSimHash ^ temp) & m;
                int tot = 0;
                while (x.Sign != 0)
                {
                    tot += 1;
                    x = x & (x - sysnum.BigInteger.One);
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