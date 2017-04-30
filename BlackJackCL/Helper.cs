using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL
{
    public class Helper
    {
        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(Object obj)
        {
            try
            {
                if (obj == null)
                    return null;
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("ObjectToByteArray: " + ex.Message);
            }
        }
        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);

                memStream.Position = 0;
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("ByteArrayToObject: " + ex.Message);
            }
        }
    }
}
