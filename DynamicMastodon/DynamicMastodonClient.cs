using Codeplex.Data;
using DynamicMastodon.Attributes;
using DynamicMastodon.Core;
using DynamicMastodon.Extentions;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMastodon
{
    /// <summary>
    /// DynamicMastodonClient
    /// 
    /// https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md
    /// </summary>
    public class DynamicMastodonClient
    {
        private string _Host;

        private string _AccessToken;

        public DynamicMastodonClient(string host, string accessToken)
        {
            _Host = host;
            _AccessToken = accessToken;
        }

        #region * Accounts
        #region Fetching an account
        [Method(Method.GET)]
        [Query("/api/v1/accounts/{id}")]
        public async Task<dynamic> Account(int id)
        {
            var response = await Execute(nameof(Account),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }

        #endregion
        #region Getting the current user
        [Method(Method.GET)]
        [Query("/api/v1/accounts/verify_credentials")]
        public async Task<dynamic> CurrentAccount()
        {
            var response = await Execute(nameof(CurrentAccount));

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Updating the current user
        [Method(Method.PATCH)]
        [Query("/api/v1/accounts/update_credentials")]
        public async Task<dynamic> UpdateAccount(string display_name = null, string note = null, string avatar = null, string header = null)
        {
            var response = await Execute(nameof(UpdateAccount),
                new {display_name, note, avatar, header}.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Getting an account's followers
        [Method(Method.GET)]
        [Query("/api/v1/accounts/{id}/followers")]
        public async Task<StreamContent> GettingFollowers(int id, int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(GettingFollowers),
                new { max_id, since_id, limit }.ToDictionary(),
                new { id }.ToUrlSegment());

            return CreateStreamContent(response);
        }
        #endregion
        #region Getting who account is following
        [Method(Method.GET)]
        [Query("/api/v1/accounts/{id}/following")]
        public async Task<StreamContent> GettingFollowing(int id, int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(GettingFollowing),
                new { max_id, since_id, limit }.ToDictionary(),
                new { id }.ToUrlSegment());

            return CreateStreamContent(response);
        }
        #endregion
        #region Getting an account's statuses
        [Method(Method.GET)]
        [Query("/api/v1/accounts/{id}/statuses")]
        public async Task<dynamic> GettingStatuses(int id, bool? only_media = null, bool? exclude_replies = null, int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(GettingStatuses),
                new { only_media, exclude_replies, max_id, since_id, limit }.ToDictionary(),
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Following/unfollowing an account
        [Method(Method.POST)]
        [Query("/api/v1/accounts/{id}/follow")]
        public async Task<dynamic> Follow(int id)
        {
            var response = await Execute(nameof(Follow),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        [Method(Method.POST)]
        [Query("/api/v1/accounts/{id}/unfollow")]
        public async Task<dynamic> Unfollow(int id)
        {
            var response = await Execute(nameof(Unfollow),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Blocking/unblocking an account
        [Method(Method.POST)]
        [Query("/api/v1/accounts/{id}/block")]
        public async Task<dynamic> Blocking(int id)
        {
            var response = await Execute(nameof(Blocking),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        [Method(Method.POST)]
        [Query("/api/v1/accounts/{id}/unblock")]
        public async Task<dynamic> Unblocking(int id)
        {
            var response = await Execute(nameof(Unblocking),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Muting/unmuting an account
        [Method(Method.POST)]
        [Query("/api/v1/accounts/{id}/mute")]
        public async Task<dynamic> Muting(int id)
        {
            var response = await Execute(nameof(Muting),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        [Method(Method.POST)]
        [Query("/api/v1/accounts/{id}/unmute")]
        public async Task<dynamic> Unmuting(int id)
        {
            var response = await Execute(nameof(Unmuting),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Getting an account's relationships
        [Method(Method.GET)]
        [Query("/api/v1/accounts/relationships")]
        public async Task<dynamic> Relationships(int id)
        {
            var method = typeof(DynamicMastodonClient).GetMethod(nameof(Relationships), new Type[] { typeof(int) });
            var response = await Execute(method,
            new { id }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        [Method(Method.GET)]
        [Query("/api/v1/accounts/relationships")]
        public async Task<dynamic> Relationships(IEnumerable<int> id)
        {
            var method = typeof(DynamicMastodonClient).GetMethod(nameof(Relationships), new Type[] { typeof(IEnumerable<int>) });
            var response = await Execute(method,
            new { id }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Searching for accounts
        [Method(Method.GET)]
        [Query("/api/v1/accounts/search")]
        public async Task<dynamic> SearchAccounts(string q, int limit)
        {
            var response = await Execute(nameof(SearchAccounts),
                new { q, limit }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Apps
        #region Registering an application
        [Method(Method.POST)]
        [Query("/api/v1/apps")]
        public async Task<dynamic> Registering(string client_name, OAuthScope scope, string redirect_uris = "urn:ietf:wg:oauth:2.0:oob", string website = null)
        {
            var response = await Execute(nameof(Registering),
                new { client_name, redirect_uris, scope, website }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Blocks
        #region Fetching a user's blocks
        [Method(Method.GET)]
        [Query("/api/v1/blocks")]
        public async Task<StreamContent> Blocks(int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(Blocks),
                new { max_id, since_id, limit }.ToDictionary());

            return CreateStreamContent(response);
        }
        #endregion
        #endregion
        #region * Favourites
        #region Fetching a user's favourites
        [Method(Method.GET)]
        [Query("/api/v1/favourites")]
        public async Task<StreamContent> FetchingFavourites(int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(FetchingFavourites),
                new { max_id, since_id, limit }.ToDictionary());

            return CreateStreamContent(response);
        }
        #endregion
        #endregion
        #region * Follow Requests
        #region Fetching a list of follow requests
        [Method(Method.GET)]
        [Query("/api/v1/follow_requests")]
        public async Task<StreamContent> FollowRequests(int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(FollowRequests),
                new { max_id, since_id, limit }.ToDictionary());

            return CreateStreamContent(response);
        }
        #endregion
        #region Authorizing or rejecting follow requests
        [Method(Method.POST)]
        [Query("/api/v1/follow_requests/{id}/authorize")]
        public async Task AuthorizeFollowRequests(int id)
        {
            await Execute(nameof(AuthorizeFollowRequests),
                null, new { id }.ToUrlSegment());
        }
        [Method(Method.POST)]
        [Query("/api/v1/follow_requests/{id}/reject")]
        public async Task RejectFollowRequests(int id)
        {
            await Execute(nameof(RejectFollowRequests),
                null, new { id }.ToUrlSegment());
        }
        #endregion
        #endregion
        #region * Follows
        #region Following a remote user
        [Method(Method.POST)]
        [Query("/api/v1/follows")]
        public async Task<dynamic> Follows(string uri)
        {
            var response = await Execute(nameof(Follows),
                new { uri }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Instances
        #region Getting instance information
        [Method(Method.GET)]
        [Query("/api/v1/instance")]
        public async Task<dynamic> GettingInstance()
        {
            var response = await Execute(nameof(GettingInstance));

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Media
        #region Uploading a media attachment
        [Method(Method.POST)]
        [Query("/api/v1/media")]
        public async Task<dynamic> UploadingMedia(string file)
        {
            var response = await Execute(nameof(UploadingMedia),
                new { file }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Mutes
        #region Fetching a user's mutes
        [Method(Method.GET)]
        [Query("/api/v1/mutes")]
        public async Task<StreamContent> Mutes(int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(Mutes),
                new { max_id, since_id, limit }.ToDictionary());

            return CreateStreamContent(response);
        }
        #endregion
        #endregion
        #region * Notifications
        #region Fetching a user's notifications
        [Method(Method.GET)]
        [Query("/api/v1/notifications")]
        public async Task<StreamContent> Notifications(int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(Notifications),
                new { max_id, since_id, limit }.ToDictionary());

            return CreateStreamContent(response);
        }
        #endregion
        #region Getting a single notification
        [Method(Method.GET)]
        [Query("/api/v1/notifications/{id}")]
        public async Task<dynamic> GettingNotification(int id)
        {
            var response = await Execute(nameof(GettingNotification),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Clearing notifications
        [Method(Method.POST)]
        [Query("/api/v1/notifications/clear")]
        public async Task ClearNotifications()
        {
            await Execute(nameof(ClearNotifications));
        }
        #endregion
        #endregion
        #region * Reports
        #region Fetching a user's reports
        [Method(Method.GET)]
        [Query("/api/v1/reports")]
        public async Task<dynamic> Reports()
        {
            var response = await Execute(nameof(Reports));

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Reporting a user
        [Method(Method.POST)]
        [Query("/api/v1/reports")]
        public async Task<dynamic> ReportingUser(int account_id, IEnumerable<int> status_ids, string comment)
        {
            var response = await Execute(nameof(ReportingUser), new { account_id, status_ids, comment }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Search
        #region Searching for content
        [Method(Method.GET)]
        [Query("/api/v1/search")]
        public async Task<dynamic> Search(string q, bool resolve = false)
        {
            var response = await Execute(nameof(Search),
                new { q, resolve }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Statuses
        #region Fetching a status
        [Method(Method.GET)]
        [Query("/api/v1/statuses/{id}")]
        public async Task<dynamic> Statuses(int id)
        {
            var response = await Execute(nameof(Statuses),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Getting status context
        [Method(Method.GET)]
        [Query("/api/v1/statuses/{id}/context")]
        public async Task<dynamic> StatusContext(int id)
        {
            var response = await Execute(nameof(StatusContext),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Getting a card associated with a status
        [Method(Method.GET)]
        [Query("/api/v1/statuses/{id}/card")]
        public async Task<dynamic> StatusCard(int id)
        {
            var response = await Execute(nameof(StatusCard),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Getting who reblogged/favourited a status
        [Method(Method.GET)]
        [Query("/api/v1/statuses/{id}/reblogged_by")]
        public async Task<dynamic> Reblogged(int id, int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(Reblogged),
                new { max_id, since_id, limit }.ToDictionary(),
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        [Method(Method.GET)]
        [Query("/api/v1/statuses/{id}/favourited_by")]
        public async Task<dynamic> Favourited(int id, int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(Favourited),
                new { max_id, since_id, limit }.ToDictionary(),
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Posting a new status
        [Method(Method.POST)]
        [Query("/api/v1/statuses")]
        public async Task<dynamic> PostingStatus(string status, int? in_reply_to_id = null, IEnumerable<int> media_ids = null, bool? sensitive = null, string spoiler_text = null, Visibility? visibility = null)
        {
            var response = await Execute(nameof(PostingStatus),
                new { status, in_reply_to_id, media_ids, sensitive, spoiler_text, visibility }.ToDictionary());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Deleting a status
        [Method(Method.DELETE)]
        [Query("/api/v1/statuses/{id}")]
        public async Task<dynamic> DeletingStatus(int id)
        {
            var response = await Execute(nameof(DeletingStatus),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Reblogging/unreblogging a status
        [Method(Method.POST)]
        [Query("/api/v1/statuses/{id}/reblog")]
        public async Task<dynamic> Reblog(int id)
        {
            var response = await Execute(nameof(Reblog),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        [Method(Method.POST)]
        [Query("/api/v1/statuses/{id}/unreblog")]
        public async Task<dynamic> Unreblog(int id)
        {
            var response = await Execute(nameof(Unreblog),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #region Favouriting/unfavouriting a status
        [Method(Method.POST)]
        [Query("/api/v1/statuses/{id}/favourite")]
        public async Task<dynamic> Favourite(int id)
        {
            var response = await Execute(nameof(Favourite),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        [Method(Method.POST)]
        [Query("/api/v1/statuses/{id}/unfavourite")]
        public async Task<dynamic> Unfavourite(int id)
        {
            var response = await Execute(nameof(Unfavourite),
                null,
                new { id }.ToUrlSegment());

            return DynamicJson.Parse(response.Content);
        }
        #endregion
        #endregion
        #region * Timelines
        #region Retrieving a timeline
        [Method(Method.GET)]
        [Query("/api/v1/timelines/home")]
        public async Task<StreamContent> HomeTimeline(int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(HomeTimeline),
                new { max_id, since_id, limit }.ToDictionary());

            return CreateStreamContent(response);
        }

        [Method(Method.GET)]
        [Query("/api/v1/timelines/public")]
        public async Task<StreamContent> PublicTimeline(bool? local = null, int? max_id = null, int? since_id = null, int? limit = 20)
        {
            var response = await Execute(nameof(PublicTimeline),
                new { local, max_id, since_id, limit }.ToDictionary());

            return CreateStreamContent(response);
        }
        
        [Method(Method.GET)]
        [Query("/api/v1/timelines/tag/{hashtag}")]
        public async Task<StreamContent> HashtagTimeline(string hashtag, bool? local = null, int? max_id = null, int? since_id = null, int? limit = null)
        {
            var response = await Execute(nameof(PublicTimeline),
                new { local, max_id, since_id, limit }.ToDictionary(),
                new { hashtag }.ToUrlSegment());

            return CreateStreamContent(response);
        }
        #endregion
        #endregion
        #region private Methods
        private Method GetMethod(MethodBase methodBase)
        {
            var methodAttribute = (MethodAttribute)methodBase.GetCustomAttribute(typeof(MethodAttribute));
            return methodAttribute.Method;
        }

        private string GetQuery(MethodBase methodBase)
        {
            var queryAttribute = (QueryAttribute)methodBase.GetCustomAttribute(typeof(QueryAttribute));
            return queryAttribute.Query;
        }

        private Task<IRestResponse> Execute(MethodBase methodBase, IDictionary<string, object> parameter = null, IDictionary<string, string> urlSegment = null)
        {
            var client = new RestClient($"https://{_Host}");

            var query = GetQuery(methodBase);

            var method = GetMethod(methodBase);

            var request = CreateRequest(query, method, parameter, urlSegment);

            return client.ExecuteTaskAsync(request);
        }

        private Task<IRestResponse> Execute(string methodName, IDictionary<string, object> parameter = null, IDictionary<string, string> urlSegment = null)
        {
            return Execute(typeof(DynamicMastodonClient).GetMethod(methodName), parameter, urlSegment);
        }

        private RestRequest CreateRequest(string query, Method method, IDictionary<string, object> parameter, IDictionary<string, string> urlSegment)
        {
            var request = new RestRequest(query, method);
            request.AddHeader("Authorization", $"Bearer {_AccessToken}");

            if (parameter != null)
            {
                foreach (var item in parameter)
                {
                    if(item.Value is string)
                    {
                        request.AddParameter(item.Key, item.Value);
                    }
                    else if (item.Value is IEnumerable enumerable)
                    {
                        foreach (var element in enumerable)
                        {
                            request.AddParameter($"{item.Key}[]", element);
                        }
                    }
                    else if(item.Value is Visibility visibility)
                    {
                        request.AddParameter(item.Key, visibility.ToParam());
                    }
                    else if(item.Value is bool flag && flag)
                    {
                        request.AddParameter(item.Key, "1");
                    }
                    else if(item.Value is OAuthScope scope)
                    {
                        request.AddParameter(item.Key, scope.ToString());
                    }
                    else
                    {
                        request.AddParameter(item.Key, item.Value);
                    }
                }
            }

            if (urlSegment != null)
            {
                foreach (var item in urlSegment)
                {
                    request.AddUrlSegment(item.Key, item.Value);
                }
            }

            return request;
        }

        private StreamContent CreateStreamContent(IRestResponse response)
        {
            var linkHeader = response.Headers.FirstOrDefault(x => x.Name == "Link");

            var streamContent = new StreamContent();

            streamContent.Statuses = DynamicJson.Parse(response.Content);
            var header = linkHeader.Value.ToString().GetHeader();
            streamContent.Next = header.next;
            streamContent.Prev = header.prev;

            return streamContent;
        }
        #endregion
    }
}
