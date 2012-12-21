// Copyright (c) 2011 Andrea Martinelli
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.IO;
using System.Text;

namespace EsriUK.NETPortalAPI.Helpers
{
    internal class TextFormData : FormData
    {
        private string _value;
        public TextFormData(string value)
        {
            _value = value;

            bytes = System.Text.Encoding.UTF8.GetBytes(value);
            actualLength = bytes.Length;
        }


        private Byte[] bytes;
        int actualLength;

        public override long InternalGetLength()
        {
            return actualLength;
        }

        protected override byte[] GetMultipartHeader()
        {
            return System.Text.Encoding.ASCII.GetBytes("\r\n\r\n");
        }


        protected override void InternalWriteTo(Stream stream)
        {
            stream.Write(bytes, 0, actualLength);
        }

        public void WriteUrlEncoded(Stream stream, string key)
        {
            var b = Encoding.ASCII.GetBytes(key);
            stream.Write(b, 0, b.Length);
            stream.WriteByte((byte)'=');
            var v = Encoding.ASCII.GetBytes(Uri.EscapeDataString(_value));
            stream.Write(v, 0, v.Length);
        }




    }
}







/*
 * How should we encode non-ASCII chars?
 * 
//BUGBUG Will crash if the string contains too many strange characters
bytes = new byte[(int)(value.Length * 1.5 + 100)];
for (int i = 0; i < value.Length; i++)
{
    var ch = value[i];
    if (!Between(ch, '0', '9') &&
        !Between(ch, 'A', 'Z') &&
        !Between(ch, 'a', 'z') &&
        !goodChars.Contains(ch)
        )
    {
        bytes[actualLength++] = (byte)'&';
        bytes[actualLength++] = (byte)'#';
        var n = System.Text.Encoding.ASCII.GetBytes( ((int)ch).ToString());
        Buffer.BlockCopy(n, 0, bytes, actualLength, n.Length);
        actualLength += n.Length;
        bytes[actualLength++] = (byte)';';
    }
    else
    {
        bytes[actualLength++] = (byte)ch;
    }
}
*/


/*  private readonly char[] goodChars = new char[] { ' ', '\r', '\n' };

   private bool Between(char ch, char min, char max)
   {
       return min <= ch && ch <= max;
   }*/