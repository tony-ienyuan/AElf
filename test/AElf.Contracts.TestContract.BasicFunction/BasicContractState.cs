using AElf.Standards.ACS1;
using AElf.Sdk.CSharp.State;
using AElf.Types;

namespace AElf.Contracts.TestContract.BasicFunction
{
    public partial class BasicFunctionContractState : ContractState
    {
        public BoolState Initialized { get; set; }
        public StringState ContractName { get; set; }
        public ProtobufState<Address> ContractManager { get; set; }
        public Int64State MinBet { get; set; }
        public Int64State MaxBet { get; set; }

        public Int64State MortgageBalance { get; set; }
        public Int64State TotalBetBalance { get; set; }
        public Int64State RewardBalance { get; set; }

        public MappedState<Address, long> WinerHistory { get; set; }
        public MappedState<Address, long> LoserHistory { get; set; }
        
        public MappedState<string, MethodFees> TransactionFees { get; set; }

        public SingletonState<UInt256Value> UInt256State { get; set; }
        public SingletonState<Int256Value> Int256State { get; set; }
    }
}