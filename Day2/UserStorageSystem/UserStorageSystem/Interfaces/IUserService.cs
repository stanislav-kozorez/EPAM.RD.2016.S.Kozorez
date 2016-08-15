using System;
using System.ServiceModel;
using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    /// <summary>
    /// Used by UserManagementSystem and UserService classes. Describes Wcf service contract.
    /// </summary>
    [ServiceContract]
    public interface IUserService
    {
        /// <summary>
        ///     Enables to add new user
        /// </summary>
        /// <param name="user">
        ///     User, that will be added to user storage system.
        /// </param>
        /// <returns>
        ///     Generated user id.
        /// </returns>
        [OperationContract]
        string AddUser(User user);

        /// <summary>
        ///     Enables to remove user from user storage system.
        /// </summary>
        /// <param name="id">
        ///     User's id, that will be removed
        /// </param>
        [OperationContract]
        void RemoveUser(string id);

        /// <summary>
        ///     Enables to find users by predicate
        /// </summary>
        /// <param name="predicate">
        ///     Function that is used to find appropriate user
        /// </param>
        /// <returns>
        ///     Array of users' ids
        /// </returns>
        string[] FindUser(Func<User, bool> predicate);

        /// <summary>
        ///     Enables to find user by id
        /// </summary>
        /// <param name="id">
        ///     id of the user, to find
        /// </param>
        /// <returns>
        ///     User with appropriate id 
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     thrown when user with such id doesn't exist.
        /// </exception>
        [OperationContract]
        User FindUser(string id);

        /// <summary>
        ///     Enables to store service state on disk.
        /// </summary>
        [OperationContract]
        void CommitChanges();


        /// <summary>
        ///     Used only for Wcf Service. Uses Find( Func< User, Bool> predicate) method internally.
        /// </summary>
        /// <returns>
        ///     array of user ids.
        /// </returns>
        [OperationContract]
        string[] FindUsersByFirstName(string firstName);

        /// <summary>
        ///     Used only for Wcf Service. Uses Find( Func< User, Bool> predicate) method internally.
        /// </summary>
        /// <returns>
        ///     array of user ids.
        /// </returns>
        [OperationContract]
        string[] FindUsersByLastName(string lastName);

        /// <summary>
        ///     Used only for Wcf Service. Uses Find( Func< User, Bool> predicate) method internally.
        /// </summary>
        /// <returns>
        ///     array of user ids.
        /// </returns>
        [OperationContract]
        string[] FindUsersByBirthDate(DateTime birthDate);

        /// <summary>
        ///     Used only for Wcf Service. Uses Find( Func< User, Bool> predicate) method internally.
        /// </summary>
        /// <returns>
        ///     array of user ids.
        /// </returns>
        [OperationContract]
        string[] FindUsersWhoseNameContains(string word);
    }
}
