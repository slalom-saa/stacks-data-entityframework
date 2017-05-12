/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Microsoft.Extensions.Configuration;
using Slalom.Stacks.Services;

namespace Slalom.Stacks.EntityFramework.EndPoints
{
    /// <summary>
    /// Gets the Entity Framework configuration.
    /// </summary>
    [EndPoint("_system/configuration/entity-framework")]
    public class GetConfiguration : EndPoint
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfiguration" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public GetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public override void Receive()
        {
            var options = new EntityFrameworkOptions();
            _configuration.GetSection("Stacks:EntityFramework").Bind(options);
            this.Respond(options);
        }
    }
}