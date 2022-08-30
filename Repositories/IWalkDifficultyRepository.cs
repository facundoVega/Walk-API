using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkDifficultyRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();

        Task<WalkDifficulty> GetAsync(Guid id);

        Task<WalkDifficulty> AddAsync(WalkDifficulty difficulty);

        Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty difficulty);

        Task<WalkDifficulty> DeleteAsync(Guid id);
    }
}
