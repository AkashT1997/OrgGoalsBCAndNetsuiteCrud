using MongoDB.Driver;
using OrgGoalsBCAndNetsuiteCrud.Common;
using OrgGoalsBCAndNetsuiteCrud.Models.MongoDB;

namespace OrgGoalsBCAndNetsuiteCrud.MongoDB_Services
{
    public class BCAuthService
    {
        private readonly IMongoCollection<BCAuthModel> _bcAuthModel;

        public BCAuthService()
        {
            var client = new MongoClient(AppConfiguration.MongoDBConnectionString);
            var database = client.GetDatabase(AppConfiguration.MongoDatabaseName);
            _bcAuthModel = database.GetCollection<BCAuthModel>("BussinessCentralAuth");
        }

        // Create
        public async Task<BCAuthModel> CreateAsync(BCAuthModel BCAuthModel)
        {
            await _bcAuthModel.InsertOneAsync(BCAuthModel);
            return BCAuthModel;
        }

        // Read
        public async Task<List<BCAuthModel>> GetAllAsync()
        {
            return await _bcAuthModel.Find(BCAuthModel => true).ToListAsync();
        }

        public async Task<BCAuthModel> GetByIdAsync(string id)
        {
            return await _bcAuthModel.Find(BCAuthModel => BCAuthModel.id == id).FirstOrDefaultAsync();
        }

        public async Task<BCAuthModel> GetByFilterAsync(string id)
        {
            var filter = Builders<BCAuthModel>.Filter.Eq(x => x.id, id);
            return await _bcAuthModel.Find(filter).FirstOrDefaultAsync();
        }

        // Update
        public async Task UpdateAsync(string id, BCAuthModel BCAuthModel)
        {
            await _bcAuthModel.ReplaceOneAsync(b => b.id == id, BCAuthModel);
        }

       

        // Delete
        public async Task DeleteAsync(string id)
        {
            await _bcAuthModel.DeleteOneAsync(BCAuthModel => BCAuthModel.id == id);
        }
    }
}
