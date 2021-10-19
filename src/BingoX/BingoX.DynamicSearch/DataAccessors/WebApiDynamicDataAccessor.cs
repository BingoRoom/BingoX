using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace BingoX.DynamicSearch
{
    public abstract class WebRemoteDynamicDataAccessor : IDynamicDataAccessor, IWebRemoteDynamicDataAccessor
    {


        public WebRemoteDynamicDataAccessor(string nodeName, string remotetype, string hosturl)
        {
            this.Node = nodeName;
            this.Remotetype = remotetype;
            this.HostUrl = hosturl;
        }

        public string Node { get; set; }
        public string Remotetype { get; set; }
        public string HostUrl { get; set; }

        public static IDynamicDataAccessor Create(string nodeName, string remotetype, string hosturl)
        {
            switch (remotetype)
            {
                case "webapi": return new WebApiDynamicDataAccessor(nodeName, hosturl);
                case "wcf": return new WCFDynamicDataAccessor(nodeName, hosturl);
                default:
                    break;
            }
            throw new NotSupportedException(remotetype);
        }
    }
    public class WCFDynamicDataAccessor : WebRemoteDynamicDataAccessor, IWebApiDynamicDataAccessor
    {


        public WCFDynamicDataAccessor(string nodeName, string hosturl) : base(nodeName, "wcf", hosturl)
        {

        }


        public object GetMany(Type classTye, string url, string method, NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }
        public object[] GetOne(Type classTye, string url, string method, NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable GetTable(string url, string method, NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }
    }
    public class WebApiDynamicDataAccessor : WebRemoteDynamicDataAccessor, IWebApiDynamicDataAccessor
    {


        public WebApiDynamicDataAccessor(string nodeName, string hosturl) : base(nodeName, "webapi", hosturl)
        {

        }


        public object GetMany(Type classTye, string url, string method, NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }
        public object[] GetOne(Type classTye, string url, string method, NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable GetTable(string url, string method, NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }
    }
}
