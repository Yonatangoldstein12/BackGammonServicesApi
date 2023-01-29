using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackGammonModels;

    [CollectionName("users")]
public class ApplicationUser:MongoIdentityUser<Guid>
{
    public string UserName { get; set; } = string.Empty;
}
