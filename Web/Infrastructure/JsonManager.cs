using Newtonsoft.Json;

namespace Web.Infrastructure {
    public class JsonManager {
        public static string Serialize<T>(T data) {
            return JsonConvert.SerializeObject(data);
        }

        public static T Deserialize<T>(string data) {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}