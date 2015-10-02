using GoogleSearchAPI;
using GoogleSearchAPI.Query;
using GoogleSearchAPI.Resources;
using System.Linq;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Action Command
        [MultipleCommand("g", "google")]
        public static void Google(string msg, bool state)
        {
            // Get the googleing
            string google = Utils.Remove(msg, state ? "g" : "google", true);

            // Google
            WebQuery query = new WebQuery(google);
            query.HostLangauge.Value = Languages.English;
            query.CountryCode.Value = CountryCode.United_States;
            IGoogleResultSet<GoogleWebResult> result = null;

            try
            {
                result = GoogleService.Instance.Search<GoogleWebResult>(query);

                // Return a link
                GoogleWebResult web = result.Results.First();
                SendMessage(message.User.Nick + ": " + web.Url + " [" + web.TitleNoFormatting + "]");
            }
            catch 
            {
                SendMessage(message.User.Nick + ": I couldn't find anything related to your search on Google.");
            }            
        }
    }
}