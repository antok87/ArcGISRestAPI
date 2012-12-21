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

namespace EsriUK.NETPortalAPI.Helpers
{
    internal enum ContentType
    {
        Application_OctetStream,
        Application_vnd_ms_excel
    }

    internal class StreamFormData : FormData, IDisposable
    {
        public StreamFormData(Stream stream, string filename)
        {
            this.fileStream = stream;
            this.filename = filename;
            //  TODO: make this dynamic
            this.contentType = ContentType.Application_vnd_ms_excel;
        }
        private string filename;
        internal readonly Stream fileStream;
        private ContentType contentType;

        private string ContentTypeString
        {
            get
            {
                var typeString = "";
                switch (contentType)
                {
                    case ContentType.Application_vnd_ms_excel:
                        typeString = "application/vnd.ms-excel";
                        break;
                    case ContentType.Application_OctetStream:
                        typeString = "application/octet-stream";
                        break;
                }
                return typeString;
            }

        }

        protected override byte[] GetMultipartHeader()
        {
            return System.Text.Encoding.UTF8.GetBytes(" filename=\"" + filename + "\"\r\n" +
    "Content-Type: " + ContentTypeString + "\r\n\r\n");
        }

        public override long InternalGetLength()
        {
            return fileStream.Length;
        }

        protected override void InternalWriteTo(Stream stream)
        {
            var buffer = new byte[1024];
            var bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                stream.Write(buffer, 0, bytesRead);
            }
            Dispose();
        }

        public void Dispose()
        {
            fileStream.Dispose();
        }
    }
}
