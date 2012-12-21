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


using System.IO;

namespace EsriUK.NETPortalAPI.Helpers
{

    internal abstract class FormData
    {

        private byte[] _header;
        protected byte[] Header
        {
            get
            {
                return _header ?? (_header = GetMultipartHeader());
            }
        }

        public long GetLength(int boundaryPlusNameLength)
        {
            return boundaryPlusNameLength + Header.Length + InternalGetLength() + 42;
        }
        public abstract long InternalGetLength();

        public void WriteTo(Stream stream, string name, byte[] boundary)
        {

            stream.Write(boundary, 0, boundary.Length);

            var firstHeader = System.Text.Encoding.UTF8.GetBytes("\r\nContent-Disposition: form-data; name=\"" + name + "\";");
            stream.Write(firstHeader, 0, firstHeader.Length);

            stream.Write(Header, 0, Header.Length);

            InternalWriteTo(stream);

        }
        protected abstract void InternalWriteTo(Stream stream);

        protected abstract byte[] GetMultipartHeader();

    }
}
