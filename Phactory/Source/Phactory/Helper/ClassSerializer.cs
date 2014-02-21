using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Project.Helper
{
    static class ClassSerializer
    {
        static public T Read<T>(string filename)
        {
            T instance = default(T);

            if (!File.Exists(filename))
            {
                App.Controller.Log.Append(new FileInfo(filename).Name + " does not exist !");
                return default(T);
            }
            
            FileStream stream = null;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                instance = (T)xmlSerializer.Deserialize(stream);
            }
            catch (System.Exception)
            {
            }
            finally
            {
            }

            string name = new FileInfo(filename).Name;
            if (stream == null)
            {
                App.Controller.Log.Append(name + " load failure !");
            }
            else
            {
                stream.Close();

                if (App.Controller.UserConfig.VerboseOutput)
                {
                    App.Controller.Log.Append(name + " loaded");
                }
                return instance;
            }

            return default(T);
        }
        
        static public bool Write(string filename, object instance)
        {
            FileStream stream = null;
            
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType());
                
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                xmlSerializer.Serialize(stream, instance);
            }
            finally
            {
            }

            string name = new FileInfo(filename).Name;

            if (stream == null)
            {
                App.Controller.Log.Append(name + " write failure !"); 
                return false;
            }

            stream.Close();

            if (App.Controller.UserConfig.VerboseOutput)
            {
                App.Controller.Log.Append(name + " written");
            }
            return true;
        }
    }
}
