using System.Threading.Tasks;
using AElf.Contracts.TestKit;
using AElf.Kernel;
using Shouldly;
using Vote;
using Xunit;

namespace AElf.Contracts.Vote
{
    public partial class VoteTests : VoteContractTestBase
    {
        [Fact]
        public async Task VoteContract_TakeSnapshot()
        {
            //without permission
            {
                var votingItem = await RegisterVotingItemAsync(10, 4, true, DefaultSender, 1);

                var otherUser = SampleECKeyPairs.KeyPairs[2];
                var transactionResult = (await GetVoteContractTester(otherUser).TakeSnapshot.SendAsync(
                    new TakeSnapshotInput
                    {
                        VotingItemId = votingItem.VotingItemId,
                        SnapshotNumber = 1
                    })).TransactionResult;
                
                transactionResult.Status.ShouldBe(TransactionResultStatus.Failed);
                transactionResult.Error.Contains("").ShouldBeTrue();
            }
            
            //voting event not exist
            {
                var transactionResult = (await VoteContractStub.TakeSnapshot.SendAsync(
                    new TakeSnapshotInput
                    {
                        VotingItemId = Hash.Generate(),
                        SnapshotNumber = 1
                    })).TransactionResult;
                
                transactionResult.Status.ShouldBe(TransactionResultStatus.Failed);
                transactionResult.Error.Contains("").ShouldBeTrue();
            }
            
            //snapshot number not correct
            {
                var votingItem = await RegisterVotingItemAsync(10, 4, true, DefaultSender, 2);
                var transactionResult = (await VoteContractStub.TakeSnapshot.SendAsync(
                    new TakeSnapshotInput
                    {
                        VotingItemId = votingItem.VotingItemId,
                        SnapshotNumber = 2
                    })).TransactionResult;
                
                transactionResult.Status.ShouldBe(TransactionResultStatus.Failed);
                transactionResult.Error.Contains("").ShouldBeTrue();
            }
            
            //success
            {
                var registerItem = await RegisterVotingItemAsync(10, 4, true, DefaultSender, 3);
                for (int i = 0; i < 3; i++)
                {
                    var transactionResult = (await VoteContractStub.TakeSnapshot.SendAsync(
                        new TakeSnapshotInput
                        {
                            VotingItemId = registerItem.VotingItemId,
                            SnapshotNumber = i+1
                        })).TransactionResult;
                
                    transactionResult.Status.ShouldBe(TransactionResultStatus.Mined);

                    var votingItem = await GetVoteItem(registerItem.VotingItemId);
                    votingItem.CurrentSnapshotNumber.ShouldBe(i+2);
                }
            }
        }
    }
}