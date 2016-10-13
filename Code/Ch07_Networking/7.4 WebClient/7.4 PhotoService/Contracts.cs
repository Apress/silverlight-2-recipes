using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Ch07_Networking.Recipe7_4.PhotoService
{
  [ServiceContract]
  public interface IPhotoDownload
  {
    [OperationContract]
    [WebGet()]
    //get the zip file containing the thumbnails
    Stream GetThumbs();

    [OperationContract]
    [WebGet(UriTemplate = "Photos?Name={PhotoName}")]
    //get a full resolution image 
    byte[] GetPhoto(string PhotoName);
  }
  
  [ServiceContract]
  public interface IMetadata
  {
    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Json)]
    //get the names of all the JPEG images available for download
    List<string> GetPhotoFileNames();

    [OperationContract]
    [WebGet(UriTemplate = "PhotoMetadata?Id={PhotoId}", ResponseFormat = WebMessageFormat.Json)]
    //get the metadata for a specific image
    PhotoMetaData GetPhotoMetaData(string PhotoId);
 
  }

   

  [DataContract]
  public class PhotoMetaData
  { 
    [DataMember]
    public string Id { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Location { get; set; }
    [DataMember]
    public int? Rating { get; set; }
    [DataMember]
    public DateTime? DateTaken { get; set; }
  }
}