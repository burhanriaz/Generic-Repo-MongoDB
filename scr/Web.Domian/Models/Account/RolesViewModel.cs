using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using Web.Domain;
using Web.Entity;

namespace Web.Domain.Models.Account
{
    public class RolesViewModel :  IBaseDomain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public IEnumerable<string> Permissions { get; set; } 
    }
}
