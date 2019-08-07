using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Kernel;
using AElf.OS.Network;
using AElf.Types;

namespace AElf.OS.BlockSync.Application
{
    public interface IBlockSyncValidationService
    {
        Task<bool> ValidateAnnouncementAsync(Chain chain, BlockAnnouncement blockAnnouncement);

        Task<bool> ValidateBlockAsync(Chain chain, BlockWithTransactions blockWithTransactions);

        Task<bool> ValidateTransactionAsync(IEnumerable<Transaction> transactions);

        Task<bool> ValidateBlockBeforeAttachAsync(BlockWithTransactions blockWithTransactions);
    }
}