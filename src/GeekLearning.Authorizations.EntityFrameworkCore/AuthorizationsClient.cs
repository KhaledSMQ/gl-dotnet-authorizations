﻿namespace GeekLearning.Authorizations.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using System.Data;
    using Model;
    using System.Data.Common;
    using System.Data.SqlClient;
    using GeekLearning.Authorizations.Projections;

    public class AuthorizationsClient<TContext> : IAuthorizationsClient where TContext : DbContext
    {
        private readonly TContext context;

        private readonly IPrincipalIdProvider principalIdProvider;

        public AuthorizationsClient(TContext context, IPrincipalIdProvider principalIdProvider)
        {
            this.context = context;
            this.principalIdProvider = principalIdProvider;
        }

        public async Task<RightsResult> GetRightsAsync(string scopeKey, bool withChildren = false)
        {
            RightsResult rightsResult = new RightsResult();

            DbCommand com = this.context.Database.GetDbConnection().CreateCommand();
            var function = withChildren ? "Authorizations.GetRightsForScopeAndChildren" : "Authorizations.GetInheritedRightsForScope";
            com.CommandText = $"select {function}(@scopeName,@principalId)";
            com.CommandType = CommandType.Text;
            com.Parameters.Add(new SqlParameter("@scopeName", scopeKey));
            com.Parameters.Add(new SqlParameter("@principalId", this.principalIdProvider.PrincipalId));

            var reader = await com.ExecuteReaderAsync();

            return await reader.ToRightsResultAsync();
        }

        public Task<bool> HasRightAsync(string rightKey, string scopeKey)
        {
            throw new NotImplementedException();
        }
    }
}
