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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace EsriUK.NETPortalAPI.Helpers
{
    internal class HttpPostRequest : IDisposable
    {
        public HttpWebRequest _request;
        private Dictionary<string, FormData> _fields;
        private string boundary;
        private byte[] boundarybytes;

        public System.Collections.Specialized.NameValueCollection _textFieldColl = new System.Collections.Specialized.NameValueCollection();

        public HttpPostRequest(Uri url)
        {
            _request = (HttpWebRequest)WebRequest.Create(url);
            _request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            Init();
        }
        public HttpPostRequest(string url) : this(new Uri(url)) { }

        public HttpPostRequest(HttpWebRequest request)
        {
            _request = request;
            Init();
        }

        private void Init()
        {
            _fields = new Dictionary<string, FormData>();
        }

        public void AddFields(Dictionary<string, string> fields)
        {
            foreach (string key in fields.Keys)
            {
                AddField(key, fields[key]);
            }
        }

        public void AddField(string name, string value)
        {
            if (name == null || value == null) throw new ArgumentNullException();
            _fields.Add(name, new TextFormData(value));
            _textFieldColl.Add(name, value);
        }

        public void AddField(string name, int value)
        {
            if (name == null) throw new ArgumentNullException();
            AddField(name, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        public void AddFile(string name, string path)
        {
            if (name == null || path == null) throw new ArgumentNullException();
            AddFile(name, path, System.IO.Path.GetFileName(path));
        }

        public void AddFile(string name, string path, string filename)
        {
            if (name == null || path == null || filename == null) throw new ArgumentNullException();
            var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            _fields.Add(name, new StreamFormData(file, filename));
        }

        public void AddStream(string name, Stream stream)
        {
            if (name == null || stream == null) throw new ArgumentNullException();
            AddStream(name, stream, name + ".dat");
        }

        public void AddStream(string name, Stream stream, string filename)
        {
            if (name == null || stream == null || filename == null) throw new ArgumentNullException();
            _fields.Add(name, new StreamFormData(stream, filename));
        }






        private long GetMultipartContentLength()
        {

            long contentLength = 0;
            foreach (var field in _fields)
            {
                var v = field.Value.GetLength(boundarybytes.Length + field.Key.Length);
                if (v == -1) return -1;
                contentLength += v;
            }
            return contentLength + boundarybytes.Length + 2;

        }


        private HttpWebResponse PostMultipart()
        {
            _request.Method = "POST";

            boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary);

            _request.ContentType = "multipart/form-data; boundary=" + boundary;
            _request.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");

            try
            {
                var contentLength = GetMultipartContentLength();
                _request.ContentLength = contentLength;
            }
            catch (NotSupportedException)
            {
                //Some streams can't report their Length
            }



            var requestStream = _request.GetRequestStream();


            foreach (var field in _fields)
                field.Value.WriteTo(requestStream, field.Key, boundarybytes);


            requestStream.Write(boundarybytes, 0, boundarybytes.Length);
            requestStream.WriteByte((byte)'-');
            requestStream.WriteByte((byte)'-');

            Dispose();

            //requestStream.WriteTo(System.IO.File.OpenWrite(@"c:\temp\post.txt"));
            //return null;
            return (HttpWebResponse)_request.GetResponse();
        }


        private MemoryStream GetUrlEncodedData()
        {

            var requestStream = new MemoryStream();

            bool first = true;
            foreach (var field in _fields)
            {
                var t = field.Value as TextFormData;
                if (t == null) throw new NotSupportedException("File uploads must be executed with multipart/form-data method.");

                if (!first) requestStream.WriteByte((byte)'&');
                first = false;

                t.WriteUrlEncoded(requestStream, field.Key);
            }

            return requestStream;
        }

        private HttpWebResponse PostUrlEncoded()
        {

            _request.Method = "POST";
            _request.ContentType = "application/x-www-form-urlencoded";

            var data = GetUrlEncodedData();

            _request.ContentLength = data.Length;

            var s = _request.GetRequestStream();

            data.WriteTo(s);

            Dispose();

            return (HttpWebResponse)_request.GetResponse();

        }

        public HttpWebResponse PostData(PostMethod method)
        {
            if (method == PostMethod.Multipart) return PostMultipart();
            else if (method == PostMethod.UrlEncoded) return PostUrlEncoded();
            else throw new ArgumentException();

        }

        public HttpWebResponse PostData()
        {
            if (_fields.Values.All(x => x is TextFormData)) return PostUrlEncoded();
            else return PostMultipart();
        }



        public void Dispose()
        {
            foreach (var field in _fields)
                if (field.Value is IDisposable) ((IDisposable)field.Value).Dispose();
        }
    }

    internal enum PostMethod
    {
        UrlEncoded,
        Multipart
    }

}
