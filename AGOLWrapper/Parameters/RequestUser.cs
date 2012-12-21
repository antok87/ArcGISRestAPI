using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.Parameters
{
    public abstract class RequestUser : RequestCommon
    {
        public string access { get; set; }
        public string preferredView { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        public string thumbnail { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string securityQuestionIdx { get; set; }
        public string securityAnswer { get; set; }
        public string culture { get; set; }
        public string region { get; set; }
    }
}
