﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoxApi.V2.Model;
using RestSharp;

namespace BoxApi.V2
{
    public partial class BoxManager
    {
        /// <summary>
        /// Returns the list of all users for the Enterprise with their user_id, public_name, and login if the user is an enterprise admin. If the user is not an admin, this request returns the current user’s user_id, public_name, and login.
        /// </summary>
        /// <param name="filterTerm"> </param>
        /// <param name="limit"> </param>
        /// <param name="offset"> </param>
        /// <returns></returns>
        public UserCollection GetUsers(string filterTerm = null, int? limit = null, int? offset = null, Field[] fields = null)
        {
            IRestRequest request = _requestHelper.GetEnterpriseUsers(filterTerm, limit, offset, fields);
            return _restClient.ExecuteAndDeserialize<UserCollection>(request);
        }

        public User GetUser(User user, Field[] fields = null)
        {
            GuardFromNull(user, "user");
            return GetUser(user.Id, fields);
        }

        public User GetUser(string id, Field[] fields = null)
        {
            GuardFromNull(id, "id");
            IRestRequest request = _requestHelper.GetEnterpriseUser(id, fields);
            return _restClient.ExecuteAndDeserialize<User>(request);
        }
    }
}
