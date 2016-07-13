﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class UserService
    {
        private readonly IUserStorage _userStorage;

        public UserService(IUserStorage userStorage)
        {
            this._userStorage = userStorage;
        }

        private bool UserIsValid(User user)
        {
            bool result = true;
            return result;
        }
    }
}

/*
 *  There should be several ways to store users (for ex. In DB), but we need only one implementation – in memory. But there should be a possibility to add an another implementation.
 *	Methods for storage:
 *	Add a new user: Add(User user) -> returns User ID
 *	Search for an user: SearchForUser(ISearchCriteria criteria) -> returns User IDs. At least 3 criteria.
 *	Possble to use predicate here SearchForUser(Func<T>[] criteria).
 *	Delete an user: void Delete(...)
 *	When creating a new there should be a possibility to change the strategy to generate an ID.
 *  When adding a new user there should be a way to set a different set of rules for validating an user entity before adding it: Add(user) -> validation -> exception if not valid or generate and return a new id.
 */
