﻿using System;
using Slalom.Stacks.Services.Validation;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.EntityFramework.EndPoints.GetConfiguration.Rules
{
    /// <summary>
    /// Validates that the current user is a system administrator.
    /// </summary>
    public class user_must_be_system_admin : SecurityRule<GetConfigurationRequest>
    {
        /// <inheritdoc />
        public override ValidationError Validate(GetConfigurationRequest instance)
        {
            if (!this.Request.User.IsInRole("System Administrator"))
            {
                return "You must be system administrator to get the configuration";
            }
            return null;
        }
    }
}