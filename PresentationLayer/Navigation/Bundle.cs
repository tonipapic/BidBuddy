using System.Collections.Generic;

namespace PresentationLayer.Navigation {
    
    /// <summary>
    /// Used to store data.
    /// </summary>
    public class Bundle {

        private Dictionary<string, object> Data = new Dictionary<string, object>();

        public void PutString(string key, string value) {
            Data[key] = value;
        }

        public void PutObject(string key, object value) {
            Data[key] = value;
        }

        public string GetString(string key) {
            if (Data.ContainsKey(key) == false) return null;
            return Data[key] as string;
        }

        public object GetObject(string key) {
            if(Data.ContainsKey(key) == false) return null;
            return Data[key];
        }

    }

}
