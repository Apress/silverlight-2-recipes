using System;
using System.IO;
using System.Web;

namespace Ch07_Networking.Recipe7_4.PhotoService
{
  public partial class PhotoUpload1 : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (Request.HttpMethod == "POST")
      {
        AddPhoto(Request.InputStream);
        Response.SuppressContent = true;
      }
    }

    public void AddPhoto(Stream PhotoStream)
    {
      //get the file name for the photo
      string PhotoName = 
        HttpContext.Current.Request.Headers["Image-Name"];
      if (PhotoName == null) return;
      //open a file stream to store the photo
      FileStream fs = new FileStream(
        HttpContext.Current.Request.MapPath
        (string.Format("APP_DATA/Photos/{0}", PhotoName)),
        FileMode.Create, FileAccess.Write);

      //read and store
      BinaryReader br = new BinaryReader(PhotoStream);
      BinaryWriter bw = new BinaryWriter(fs);

      int ChunkSize = 1024 * 1024;
      byte[] Chunk = null;
      do
      {
        Chunk = br.ReadBytes(ChunkSize);
        bw.Write(Chunk);
        bw.Flush();
      } while (Chunk.Length == ChunkSize);

      br.Close();
      bw.Close();
    }
  }
}
