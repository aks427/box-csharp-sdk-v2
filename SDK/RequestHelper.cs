using System.IO;
using BoxApi.V2.SDK.Model;
using BoxApi.V2.SDK.Serialization;
using RestSharp;

namespace BoxApi.V2
{
    public class RequestHelper
    {
        private readonly string _authorizationApiVersion;
        private readonly string _contentApiVersion;

        public RequestHelper(string authorizationApiVersion, string contentApiVersion)
        {
            _authorizationApiVersion = authorizationApiVersion;
            _contentApiVersion = contentApiVersion;
        }

        public IRestRequest Get(Type resourceType, string id)
        {

            var request = JsonRequest(resourceType, "{id}");
            request.AddUrlSegment("id", id);
            return request;
        }

        public IRestRequest GetItems(string id)
        {
            var request = JsonRequest(Type.Folder, "{id}/items");
            request.AddUrlSegment("id", id);
            return request;
        }

        public IRestRequest CreateFolder(string parentId, string name)
        {
            var request = JsonRequest(Type.Folder, "{parentId}", Method.POST);
            request.AddUrlSegment("parentId", parentId);
            request.AddBody(new {name});
            return request;
        }

        public IRestRequest CreateFile(string parentId, string name, byte[] content)
        {
            var request = JsonRequest(Type.File, "data", Method.POST);
            request.AddFile("filename1", content, name);
            request.AddParameter("folder_id", parentId);
            return request;
        }

        public IRestRequest DeleteFolder(string id, bool recursive)
        {
            var request = JsonRequest(Type.Folder, "{id}", Method.DELETE);
            request.AddUrlSegment("id", id);
            request.AddParameter("recursive", recursive.ToString().ToLower());
            return request;
        }

        public IRestRequest DeleteFile(string id, string etag)
        {
            var request = JsonRequest(Type.File, "{id}", Method.DELETE);
            request.AddUrlSegment("id", id);
            request.AddHeader("If-Match", etag ?? string.Empty);
            return request;
        }

        public IRestRequest Copy(Type resourceType, string id, string newParentId, string name)
        {
            var request = JsonRequest(resourceType, "{id}/copy", Method.POST);
            request.AddUrlSegment("id", id);
            request.AddBody(new {parent = new {id = newParentId}, name});
            return request;
        }

        public IRestRequest ShareLink(Type resourceType, string id, SharedLink sharedLink)
        {
            var request = JsonRequest(resourceType, "{id}", Method.PUT);
            request.AddUrlSegment("id", id);
            request.AddBody(new {shared_link = sharedLink});
            return request;
        }

        public IRestRequest Move(Type resourceType, string id, string newParentId)
        {
            var request = JsonRequest(resourceType, "{id}", Method.PUT);
            request.AddUrlSegment("id", id);
            request.AddBody(new {parent = new {id = newParentId}});
            return request;
        }

        public IRestRequest Rename(Type resourceType, string id, string newName)
        {
            var request = JsonRequest(resourceType, "{id}", Method.PUT);
            request.AddUrlSegment("id", id);
            request.AddBody(new {name = newName});
            return request;
        }


        public IRestRequest ReadFile(string id)
        {
            var request = RawRequest(Type.File, "{id}/data");
            request.AddUrlSegment("id", id);
            return request;
        }

        private IRestRequest RawRequest(Type resourceType, string resource, Method method = Method.GET)
        {
            string path = "{version}/{type}" + (string.IsNullOrEmpty(resource) ? string.Empty : string.Format("/{0}", resource));
            var request = new RestRequest(path, method);
            request.AddUrlSegment("version", _contentApiVersion);
            request.AddUrlSegment("type", resourceType.Description());
            return request;
        }

        private IRestRequest JsonRequest(Type resourceType, string resource = null, Method method = Method.GET)
        {
            var request = RawRequest(resourceType, resource, method);
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new AttributableJsonSerializer();
            return request;
        }
    }
}