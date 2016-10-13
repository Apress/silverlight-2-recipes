using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.IO;
using System.Web;
using System.Threading;

namespace Ch07_Networking.Recipe7_4.PhotoService
{
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
  public class PhotoStore : IPhotoDownload
  {

    public byte[] GetPhoto(string PhotoName)
    {

      FileStream fs = new FileStream(HttpContext.Current.Request.MapPath(string.Format("APP_DATA/Photos/{0}", PhotoName)), FileMode.Open, FileAccess.Read);
      byte[] Ret = new byte[fs.Length];
      fs.Read(Ret, 0, (int)fs.Length);
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return Ret;
    }

    public Stream GetThumbs()
    {
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return new FileStream(HttpContext.Current.Request.MapPath("APP_DATA/Thumbs.zip"), FileMode.Open, FileAccess.Read);
    }


  }
}
