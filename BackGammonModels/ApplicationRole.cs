using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace BackGammonModels;

[CollectionName("roles")]
public class ApplicationRole:MongoIdentityRole<Guid>
{

}
