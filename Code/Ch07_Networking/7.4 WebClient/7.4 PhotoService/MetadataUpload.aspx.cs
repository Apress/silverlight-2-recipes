using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace Ch07_Networking.Recipe7_4.PhotoService
{
  public partial class MetadataUpload : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (Request.HttpMethod == "POST")
      {
        DataContractJsonSerializer jsonSer = 
          new DataContractJsonSerializer(typeof(PhotoMetaData));
        SetPhotoMetaData(
          jsonSer.ReadObject(Request.InputStream) as PhotoMetaData);
        Response.SuppressContent = true;
      }
    }

    public void SetPhotoMetaData(PhotoMetaData MetaData)
    {
      PhotoStoreDataContext dcPhoto = new PhotoStoreDataContext();
      List<PhotoData> pds = (from pd in dcPhoto.PhotoDatas
                             where pd.PhotoId == MetaData.Id
                             select pd).ToList();
      if (pds.Count == 0)
      {
        dcPhoto.PhotoDatas.InsertOnSubmit(new PhotoData {
          PhotoId = MetaData.Id, Name = MetaData.Name,
          Location = MetaData.Location, DateTaken = MetaData.DateTaken,
          Description = MetaData.Description, Rating = MetaData.Rating });
      }
      else
      {
        pds[0].Name = MetaData.Name;
        pds[0].DateTaken = MetaData.DateTaken;
        pds[0].Description = MetaData.Description;
        pds[0].Location = MetaData.Location;
        pds[0].Rating = MetaData.Rating;
      }
      dcPhoto.SubmitChanges();
    }
  }
}
