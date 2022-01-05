using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common
{
    class APIstuff
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Urls
        {
            public string raw { get; set; }
            public string full { get; set; }
            public string regular { get; set; }
            public string small { get; set; }
            public string thumb { get; set; }
        }

        public class Links
        {
            public string self { get; set; }
            public string html { get; set; }
            public string download { get; set; }
            public string download_location { get; set; }
            public string photos { get; set; }
            public string likes { get; set; }
            public string portfolio { get; set; }
            public string following { get; set; }
            public string followers { get; set; }
        }

        public class ProfileImage
        {
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public DateTime updated_at { get; set; }
            public string username { get; set; }
            public string name { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public object twitter_username { get; set; }
            public string portfolio_url { get; set; }
            public object bio { get; set; }
            public string location { get; set; }
            public Links links { get; set; }
            public ProfileImage profile_image { get; set; }
            public string instagram_username { get; set; }
            public int total_collections { get; set; }
            public int total_likes { get; set; }
            public int total_photos { get; set; }
            public bool accepted_tos { get; set; }
            public bool for_hire { get; set; }
        }

        public class Exif
        {
            public string make { get; set; }
            public string model { get; set; }
            public string exposure_time { get; set; }
            public string aperture { get; set; }
            public string focal_length { get; set; }
            public int iso { get; set; }
        }

        public class Position
        {
            public object latitude { get; set; }
            public object longitude { get; set; }
        }

        public class Location
        {
            public object title { get; set; }
            public object name { get; set; }
            public object city { get; set; }
            public object country { get; set; }
            public Position position { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public DateTime promoted_at { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string color { get; set; }
            public string blur_hash { get; set; }
            public object description { get; set; }
            public string alt_description { get; set; }
            public Urls urls { get; set; }
            public Links links { get; set; }
            public List<object> categories { get; set; }
            public int likes { get; set; }
            public bool liked_by_user { get; set; }
            public List<object> current_user_collections { get; set; }
            public object sponsorship { get; set; }
            public User user { get; set; }
            public Exif exif { get; set; }
            public Location location { get; set; }
            public int views { get; set; }
            public int downloads { get; set; }
        }


    }
}
