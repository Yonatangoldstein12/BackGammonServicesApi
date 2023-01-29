using BackGammonModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackGammonDAL;

public class UsersRespository
{
    public MongoDB db = new MongoDB("TalkBack");
    private IMongoCollection<User> _collection;

    public UsersRespository() => _collection = db.Client.GetCollection<User>("Users");

    public List<User> GetUsers => db.Client.GetCollection<User>("Users").Find(new BsonDocument()).ToList();

    public void Add(User user) => _collection.InsertOne(user);
    public User GetUserById(BsonObjectId id)
    {
        var filter = Builders<User>.Filter.Eq(x => x._id, id);

        return _collection.Find(filter).FirstOrDefault();
    }
    public User GetUserName()
    {
        return _collection.Find(x => x.UserName == x.UserName).FirstOrDefault();
    }
    public void Remove(BsonObjectId id)
    {
        var filter = Builders<User>.Filter.Eq(x => x._id, id);
        _collection.DeleteOne(filter);
    }
    public void Update(User user)
    {
        var filter = Builders<User>.Filter.Eq(x => x._id, user._id);
        _collection.ReplaceOne(filter, user);
    }
}
