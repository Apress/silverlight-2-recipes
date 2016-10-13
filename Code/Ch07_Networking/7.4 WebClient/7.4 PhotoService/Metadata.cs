using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Web;
using System.ServiceModel.Activation;
using System.Data.Linq;
using System.Runtime.Serialization.Json;

namespace Ch07_Networking.Recipe7_4.PhotoService
{
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
  public class Metadata : IMetadata
  {
    public List<string> GetPhotoFileNames()
    {
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      IEnumerable<string> FullNames = Directory.GetFiles(HttpContext.Current.Request.MapPath("APP_DATA/Photos"), "*.jp*");
      return (from fnm in FullNames
              select Path.GetFileNameWithoutExtension(fnm)).ToList();

    }




    public PhotoMetaData GetPhotoMetaData(string PhotoId)
    {
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      PhotoStoreDataContext dcPhoto = new PhotoStoreDataContext();

      List<PhotoData> pds = (from pd in dcPhoto.PhotoDatas
                             where pd.PhotoId == PhotoId
                             select pd).ToList();
      if (pds.Count != 0)
        return new PhotoMetaData { Id = pds[0].PhotoId, Name = pds[0].Name, DateTaken = pds[0].DateTaken, Description = pds[0].Description, Location = pds[0].Location, Rating = pds[0].Rating };
      else
        return new PhotoMetaData { Id = PhotoId };

    } 
  }
}
