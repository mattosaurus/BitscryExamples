using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADGroupAuthorization.Services
{
    /// <summary>
    /// IMSGraph service has samples to show how to call Microsoft Graph using the Graph SDK
    /// </summary>
    public interface IMSGraphService
    {
        /// <summary>Gets the current user directory roles.</summary>
        /// <param name="accessToken">The access token for MS Graph.</param>
        /// <returns>A list of directory roles</returns>
        Task<IList<DirectoryRole>> GetCurrentUserDirectoryRolesAsync(string accessToken);

        /// <summary>Gets the signed-in user groups and roles. A more efficient implementation that gets both group and role membership in one call</summary>
        /// <param name="accessToken">The access token for MS Graph.</param>
        /// <returns>A list of UserGroupsAndDirectoryRoles</returns>
        Task<UserGroupsAndDirectoryRoles> GetCurrentUserGroupsAndRolesAsync(string accessToken);

        /// <summary>Gets the groups the signed-in user's is a member of.</summary>
        /// <param name="accessToken">The access token for MS Graph.</param>
        /// <returns>A list of Groups</returns>
        Task<IList<Group>> GetCurrentUsersGroupsAsync(string accessToken);

        /// <summary>Gets basic details about the signed-in user.</summary>
        /// <param name="accessToken">The access token for MS Graph.</param>
        /// <returns>A detail of the User object</returns>
        Task<User> GetMeAsync(string accessToken);

        /// <summary>Gets the groups the signed-in user's is a direct member of.</summary>
        /// <param name="accessToken">The access token for MS Graph.</param>
        /// <returns>A list of Groups</returns>
        Task<List<Group>> GetMyMemberOfGroupsAsync(string accessToken);

        /// <summary>Gets the signed-in user's photo.</summary>
        /// <param name="accessToken">The access token for MS Graph.</param>
        /// <returns>The photo of the signed-in user as a base64 string</returns>
        Task<string> GetMyPhotoAsync(string accessToken);

        /// <summary>Gets the users in a tenant.</summary>
        /// <param name="accessToken">The access token for MS Graph.</param>
        /// <returns>A list of users</returns>
        Task<List<User>> GetUsersAsync(string accessToken);
    }
}
