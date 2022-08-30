using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public WalkDifficultyRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty difficulty)
        {
            
            difficulty.Id = new Guid();

            await _dbContext.WalkDifficulty.AddAsync(difficulty);
            await _dbContext.SaveChangesAsync();
            
            return difficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);
            
            if(existingWalkDifficulty == null)
            {
                return null;
            }
            //remove it
            _dbContext.WalkDifficulty.Remove(existingWalkDifficulty);
            await _dbContext.SaveChangesAsync();
            return existingWalkDifficulty;

        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync() =>
            await _dbContext.WalkDifficulty.ToListAsync();
        

        public  Task<WalkDifficulty> GetAsync(Guid id) =>
            _dbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty difficulty)
        {
            var existingWalkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);

            if(existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = difficulty.Code;
            await _dbContext.SaveChangesAsync();

            return existingWalkDifficulty;
        }
    }
}
